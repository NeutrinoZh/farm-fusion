using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Serialization;

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
        private readonly AchievementManager _achievementManager;
        private readonly StatisticsManager _statisticsManager;

        private const int k_currentVersion = 1;
        
        [Serializable]
        private class GameData
        { 
            public int Version { get; set; }
            public GridData Grid { get; set; }
            public ResourceData Resource { get; set; }
            public UpgradesData Upgrades { get; set; }
            public AchievementsData Achievements { get; set; }
            public GameStatistics Statistics { get; set; }
            
            public int CurrentQuestIndex { get; set; }
            public int CurrentQuestProgress { get; set; }
            public DateTime CloseTime { get; set; }
        }
        
        public SaveController(
            LifeController lifeController,
            GameGrid gameGrid,
            GridLevels gridLevels,
            UpgradesManager upgradesManager,
            ResourceManager resourceManager,
            QuestManager questManager,
            AchievementManager achievementManager,
            StatisticsManager statisticsManager)
        {
            _lifeController = lifeController;
            _gameGrid = gameGrid;
            _resourceManager = resourceManager;
            _questManager = questManager;
            _gridLevels = gridLevels;
            _upgradesManager = upgradesManager;
            _achievementManager = achievementManager;
            _statisticsManager = statisticsManager;
            
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
                Version = k_currentVersion,
                Grid = GridData.k_defaultData,
                Resource = ResourceData.k_defaultData,
                Upgrades = UpgradesData.k_defaultData,
                Achievements = AchievementsData.k_defaultData,
                Statistics = GameStatistics.k_defaultData,
                CurrentQuestIndex = 0,
                CurrentQuestProgress = 0,
                CloseTime = DateTime.Now,
            };
            
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);

                var jObject = JObject.Parse(json);
                int version = jObject["Version"]?.Value<int>() ?? 0;


                if (version > 0 && version <= k_currentVersion)
                    JsonConvert.PopulateObject(json, gameData);
                else 
                    Debug.LogError("Invalid version of save file");
            }
            
            _resourceManager.Data = gameData.Resource;
            _upgradesManager.Data = gameData.Upgrades;
            _achievementManager.Data = gameData.Achievements;
            _questManager.CurrentQuestIndex = gameData.CurrentQuestIndex;
            _questManager.CurrentQuestProgress = gameData.CurrentQuestProgress;
            _statisticsManager.Data = gameData.Statistics;
            
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
            string json = JsonConvert.SerializeObject(new GameData()
            {
                Version = k_currentVersion,
                Grid = _gameGrid.Data,
                Resource = _resourceManager.Data,
                Upgrades = _upgradesManager.Data,
                Statistics = _statisticsManager.Data,
                Achievements = _achievementManager.Data,
                CurrentQuestIndex = _questManager.CurrentQuestIndex,
                CurrentQuestProgress = _questManager.CurrentQuestProgress,
                CloseTime = DateTime.Now
            }, prettyPrint ? Formatting.Indented : Formatting.None);
            
            File.WriteAllText(SavePath, json);
        }
    }
}