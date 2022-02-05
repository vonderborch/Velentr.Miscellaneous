using System;
using System.Collections.Generic;
using Velentr.Miscellaneous.CommandParsing;

namespace Velentr.Miscellaneous.dev
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new CommandParserDev();
            parser.Main(args);
        }
    }
}