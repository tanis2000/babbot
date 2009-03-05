using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        void Execute();
    }

    public class Host : IHost
    {
        public static void Go()
        {
            Host host = new Host();
            host.Start();
        }


        void Start()
        {
            IScript script = Load("Scripts/script.cs");
            script.Parent = this;
            script.Execute();
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


    }
}
