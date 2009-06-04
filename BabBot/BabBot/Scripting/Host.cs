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
using BabBot.Manager;
using CSScriptLibrary;

namespace BabBot.Scripting
{
    public interface IHost
    {
        void Test();
    }

    public interface IScript
    {
        IHost Parent { set; }
        IPlayerWrapper Player { set; }
        void Init();
        void Update();
    }

    public class Host : IHost
    {
        private IScript script;

        #region IHost Members

        public void Test()
        {
            Console.WriteLine("test");
        }

        #endregion

        public void Start()
        {
            //script = Load("Scripts/script.cs");
            script = Load("Scripts/paladin.cs");
            script.Parent = this;
            script.Player = new PlayerWrapper(ProcessManager.Player);
            script.Init();
        }

        private IScript Load(string script)
        {
            CSScript.ShareHostRefAssemblies = true;

            var helper = new AsmHelper(CSScript.Load(Path.GetFullPath(script), null, true));
            //return (IScript)helper.CreateObject("BabBot.Scripts.Script");
            return (IScript) helper.CreateObject("BabBot.Scripts.Paladin");
        }

        public void Update()
        {
            script.Update();
        }
    }
}