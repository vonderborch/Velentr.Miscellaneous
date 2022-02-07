/// <file>
/// Velentr.Miscellaneous\CommandParsing\DefaultHelpCommand.cs
/// </file>
///
/// <copyright file="DefaultHelpCommand.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the default help command class.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Velentr.Miscellaneous.StringHelpers;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// A default help command.
    /// </summary>
    ///
    /// <seealso cref="Velentr.Miscellaneous.CommandParsing.AbstractCommand"/>
    /// <seealso cref="AbstractCommand"/>
    public class DefaultHelpCommand : AbstractCommand
    {
        /// <summary>
        /// The command cache.
        /// </summary>
        private List<AbstractCommand> _commandCache = new List<AbstractCommand>();

        /// <summary>
        /// Gets or sets a value indicating whether the print to console.
        /// </summary>
        ///
        /// <value>
        /// True if print to console, false if not.
        /// </value>
        public bool PrintToConsole { get; set; }

        /// <summary>
        /// The help columns.
        /// </summary>
        private List<string> _helpColumns = new List<string>();

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="printToConsole">   (Optional) True to print to console. </param>
        public DefaultHelpCommand(bool printToConsole = false) : base("help", "Displays a general help message to the user", false, 2, false)
        {
            PrintToConsole = printToConsole;

            _helpColumns.Add("Command");
            _helpColumns.Add("Description");
            _helpColumns.Add("Example Command Format");

            AddArgument("command", "The command to display more information about", typeof(string), "", false);
            AddArgument("show_parameters", "Whether to show the parameters for each command", typeof(bool), true, false);
            AddArgument("as_table", "Whether to display the results as a table or not", typeof(bool), true, false);
        }

        /// <summary>
        /// Gets or sets the last string output.
        /// </summary>
        ///
        /// <value>
        /// The last string output.
        /// </value>
        public string LastStringOutput { get; private set; } = "";

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
        ///
        /// <seealso cref="Velentr.Miscellaneous.CommandParsing.AbstractCommand.ExecuteCommand(Dictionary{string,IParameter},Dictionary{string,object})"/>
        public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
        {
            var str = new StringBuilder();
            ExecutePreCommand(str, parameters, args);

            var commandToExecuteOn = parameters["command"].GetValue<string>().ToLowerInvariant();
            var showParameters = parameters["show_parameters"].GetValue<bool>();
            var asTable = parameters["as_table"].GetValue<bool>();

            // Get special Debug Parameters, if they exist
            var failureCase = GetParameterValueIfExistsAsString("failure_case", parameters);
            var failureException = GetParameterValueIfExistsAsString("failure_exception", parameters);
            var commandDoesNotExist = GetParameterValueIfExistsAsString("command_does_not_exist", parameters);
            //var failedCommandMessage = GetParameterValueIfExistsAsString("failed_command", parameters); // this parameter allows us to get the actual message that failed to be parsed not used atm
            var isValidCommand = string.IsNullOrWhiteSpace(commandDoesNotExist);
            var isDebugHelp = !string.IsNullOrWhiteSpace(failureCase);
            var failedCommand = commandToExecuteOn;

            // validate that the command we'll look for a help message on exists...
            if (isDebugHelp && !isValidCommand)
            {
                commandToExecuteOn = string.Empty;
            }
            else if (!isDebugHelp && !string.IsNullOrWhiteSpace(commandToExecuteOn))
            {
                var exists = Parser.Commands.Any(x => x.Key == commandToExecuteOn);
                if (!exists)
                {
                    commandToExecuteOn = string.Empty;
                }
            }

            if (isDebugHelp)
            {
                str.AppendLine($"{failureCase}: {failureException}");
                str.AppendLine("");

                if (string.IsNullOrWhiteSpace(commandToExecuteOn) && !string.IsNullOrWhiteSpace(failedCommand))
                {
                    var commandNames = Parser.Commands.Select(x => x.Key).ToList();
                    var currentMostSimilar = string.Empty;
                    var currentMostSimilarSimilarity = int.MaxValue;

                    for (var i = 0; i < commandNames.Count; i++)
                    {
                        var similarity = StringSimilarity.GetDamerauLevenshteinDistance(commandNames[i], failedCommand);
                        if (similarity < currentMostSimilarSimilarity)
                        {
                            currentMostSimilarSimilarity = similarity;
                            currentMostSimilar = commandNames[i];
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(currentMostSimilar))
                    {
                        str.AppendLine($"Did you mean to use the command [{currentMostSimilar}]?");
                    }
                }

                if (isValidCommand)
                {
                    str.AppendLine($"Help for [{commandToExecuteOn}] Command:");
                }
                else
                {
                    str.AppendLine("Available Commands:");
                }
            }

            // Path 1 - Display Generic Message for All Commands
            if (string.IsNullOrWhiteSpace(commandToExecuteOn))
            {
                var commandRows = new List<List<string>>();
                for (var i = 0; i < Parser.Commands.Count; i++)
                {
                    if (!Parser.Commands[i].IsHidden)
                    {
                        var row = new List<string>
                        {
                            Parser.Commands[i].CommandName,
                            Parser.Commands[i].Description
                        };

                        if (showParameters)
                        {
                            row.Add(Parser.Commands[i].GetExampleCommand());
                        }

                        commandRows.Add(row);
                    }
                }

                if (asTable)
                {
                    str.AppendLine(TableOutputHelper.ConvertToTable(_helpColumns, commandRows));
                }
                else
                {
                    for (var i = 0; i < commandRows.Count; i++)
                    {
                        var line = new StringBuilder();
                        line.Append(commandRows[i][0]);
                        for (var j = 1; j < commandRows[i].Count; j++)
                        {
                            line.Append($" - {commandRows[i][j]}");
                        }

                        str.AppendLine(line.ToString());
                    }
                }
            }
            // Path 2 - Display Message for Specific Command
            else
            {
                var command = Parser.Commands[commandToExecuteOn];

                // print out the example command
                str.AppendLine(command.GetExampleCommand());
                str.AppendLine("");

                // print out the description
                str.AppendLine($"Description: {command.Description}");
                str.AppendLine("");

                // print out argument help
                str.AppendLine("Arguments:");
                for (var i = 0; i < command.Arguments.Count; i++)
                {
                    if (!command.Arguments[i].IsHidden)
                    {
                        str.AppendLine($"  {command.Arguments[i].AsParameterDescription}");
                    }
                }
            }

            LastStringOutput = str.ToString();
            ExecutePostCommand(str, parameters, args);
            return true;
        }

        /// <summary>
        /// Executes the 'pre' command.
        /// </summary>
        ///
        /// <param name="str">          The string. </param>
        /// <param name="parameters">   Options for controlling the operation. </param>
        /// <param name="args">         The arguments. </param>
        ///
        /// <returns>
        /// A StringBuilder.
        /// </returns>
        public virtual StringBuilder ExecutePreCommand(StringBuilder str, Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
        {
            return str;
        }

        /// <summary>
        /// Executes the 'post' command.
        /// </summary>
        ///
        /// <param name="str">          The string. </param>
        /// <param name="parameters">   Options for controlling the operation. </param>
        /// <param name="args">         The arguments. </param>
        public virtual void ExecutePostCommand(StringBuilder str, Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
        {
            if (PrintToConsole)
            {
                Console.WriteLine(str.ToString());
            }
        }
    }
}