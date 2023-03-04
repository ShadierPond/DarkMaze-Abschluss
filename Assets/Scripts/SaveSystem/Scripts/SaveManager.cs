using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GlobalSaveLoad
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

        public static SaveManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private SaveLocation saveLocation;
        [SerializeField] private string customPath;
        private string _savePath;
        [SerializeField] private string folderName;
        [SerializeField] private string fileExtension;

        [Header("Data")]
        public Data dataClass;
        
        [Header("Debug")]
        private JsonSave _jsonSave;

        private void Awake()
        {
            Instance = this;
            dataClass = new Data();

            _savePath = saveLocation switch
            {
                SaveLocation.PersistentDataPath => Application.persistentDataPath + "/" + Application.productName + "/" + folderName + "/",
                SaveLocation.ApplicationDataPath => Application.dataPath + "/" + Application.productName + "/" + folderName + "/",
                SaveLocation.StreamingAssetsPath => Application.streamingAssetsPath + "/" + Application.productName + "/" + folderName + "/",
                SaveLocation.DocumentsPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.companyName + "/" + Application.productName + "/" + folderName + "/",
                SaveLocation.CustomPath => customPath + "/" + Application.companyName + "/" + Application.productName + "/" + folderName + "/",
                _ => _savePath
            };
            
            _jsonSave = new JsonSave();
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
        
        public List<string> LoadSaveList()
        {
            if (!Directory.Exists(_savePath)) 
                return null;

            var files = Directory.GetFiles(_savePath);
            return files.Select(file => file.Replace("." + fileExtension, "")).ToList();
        }
        
        public void Load(string saveName)
        {
            if(File.Exists(_savePath + saveName + "." + fileExtension))
                _jsonSave.Load(_savePath + saveName + "." + fileExtension);
            else
                throw new DirectoryNotFoundException("Save file not found at " + _savePath + saveName + "." + fileExtension);
        }
    }
}
