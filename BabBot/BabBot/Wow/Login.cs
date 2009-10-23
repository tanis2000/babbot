using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Manager;
using BabBot.Common;
using System.Threading;

// TODO Critical
// Patch downloading, trace download process, bot restart
// Cancel TOC, EULA (we violating it anyway :)
// Cancel movie (we've seen it already :)

// TODO Minor
// Detect queue and wait
// Localization

// TOSScrollFrameScrollBar:SetValue(0); 
// local scrollbar = _G[scrollFrame:GetName().."ScrollBar"];
//	local min, max = scrollbar:GetMinMaxValues();

//	-- HACK: scrollbars do not handle max properly
//	-- DO NOT CHANGE - without speaking to Mikros/Barris/Thompson
//	if (scrollbar:GetValue() >= max - 20) then
//		TOSAccept:Enable();

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

        // State Machine
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
                if (!GetGlueState())
                    return false;

                if (RetryCount > 0)
                    Output.Instance.Log("Retrying " + RetryCount + " of " + retry);

                while (State != -1)
                {
                    switch (State)
                    {
                        case 0: // Login
                            SendLogin(realm, account, pwd);

                            if (!WaitGlueScreen(0, 1))
                                return false;
                            break;

                        case 1: // Character Selection
                            int idx = SelectCharacter(name);
                            if (idx > 0)
                            {
                                Output.Instance.Log("Found " + name + " as id:" + idx);
                                ProcessManager.CommandManager.SendKeys(CommandManager.SK_ENTER);
                            }
                            else if (idx == 0)
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
                            if (!GetGlueState())
                                return false;
                            break;
                        
                        // case 9: // eula
                        // case 10: // tos
                           // ProcessManager.CommandManager.SendKeys(CommandManager.SK_ENTER);
                           // break;
                         
                        default:
                            Output.Instance.Log("'" + CurrentGlueScreen + "' not implemented yet");
                            return false;
                        // 
                        /*
                        // This should return whether we are connected or not but it isn't working
                        Injector.Lua_DoString(@"(function()
                            local connected = IsConnectedToServer()
                            return connected
                        end)()");

                        string s = Injector.Lua_GetLocalizedText(0);
                        Output.Instance.Log("Connected: [" + s + "]");

                        // TODO: If we are connected to the server we should now select the character and press the enter world button
                        CommandManager.SendKeys(CommandManager.SK_ENTER); */

                    }

                    if (NeedRetry)
                    {
                        NeedRetry = false;
                        RetryCount++;
                        Thread.Sleep(10000);
                    }

                }
            } while (NeedRetry && (RetryCount <= retry));

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

            if (idx == null)
            {
                Output.Instance.Log("Unable found character name '" + name + "'");
                return -1;
            } else
                return Convert.ToInt32(idx);
		}
		
        private static bool WaitGlueScreen(int state_old, int state_new)
        {
            bool f = false;
            DateTime CurrTime = DateTime.Now;

            while (!f && (DateTime.Now.Millisecond - CurrTime.Millisecond <= MaxScreenWaitTime))
            {
                if (!GetGlueState())
					return false;
				
                f = (State == state_new);
                if (!f)
                {
                    if (State != state_old)
                        // Something unexpected
                        return true;

					if (State == 0)
					{
                        // Check for glue dialog. Only valid is handshaking or connection fail
                        if (!(CurrentGlueDialog.Equals("CANCEL") ||
                                NeedRetry))
                            // Something wrong
                            return false;
                        else
                        {
                            if (CurrentGlueDialog.Equals(LOGIN_STATE_HANDSHAKING))
                                // Keep waiting
                                Thread.Sleep(1000);
                        }
					}
                }
            }
			
			if (!f) {
				Output.Instance.Log("Login pending longer than " + 
                    MaxScreenWaitTime/1000 + " sec.\n" + "Cancelling this try");
				ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
			}
			
			return f;
        }

		static bool GetGlueState()
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
					local min, max = TOSScrollFrameScrollBar:GetMinMaxValues();
					TOSScrollFrameScrollBar:SetValue(max);
                end
              end
              return CURRENT_GLUE_SCREEN, CURRENT_GLUE_PENDING, d1, d2, d3, IsConnectedToServer()
            end)()");

            CurrentGlueScreen = ProcessManager.Injector.Lua_GetLocalizedText(0);
            string PendingScreen = ProcessManager.Injector.Lua_GetLocalizedText(1);
            CurrentGlueDialog = ProcessManager.Injector.Lua_GetLocalizedText(2);
            string d2 = ProcessManager.Injector.Lua_GetLocalizedText(3);
            string d3 = ProcessManager.Injector.Lua_GetLocalizedText(4);
            string Connected = ProcessManager.Injector.Lua_GetLocalizedText(5);

            if (PendingScreen != null)
                        Output.Instance.Log("'" + PendingScreen + "' coming ...");

            if (CurrentGlueScreen != null)
			{
                int idx = Array.IndexOf(LoginState, CurrentGlueScreen);
                if (idx < 0)
                {
                    Output.Instance.Log("Unknown GlueScreen '" + CurrentGlueScreen + "'");
                    return false;
                }
                else
                {
                    State = idx;

                    // Check for dialog
                    if (CurrentGlueDialog != null)
                    {
                        // TOS & EULA shown in dialog
                        int idy = Array.IndexOf(LoginState, CurrentGlueDialog);
                        if (idy >= 0)
                            State = idy;
                        else
                        {
                            // Check for connection problem
                            if (CurrentGlueDialog.Equals("CONNECTION_HELP_HTML"))
                            {
                                ProcessManager.CommandManager.SendKeys(CommandManager.SK_ESC);
                                NeedRetry = true;
                            }
                        }
                    }
                }
			} else {
				if (PendingScreen != null)
				// Wait screen to fade in / fade out
                Thread.Sleep(1000);
			}

            return true;
		}



    }
}
