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
        
        private readonly GridLevels _gridLevels;
        private readonly UpgradesManager _upgradesManager;
        
        [Serializable]
        private struct GameData
        { 
            public GridData Grid;
            public ResourceData Resource;
            public UpgradesData Upgrades;
            
            public int CurrentQuestIndex;
            public int CurrentQuestProgress;
            public DateTime CloseTime;
        }
        
        public SaveController(LifeController lifeController, GameGrid gameGrid, GridLevels gridLevels, UpgradesManager upgradesManager, ResourceManager resourceManager, QuestManager questManager)
        {
            _lifeController = lifeController;
            _gameGrid = gameGrid;
            _resourceManager = resourceManager;
            _questManager = questManager;
            _gridLevels = gridLevels;
            _upgradesManager = upgradesManager;
            
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
                Upgrades = UpgradesData.k_defaultData,
                CurrentQuestIndex = 0,
                CurrentQuestProgress = 0,
                CloseTime = DateTime.Now
            };
            
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                gameData = JsonUtility.FromJson<GameData>(json);
            }
            
            _resourceManager.Data = gameData.Resource;
            _upgradesManager.Data = gameData.Upgrades;
            _questManager.CurrentQuestIndex = gameData.CurrentQuestIndex;
            _questManager.CurrentQuestProgress = gameData.CurrentQuestProgress;
            
            LoadGameGrid(gameData);
            
            _resourceManager.HandleLaunch(gameData.CloseTime);
        }

        private void LoadGameGrid(GameData gameData)
        {
            var gridParams = _gridLevels.levels[_upgradesManager.Data.gridLevel];
            _gameGrid.Resize(gridParams.size, gridParams.backgroundPosition);
            
            foreach (var obj in gameData.Grid.objects)
                _gameGrid.AddObject(obj);
            
            if (
                gameData.Grid.objects.Count == 0 && 
                _gameGrid.GetRandomPosition(out Vector2Int position)
                )
                _gameGrid.AddObject(new GridObjectData
                {
                    type = 0,
                    position = position
                });
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
                Upgrades = _upgradesManager.Data,
                CurrentQuestIndex = _questManager.CurrentQuestIndex,
                CurrentQuestProgress = _questManager.CurrentQuestProgress,
                CloseTime = DateTime.Now
            }, prettyPrint);
            
            File.WriteAllText(SavePath, json);
        }
    }
}