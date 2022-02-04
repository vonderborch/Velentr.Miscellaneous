using System;
using System.Collections.Generic;
using Velentr.Miscellaneous.CommandParsing;

namespace Velentr.Miscellaneous.Dev
{
    public class ExitConsole : AbstractCommand
    {
        public ExitConsole() : base("exit", "Exits the console", false)
        {
        }

        public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
        {
            Environment.Exit(0);
            return true;
        }
    }
}