# Velentr.Miscellaneous.CommandParsing
Helpers to help with converting strings into a command, which can subsequently be executed, and the parameters for it.

# Installation
A nuget package is available: [Velentr.Miscellaneous](https://www.nuget.org/packages/Velentr.Miscellaneous/)

# Arguments vs Parameters
In the current implementation, an Argument is defined when a Command is initialized. These are any arguments that can/should be given when a command is called.

On the other hand, a Parameter is what is what is actually provided to the Command when it is called for Execution.

# Current Files
File | Type | Description | Min Supported Version | Example Usage | Notes
---- | ---- | ----------- | --------------------- | ------------- | -----
IArgument | Interface | The interface for what an argument for a `Command` should look like. | 1.0.0 | N/A (Interface) | This is implemented internally by the `Argument` struct. If doing a lot of custom work, you can re-implement the `Argument` class as needed extending from this class, but most people won't need to touch this or the `Parameter` class.
IParameter | Interface | The interface for what a parameter for a `Command` should look like. | 1.0.0 | N/A (Interface) | This is implemented internally by the `Parameter` struct. If doing a lot of custom work, you can re-implement the `Parameter` class as needed extending from this class, but most people won't need to touch this or the `Argument` class.
IParseResult | Interface | The interface for that defines what the response for a `ParseCommand` method call looks like. | 1.0.0 | N/A (Interface) | This is implemented internally by the `ParseResult` struct. If doing a lot of custom work, you can re-implement the `ParseResult` class as needed extending from this class, but most people won't need to touch it.
Argument | struct | Represents an argument that can be passed to a command. | 1.0.0 | N/A (internal-only) | An instance of this class, which implements the `IArgument` interface, is automatically created when the `AddArgument` method is called in a `Command`.
Parameter | struct | Represents a parameter that has actually been passed to the command (or the default value for the Argument that the Parameter is associated with). | 1.0.0 | N/A (internal-only) | An instance of this class, which implements the `IParameter` interface, is automatically created when a Command is being executed and is passed in the `parameters` dictionary to the `ExecuteCommand` method.
ParseResult | struct | Contains the results of a `ParseCommand` method call, which is the Command that we were able to Parse (or null) and a Dictionary containing the parameters that were parsed with the command (and/or default values for the arguments). | 1.0.0 | N/A (internal-only) | An instance of this class, which implements the `IParseResult` interface, is automatically created when a Command is parsed.
DefaultHelpCommand | class | The default Help Command. This can automatically be added to the Command Parser based on what parameters are passed to the CommandParser on initialization. | 1.0.0 | `var cmd = new DefaultHelpCommand(false);` | You can use this class as-is or extend it. If replacing/extending it, make sure to implement functionality for handling the different cases for error handling and providing general help vs. specific command help!
CommandParser | class | The core of the helpers. This class handles registering and parsing commands that can be called. | 1.0.0 | `var parser = new CommandParser();` | N/A

# Parameters for the Command Parser
Parameter Name | Description | Default Value | Min Supported Version
-------------- | ----------- | ------------- | ---------------------
commandPrefix | A prefix to prepend to any registered commands. I.e. set it to `!cmd_` to prefix the string `!cmd_` to any commands that get registered by the Parser. | `string.Empty` | 1.0.0
autoSearchForCommands | If True, this will search the Assembly for any classes implementing the `AbstractCommand` class which have the `AutoRegisterCommand` flag set to true, and automatically register them. If False, you'll need to manually register commands with the `RegisterCommand()` method. | `True` | 1.0.0
autoRegisterDefaultHelpCommand | If True the `DefaultHelpCommand` class will be registered as the `help` Command for the Parser. If False, you'll need to register a help Command using the `RegisterHelpCommand` method. | `True` | 1.0.0
defaultHelpCommandPrintsToConsole | If True, the `DefaultHelpCommand` class (if it is enabled in the parameter above) will print to the Console automatically. If False, it will not. | `True` | 1.0.0

# Full Example
```
using System;
using System.Collections.Generic;
using Velentr.Miscellaneous.CommandParsing;

namespace Velentr.Miscellaneous.dev
{
    public class ExitConsole : AbstractCommand
    {
        public ExitConsole() : base("exit", "Exits the console", false)
        {
        }

        public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
        {
            Environment.Exit(0);
            return true;
        }
    }
    public class HelloWorld : AbstractCommand
    {
        public HelloWorld() : base("hello_world", "Says hello to things")
        {
            AddArgument("item", "what to say hello to", typeof(string), "World", false);
        }

        public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
        {
            var item = (string)parameters["item"].Value;
            Console.WriteLine($"Hello {item}!");

            return true;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new CommandParser();

            while (true)
            {
                var command = Console.ReadLine();

                var arguments = new Dictionary<string, object>();
                var cmd = parser.ParseCommand(command, arguments);

                cmd.Command.ExecuteCommand(cmd.Parameters, arguments);
            }
        }
    }
}
```
