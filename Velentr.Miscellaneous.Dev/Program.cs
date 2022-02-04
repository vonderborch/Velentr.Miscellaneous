using System;
using System.Collections.Generic;
using Velentr.Miscellaneous.CommandParsing;

namespace Velentr.Miscellaneous.dev
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new CommandParser();

            while (true)
            {
                var command = Console.ReadLine();

                var arguments = new Dictionary<string, object>();
                var cmd = parser.ParseCommand(command);

                cmd.Command.ExecuteCommand(cmd.Parameters, arguments);
            }
        }
    }
}