/// <file>
/// Velentr.Miscellaneous\CommandParsing\AbstractCommand.cs
/// </file>
///
/// <copyright file="AbstractCommand.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the abstract command class.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Velentr.Collections.Collections;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// An abstract command.
    /// </summary>
    public abstract class AbstractCommand
    {
        /// <summary>
        /// The required arguments cache.
        /// </summary>
        private int _requiredArgumentsCache = int.MinValue;

        /// <summary>
        /// The example command cache.
        /// </summary>
        private string _exampleCommandCache = "";

        /// <summary>
        /// The arguments.
        /// </summary>
        internal OrderedDictionary<string, IArgument> Arguments;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        ///
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        ///
        /// <value>
        /// The name of the command.
        /// </value>
        public string CommandName { get; internal set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        ///
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this object is hidden.
        /// </summary>
        ///
        /// <value>
        /// True if this object is hidden, false if not.
        /// </value>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the automatic register command.
        /// </summary>
        ///
        /// <value>
        /// True if automatic register command, false if not.
        /// </value>
        public bool AutoRegisterCommand { get; set; }

        /// <summary>
        /// Gets or sets the parser.
        /// </summary>
        ///
        /// <value>
        /// The parser.
        /// </value>
        public CommandParser Parser { get; internal set; }

        /// <summary>
        /// Specialized constructor for use only by derived class.
        /// </summary>
        ///
        /// <param name="name">                 The name. </param>
        /// <param name="description">          The description. </param>
        /// <param name="isHidden">             (Optional) True if is hidden, false if not. </param>
        /// <param name="numArguments">         (Optional) Number of arguments. </param>
        /// <param name="autoRegisterCommand">  (Optional) True to automatically register command. </param>
        protected AbstractCommand(string name, string description, bool isHidden = false, int numArguments = 2, bool autoRegisterCommand = true)
        {
            Name = name;
            Description = description;
            IsHidden = isHidden;
            AutoRegisterCommand = autoRegisterCommand;
            Arguments = new OrderedDictionary<string, IArgument>(numArguments);
        }

        /// <summary>
        /// Adds an argument.
        /// </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="name">         The name. </param>
        /// <param name="description">  The description. </param>
        /// <param name="defaultValue"> (Optional) The default value. </param>
        /// <param name="isRequired">   (Optional) True if is required, false if not. </param>
        /// <param name="isHidden">     (Optional) True if is hidden, false if not. </param>
        public void AddArgument<T>(string name, string description, object defaultValue = null, bool isRequired = false,
            bool isHidden = false)
        {
            AddArgument(name, description, typeof(T), defaultValue, isRequired, isHidden);
        }

        /// <summary>
        /// Adds an argument.
        /// </summary>
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="name">         The name. </param>
        /// <param name="description">  The description. </param>
        /// <param name="valueType">    Type of the value. </param>
        /// <param name="defaultValue"> (Optional) The default value. </param>
        /// <param name="isRequired">   (Optional) True if is required, false if not. </param>
        /// <param name="isHidden">     (Optional) True if is hidden, false if not. </param>
        public void AddArgument(string name, string description, Type valueType, object defaultValue = null, bool isRequired = false, bool isHidden = false)
        {
            // hidden arguments cannot be required...
            if (isHidden && isRequired)
            {
                throw new ArgumentException("Hidden arguments cannot be required!");
            }

            // find the index to insert the argument into...
            var index = (int)Arguments.Count;
            // if this is _not_ a hidden arg, insert it _before_ the last hidden arg and _before_ the last optional arg
            if (!isHidden)
            {
                var hiddenArguments = Arguments.Where(x => x.Value.IsHidden).ToList();
                var optionalArguments = Arguments.Where(x => !x.Value.IsRequired && !x.Value.IsHidden).ToList();
                if (hiddenArguments.Count > 0)
                {
                    index = Arguments.GetIndexForKey(hiddenArguments[(int)hiddenArguments.Count - 1].Key);
                }
                if (optionalArguments.Count > 0 && isRequired)
                {
                    index = Arguments.GetIndexForKey(optionalArguments[(int)optionalArguments.Count - 1].Key);
                }
            }

            // Add the new item...
            if (defaultValue == null && valueType.IsValueType)
            {
                defaultValue = Activator.CreateInstance(valueType);
            }
            Arguments.AddItem(name, new Argument(name, description, valueType, defaultValue, isRequired, isHidden), index);
            _exampleCommandCache = "";
            _requiredArgumentsCache = int.MinValue;
        }

        /// <summary>
        /// Gets required arguments count.
        /// </summary>
        ///
        /// <returns>
        /// The required arguments count.
        /// </returns>
        public int GetRequiredArgumentsCount()
        {
            if (_requiredArgumentsCache < 0)
            {
                _requiredArgumentsCache = Arguments.Count(x => x.Value.IsRequired);
            }

            return _requiredArgumentsCache;
        }

        /// <summary>
        /// Gets example command.
        /// </summary>
        ///
        /// <returns>
        /// The example command.
        /// </returns>
        public string GetExampleCommand()
        {
            if (string.IsNullOrWhiteSpace(_exampleCommandCache))
            {
                var str = new StringBuilder();
                str.Append($"{CommandName}");

                for (var i = 0; i < Arguments.Count; i++)
                {
                    if (!Arguments[i].IsHidden)
                    {
                        str.Append($" {Arguments[i].AsCommandParameter}");
                    }
                }

                _exampleCommandCache = str.ToString();
            }

            return _exampleCommandCache;
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
        public abstract bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args);

        /// <summary>
        /// Gets parameter value if exists as string.
        /// </summary>
        ///
        /// <param name="parameter">    The parameter. </param>
        /// <param name="parameters">   Options for controlling the operation. </param>
        ///
        /// <returns>
        /// The parameter value if exists as string.
        /// </returns>
        protected string GetParameterValueIfExistsAsString(string parameter, Dictionary<string, IParameter> parameters)
        {
            if (parameters.TryGetValue(parameter, out var value))
            {
                return value.Value<string>();
            }

            return string.Empty;
        }

        /// <summary>
        /// Removes the argument described by name.
        /// </summary>
        ///
        /// <param name="name"> The name. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        public bool RemoveArgument(string name)
        {
            if (Arguments.Exists(name))
            {
                Arguments.RemoveItem(name);
            }

            return false;
        }
    }
}