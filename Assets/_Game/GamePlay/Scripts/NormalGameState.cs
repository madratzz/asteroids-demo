using System.Collections;
using ProjectCore.Utilities;
using ProjectCore.Variables;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProjectCore.GamePlay
{
    [CreateAssetMenu(fileName = "NormalGameState", menuName = "ProjectCore/State Machine/States/Normal Game State")]
    public class NormalGameState : GameState
    {
        [Header("Level Configuration")]
        [SerializeField] private Int GameLevel;

        //GameStateEnvironment
        [SerializeField] private AssetReferenceGameObject LevelEnvironmentReference;
        
        public override IEnumerator Execute()
        {
            //Run Base (Loads HUD)
            yield return base.Execute();
            
            //Load the Gameplay Objects
            yield return InstantiateLevelObject();

            //Start the Game Flow
            GameStateStart.Invoke();
            LogLevelStartedEvent();
            
        }

        private IEnumerator InstantiateLevelObject()
        {
            return AddressablesHelper.InstantiateGameObject(
                LevelEnvironmentReference,
                (levelObj, handle) =>
                {
                    _levelObject = levelObj; // Stored in Base Class
                    _levelHandle = handle;   // Stored in Base Class
                },
                ()=> Debug.LogWarning("Failed to load level"), 
                this
            );
        }

        private void LogLevelStartedEvent()
        {
            Debug.Log($"[NormalGameState] Level {GameLevel.GetValue()} Started.");
        }
    }
}