using System;
using BabBot.Scripting;

public class Script : IScript
{
    IHost parent;
    IHost IScript.Parent { set { parent = value; } }
    void IScript.Execute()
    {
		Console.WriteLine("Script: working with host");
        parent.Test();
    }
}

