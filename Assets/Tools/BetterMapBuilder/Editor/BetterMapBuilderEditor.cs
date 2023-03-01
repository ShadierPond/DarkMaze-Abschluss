using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tools.BetterMapBuilder.Editor
{
    public class BetterMapBuilderEditor : EditorWindow
    {
        [MenuItem("Tools/Better Map Builder")]
        private static void ShowWindow()
        {
            var window = GetWindow<BetterMapBuilderEditor>();
            window.titleContent = new GUIContent("Better Map Builder");
            window.Show();
        }
        
        private MapBrush _currentBrush;

        private void OnEnable()
        {
            var mapBuilder = Selection.activeGameObject?.GetComponent<BetterMapBuilder>();
            CreateMapGrid(mapBuilder);
        }

        private void OnDisable()
        {
            _currentBrush = null;
        }

        private void OnGUI()
        {
            var mapBuilder = Selection.activeGameObject?.GetComponent<BetterMapBuilder>();
            if (mapBuilder is null)
            {
                EditorGUILayout.HelpBox("Select a GameObject with a BetterMapBuilder component", MessageType.Info);
                return;
            }
            
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(600), GUILayout.Height(300), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(600), GUILayout.MaxHeight(300));
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(600), GUILayout.Height(300), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(600), GUILayout.MaxHeight(300));
                    EditorGUILayout.LabelField("Map Settings");
                    mapBuilder.mapParent = (GameObject) EditorGUILayout.ObjectField("Map Parent", mapBuilder.mapParent, typeof(GameObject), true);
                    mapBuilder.mapName = EditorGUILayout.TextField("Map Name", mapBuilder.mapName);
                    if (string.IsNullOrEmpty(mapBuilder.mapName))
                    {
                        EditorGUILayout.HelpBox("Map Name is required", MessageType.Error);
                        return;
                    }
                    
                    mapBuilder.mapSize = EditorGUILayout.Vector2IntField("Map Size", mapBuilder.mapSize);
                    mapBuilder.tileSize = EditorGUILayout.Vector3IntField("Tile Size", mapBuilder.tileSize);
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(600), GUILayout.Height(300), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(600), GUILayout.MaxHeight(300));
                    EditorGUILayout.LabelField("Save/Load");
                    if (GUILayout.Button("Save Map"))
                    {
                        SaveMap(mapBuilder);
                    }

                    foreach (var map in GetSavedMaps())
                    {
                        if (GUILayout.Button(map.ToString()))
                        {
                            LoadMap(mapBuilder, map.ToString());
                        }
                    }
                    
                EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            
            CreateMapGrid(mapBuilder);
            
            EditorGUILayout.Space();
            BrushEditorGUI(mapBuilder);
            
            EditorGUILayout.Space();
            BrushPickerGUI(mapBuilder);

            if (GUILayout.Button("Build Map"))
            {
                mapBuilder.Build();
            }
            
            EditorGUILayout.Space();
            MapGridGUI(mapBuilder);
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(mapBuilder);
            }
        }
        
        private void BrushEditorGUI(BetterMapBuilder mapBuilder)
        {
            EditorGUILayout.LabelField("Edit Brushes");
            EditorGUILayout.BeginVertical(EditorStyles.helpBox ,GUILayout.Width(200), GUILayout.Height(100), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(200), GUILayout.MaxHeight(100));
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(200), GUILayout.Height(100), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(200), GUILayout.MaxHeight(100));
            
            foreach (var brush in mapBuilder.Brushes)
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(50), GUILayout.Height(50));
                brush.Texture = (Texture2D) EditorGUILayout.ObjectField(brush.Texture, typeof(Texture2D), false);
                if(brush.Texture != null)
                    EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(50, 50), brush.Texture);
                else
                    EditorGUILayout.LabelField("No Texture");
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(GUILayout.Width(100), GUILayout.Height(100));
                brush.GameObject = (GameObject) EditorGUILayout.ObjectField(brush.GameObject, typeof(GameObject), false);
                brush.RotationY = EditorGUILayout.IntField("Rotation Y", brush.RotationY);
                if(GUILayout.Button("Remove", GUILayout.Width(100), GUILayout.Height(20), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(100), GUILayout.MaxHeight(20)))
                {
                    mapBuilder.Brushes.Remove(brush);
                    break;
                }   
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            if(GUILayout.Button("Add Brush", GUILayout.Width(200), GUILayout.Height(20), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(200), GUILayout.MaxHeight(20)))
            {
                mapBuilder.Brushes.Add(new MapBrush());
            }
            EditorGUILayout.EndVertical();
        }
        
        private void BrushPickerGUI(BetterMapBuilder mapBuilder)
        {
            EditorGUILayout.LabelField("Brush Picker");
            EditorGUILayout.BeginHorizontal();
            foreach (var brush in mapBuilder.Brushes)
            {
                if (GUILayout.Button(brush.Texture, GUILayout.Width(50), GUILayout.Height(50)))
                {
                    _currentBrush = brush;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        
        private void CreateMapGrid(BetterMapBuilder mapBuilder)
        {
            if (mapBuilder.Map != null) 
                return;
            mapBuilder.Map = new MapCell[mapBuilder.mapSize.x, mapBuilder.mapSize.y];
            for (var y = 0; y < mapBuilder.mapSize.y; y++)
            {
                for (var x = 0; x < mapBuilder.mapSize.x; x++)
                {
                    mapBuilder.Map[x, y] = new MapCell(new Vector2Int(x, y), null, null, 0);
                }
            }
        }
        
        private void MapGridGUI(BetterMapBuilder mapBuilder)
        {
            for (var y = 0; y < mapBuilder.mapSize.y; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (var x = 0; x < mapBuilder.mapSize.x; x++)
                {
                    if (GUILayout.Button(mapBuilder.Map[x, y].Texture, GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        if (_currentBrush is null)
                        {
                            return;
                        }
                        var newCell = new MapCell(new Vector2Int(x, y), _currentBrush.GameObject, _currentBrush.Texture, _currentBrush.RotationY);
                        if (Event.current.button == 0 && mapBuilder.Map[x, y] != newCell)
                        { 
                            Debug.Log("Set");
                            mapBuilder.Map[x, y] = newCell;
                        }
                        if (Event.current.button == 1 && mapBuilder.Map[x, y].Texture is not null)
                        {
                            Debug.Log("Rotate Counter Clockwise");
                            RotateCellGUI(mapBuilder, x, y, false);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        
        private void RotateCellGUI(BetterMapBuilder mapBuilder, int x, int y, bool clockwise)
        {
            var cell = mapBuilder.Map[x, y];
            cell.RotationY = clockwise ? cell.RotationY + 90 : cell.RotationY - 90;
            cell.Texture = RotateTexture(cell.Texture, clockwise);
            mapBuilder.Map[x, y] = cell;
        }
        
        private Texture2D RotateTexture(Texture2D originalTexture, bool clockwise)
        {
            var original = originalTexture.GetPixels32();
            var rotated = new Color32[original.Length];
            var width = originalTexture.width;
            var height = originalTexture.height;
            
            for (var i = 0; i < height; ++i)
            for (var j = 0; j < width; ++j)
                rotated[(j + 1) * height - i - 1] = original[clockwise ? original.Length - 1 - (i * width + j) : i * width + j];

            var rotatedTexture = new Texture2D(height, width);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            return rotatedTexture;
        }
        
        private void SaveBrushes(BetterMapBuilder mapBuilder)
        {
            var path = EditorUtility.SaveFilePanel("Brushes Data", Application.dataPath + "/Tools/BetterMapBuilder/Saves", mapBuilder.mapName, "json");
            if (string.IsNullOrEmpty(path))
                return;
            var data = new BrushesData(mapBuilder.Brushes);
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }
        
        private void LoadBrushes(BetterMapBuilder mapBuilder)
        {
            var path = EditorUtility.OpenFilePanel("Brushes Data", Application.dataPath + "/Tools/BetterMapBuilder/Saves", "json");
            if (string.IsNullOrEmpty(path))
                return;
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<BrushesData>(json);
            mapBuilder.Brushes = data.Brushes;
        }
        
        private void SaveMap(BetterMapBuilder mapBuilder)
        {
            var path = EditorUtility.SaveFilePanel(mapBuilder.mapName, Application.dataPath + "/Tools/BetterMapBuilder/Saves", mapBuilder.mapName, "map");
            if (string.IsNullOrEmpty(path))
                return;
            var data = new MapData(mapBuilder.Map, mapBuilder.mapSize, mapBuilder.tileSize);
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }
        
        private void LoadMap(BetterMapBuilder mapBuilder, string mapName)
        {
            var path = EditorUtility.OpenFilePanel(mapName, Application.dataPath + "/Tools/BetterMapBuilder/Saves", "map");
            if (string.IsNullOrEmpty(path))
                return;
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<MapData>(json);
            mapBuilder.Map = data.Map;
            mapBuilder.mapSize = data.mapSize;
            mapBuilder.tileSize = data.tileSize;
        }
        
        private string GetSavedMaps()
        {
            var path = Application.dataPath + "/Tools/BetterMapBuilder/Saves";
            var files = Directory.GetFiles(path, "*.map");
            var mapNames = new List<string>();
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                mapNames.Add(fileName);
            }
            return string.Join(",", mapNames);
        }
    }
}