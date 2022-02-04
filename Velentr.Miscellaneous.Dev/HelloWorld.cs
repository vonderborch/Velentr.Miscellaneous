using System;
using System.Collections.Generic;
using Velentr.Miscellaneous.CommandParsing;

namespace Velentr.Miscellaneous.Dev
{
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
}