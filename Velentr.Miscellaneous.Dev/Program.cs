/// <file>
/// Velentr.Miscellaneous.Dev\Program.cs
/// </file>
///
/// <copyright file="Program.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the program class.
/// </summary>
namespace Velentr.Miscellaneous.dev
{
    /// <summary>
    /// A program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main entry-point for this application.
        /// </summary>
        ///
        /// <param name="args"> An array of command-line argument strings. </param>
        private static void Main(string[] args)
        {
            var parser = new CommandParserDev();
            parser.Main(args);
        }
    }
}