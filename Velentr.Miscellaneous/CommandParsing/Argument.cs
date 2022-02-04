/// <file>
/// Velentr.Miscellaneous\CommandParsing\Argument.cs
/// </file>
///
/// <copyright file="Argument.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the argument class.
/// </summary>
using System;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// An argument.
    /// </summary>
    internal readonly struct Argument : IArgument
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="name">         The name. </param>
        /// <param name="description">  The description. </param>
        /// <param name="valueType">    The type of the value. </param>
        /// <param name="defaultValue"> (Optional)
        ///                             The default value. </param>
        /// <param name="isRequired">   (Optional)
        ///                             True if this object is required, false if not. </param>
        public Argument(string name, string description, Type valueType, object defaultValue = null, bool isRequired = false)
        {
            Name = name.ToLowerInvariant();
            Description = description;
            ValueType = valueType;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
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
        public string AsCommandParameter => $"{(IsRequired ? "<" : "[")}{Name}{(IsRequired ? ">" : "]")}";

        /// <summary>
        /// Gets information describing as parameter.
        /// </summary>
        ///
        /// <value>
        /// Information describing as parameter.
        /// </value>
        public string AsParameterDescription => $"  {Name} - ({(IsRequired == false ? "Optional" : "Required")}, {ValueType.ToString().Replace("System.", "")}) {Description}";
    }
}