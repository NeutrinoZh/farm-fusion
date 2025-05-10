using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SaveController
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
        
        private readonly ResourceManager _resourceManager;
        private readonly GameGrid _gameGrid;
        private readonly QuestManager _questManager;
        private readonly LifeController _lifeController;
        
        [Serializable]
        private struct GameData
        { 
            public GridData Grid;
            public ResourceData Resource;
            
            public int CurrentQuestIndex;
            public int CurrentQuestProgress;
            public DateTime CloseTime;
        }
        
        public SaveController(LifeController lifeController, GameGrid gameGrid, ResourceManager resourceManager, QuestManager questManager)
        {
            _lifeController = lifeController;
            _gameGrid = gameGrid;
            _resourceManager = resourceManager;
            _questManager = questManager;
            
            Load();

            lifeController.OnQuit += Save;
            lifeController.OnPause += Save;
        }

        ~SaveController()
        {
            _lifeController.OnQuit -= Save;
            _lifeController.OnPause -= Save;
        }
        
        private void Load()
        {
            GameData gameData = new()
            {
                Grid = GridData.k_defaultData,
                Resource = ResourceData.k_defaultData,
                CurrentQuestIndex = 0,
                CurrentQuestProgress = 0,
                CloseTime = DateTime.Now
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
            
            _gameGrid.Resize(gameData.Grid.size);
            
            foreach (var obj in objects)
                _gameGrid.AddObject(obj);

            _resourceManager.Data = gameData.Resource;
            _questManager.CurrentQuestIndex = gameData.CurrentQuestIndex;
            _questManager.CurrentQuestProgress = gameData.CurrentQuestProgress;
            _resourceManager.HandleLaunch(gameData.CloseTime);
        }

        private void Save()
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
                CurrentQuestProgress = _questManager.CurrentQuestProgress,
                CloseTime = DateTime.Now
            }, prettyPrint);
            
            File.WriteAllText(SavePath, json);
        }
    }
}