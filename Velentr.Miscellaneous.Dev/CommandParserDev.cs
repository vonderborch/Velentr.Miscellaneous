using System;
using System.Collections.Generic;
using Velentr.Miscellaneous.CommandParsing;

namespace Velentr.Miscellaneous.dev
{
    internal class CommandParserDev
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

        public class HelloWorld : AbstractCommand
        {
            public HelloWorld() : base("hello_world", "Says hello to things")
            {
                AddArgument("item", "what to say hello to", typeof(string), "World", false);
            }

            public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
            {
                var item = (string)parameters["item"].Value;
                Console.WriteLine($"Hello {item}!");

                return true;
            }
        }

        public class ComplexHello : AbstractCommand
        {
            public ComplexHello() : base("complex_hello", "Says hello to more things")
            {
                AddArgument("item1", "what to say hello to", typeof(string), "World", true);
                AddArgument("item2", "what else to say hello to", typeof(string), "Moon", false);
                AddArgument("item3", "what else to say hello to, but only if this parameter is actually specified", typeof(string), "", false, true);
            }

            public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
            {
                var item1 = (string)parameters["item1"].Value;
                var item2 = (string)parameters["item2"].Value;
                Console.WriteLine($"Hello {item1}!");
                Console.WriteLine($"Hello {item2}!");

                var item3 = (string)parameters["item3"].Value;
                if (!string.IsNullOrWhiteSpace(item3))
                {
                    Console.WriteLine($"Hello {item3}!");
                }

                return true;
            }
        }

        public void Main(string[] args)
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