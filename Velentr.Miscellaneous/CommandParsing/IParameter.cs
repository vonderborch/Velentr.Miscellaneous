/// <file>
/// Velentr.Miscellaneous\CommandParsing\Parameter.cs
/// </file>
///
/// <copyright file="Parameter.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the parameter interface.
/// </summary>
using System;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// A parameter.
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        ///
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the parameter is valid type.
        /// </summary>
        ///
        /// <value>
        /// True if parameter is valid type, false if not.
        /// </value>
        public bool ParameterIsValidType { get; }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        ///
        /// <value>
        /// The type of the value.
        /// </value>
        public Type ValueType { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        ///
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; }

        /// <summary>
        /// Gets a value indicating whether the was provided by user.
        /// </summary>
        ///
        /// <value>
        /// True if was provided by user, false if not.
        /// </value>
        public bool WasProvidedByUser { get; }
    }
}