using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Management.SaveSystem.SaveTypes;
using UnityEngine;

namespace Management.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }
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
        private string _savePath;
        private string _settingsPath;
        [SerializeField] private string folderName;
        public string FolderName { get => folderName; set => folderName = value; }
        [SerializeField] private string fileExtension;

        [Header("Data")]
        public Data dataClass;
        public SettingsData settingsDataClass;
        
        [Header("Debug")]
        private JsonSave _jsonSave;

        private void Awake()
        {
            Instance = this;
            dataClass = new Data();
            settingsDataClass = new SettingsData();

            _savePath = saveLocation switch
            {
                SaveLocation.PersistentDataPath => Application.persistentDataPath + "/" + Application.productName + "/" + folderName + "/",
                SaveLocation.ApplicationDataPath => Application.dataPath + "/" + Application.productName + "/" + folderName + "/",
                SaveLocation.StreamingAssetsPath => Application.streamingAssetsPath + "/" + Application.productName + "/" + folderName + "/",
                SaveLocation.DocumentsPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.companyName + "/" + Application.productName + "/" + folderName + "/",
                SaveLocation.CustomPath => customPath + "/" + Application.companyName + "/" + Application.productName + "/" + folderName + "/",
                _ => _savePath
            };
            
            _settingsPath = saveLocation switch
            {
                SaveLocation.PersistentDataPath => Application.persistentDataPath + "/" + Application.productName + "/",
                SaveLocation.ApplicationDataPath => Application.dataPath + "/" + Application.productName + "/",
                SaveLocation.StreamingAssetsPath => Application.streamingAssetsPath + "/" + Application.productName + "/",
                SaveLocation.DocumentsPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.companyName + "/" + Application.productName + "/",
                SaveLocation.CustomPath => customPath + "/" + Application.companyName + "/" + Application.productName + "/",
                _ => _settingsPath
            };
            _jsonSave = new JsonSave();
            LoadSettings();
        }

        public void Clear()
            => _jsonSave.ClearData();

        public void Save()
        {
            if(!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);
            var dateTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            _jsonSave.Save(_savePath  + dateTime + "." + fileExtension);
        }

        public void SaveSettings()
        {
            if(!Directory.Exists(_settingsPath))
                Directory.CreateDirectory(_settingsPath);
            _jsonSave.SaveSettings(_settingsPath + "settings." + fileExtension);
        }
        
        public void LoadSettings()
        {
            if(File.Exists(_settingsPath + "settings." + fileExtension))
                _jsonSave.LoadSettings(_settingsPath + "settings." + fileExtension);
            else
            {
                Debug.LogWarning($"Settings file not found at {_settingsPath}settings.{fileExtension}. Creating new settings file.");
                SaveSettings();
            }
        }

        public List<string> LoadSaveList()
        {
            if (!Directory.Exists(_savePath)) 
                return null;

            var files = Directory.GetFiles(_savePath);
            return files.Select(file => file.Replace("." + fileExtension, "")).ToList();
        }
        
        public void Load(string saveName)
        {
            if (File.Exists(_savePath + saveName + "." + fileExtension))
            {
                _jsonSave.Load(_savePath + saveName + "." + fileExtension);
            }
            else
                throw new DirectoryNotFoundException("Save file not found at " + _savePath + saveName + "." + fileExtension);
        }
    }
}
