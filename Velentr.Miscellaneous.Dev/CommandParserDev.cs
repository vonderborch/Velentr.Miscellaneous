/// <file>
/// Velentr.Miscellaneous.Dev\CommandParserDev.cs
/// </file>
///
/// <copyright file="CommandParserDev.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the command parser development class.
/// </summary>
using System;
using System.Collections.Generic;
using Velentr.Miscellaneous.CommandParsing;

namespace Velentr.Miscellaneous.dev
{
    /// <summary>
    /// A command parser development.
    /// </summary>
    internal class CommandParserDev
    {
        /// <summary>
        /// An exit console.
        /// </summary>
        ///
        /// <seealso cref="AbstractCommand"/>
        public class ExitConsole : AbstractCommand
        {
            /// <summary>
            /// Default constructor.
            /// </summary>
            public ExitConsole() : base("exit", "Exits the console", false)
            {
            }

            /// <summary>
            /// Executes the 'command' operation.
            /// </summary>
            ///
            /// <param name="parameters">   Options for controlling the operation. </param>
            /// <param name="args">         The arguments. </param>
            ///
            /// <returns>
            /// True if it succeeds, false if it fails.
            /// </returns>
            ///
            /// <seealso cref="Velentr.Miscellaneous.CommandParsing.AbstractCommand.ExecuteCommand(Dictionary{string,IParameter},Dictionary{string,object})"/>
            public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
            {
                Environment.Exit(0);
                return true;
            }
        }

        /// <summary>
        /// A hello world.
        /// </summary>
        ///
        /// <seealso cref="AbstractCommand"/>
        public class HelloWorld : AbstractCommand
        {
            /// <summary>
            /// Default constructor.
            /// </summary>
            public HelloWorld() : base("hello_world", "Says hello to things")
            {
                AddArgument("item", "what to say hello to", typeof(string), "World", false);
            }

            /// <summary>
            /// Executes the 'command' operation.
            /// </summary>
            ///
            /// <param name="parameters">   Options for controlling the operation. </param>
            /// <param name="args">         The arguments. </param>
            ///
            /// <returns>
            /// True if it succeeds, false if it fails.
            /// </returns>
            ///
            /// <seealso cref="Velentr.Miscellaneous.CommandParsing.AbstractCommand.ExecuteCommand(Dictionary{string,IParameter},Dictionary{string,object})"/>
            public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
            {
                var item = parameters["item"].RawValue;
                Console.WriteLine($"Hello {item}!");

                return true;
            }
        }

        /// <summary>
        /// A complex hello.
        /// </summary>
        ///
        /// <seealso cref="AbstractCommand"/>
        public class ComplexHello : AbstractCommand
        {
            /// <summary>
            /// Default constructor.
            /// </summary>
            public ComplexHello() : base("complex_hello", "Says hello to more things")
            {
                AddArgument("item1", "what to say hello to", typeof(string), "World", true);
                AddArgument("item2", "what else to say hello to", typeof(string), "Moon", false);
                AddArgument("item3", "what else to say hello to, but only if this parameter is actually specified", typeof(string), "", false, true);
            }

            /// <summary>
            /// Executes the 'command' operation.
            /// </summary>
            ///
            /// <param name="parameters">   Options for controlling the operation. </param>
            /// <param name="args">         The arguments. </param>
            ///
            /// <returns>
            /// True if it succeeds, false if it fails.
            /// </returns>
            ///
            /// <seealso cref="Velentr.Miscellaneous.CommandParsing.AbstractCommand.ExecuteCommand(Dictionary{string,IParameter},Dictionary{string,object})"/>
            public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
            {
                var item1 = parameters["item1"].Value<string>();
                var item2 = parameters["item2"].GetValue<string>();
                Console.WriteLine($"Hello {item1}!");
                Console.WriteLine($"Hello {item2}!");

                var item3 = (string)parameters["item3"].RawValue;
                if (!string.IsNullOrWhiteSpace(item3))
                {
                    Console.WriteLine($"Hello {item3}!");
                }

                return true;
            }
        }

        /// <summary>
        /// Main entry-point for this application.
        /// </summary>
        ///
        /// <param name="args"> An array of command-line argument strings. </param>
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