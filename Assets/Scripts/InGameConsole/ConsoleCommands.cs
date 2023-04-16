using System;

namespace InGameConsole
{
    public class CommandBase
    {
        public string CommandName { get; }
        public string CommandDescription { get; }
        public string CommandSyntax { get; }

        /// <summary>
        /// This method is the constructor of the CommandBase class that initializes the command name, description and syntax fields.
        /// </summary>
        /// <param name="commandName">string - the name of the command.</param>
        /// <param name="commandDescription">string - the description of what the command does.</param>
        /// <param name="commandSyntax">string - the syntax of how to use the command.</param>
        protected CommandBase(string commandName, string commandDescription, string commandSyntax)
        {
            // Assign the parameters to the corresponding fields
            CommandName = commandName;
            CommandDescription = commandDescription;
            CommandSyntax = commandSyntax;
        }
    }
    
    public class ConsoleCommand : CommandBase
    {
        private readonly Action _commandAction;
        
        /// <summary>
        /// This method is the constructor of the ConsoleCommand class that inherits from the CommandBase class and initializes the command action field.
        /// </summary>
        /// <param name="commandName">string - the name of the command.</param>
        /// <param name="commandDescription">string - the description of what the command does.</param>
        /// <param name="commandSyntax">string - the syntax of how to use the command.</param>
        /// <param name="commandAction">Action - the delegate that defines the logic of the command.</param>
        public ConsoleCommand(string commandName, string commandDescription, string commandSyntax, Action commandAction) : base(commandName, commandDescription, commandSyntax)
        {
            // Assign the parameter to the corresponding field
            _commandAction = commandAction;
        }
        
        /// <summary>
        /// This method executes the command by invoking the command action delegate.
        /// </summary>
        public void ExecuteCommand()
        {
            // Invoke the command action delegate
            _commandAction.Invoke();
        }
    }
    
    public abstract class ConsoleCommand<T1> : CommandBase
    {
        private readonly Action<T1> _commandAction;

        /// <summary>
        /// This method is the constructor of the ConsoleCommand class that inherits from the CommandBase class and initializes the command action field with a generic parameter.
        /// </summary>
        /// <param name="commandName">string - the name of the command.</param>
        /// <param name="commandDescription">string - the description of what the command does.</param>
        /// <param name="commandSyntax">string - the syntax of how to use the command.</param>
        /// <param name="commandAction">Action<T1> - the delegate that defines the logic of the command with a generic parameter.</param>
        protected ConsoleCommand(string commandName, string commandDescription, string commandSyntax, Action<T1> commandAction) : base(commandName, commandDescription, commandSyntax)
        {
            // Assign the parameter to the corresponding field
            _commandAction = commandAction;
        }
        
        /// <summary>
        /// This method executes the command by invoking the command action delegate with a generic argument.
        /// </summary>
        /// <param name="arg1">T1 - the generic argument to pass to the command action delegate.</param>
        public void ExecuteCommand(T1 arg1)
        {
            // Invoke the command action delegate with the argument
            _commandAction.Invoke(arg1);
        }
    }
}