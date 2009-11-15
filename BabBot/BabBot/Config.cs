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
using System.Drawing;

namespace BabBot
{
    [Serializable]
    public class Config
    {
        public bool DebugMode;
        public string GuestPassword = "";
        public string GuestUsername = "Guest";
        public string InteractKey = "{SHIFTD}ì{SHIFTU}";
        public string WowExePath = "";
        public string LogPath = Environment.CurrentDirectory+@"\Log";
        public bool AutoLogin = false;
        public LoginInfo Account = new LoginInfo();
        public bool NoSound = false;
        public bool Windowed = false;
        public bool Resize = false;
        public string Character = "";
        public WinPos WowPos;
        public WinPos BotPos;
    }

    [Serializable]
    public class LoginInfo
    {
        public string LoginUsername = "";
        public string LoginPassword = "";
        public string Realm = "";
        public string RealmLocation = "";
        public string GameType = "";

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
        public Rectangle Pos;
		public bool TopMost = false;
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
}