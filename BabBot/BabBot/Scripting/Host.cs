using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        /*
        public static void Go()
        {
            Host host = new Host();
            host.Start();
        }
        */

        public void Start()
        {
            script = Load("Scripts/script.cs");
            script.Parent = this;
            script.Player = new PlayerWrapper(ProcessManager.Player);
            script.Init();
        }

        IScript Load(string script)
        {
            CSScript.ShareHostRefAssemblies = true;

            AsmHelper helper = new AsmHelper(CSScript.Load(Path.GetFullPath(script), null, true));
            return (IScript)helper.CreateObject("Script");
        }

        public void Test()
        {
            Console.WriteLine("test");
        }

        public void Update()
        {
            script.Update();
        }


    }
}
