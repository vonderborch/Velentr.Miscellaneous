/// <file>
/// Velentr.Miscellaneous\CommandParsing\CommandParser.cs
/// </file>
///
/// <copyright file="CommandParser.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the command parser class.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using Velentr.Collections.Collections;
using AppDomain = System.AppDomain;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// A command parser.
    /// </summary>
    public class CommandParser
    {
        /// <summary>
        /// (Immutable) the aliases.
        /// </summary>
        private readonly Dictionary<string, string> _aliases;

        /// <summary>
        /// (Immutable)
        /// List of types of the command bases.
        /// </summary>
        private readonly List<Type> _commandBaseTypes;

        /// <summary>
        /// Gets the command prefix.
        /// </summary>
        ///
        /// <value>
        /// The command prefix.
        /// </value>
        public string CommandPrefix { get; }

        /// <summary>
        /// The commands.
        /// </summary>
        public OrderedDictionary<string, AbstractCommand> Commands = new OrderedDictionary<string, AbstractCommand>();

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="commandPrefix">                        (Optional)
        ///                                                     The command prefix. </param>
        /// <param name="autoSearchForCommands">                (Optional) True to automatically search
        ///                                                     for commands. </param>
        /// <param name="autoRegisterDefaultHelpCommand">       (Optional) True to automatically register
        ///                                                     default help command. </param>
        /// <param name="defaultHelpCommandPrintsToConsole">    (Optional) True to default help command
        ///                                                     prints to console. </param>
        /// <param name="aliases">                              (Optional)
        ///                                                     (Immutable) the aliases. </param>
        /// <param name="baseTypes">                            A variable-length parameters list
        ///                                                     containing base types. </param>
        public CommandParser(string commandPrefix = "", bool autoSearchForCommands = true, bool autoRegisterDefaultHelpCommand = true, bool defaultHelpCommandPrintsToConsole = true, Dictionary<string, string> aliases = null, params Type[] baseTypes)
        {
            CommandPrefix = commandPrefix;

            _commandBaseTypes = new List<Type>(baseTypes);
            _commandBaseTypes.Add(typeof(AbstractCommand));

            if (autoSearchForCommands)
            {
                SearchForCommands();
            }

            if (autoRegisterDefaultHelpCommand)
            {
                RegisterHelpCommand(new DefaultHelpCommand(defaultHelpCommandPrintsToConsole));
            }

            _aliases = new Dictionary<string, string>();
            if (aliases != null)
            {
                foreach (var alias in aliases)
                {
                    var actualCommandName = $"{CommandPrefix}{alias.Value}";
                    if (!Commands.Exists(actualCommandName))
                    {
                        throw new ArgumentException($"The alias [{alias.Key}] points to an invalid command [{alias.Value}]!");
                    }
                    _aliases.Add(alias.Key, actualCommandName);
                }
            }
        }

        /// <summary>
        /// Searches for the first commands.
        /// </summary>
        private void SearchForCommands()
        {
            var classes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(
                    x => !x.IsInterface
                         && !x.IsAbstract
                         && _commandBaseTypes.Contains(x.BaseType)
                ).ToList();

            for (var i = 0; i < classes.Count; i++)
            {
                try
                {
                    var instance = (AbstractCommand)Activator.CreateInstance(classes[i]);
                    if (instance.AutoRegisterCommand)
                    {
                        RegisterCommand(instance);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Registers the help command described by command.
        /// </summary>
        ///
        /// <param name="command">  The command. </param>
        public void RegisterHelpCommand(AbstractCommand command)
        {
            command.Name = "help";
            RegisterCommand(command);
        }

        /// <summary>
        /// Registers the command described by command.
        /// </summary>
        ///
        /// <param name="command">  The command. </param>
        public void RegisterCommand(AbstractCommand command)
        {
            command.Parser = this;
            command.CommandName = $"{CommandPrefix}{command.Name}";
            Commands.AddItem(command.CommandName, command);
        }

        /// <summary>
        /// Gets the 'help' command.
        /// </summary>
        ///
        /// <value>
        /// The 'help' command.
        /// </value>
        private AbstractCommand HelpCommand
        {
            get
            {
                if (Commands.Exists($"{CommandPrefix}help"))
                {
                    return Commands[$"{CommandPrefix}help"];
                }

                return null;
            }
        }

        /// <summary>
        /// Parse command.
        /// </summary>
        ///
        /// <param name="messageToParse">   The message to parse. </param>
        ///
        /// <returns>
        /// A Tuple.
        /// </returns>
        public virtual IParseResult ParseCommand(string messageToParse)
        {
            //// return early if there's nothing to attempt to parse...
            if (!messageToParse.StartsWith(CommandPrefix))
            {
                return new ParseResult(null, null);
            }

            //// Get the Command we'll be running and figure out the argument parts
            AbstractCommand command = null;
            var output = ConvertStringToCommandAndArguments(messageToParse);
            var cmdName = output.Item2;
            // Check if the command is an alias...
            if (_aliases.TryGetValue(cmdName, out var tmpName))
            {
                cmdName = tmpName;
            }

            // Check if the command exists and setup some default stuffs...
            var commandExists = Commands.Exists(cmdName);
            if (commandExists)
            {
                command = Commands[cmdName];
            }
            var helpParameters = new Dictionary<string, IParameter>() { { "command", new Parameter("command", typeof(string), cmdName) } };
            var executeHelpMessageOnFailure = false;
            var commandParameters = new Dictionary<string, IParameter>();

            //// If the message is _just_ the CommandPrefix (assuming one exists), return the help message
            if (!string.IsNullOrWhiteSpace(CommandPrefix) && string.Equals(messageToParse, CommandPrefix, StringComparison.InvariantCultureIgnoreCase))
            {
                return AddOptionalParameters(HelpCommand, new Dictionary<string, IParameter>());
            }

            //// Check for basic failure cases...
            // Failure Case 1 - Invalid Parameters, Command Exists
            // Failure Case 2 - Invalid Parameters, Command Does Not Exist
            if (!output.Item1)
            {
                executeHelpMessageOnFailure = true;
                // Failure Case 1 - Invalid Parameters, Command Exists
                if (commandExists)
                {
                    helpParameters.Add("failure_case", new Parameter("failure_case", typeof(string), "Error #1"));
                    helpParameters.Add("failure_exception", new Parameter("failure_exception", typeof(string), $"Failed to parse the parameters for the command [{cmdName}]!"));
                }
                // Failure Case 2 - Invalid Parameters, Command Does Not Exist
                else
                {
                    helpParameters.Add("failure_case", new Parameter("failure_case", typeof(string), "Error #2"));
                    helpParameters.Add("failure_exception", new Parameter("failure_exception", typeof(string), $"Command [{cmdName}] does not exist!"));
                    helpParameters.Add("command_does_not_exist", new Parameter("command_does_not_exist", typeof(string), "yes"));
                }
            }
            // Failure Case 3 - Valid Parameters, Command Does Not Exist
            else if (!commandExists)
            {
                executeHelpMessageOnFailure = true;
                helpParameters.Add("failure_case", new Parameter("failure_case", typeof(string), "Error #3"));
                helpParameters.Add("failure_exception", new Parameter("failure_exception", typeof(string), $"Command [{cmdName}] does not exist!"));
                helpParameters.Add("command_does_not_exist", new Parameter("command_does_not_exist", typeof(string), "yes"));
            }
            // Failure Case 4 - We have more arguments than are possible for the command!
            else if (command.Arguments.Count < output.Item3.Count)
            {
                executeHelpMessageOnFailure = true;
                helpParameters.Add("failure_case", new Parameter("failure_case", typeof(string), "Error #4"));
                helpParameters.Add("failure_exception", new Parameter("failure_exception", typeof(string), $"Number of arguments exceeds the amount available for the command [{cmdName}]!"));
            }

            if (!executeHelpMessageOnFailure)
            {
                //// Next, let's create the arguments dict and make sure we have all required args
                var requiredArgs = 0;
                var lastParameterWasNamed = false;
                for (var i = 0; i < output.Item3.Count; i++)
                {
                    var name = output.Item3[i].Item1;
                    var value = output.Item3[i].Item2;

                    var actualName = name;
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        if (lastParameterWasNamed)
                        {
                            // Failure Case 5 - No unnamed parameters after a named parameter
                            executeHelpMessageOnFailure = true;
                            helpParameters.Add("failure_case", new Parameter("failure_case", typeof(string), "Error #5"));
                            helpParameters.Add("failure_exception", new Parameter("failure_exception", typeof(string), $"No unnamed parameters may be specified after a named parameter has already been specified!"));
                        }

                        actualName = command.Arguments[i].Name;
                    }
                    else
                    {
                        lastParameterWasNamed = true;
                    }

                    if (!executeHelpMessageOnFailure)
                    {
                        actualName = actualName.Trim();
                        // Failure Case 6 - An argument with the name specified doesn't exist
                        if (!command.Arguments.Exists(actualName))
                        {
                            executeHelpMessageOnFailure = true;
                            helpParameters.Add("failure_case", new Parameter("failure_case", typeof(string), "Error #6"));
                            helpParameters.Add("failure_exception", new Parameter("failure_exception", typeof(string), $"The argument with the name [{actualName}] isn't a valid argument for the command [{cmdName}]!"));
                        }
                        else
                        {
                            actualName = actualName.ToLowerInvariant();
                            var arg = command.Arguments[actualName];
                            if (string.IsNullOrWhiteSpace(value) && arg.ValueType == TypeConstants.BoolType)
                            {
                                value = (!((bool)arg.DefaultValue)).ToString();
                            }

                            var parameter = new Parameter(actualName, arg.ValueType, value);

                            // Failure Case 7 - Invalid Parameter Type
                            if (!parameter.ParameterIsValidType)
                            {
                                executeHelpMessageOnFailure = true;
                                helpParameters.Add("failure_case", new Parameter("failure_case", typeof(string), "Error #7"));
                                helpParameters.Add("failure_exception", new Parameter("failure_exception", typeof(string), $"The argument with the name [{actualName}] isn't a valid type for the command [{cmdName}]!"));
                            }
                            else
                            {
                                if (arg.IsRequired)
                                {
                                    requiredArgs++;
                                }
                                commandParameters.Add(actualName, parameter);
                            }
                        }
                    }
                }

                // Failure Case 8 - Not all required arguments have been specified
                if (!executeHelpMessageOnFailure && requiredArgs != command.GetRequiredArgumentsCount())
                {
                    executeHelpMessageOnFailure = true;
                    helpParameters.Add("failure_case", new Parameter("failure_case", typeof(string), "Error #8"));
                    helpParameters.Add("failure_exception", new Parameter("failure_exception", typeof(string), $"Not all required arguments have been specified for the command [{cmdName}]!"));
                }
            }

            // Execute the Help Command, or execute the actual command
            if (executeHelpMessageOnFailure)
            {
                helpParameters.Add("failed_command", new Parameter("failed_command", typeof(string), messageToParse));
                return AddOptionalParameters(HelpCommand, helpParameters);
            }
            else
            {
                return AddOptionalParameters(command, commandParameters);
            }
        }

        /// <summary>
        /// Adds an optional parameters.
        /// </summary>
        ///
        /// <param name="command">      The command. </param>
        /// <param name="parameters">   Options for controlling the operation. </param>
        ///
        /// <returns>
        /// A Tuple.
        /// </returns>
        ///
        /// ### <param name="args"> The arguments. </param>
        private IParseResult AddOptionalParameters(AbstractCommand command, Dictionary<string, IParameter> parameters)
        {
            var argumentsToDefault = command.Arguments.Where(x => !x.Value.IsRequired && !parameters.ContainsKey(x.Key));
            foreach (var argument in argumentsToDefault)
            {
                parameters.Add(argument.Key, new Parameter(argument.Key, argument.Value.ValueType, argument.Value.DefaultValue.ToString(), false));
            }

            return new ParseResult(command, parameters);
        }

        /// <summary>
        /// Convert string to command arguments.
        /// </summary>
        ///
        /// <param name="messageToParse">   The message to parse. </param>
        ///
        /// <returns>
        /// The string converted to command arguments.
        /// </returns>
        private (bool, string, List<(string, string)>) ConvertStringToCommandAndArguments(string messageToParse)
        {
            var firstSpaceIndex = messageToParse.IndexOf(' ');
            var command = messageToParse.Substring(0, firstSpaceIndex == -1 ? messageToParse.Length : firstSpaceIndex);
            var arguments = new List<(string, string)>();

            if (firstSpaceIndex != -1)
            {
                var argToParse = $"{messageToParse.Substring(command.Length).Trim()} ";
                var remainingLength = argToParse.Length;
                var currentParameter = string.Empty;
                while (!string.IsNullOrWhiteSpace(argToParse))
                {
                    // hello_world test -named test2 -othername "this is a test"
                    var nextIndex = 0;
                    // if the character is a quote, we get the next corresponding quote and treat it as a single string
                    if (argToParse[0] == '\'' || argToParse[0] == '"')
                    {
                        var c = argToParse[0];
                        nextIndex = argToParse.IndexOf(c, 1);

                        // we need a matching quote!
                        if (nextIndex == -1)
                        {
                            return (false, command, new List<(string, string)>());
                        }

                        arguments.Add((currentParameter, argToParse.Substring(1, nextIndex - 1)));
                        nextIndex++;
                        currentParameter = string.Empty;
                    }

                    // if the character is a -, we are looking at a named parameter
                    else if (argToParse[0] == '-')
                    {
                        if (!string.IsNullOrWhiteSpace(currentParameter))
                        {
                            arguments.Add((currentParameter, string.Empty));
                        }

                        nextIndex = argToParse.IndexOf(argToParse.First(char.IsWhiteSpace));
                        currentParameter = argToParse.Substring(1, nextIndex);
                    }

                    // otherwise if the character is anything else, look for the next whitespace character...
                    else
                    {
                        nextIndex = argToParse.IndexOf(argToParse.First(char.IsWhiteSpace));
                        arguments.Add((currentParameter, argToParse.Substring(0, nextIndex)));
                        currentParameter = string.Empty;
                    }
                    argToParse = $"{argToParse.Substring(nextIndex).Trim()} ";
                }

                if (!string.IsNullOrWhiteSpace(currentParameter))
                {
                    arguments.Add((currentParameter, string.Empty));
                }
            }

            return (true, command, arguments);
        }
    }
}