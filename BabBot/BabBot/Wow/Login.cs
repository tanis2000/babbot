/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Manager;
using BabBot.Common;
using System.Threading;

// TODO Critical
// Realm wizard
// Patch downloading, trace download process, bot restart

// TODO Minor
// Detect realm queue and wait
// Localization


namespace BabBot.Wow
{
    static class Login
    {
        private static int State;
        private static int MaxScreenWaitTime = 30000; // 30 sec
        private static bool NeedRetry;
        private static bool RetryCount;
        private static string CurrentGlueScreen;
        private static string CurrentGlueDialog;
        private static DateTime StateChangeTime;
        private static string DInfo;

        // Login States
		static Array LoginState = Array.CreateInstance( typeof(String), 12);
		
		// Login progress
		static string LOGIN_SERVER_DOWN = "Login Server Down";
		static string LOGIN_SRP_ERROR = "Authentication Error";
		static string LOGIN_STATE_AUTHENTICATED = "Success!";
		static string LOGIN_STATE_AUTHENTICATING = "Authenticating";
		static string LOGIN_STATE_CHECKINGVERSIONS = "Validating Version";
		static string LOGIN_STATE_CONNECTING = "Connecting";
		static string LOGIN_STATE_DOWNLOADFILE = "Downloading";
		static string LOGIN_STATE_HANDSHAKING = "Handshaking";
		static string LOGIN_STATE_INITIALIZED = "Initialized";
		static string LOGIN_STATE_SURVEY = "Submitting non-personal system specification";

        // Log facility
        private static string LogFS = "char";

        // Default waiting time if some process pending
        private static int psleepTime = 5000; // msec, as usual

        static Login()
        {
            LoginState.SetValue("login", 0); // "AccountLogin";
		    LoginState.SetValue("charselect", 1); // "CharacterSelect";
		    LoginState.SetValue("realmwizard", 2); // "RealmWizard";
		    LoginState.SetValue("realmsuggest", 3); // "Realm Suggestion";
		    LoginState.SetValue("patchdownload", 4); // "PatchDownload";
		    LoginState.SetValue("trialconvert", 5); // "TrialConvert";
		    LoginState.SetValue("movie", 6); // "MovieFrame";
		    LoginState.SetValue("credits", 7); // "CreditsFrame";
		    LoginState.SetValue("options", 8); // "OptionsFrame";
		    LoginState.SetValue("tos", 9); // "TOS Dialog";
		    LoginState.SetValue("eula", 10); // EULA
            LoginState.SetValue("realmselect", 11); // Realm Selection
        }

        private static void Log(String msg)
        {
            Output.Instance.Log(LogFS, msg);
        }

        private static void Debug(String msg)
        {
            Output.Instance.Debug(LogFS, msg);
        }

        /// <summary>
        /// Execute Auto-Login
        /// </summary>
        /// <param name="realm">Realm Name</param>
        /// <param name="account">WoW account</param>
        /// <param name="pwd">Account password</param>
        /// <param name="name">Character name</param>
        /// <param name="retry"># of retries</param>
        /// <returns>TRUE if succeed and FALSE if not</returns>
        public static bool AutoLogin(string location, string type, 
                string realm, string account, string pwd, string name, int retry)
        {
			int RetryCount = 0;

            do
            {
                if (SetGlueState(location, type, realm) == -1)
                    return false;

                switch (State)
                {
                    case 0: // Login
                        SendLogin(realm, account, pwd);
                        break;

                    case 1: // Character Selection
                        int idx = SelectCharacter(name);
                        if (idx > 0)
                        {
                            Log("Found " + name + " as id:" + idx);
                            ProcessManager.CommandManager.SendKeys(CommandManager.SK_ENTER);
                            
                            // We done. World loading
                            State = 999;
                        }
                        else if (idx == -1)
                        {
                            Log("Character '" + name +
                                "' not found in list for realm '" + realm + "'");
                            return false;
                        }
                        else
                            return false;
                        break;

                    case 2: // Realm Wizard
                        if (string.IsNullOrEmpty(DInfo))
                        {
                            Log("Unable retrive information from RealmWizard screen");
                            return false;
                        } else if (DInfo.Equals("ok")) {
                            Log(string.Format(
                                "Selected Realm Location: '{0}'; Type: '{1}'", location, type));
                            ProcessManager.CommandManager.SendKeys(CommandManager.SK_ENTER);
                            Thread.Sleep(psleepTime);
                        } else if (DInfo.Equals("location_not_found")) {
                            Log(string.Format("Realm Location '{0}' not found", location));
                        } else if (DInfo.Equals("")) {
                            Log(string.Format("Realm Type '{0}' not found", type));
                        } else {
                            Log(string.Format("Unknown response '{0}' from lua script", DInfo));
                            return false;
                        }
                        break;
                    
                    case 3: // Realm Suggestion
                        if (DInfo.Equals("accept"))
                        {
                            Log(string.Format(
                                "Accepting Realm '{0}' suggestion", realm));
                            ProcessManager.CommandManager.SendKeys(CommandManager.SK_ENTER);
                        }
                        else
                        {
                            Log(string.Format(
                                "Realm '{0} not suggested. Looking realm list ...", realm));
                            ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                        }
                            
                        Thread.Sleep(psleepTime);
                        break;

                    case 4: // Patch Download
                        if (string.IsNullOrEmpty(DInfo))
                        {
                            // Exit bot
                            throw new Exception("Unable download patch");
                        } else {

                            if (DInfo.Equals("100%"))
                            {
                                // Download completed. Can't continue bot without patch verification
                                ProcessManager.CommandManager.SendKeys(CommandManager.SK_ENTER);
                                throw new Exception("New patch just arrived.");
                            } else {
                                // Keep waiting
                                Log(string.Format("{0} downloaded. Keep waiting ...", DInfo));
                                Thread.Sleep(psleepTime);
                            }
                        }

                        break;
                        
                    case 6: // Movie
                        // Ohhh, cmon
                        ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                        break;
                        
                    case 9: // tos
                    case 10: // eula
                        ProcessManager.Injector.Lua_DoString(string.Format(
                           @"(function()
                            Accept{0}()
                            TOSFrame:Hide()
                            TOSNotice:Hide()
                            AccountLogin_ShowUserAgreements()
                        end)()", CurrentGlueDialog.ToUpper()));
                        break;
                    
                    case 11: // Realm suggestion
                        if (DInfo.Equals("ok"))
                        {
                            // Desired realm selected and it active. Accept selection
                            Log(string.Format("Selecting '{0}' realm", realm));
                            ProcessManager.CommandManager.SendKeys(CommandManager.SK_ENTER);
                        } else if (DInfo.Equals("down"))
                        {
                            // Go to realm selection
                            Log(string.Format(
                                "Realm '{0}' down. Checking again in 10 sec ...", realm));
                            Thread.Sleep(10000);
                        } else if (DInfo.Equals("not_found"))
                        {
                            Log(string.Format(
                                "Realm '{0}' not found. Canceling login", realm));
                            return false;
                        }
                        break;
                    case 99: // disconnect
                        ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                        break;

                    case 100: // Pending
                        if (DateTime.Now.Millisecond - StateChangeTime.Millisecond > MaxScreenWaitTime) {
                            // Cancel current process and retry
                            RetryCount++;
                            Log("Session stack. Canceling ...");
                            ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                            Thread.Sleep(10000);

                            Log("Retrying " + RetryCount + " of " + retry);
                            StateChangeTime = DateTime.Now;
                        } else
                            Thread.Sleep(psleepTime);

                        break;
                    
                    case 101: // Retry
                        RetryCount++;
                        Thread.Sleep(10000);

                        Log("Retrying " + RetryCount + " of " + retry);
                        StateChangeTime = DateTime.Now;
                        break;

                    default:
                        Log("'" + LoginState.GetValue(State) + 
                                                                  "' not implemented yet");
                        return false;
                }

                if ((State >= 0) && (State < 100))
                    // Wait after execution of last command
                    Thread.Sleep(1000);

                
            } while (!((State == 999) || (RetryCount > retry)));

            return true;
        }

        /// <summary>
        /// Sending account/password
        /// </summary>
        /// <param name="realm">Realm name</param>
        /// <param name="user">Account</param>
        /// <param name="pwd">Password</param>
        static void SendLogin(string realm, string user, string pwd)
		{
			string realm_cmd = "";
			if (!realm.Equals(""))
				realm_cmd = string.Format(@"local realm = AccountLoginRealmName:GetText(serverName);
                    if (realm ~= '{0}') then
                        AccountLoginRealmName:SetText('{0}')
                    end", realm);
			
			ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
						{0}
                        AccountLoginAccountEdit:SetText('{1}')
                        AccountLoginPasswordEdit:SetText('{2}')
                        DefaultServerLogin(AccountLoginAccountEdit:GetText(), AccountLoginPasswordEdit:GetText())
                    end)()", realm_cmd, user, pwd));

            // Unregister our hook right away before wow tries to login
            ProcessManager.Injector.Lua_UnRegisterInputHandler();

            // Wait
            Thread.Sleep(5000);

            // ReRegister the hook
            ProcessManager.Injector.Lua_RegisterInputHandler();
		}
		
		/// <summary>
		/// Selecting character
		/// </summary>
		/// <param name="name">Character name</param>
		/// <returns>Character index or -1 if character not found on realm</returns>
		static int SelectCharacter(string name)
		{
			ProcessManager.Injector.Lua_DoStringEx(string.Format(@"(function()
						local found = nill

                        local numChars = GetNumCharacters();
		                for i=1, numChars, 1 do
							local name = GetCharacterInfo(i);
							if (name == '{0}') then
								found = i
								break
							end
						end
						if (found) then
							CharacterSelect_SelectCharacter(found)
                        end
						return found
                    end)()", name));

            string idx = ProcessManager.Injector.Lua_GetLocalizedText(0);

            if ((idx == null) || idx.Equals(""))
            {
                Log("Unable found character name '" + name + "'");
                return -1;
            } else
                return Convert.ToInt32(idx);
		}
		
        /// <summary>
        /// Set login state
        /// </summary>
        /// <param name="state">State ID</param>
        /// <returns>State ID</returns>
        private static int SetState(int state)
        {
            State = state;
            StateChangeTime = DateTime.Now;
            return State;
        }

        /// <summary>
        /// Check current Glue window and set login state
        /// </summary>
        /// <returns>Login State ID or -1 if Glue Window not found (unknown)</returns>
		static int SetGlueState(string location, string type, string realm)
		{
            // We need returning values
            string cmd = string.Format(@"(function()
    local d1, d2, d3
    local found = false

    if (GlueDialog:IsShown()) then
        d1 = GlueDialog.which
        d2 = GlueDialogText:GetText()
        if (GlueDialogHTML:IsShown()) then
            d3 = 'html'
        end
    else
        -- TOS/EULA
        if (TOSFrame:IsShown()) then
            d1 = string.lower(TOSFrame.noticeType)
            local scrollbar = _G[TOSFrame.noticeType .. 'ScrollFrameScrollBar'];
            if (scrollbar:IsShown()) then
                local min, max = scrollbar:GetMinMaxValues()
                scrollbar:SetValue(max)
            end
        else
            -- Realm Selection
            if (RealmList:IsShown()) then
                d1 = 'realmselect'
                d3 = 'not_found'
                for i=1, MAX_REALMS_DISPLAYED, 1 do
                    realmIndex = RealmList.offset + i
                    name, numCharacters, invalidRealm, realmDown = GetRealmInfo(1, realmIndex);
                
                    if (name == '{0}') then
                        if (realmDown) then
                            d3 = 'down'
                        else 
                            if (RealmList.currentRealm ~= realmIndex) then
                                RealmList.refreshTime = {3}

                                prealm = _G['RealmListRealmButton'..RealmList.currentRealm]
                                prealm:UnlockHighlight();

                                realm = _G['RealmListRealmButton'..realmIndex]
                                RealmList.currentRealm = realm:GetID()
		                        RealmList.selectedName = realm.name

                                realm:LockHighlight()
						        RealmListHighlight:SetPoint('TOPLEFT', realm, 'TOPLEFT', 0, 0)
                                RealmListHighlight:Show()
                            end
                        
                            d3 = 'ok'
                        end

                        break;
                    end
                end
            else
                -- Realm Wizard
                if (RealmWizard:IsShown()) then
                    found = false
                    for i = 1, 8 do
                        chkLocationBox = _G['RealmWizardLocationButton' .. i]
                        chkLocationLabel = _G['RealmWizardLocationButton' .. i .. 'Text']
                        chkLocationText = chkLocationLabel:GetText()
                        if (chkLocationText == '{1}') then
                            chkLocationBox:SetChecked(1)
                            RealmWizardLocationButton_OnClick(chkLocationBox.categoryIndex)
            
                            found = true
                        end
                    end

                    if (found) then
                        found = false
                            
                        for i = 1, 4 do
                            checkBox = _G['RealmWizardGameTypeButton'..i]
                            checkLabel = _G['RealmWizardGameTypeButton' .. i .. 'Text']
                            checkText = checkLabel:GetText()
                            if (checkText ~= {2}) then
                                checkBox:SetChecked(0);
                            else
                                checkBox:SetChecked(1);
                                RealmWizardGametypeLabel:SetText(checkText);
                                d3 = 'ok'
                                found = true
                                    
                                break
                            end
                        end

                        if (not found) then
                            d3 = 'type_not_found'
                        end
                    else
                        d3 = 'location_not_found'
                    end
                else
                    -- Patch download
                    if (PatchDownloadUI:IsShown()) then
                        d3 = PatchProgressText:GetText()
                    end
                end
            end
        end
    end

    return CURRENT_GLUE_SCREEN, CURRENT_GLUE_PENDING, d1, d2, d3, IsConnectedToServer()
end)()", realm, location, type, (int)((psleepTime/1000) * 2));

            ProcessManager.Injector.Lua_DoStringEx(cmd);
            CurrentGlueScreen = ProcessManager.Injector.Lua_GetLocalizedText(0);
            string PendingScreen = ProcessManager.Injector.Lua_GetLocalizedText(1);
            CurrentGlueDialog = ProcessManager.Injector.Lua_GetLocalizedText(2);
            string DialogText = ProcessManager.Injector.Lua_GetLocalizedText(3);
            DInfo = ProcessManager.Injector.Lua_GetLocalizedText(4);
            string Connected = ProcessManager.Injector.Lua_GetLocalizedText(5);

            bool IsDialogText = (!((DialogText == null) || DialogText.Equals("")) && (DialogText.Equals("html")));
            bool IsHtml = ((DInfo != null) && !DInfo.Equals(""));

            Debug(string.Format("Screen: {0}; Pending: {1};" +
                " Dialog: {2}; DialogText: {3}; DInfo: {4}; Connected: {5}",
                CurrentGlueScreen, PendingScreen, CurrentGlueDialog, DialogText, DInfo, Connected));

            if (!((PendingScreen == null) || PendingScreen.Equals("")))
            {
                Log("'" + PendingScreen + "' coming ...");
                return SetState(100);
            }

            if (CurrentGlueScreen == null)
            {
                Log("Not on login page");
                // not on login page
                return -1;
            }

            int idx = Array.IndexOf(LoginState, CurrentGlueScreen);
            if (idx < 0)
            {
                Log("Unknown GlueScreen '" + CurrentGlueScreen + "'");
                return -1;
            }

            SetState(idx);

            // Check for dialog
            if ((CurrentGlueDialog == null) || CurrentGlueDialog.Equals(""))
            {
                // we done
                return State;
            }

            // Analyze dialogs
            
            // 1. TOS & EULA & realm select shown in dialog
            int idy = Array.IndexOf(LoginState, CurrentGlueDialog);
            if (idy >= 0)
            {
                return SetState(idy);
            }

             // Check for progress dialog
             if (CurrentGlueDialog.Equals("CANCEL")) {
                // Current state in progress
                string s = "Current state in progress";
                if (IsDialogText) 
                    s += " '" + DialogText + "'";
                 s += " ...";
                 Log(s);
                 return SetState(100);
             }
             
            // Check for realm suggest
            if (CurrentGlueDialog.Equals("SUGGEST_REALM"))
            {
                // See what suggested
                if (DialogText.IndexOf(realm) >= 0)
                    DInfo = "accept";
                else
                    DInfo = "change";

                return SetState(3);
            }
                
            if (CurrentGlueDialog.Equals("DISCONNECT"))
                return SetState(99);

            if (IsHtml) {
                if (CurrentGlueDialog.Equals("CONNECTION_HELP_HTML") && IsHtml)
                {
                    Log("Networking problem. Retrying in 10 sec");
                    // Connection problem
                    ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                    return SetState(101);
                } else {
                    Log("Received Blizz. message. Interrupting login");
                    return -1;
                }
            }

            if (IsDialogText)
            {
                Log("Received" + DialogText);
                return SetState(100);
            }

            // If we still here than something wrong
            Log(string.Format(@"Unknow state detected for GlueScreen: {0}; 
                    GlueDialog: {1}", CurrentGlueScreen, CurrentGlueDialog));
            return -1;
        }
    }
}
