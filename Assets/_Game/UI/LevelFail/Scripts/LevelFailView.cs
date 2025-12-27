using ProjectCore.Events;
using ProjectCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.UI
{
    public class LevelFailView : UIPanelInAndOut
    {
        [Header("Buttons")]
        [SerializeField] private Button RestartButton;

        [Header("Events (Output)")]
        [SerializeField] private GameEventWithInt LevelFailViewClosedEvent;

        private void OnEnable()
        {
            RestartButton.onClick.AddListener(OnRestartClicked);
        }

        private void OnDisable()
        {
            RestartButton.onClick.RemoveListener(OnRestartClicked);
        }
        
        private void OnRestartClicked()
        {
            SendCloseReason(UICloseReasons.Game);
        }

        private void SendCloseReason(UICloseReasons reason)
        {
            if (LevelFailViewClosedEvent != null)
            {
                LevelFailViewClosedEvent.Invoke((int)reason); 
            }
        }
    }
}
