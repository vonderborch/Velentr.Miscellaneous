/// <file>
/// Velentr.Miscellaneous\CommandParsing\Argument.cs
/// </file>
///
/// <copyright file="IArgument.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the argument interface.
/// </summary>
using System;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// An interface for a argument.
    /// </summary>
    public interface IArgument
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
        /// Gets the description.
        /// </summary>
        ///
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether this object is required.
        /// </summary>
        ///
        /// <value>
        /// True if this object is required, false if not.
        /// </value>
        public bool IsRequired { get; }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        ///
        /// <value>
        /// The type of the value.
        /// </value>
        public Type ValueType { get; }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        ///
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue { get; }

        /// <summary>
        /// Gets as command parameter.
        /// </summary>
        ///
        /// <value>
        /// as command parameter.
        /// </value>
        public string AsCommandParameter { get; }

        /// <summary>
        /// Gets information describing as parameter.
        /// </summary>
        ///
        /// <value>
        /// Information describing as parameter.
        /// </value>
        public string AsParameterDescription { get; }
    }
}