using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class LoaderGameGrid
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
        
        private readonly ResourceManager _resourceManager;
        private readonly GameGrid _gameGrid;
        private readonly QuestManager _questManager;
        
        [Serializable]
        private struct GameData
        { 
            public GridData Grid;
            public ResourceData Resource;
            
            public int CurrentQuestIndex;
            public int CurrentQuestProgress;
        }
        
        public LoaderGameGrid(GameGrid gameGrid, ResourceManager resourceManager, QuestManager questManager)
        {
            _gameGrid = gameGrid;
            _resourceManager = resourceManager;
            _questManager = questManager;
            
            Application.quitting += OnQuit;

            Load();
        }

        ~LoaderGameGrid()
        {
            Application.quitting -= OnQuit;
        }

        private void Load()
        {
            GameData gameData = new()
            {
                Grid = GridData.k_defaultData,
                Resource = ResourceData.k_defaultData,
                CurrentQuestIndex = 0,
                CurrentQuestProgress = 0
            };
            
            List<GridObjectData> objects = new();
            
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                gameData = JsonUtility.FromJson<GameData>(json);
                
                objects = gameData.Grid.objects.ToList();
            }
            else if (_gameGrid.GetRandomPosition(out Vector2Int position))
            {
               _gameGrid.AddObject(new GridObjectData { type = 0, position = position });
            }
            
            foreach (var obj in objects)
                _gameGrid.AddObject(obj);

            _resourceManager.Data = gameData.Resource;
            _questManager.CurrentQuestIndex = gameData.CurrentQuestIndex;
            _questManager.CurrentQuestProgress = gameData.CurrentQuestProgress;
        }

        private void OnQuit()
        {
        #if UNITY_EDITOR
            bool prettyPrint = true;
        #else 
            bool prettyPrint = false;
        #endif
            
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            string json = JsonUtility.ToJson(new GameData()
            {
                Grid = _gameGrid.Data,
                Resource = _resourceManager.Data,
                CurrentQuestIndex = _questManager.CurrentQuestIndex,
                CurrentQuestProgress = _questManager.CurrentQuestProgress
            }, prettyPrint);
            
            File.WriteAllText(SavePath, json);
        }
    }
}