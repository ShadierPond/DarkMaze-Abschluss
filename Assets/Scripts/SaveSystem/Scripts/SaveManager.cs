using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GlobalSaveLoad
{
    public class SaveManager : MonoBehaviour
    {
        private enum SaveType
        {
            Json
        }

        private enum SaveLocation
        {
            PersistentDataPath,
            StreamingAssetsPath,
            ApplicationDataPath,
            DocumentsPath,
            CustomPath,
            None
        }

        public static SaveManager Instance { get; private set; }

        [Header("General Settings")]
        [SerializeField] private SaveType saveType;
        [SerializeField] private SaveLocation saveLocation;
        [SerializeField] private string customPath;
        private string _savePath;
        [SerializeField] private string fileName;
        [SerializeField] private string fileExtension;
        [SerializeField] private bool useDateTimeForFileName;

        [Header("Data")]
        public Data dataClass;
        
        [Header("Debug")]
        private SaveTypeClass _saveType;

        private void Awake()
        {
            Instance = this;
            dataClass = new Data();

            _savePath = saveLocation switch
            {
                SaveLocation.PersistentDataPath => Application.persistentDataPath + "/" + Application.productName + "/Save/",
                SaveLocation.ApplicationDataPath => Application.dataPath + "/" + Application.productName + "/Save/",
                SaveLocation.StreamingAssetsPath => Application.streamingAssetsPath + "/" + Application.productName + "/Save/",
                SaveLocation.DocumentsPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.companyName + "/" + Application.productName + "/Save/",
                SaveLocation.CustomPath => customPath + "/" + Application.companyName + "/" + Application.productName + "/Save/",
                SaveLocation.None => "",
                _ => _savePath
            };
            
            _saveType = saveType switch
            {
                SaveType.Json => new JsonSave(),
                _ => throw new ArgumentOutOfRangeException()
            };


        }

        public void Clear()
        {
            _saveType.ClearData();
        }
        
        public void Save()
        {
            if(!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);
            var dateTime = useDateTimeForFileName ? "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") : "";
            _saveType.Save(_savePath + fileName + dateTime + "." + fileExtension);
        }
        
        public List<string> LoadSaveList()
        {
            if (!Directory.Exists(_savePath)) 
                return null;

            var files = Directory.GetFiles(_savePath);
            return files.Select(file => file.Replace(_savePath, "")).Select(replaceSavePath => replaceSavePath.Replace("." + fileExtension, "")).ToList();
        }
        
        public void Load(string saveName)
        {
            if(File.Exists(_savePath + saveName + "." + fileExtension))
                _saveType.Load(_savePath + saveName + "." + fileExtension);
            else
                throw new DirectoryNotFoundException("Save file not found at " + _savePath + saveName + "." + fileExtension);
        }
    }
}
