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
using System.IO;
using System.Drawing;
using System.Xml.Serialization;

namespace BabBot
{
    [XmlRoot("config")]
    public class Config
    {
        [XmlAttribute("version")]
        public int Version = 0;

        [XmlAttribute("debug")]
        public bool DebugMode = false;

#if DEBUG
        // This parameter is for developers internal use
        // 0-7 range reserved for ivp4
        [XmlAttribute("test")]
        public int Test = -1;

        [XmlIgnore]
        public bool IsTest
        {
            get { return (Test >= 0); }
        }
#endif
        // Not used
        // public string InteractKey = "{SHIFTD}ì{SHIFTU}";

        // Default directory with talents template
        [XmlAttribute("profiles")]
        public string ProfilesDir = "Profiles";
        
        [XmlElement("account")]
        public LoginInfo Account = new LoginInfo();
        public string Character = "";

        [XmlElement("wow_pos")]
        public WinPos WowPos;

        [XmlElement("bot_pos")]
        public WinPos BotPos;

        [XmlElement("wow_info")]
        public WoWInfo WoWInfo = new WoWInfo();

        [XmlElement("log_parameters")]
        public LogParams LogParams = new LogParams();

        // Bot Customization
        [XmlElement("custom")]
        public Custom CustomParams = new Custom();
    }

    [Serializable]
    public class LoginInfo
    {
        [XmlAttribute("username")]
        public string LoginUsername = "";
        [XmlAttribute("password")]
        public string LoginPassword = "";
        [XmlAttribute("realm_name")]
        public string Realm = "";
        [XmlAttribute("realm_location")]
        public string RealmLocation = "";
        [XmlAttribute("game_type")]
        public string GameType = "";
        [XmlAttribute("reconnect")]
        public bool ReConnect = false;
        [XmlAttribute("relogin")]
        public bool ReStart = false;

        // Private fields not included into serializer
        private string AutoLoginPassword = "";

        public void EncryptPassword(string PlainText)
        {
            AutoLoginPassword = PlainText;
            LoginPassword = BabBot.Common.Security.Encrypt(PlainText);
        }

        public void DecryptPassword(string ComplexText)
        {
            LoginPassword = ComplexText;
            AutoLoginPassword = BabBot.Common.Security.Decrypt(ComplexText);
        }

        public string getAutoLoginPassword()
        {
            return AutoLoginPassword;
        }
    }

    [Serializable]
    public class WinPos
    {
        [XmlIgnore]
        public Rectangle Pos;

        [XmlAttribute("x")]
        public int X
        {
            get { return Pos.X; }
            set { Pos.X = value; }
        }

        [XmlAttribute("y")]
        public int Y
        {
            get { return Pos.Y; }
            set { Pos.Y = value; }
        }

        [XmlAttribute("h")]
        public int Height
        {
            get { return Pos.Height; }
            set { Pos.Height = value; }
        }

        [XmlAttribute("w")]
        public int W
        {
            get { return Pos.Width; }
            set { Pos.Width = value; }
        }

        [XmlAttribute("top_most")]
		public bool TopMost = false;
        [XmlAttribute("opacity")]
		public double Opacity = 1.0;
		
        public WinPos() { }

        public WinPos(Rectangle r)
        {
            Pos = r;
        }

        public WinPos(Point p, Size s, bool top, double opacity)
        {
            Pos = new Rectangle(p, s);
			TopMost = top;
			Opacity = opacity;
        }
    }

    [Serializable]
    public class WoWInfo
    {
        // Start up info
        [XmlAttribute("no_sound")]
        public bool NoSound = false;
        [XmlAttribute("window_mode")]
        public bool Windowed = true;
        [XmlAttribute("min_resize")]
        public bool Resize = false;

        [XmlAttribute("guest_username")]
        public string GuestUsername = "Guest";
        [XmlAttribute("guest_pwd")]
        public string GuestPassword = "";
        [XmlAttribute("exe_path")]
        public string ExePath = "";
        [XmlAttribute("version")]
        public string Version;
        [XmlAttribute("auto-login")]
        public bool AutoLogin = false;
        // Bot refresh rate in msec
        [XmlAttribute("refresh_time")]
        public int RefreshTime = 250;
        // Bot idle sleep time (if game not started)
        [XmlAttribute("idle_sleep_time")]
        public int IdleSleepTime = 5000;

        public WoWInfo() { }
    }

    [Serializable]
    public class LogParams
    {
        [XmlAttribute("display_logs")]
        public bool DisplayLogs = true;
        [XmlAttribute("dir")]
        public string Dir = "Log";

        public LogParams() { }
    }

    [Serializable]
    public class Custom
    {
        [XmlAttribute("lua_callback")]
        public string LuaCallback;

        [XmlAttribute("win_title")]
        public string WinTitle = "";

        public Custom() { }
    }
}