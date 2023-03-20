using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGameConsole
{
    public class CommandBase
    {
        private string _commandName;
        private string _commandDescription;
        private string _commandSyntax;
        
        public string CommandName => _commandName;
        public string CommandDescription => _commandDescription;
        public string CommandSyntax => _commandSyntax;
        
        public CommandBase(string commandName, string commandDescription, string commandSyntax)
        {
            _commandName = commandName;
            _commandDescription = commandDescription;
            _commandSyntax = commandSyntax;
        }
    }
    
    public class ConsoleCommand : CommandBase
    {
        private Action _commandAction;
        
        public ConsoleCommand(string commandName, string commandDescription, string commandSyntax, Action commandAction) : base(commandName, commandDescription, commandSyntax)
        {
            _commandAction = commandAction;
        }
        
        public void ExecuteCommand()
        {
            _commandAction.Invoke();
        }
    }
    
    public class ConsoleCommand<T1> : CommandBase
    {
        private Action<T1> _commandAction;
        
        public ConsoleCommand(string commandName, string commandDescription, string commandSyntax, Action<T1> commandAction) : base(commandName, commandDescription, commandSyntax)
        {
            _commandAction = commandAction;
        }
        
        public void ExecuteCommand(T1 arg1)
        {
            _commandAction.Invoke(arg1);
        }
    }
}