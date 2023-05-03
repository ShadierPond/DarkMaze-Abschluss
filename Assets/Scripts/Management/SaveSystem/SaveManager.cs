using System;
using System.IO;
using Management.SaveSystem.SaveTypes;
using UnityEngine;

namespace Management.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        private enum SaveLocation
        {
            PersistentDataPath,
            StreamingAssetsPath,
            ApplicationDataPath,
            DocumentsPath,
            CustomPath
        }

        [Header("Settings")]
        [SerializeField] private SaveLocation saveLocation;
        [SerializeField] private string customPath;
        private string _settingsPath;
        [SerializeField] private string fileExtension;

        [Header("Data")]
        public SettingsData settingsDataClass;
        
        [Header("Debug")]
        private JsonSave _jsonSave;

        /// <summary>
        /// This method is called when the script instance is being loaded. It initializes the settings data class, sets the settings path based on the save location, creates a new JsonSave instance, and loads the settings from a JSON file.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a SettingsManager script attached to it. It also requires that the save location or custom path are defined in the inspector. It uses the JsonSave class to handle saving and loading of settings data.
        /// </remarks>
        private void Awake()
        {
            // Create a new SettingsData object
            settingsDataClass = new SettingsData();
            // Set the settings path based on the save location enum value
            _settingsPath = saveLocation switch
            {
                // Use the persistent data path for the application
                SaveLocation.PersistentDataPath => Application.persistentDataPath + "/" + Application.companyName + "/" + Application.productName + "/",
                // Use the data path for the application
                SaveLocation.ApplicationDataPath => Application.dataPath + "/" + Application.companyName + "/" + Application.productName + "/",
                // Use the streaming assets path for the application
                SaveLocation.StreamingAssetsPath => Application.streamingAssetsPath + "/" + Application.companyName + "/" + Application.productName + "/",
                // Use the documents path for the user
                SaveLocation.DocumentsPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.companyName + "/" + Application.productName + "/",
                // Use a custom path defined in the inspector
                SaveLocation.CustomPath => customPath + "/" + Application.companyName + "/" + Application.productName + "/",
                // Use the default settings path if none of the above cases match
                _ => _settingsPath
            };
            // Create a new JsonSave object with this script as the argument
            _jsonSave = new JsonSave(this);
            // Load the settings from a JSON file using the JsonSave object
            LoadSettings();
        }

        /// <summary>
        /// This method saves the settings data to a JSON file in the settings path.
        /// </summary>
        public void SaveSettings()
        {
            // Check if the settings path directory exists
            if(!Directory.Exists(_settingsPath))
                // If not, create it
                Directory.CreateDirectory(_settingsPath);
            // Save the settings data to a JSON file using the JsonSave object
            _jsonSave.SaveSettings(_settingsPath + "settings." + fileExtension);
        }

        /// <summary>
        /// This method loads the settings data from a JSON file in the settings path.
        /// </summary>
        /// <remarks>
        /// If the settings file does not exist, it creates a new one and saves the default settings data to it.
        /// </remarks>
        private void LoadSettings()
        {
            // Check if the settings file exists
            if(File.Exists(_settingsPath + "settings." + fileExtension))
                // If yes, load the settings data from it using the JsonSave object
                _jsonSave.LoadSettings(_settingsPath + "settings." + fileExtension);
            else
            {
                // If not, log a warning message
                Debug.LogWarning($"Settings file not found at {_settingsPath}settings.{fileExtension}. Creating new settings file.");
                settingsDataClass.resolutionIndex = Screen.resolutions.Length - 1;
                // Save the default settings data to a new JSON file using the SaveSettings method
                SaveSettings();
            }
        }
    }
}
