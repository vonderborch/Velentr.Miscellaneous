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
            ParameterIsValidType = true;
            ParameterIsValidType = ValidateParameterType();
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

        /// <summary>
        /// Validates the parameter type.
        /// </summary>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        private bool ValidateParameterType()
        {
            try
            {
                if (ValueType == TypeConstants.IntType)
                {
                    var _ = (int)Value;
                }
                else if (ValueType == TypeConstants.LongType)
                {
                    var _ = (long)Value;
                }
                else if (ValueType == TypeConstants.ShortType)
                {
                    var _ = (short)Value;
                }
                else if (ValueType == TypeConstants.UnsignedIntType)
                {
                    var _ = (uint)Value;
                }
                else if (ValueType == TypeConstants.UnsignedLongType)
                {
                    var _ = (ulong)Value;
                }
                else if (ValueType == TypeConstants.UnsignedShortType)
                {
                    var _ = (ushort)Value;
                }
                else if (ValueType == TypeConstants.ByteType)
                {
                    var _ = (byte)Value;
                }
                else if (ValueType == TypeConstants.BoolType)
                {
                    var _ = (bool)Value;
                }
                else if (ValueType == TypeConstants.StringType)
                {
                    var _ = Value.ToString();
                }
                else if (ValueType == TypeConstants.FloatType)
                {
                    var _ = (float)Value;
                }
                else if (ValueType == TypeConstants.DoubleType)
                {
                    var _ = (double)Value;
                }
                else if (ValueType == TypeConstants.DecimalType)
                {
                    var _ = (decimal)Value;
                }
                else if (ValueType == TypeConstants.SByteType)
                {
                    var _ = (sbyte)Value;
                }
                else if (ValueType == TypeConstants.CharType)
                {
                    var _ = (char)Value;
                }
                else if (ValueType == TypeConstants.NIntType)
                {
                    var _ = (nint)Value;
                }
                else if (ValueType == TypeConstants.NUnsignedIntType)
                {
                    var _ = (nuint)Value;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}