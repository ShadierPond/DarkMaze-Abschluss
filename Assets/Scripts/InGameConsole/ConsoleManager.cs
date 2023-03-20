using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace InGameConsole
{
    public class ConsoleManager : MonoBehaviour
    {
        public bool show;
        public static bool ShowHelp;
        public float outputHeight = 300;
        public static string Output;
        
        private string _input;
        
        public static List<object> ConsoleCommands = new();

        public static ConsoleCommand Help = new("help", "show list of all commands", "help", () =>
        {
            Output += "List of all commands: \n";
            foreach (CommandBase command in ConsoleCommands)
            {
                Output += command.CommandName + " - " + command.CommandDescription + " - " + command.CommandSyntax + "\n";
            }
        });
        
        public static ConsoleCommand Clear = new("clear", "clear the console", "clear", () =>
        {
            Output = "";
        });

        private void Awake()
        {
            ConsoleCommands.Add(Help);
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }
        
        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }


        private void Update()
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                show = !show;
                Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            }

            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                HandleInput();
                _input = "";
            }
        }
        
        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            var beginColor = "";
            var endColor = "";
            switch (type)
            {
                case LogType.Error:
                    beginColor = "<color=red>";
                    endColor = "</color>";
                    break;
                case LogType.Assert:
                    beginColor = "<color=red>";
                    endColor = "</color>";
                    break;
                case LogType.Warning:
                    beginColor = "<color=yellow>";
                    endColor = "</color>";
                    break;
                case LogType.Log:
                    beginColor = "<color=white>";
                    endColor = "</color>";
                    break;
                case LogType.Exception:
                    beginColor = "<color=red>";
                    endColor = "</color>";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            Output += beginColor + logString + endColor + "\n";
        }


        private void OnGUI()
        {
            if (!show)
                return;

            float yPos = 0;
            float height = 30;
            float buttonWidth = 100;
            
            GUI.BeginScrollView(new Rect(0, yPos, Screen.width, outputHeight), Vector2.zero, new Rect(0, yPos, Screen.width, outputHeight));
            GUI.backgroundColor = Color.black;
            GUI.Box(new Rect(0, yPos, Screen.width, outputHeight), Output, new GUIStyle(GUI.skin.box) {alignment = TextAnchor.UpperLeft, wordWrap = true, fontSize = 20});
            yPos += outputHeight;
            GUI.EndScrollView();
            
            GUI.Box(new Rect(0, yPos, Screen.width, height), "");
            GUI.backgroundColor = Color.black;
            _input = GUI.TextField(new Rect(0, yPos, Screen.width, height), _input, new GUIStyle(GUI.skin.textField) {alignment = TextAnchor.MiddleLeft, wordWrap = true, fontSize = 20});
            yPos += height;
            if (GUI.Button(new Rect(0, yPos, buttonWidth, height), "Close"))
            {
                show = !show;
                Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        private void HandleInput()
        {
            string[] properties = _input.Split(' ');
            
            for (int i = 0; i < ConsoleCommands.Count; i++)
            {
                CommandBase command = (CommandBase) ConsoleCommands[i];
                if (_input.Contains(command.CommandName))
                {
                    if (ConsoleCommands[i] as ConsoleCommand != null)
                    {
                        (ConsoleCommands[i] as ConsoleCommand)?.ExecuteCommand();
                    }
                    else if(ConsoleCommands[i] as ConsoleCommand<int> != null)
                    {
                        (ConsoleCommands[i] as ConsoleCommand<int>)?.ExecuteCommand(int.Parse(properties[1]));
                    }
                    else if(ConsoleCommands[i] as ConsoleCommand<float> != null)
                    {
                        (ConsoleCommands[i] as ConsoleCommand<float>)?.ExecuteCommand(float.Parse(properties[1]));
                    }
                    else if(ConsoleCommands[i] as ConsoleCommand<string> != null)
                    {
                        (ConsoleCommands[i] as ConsoleCommand<string>)?.ExecuteCommand(properties[1]);
                    }
                    else if(ConsoleCommands[i] as ConsoleCommand<Vector3> != null)
                    {
                        (ConsoleCommands[i] as ConsoleCommand<Vector3>)?.ExecuteCommand(new Vector3(float.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3])));
                    }
                    else if(ConsoleCommands[i] as ConsoleCommand<Vector2> != null)
                    {
                        (ConsoleCommands[i] as ConsoleCommand<Vector2>)?.ExecuteCommand(new Vector2(float.Parse(properties[1]), float.Parse(properties[2])));
                    }
                    else if(ConsoleCommands[i] as ConsoleCommand<bool> != null)
                    {
                        (ConsoleCommands[i] as ConsoleCommand<bool>)?.ExecuteCommand(bool.Parse(properties[1]));
                    }
                    else if(ConsoleCommands[i] as ConsoleCommand<Quaternion> != null)
                    {
                        (ConsoleCommands[i] as ConsoleCommand<Quaternion>)?.ExecuteCommand(new Quaternion(float.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3]), float.Parse(properties[4])));
                    }
                    else if(ConsoleCommands[i] as ConsoleCommand<Color> != null)
                    {
                        (ConsoleCommands[i] as ConsoleCommand<Color>)?.ExecuteCommand(new Color(float.Parse(properties[1]), float.Parse(properties[2]), float.Parse(properties[3]), float.Parse(properties[4])));
                    }
                }
            }
        }
    }
}