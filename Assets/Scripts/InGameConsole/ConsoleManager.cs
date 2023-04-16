using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGameConsole
{
    public class ConsoleManager : MonoBehaviour
    {
        public bool show;
        public float outputHeight = 300;
        private static string _output;
        
        private string _input;

        private static readonly List<object> ConsoleCommands = new();

        private static readonly ConsoleCommand Help = new("help", "show list of all commands", "help", () =>
        {
            // Append the output string with the header for the list of commands
            _output += "List of all commands: \n";
            // Loop through each command in the ConsoleCommands collection
            foreach (CommandBase command in ConsoleCommands)
            {
                // Append the output string with the command name, description and syntax
                _output += command.CommandName + " - " + command.CommandDescription + " - " + command.CommandSyntax + "\n";
            }
        });

        private static readonly ConsoleCommand Clear = new("clear", "clear the console", "clear", () =>
        {
            // Set the output string to an empty string
            _output = "";
        });

        /// <summary>
        /// This method adds the help and clear commands to the ConsoleCommands collection.
        /// </summary>
        private void Awake()
        {
            // Add the help command to the ConsoleCommands collection
            ConsoleCommands.Add(Help);
            // Add the clear command to the ConsoleCommands collection
            ConsoleCommands.Add(Clear);
        }

        /// <summary>
        /// This method subscribes the HandleLog method to the logMessageReceived event of the Application class.
        /// </summary>
        private void OnEnable()
        {
            // Subscribe the HandleLog method to the logMessageReceived event
            Application.logMessageReceived += HandleLog;
        }
        
        /// <summary>
        /// This method unsubscribes the HandleLog method from the logMessageReceived event of the Application class.
        /// </summary>
        private void OnDisable()
        {
            // Unsubscribe the HandleLog method from the logMessageReceived event
            Application.logMessageReceived -= HandleLog;
        }


        /// <summary>
        /// This method handles the keyboard input for toggling the console and executing the commands.
        /// </summary>
        private void Update()
        {
            // If the F1 key was pressed this frame
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                // Toggle the show flag
                show = !show;
                // Set the cursor lock state based on the show flag
                Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            }

            // If the enter key was pressed this frame
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                // Handle the input string
                HandleInput();
                // Reset the input string to an empty string
                _input = "";
            }
        }
        
        /// <summary>
        /// This method handles the log messages from the Application class and appends them to the output string with different colors based on the log type.
        /// </summary>
        /// <param name="logString">string - the content of the log message.</param>
        /// <param name="stackTrace">string - the stack trace of the log message.</param>
        /// <param name="type">LogType - the type of the log message.</param>
        private static void HandleLog(string logString, string stackTrace, LogType type)
        {
            // Declare variables for the begin and end color tags
            string beginColor;
            string endColor;
            // Switch on the log type
            switch (type)
            {
                // If the log type is error or assert, set the color to red
                case LogType.Error:
                    beginColor = "<color=red>";
                    endColor = "</color>";
                    break;
                case LogType.Assert:
                    beginColor = "<color=red>";
                    endColor = "</color>";
                    break;
                // If the log type is warning, set the color to yellow
                case LogType.Warning:
                    beginColor = "<color=yellow>";
                    endColor = "</color>";
                    break;
                // If the log type is log, set the color to white
                case LogType.Log:
                    beginColor = "<color=white>";
                    endColor = "</color>";
                    break;
                // If the log type is exception, set the color to red
                case LogType.Exception:
                    beginColor = "<color=red>";
                    endColor = "</color>";
                    break;
                // If the log type is not any of the above, throw an exception
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            // Append the output string with the log message and the color tags
            _output += beginColor + logString + endColor + "\n";
        }


        /// <summary>
        /// This method draws the console GUI on the screen if the show flag is true.
        /// </summary>
        private void OnGUI()
        {
            // If the show flag is false, return
            if (!show)
                return;

            // Declare a variable for the vertical position of the GUI elements
            float yPos = 0;
            // Declare constants for the height and width of the GUI elements
            const float height = 30;
            const float buttonWidth = 100;
    
            // Begin a scroll view for the output box
            GUI.BeginScrollView(new Rect(0, yPos, Screen.width, outputHeight), Vector2.zero, new Rect(0, yPos, Screen.width, outputHeight));
            // Set the background color to black
            GUI.backgroundColor = Color.black;
            // Draw a box with the output string and a custom style
            GUI.Box(new Rect(0, yPos, Screen.width, outputHeight), _output, new GUIStyle(GUI.skin.box) {alignment = TextAnchor.UpperLeft, wordWrap = true, fontSize = 20});
            // Increase the vertical position by the output height
            yPos += outputHeight;
            // End the scroll view
            GUI.EndScrollView();
    
            // Draw a box for the input field
            GUI.Box(new Rect(0, yPos, Screen.width, height), "");
            // Set the background color to black
            GUI.backgroundColor = Color.black;
            // Draw a text field for the input string and a custom style
            _input = GUI.TextField(new Rect(0, yPos, Screen.width, height), _input, new GUIStyle(GUI.skin.textField) {alignment = TextAnchor.MiddleLeft, wordWrap = true, fontSize = 20});
            // Increase the vertical position by the height
            yPos += height;
            // Draw a button for closing the console
            if (GUI.Button(new Rect(0, yPos, buttonWidth, height), "Close"))
            {
                // Toggle the show flag
                show = !show;
                // Set the cursor lock state based on the show flag
                Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        /// <summary>
        /// Handles the input from the user and executes the corresponding console command.
        /// </summary>
        /// <remarks>
        /// It requires that the _input field contains a valid console command name and its parameters (if any).
        /// It also requires that the ConsoleCommands list contains all the possible console commands that can be executed.
        /// </remarks>
        private void HandleInput()
        {
            var properties = _input.Split(' '); // Split the input by spaces into an array of strings
            
            for (var i = 0; i < ConsoleCommands.Count; i++) // Loop through all the console commands in the list
            {
                var command = (CommandBase) ConsoleCommands[i]; // Cast the current console command to a CommandBase type
                if (_input.Contains(command.CommandName)) // Check if the input contains the name of the current console command
                {
                    switch (ConsoleCommands[i]) // Switch on the type of the current console command
                    {
                        case ConsoleCommand: // If it is a ConsoleCommand with no parameters
                            // Execute the command with no arguments
                            (ConsoleCommands[i] as ConsoleCommand)?.ExecuteCommand();
                            break;
                        case ConsoleCommand<int>: // If it is a ConsoleCommand with an int parameter
                            // Execute the command with the int argument parsed from the second element of the properties array
                            (ConsoleCommands[i] as ConsoleCommand<int>)?.ExecuteCommand(int.Parse(properties[1]));
                            break;
                        case ConsoleCommand<float>: // If it is a ConsoleCommand with a float parameter
                            // Execute the command with the float argument parsed from the second element of the properties array
                            (ConsoleCommands[i] as ConsoleCommand<float>)?.ExecuteCommand(float.Parse(properties[1]));
                            break;
                        case ConsoleCommand<string>: // If it is a ConsoleCommand with a string parameter
                            // Execute the command with the string argument from the second element of the properties array
                            (ConsoleCommands[i] as ConsoleCommand<string>)?.ExecuteCommand(properties[1]);
                            break;
                        case ConsoleCommand<Vector3>: // If it is a ConsoleCommand with a Vector3 parameter
                            // Execute the command with the Vector3 argument constructed from the second, third and fourth elements of the properties array
                            (ConsoleCommands[i] as ConsoleCommand<Vector3>)?.ExecuteCommand(new Vector3(float.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3])));
                            break;
                        case ConsoleCommand<Vector2>: // If it is a ConsoleCommand with a Vector2 parameter
                            // Execute the command with the Vector2 argument constructed from the second and third elements of the properties array
                            (ConsoleCommands[i] as ConsoleCommand<Vector2>)?.ExecuteCommand(new Vector2(float.Parse(properties[1]), float.Parse(properties[2])));
                            break;
                        case ConsoleCommand<bool>: // If it is a ConsoleCommand with a bool parameter
                            // Execute the command with the bool argument parsed from the second element of the properties array
                            (ConsoleCommands[i] as ConsoleCommand<bool>)?.ExecuteCommand(bool.Parse(properties[1]));
                            break;
                        case ConsoleCommand<Quaternion>: // If it is a ConsoleCommand with a Quaternion parameter
                            // Execute the command with the Quaternion argument constructed from the second, third, fourth and fifth elements of the properties array
                            (ConsoleCommands[i] as ConsoleCommand<Quaternion>)?.ExecuteCommand(new Quaternion(float.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3]), float.Parse(properties[4])));
                            break;
                        case ConsoleCommand<Color>: // If it is a ConsoleCommand with a Color parameter
                            // Execute the command with the Color argument constructed from the second, third, fourth and fifth elements of the properties array
                            (ConsoleCommands[i] as ConsoleCommand<Color>)?.ExecuteCommand(new Color(float.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3]), float.Parse(properties[4])));
                            break;
                    }
                }
            }
        }
    }
}