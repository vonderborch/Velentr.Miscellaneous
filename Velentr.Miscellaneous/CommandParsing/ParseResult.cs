/// <file>
/// Velentr.Miscellaneous\CommandParsing\ParseResult.cs
/// </file>
///
/// <copyright file="ParseResult.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the parse result class.
/// </summary>
using System.Collections.Generic;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// Encapsulates the result of a parse.
    /// </summary>
    internal struct ParseResult : IParseResult
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="command">      The command. </param>
        /// <param name="parameters">   The parameters. </param>
        public ParseResult(AbstractCommand command, Dictionary<string, IParameter> parameters)
        {
            Command = command;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        ///
        /// <value>
        /// The command.
        /// </value>
        public AbstractCommand Command { get; }

        /// <summary>
        /// Gets options for controlling the operation.
        /// </summary>
        ///
        /// <value>
        /// The parameters.
        /// </value>
        public Dictionary<string, IParameter> Parameters { get; }
    }
}