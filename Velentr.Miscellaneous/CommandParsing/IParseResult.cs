/// <file>
/// Velentr.Miscellaneous\CommandParsing\IParseResult.cs
/// </file>
///
/// <copyright file="IParseResult.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Declares the IParseResult interface.
/// </summary>
using System.Collections.Generic;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// Interface for parse result.
    /// </summary>
    public interface IParseResult
    {
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