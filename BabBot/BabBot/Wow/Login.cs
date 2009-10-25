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

        // Login States
		static Array LoginState = Array.CreateInstance( typeof(String), 11);
		
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

        static Login()
        {
            LoginState.SetValue("login", 0); // "AccountLogin";
		    LoginState.SetValue("charselect", 1); // "CharacterSelect";
		    LoginState.SetValue("realmwizard", 2); // "RealmWizard";
		    LoginState.SetValue("charcreate", 3); // "CharacterCreate";
		    LoginState.SetValue("patchdownload", 4); // "PatchDownload";
		    LoginState.SetValue("trialconvert", 5); // "TrialConvert";
		    LoginState.SetValue("movie", 6); // "MovieFrame";
		    LoginState.SetValue("credits", 7); // "CreditsFrame";
		    LoginState.SetValue("options", 8); // "OptionsFrame";
		    LoginState.SetValue("tos", 9); // "TOS Dialog";
		    LoginState.SetValue("eula", 10); // EULA
        }

        public static bool AutoLogin(string realm, string account, string pwd, string name, int retry)
        {
			int RetryCount = 0;

            do
            {
                if (RetryCount > 0)
                    Output.Instance.Log("Retrying " + RetryCount + " of " + retry);

                if (SetGlueState() == -1)
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
                            Output.Instance.Log("Found " + name + " as id:" + idx);
                            ProcessManager.CommandManager.SendKeys(CommandManager.SK_ENTER);
                            
                            // We done. World loading
                            State = 999;
                        }
                        else if (idx == -1)
                        {
                            Output.Instance.Log("Character '" + name +
                                "' not found in list for realm '" + realm + "'");
                            return false;
                        }
                        else
                            return false;
                        break;

                    // case 2: // Realm Wiz
                        // break;
                        
                    case 6: // Movie
                        // Ohhh, cmon
                        ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                        break;
                        
                    case 9: // tos
                    case 10: // eula
                        ProcessManager.Injector.Lua_DoString(string.Format(
                           @"Accept{0}()
                            TOSFrame():Hide()
                            TOSNotice:Hide()
                            AccountLogin_ShowUserAgreements()", CurrentGlueDialog.ToUpper()));
                        break;
                        
                    case 99: // disconnect
                        ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                        break;

                    case 100: // Pending
                        if (DateTime.Now.Millisecond - StateChangeTime.Millisecond <= MaxScreenWaitTime) {
                            // Cancel current process and retry
                            RetryCount++;
                            ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                            Thread.Sleep(10000);
                            StateChangeTime = DateTime.Now;
                        } else 
                            Thread.Sleep(1000);

                        break;
                    
                    case 101: // Retry
                        RetryCount++;
                        Thread.Sleep(10000);
                        StateChangeTime = DateTime.Now;
                        break;

                    default:
                        Output.Instance.Log("'" + LoginState.GetValue(State) + "' not implemented yet");
                        return false;
                }

                if ((State >= 0) && (State < 100))
                    // Wait after execution of last command
                    Thread.Sleep(1000);

                
            } while (!((State == 999) || (RetryCount > retry)));

            return true;
        }


        static void SendLogin(string realm, string user, string pwd)
		{
			string realm_cmd = "";
			if (!realm.Equals(""))
				realm_cmd = string.Format(@"local realm = AccountLoginRealmName:SetText(serverName);
                    if (realm != {0})
                    AccountLoginRealmName:SetText({0})", realm);
			
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
		
		
		static int SelectCharacter(string name)
		{
			ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
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
                Output.Instance.Log("Unable found character name '" + name + "'");
                return -1;
            } else
                return Convert.ToInt32(idx);
		}
		
        private static int SetState(int state)
        {
            State = state;
            StateChangeTime = DateTime.Now;
            return State;
        }


		static int SetGlueState()
		{
            ProcessManager.Injector.Lua_DoString(@"(function()
              local d1, d2, d3
              if (GlueDialog:IsShown()) then
                d1 = GlueDialog.which
                d2 = GlueDialogText:GetText()
                if (GlueDialogHTML:IsShown()) then
                    d3 = 'html'
                 end
              else 
                if (TOSFrame:IsShown()) then
                    d1 = string.lower(TOSFrame.noticeType)
                    local scrollbar = _G[TOSFrame.noticeType .. 'ScrollFrameScrollBar'];
                    if (scrollbar:IsShown()) then
					    local min, max = scrollbar:GetMinMaxValues()
					    scrollbar:SetValue(max)
                    end
                end
              end
              return CURRENT_GLUE_SCREEN, CURRENT_GLUE_PENDING, d1, d2, d3, IsConnectedToServer()
            end)()");

            CurrentGlueScreen = ProcessManager.Injector.Lua_GetLocalizedText(0);
            string PendingScreen = ProcessManager.Injector.Lua_GetLocalizedText(1);
            CurrentGlueDialog = ProcessManager.Injector.Lua_GetLocalizedText(2);
            string DialogText = ProcessManager.Injector.Lua_GetLocalizedText(3);
            string d3 = ProcessManager.Injector.Lua_GetLocalizedText(4);
            string Connected = ProcessManager.Injector.Lua_GetLocalizedText(5);

            bool IsDialogText = (!((DialogText == null) || DialogText.Equals("")));
            bool IsHtml = ((d3 != null) && !d3.Equals(""));

            if (!((PendingScreen == null) || PendingScreen.Equals("")))
            {
                Output.Instance.Log("'" + PendingScreen + "' coming ...");
                return SetState(100);
            }

            if (CurrentGlueScreen == null)
            {
                Output.Instance.Log("Not on login page");
                // not on login page
                return -1;
            }

            int idx = Array.IndexOf(LoginState, CurrentGlueScreen);
            if (idx < 0)
            {
                Output.Instance.Log("Unknown GlueScreen '" + CurrentGlueScreen + "'");
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
                    s += DialogText;
                 s += " ...";
                 Output.Instance.Log(s);
                 return SetState(100);
             }
             
            if (CurrentGlueDialog.Equals("DISCONNECT"))
                return SetState(99);

            if (IsHtml) {
                if (CurrentGlueDialog.Equals("CONNECTION_HELP_HTML") && IsHtml)
                {
                    Output.Instance.Log("Network problem. Retrying in 10 sec");
                    // Connection problem
                    ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                    return SetState(101);
                } else {
                    Output.Instance.Log("Received Blizz. message. Interrupting login");
                    return -1;
                }
            }

            if (IsDialogText)
            {
                Output.Instance.Log("received" + DialogText);
                return SetState(100);
            }

            // If we still here than something wrong
            Output.Instance.Log(string.Format(@"Unknow state detected for GlueScreen: {0}; 
                    GlueDialog: {1}", CurrentGlueScreen, CurrentGlueDialog));
            return -1;
        }
    }
}
