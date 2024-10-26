using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Utils.Scenes;
using Utils.Singleton;

namespace Levels
{
    public class LevelsManager : DontDestroyMonoBehaviour
    {
        private const string LevelNamePattern = "Level{0}";

        [Range(0,20)]
        [SerializeField] private int _currentLevelIndex;


        private void Start()
        {
            ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));
            EventsController.Subscribe<EventModels.Game.TargetColorNodesFilled>(this, OnTargetColorNodesFilled);
        }

        private void OnTargetColorNodesFilled(EventModels.Game.TargetColorNodesFilled e)
        {
            _currentLevelIndex += 1;
            ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));
        }
    }
}