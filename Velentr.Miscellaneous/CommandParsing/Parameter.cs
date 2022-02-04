/// <file>
/// Velentr.Miscellaneous\CommandParsing\Parameter.cs
/// </file>
///
/// <copyright file="Parameter.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the parameter class.
/// </summary>
using System;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// A parameter.
    /// </summary>
    internal struct Parameter : IParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="name">                 The name. </param>
        /// <param name="valueType">            The type of the value. </param>
        /// <param name="value">                (Optional)
        ///                                     The value. </param>
        /// <param name="wasProvidedByUser">    (Optional)
        ///                                     True if was provided by user, false if not. </param>
        public Parameter(string name, Type valueType, object value = null, bool wasProvidedByUser = true)
        {
            Name = name;
            ValueType = valueType;
            Value = value;
            WasProvidedByUser = wasProvidedByUser;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        ///
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

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