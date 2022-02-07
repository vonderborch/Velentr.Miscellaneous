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
using System.ComponentModel;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// A parameter.
    /// </summary>
    internal readonly struct Parameter : IParameter
    {
        /// <summary>
        /// (Immutable) the converter.
        /// </summary>
        private readonly TypeConverter _converter;

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="name">                 The name. </param>
        /// <param name="parameterType">        The type of the value. </param>
        /// <param name="value">                (Optional)
        ///                                     The value. </param>
        /// <param name="wasProvidedByUser">    (Optional)
        ///                                     True if was provided by user, false if not. </param>
        public Parameter(string name, Type parameterType, string value = null, bool wasProvidedByUser = true)
        {
            Name = name;
            ParameterType = parameterType;
            if (!TypeConstants.ValidTypes.Contains(parameterType))
            {
                throw new Exception("Invalid type! Only base types are valid!");
            }
            RawValue = value;
            WasProvidedByUser = wasProvidedByUser;
            ParameterIsValidType = true;
            _converter = TypeDescriptor.GetConverter(ParameterType);
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
        public Type ParameterType { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        ///
        /// <value>
        /// The value.
        /// </value>
        public string RawValue { get; }

        /// <summary>
        /// Gets a value indicating whether the was provided by user.
        /// </summary>
        ///
        /// <value>
        /// True if was provided by user, false if not.
        /// </value>
        public bool WasProvidedByUser { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        ///
        /// <returns>
        /// The value.
        /// </returns>
        ///
        /// <seealso cref="IParameter.GetValue{T}()"/>
        public T GetValue<T>()
        {
            return (T)_converter.ConvertFrom(RawValue);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        ///
        /// <returns>
        /// The value.
        /// </returns>
        ///
        /// <seealso cref="IParameter.GetValue{T}()"/>
        public T Value<T>()
        {
            return (T)_converter.ConvertFrom(RawValue);
        }

        /// <summary>
        /// Validates the parameter type.
        /// </summary>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        private bool ValidateParameterType()
        {
            if (ParameterType == TypeConstants.StringType)
            {
                return true;
            }
            try
            {
                var converter = TypeDescriptor.GetConverter(ParameterType);
                var _ = converter.ConvertFrom(RawValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}