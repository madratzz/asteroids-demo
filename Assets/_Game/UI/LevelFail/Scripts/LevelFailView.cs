using ProjectCore.Events;
using ProjectCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.UI
{
    public class LevelFailView : UIPanelInAndOut
    {
        [Header("Buttons")]
        [SerializeField] private Button HomeButton;
        [SerializeField] private Button RestartButton;
        [SerializeField] private Button ReviveButton;

        [Header("Events (Output)")]
        [SerializeField] private GameEventWithInt LevelFailViewClosedEvent;

        private void OnEnable()
        {
            HomeButton.onClick.AddListener(OnHomeClicked);
            RestartButton.onClick.AddListener(OnRestartClicked);
            ReviveButton.onClick.AddListener(OnReviveClicked);
        }

        private void OnDisable()
        {
            HomeButton.onClick.RemoveListener(OnHomeClicked);
            RestartButton.onClick.RemoveListener(OnRestartClicked);
            ReviveButton.onClick.RemoveListener(OnReviveClicked);
        }

        private void OnHomeClicked()
        {
            SendCloseReason(UICloseReasons.Home); 
        }

        private void OnRestartClicked()
        {
            SendCloseReason(UICloseReasons.Game);
        }

        private void OnReviveClicked()
        {
            SendCloseReason(UICloseReasons.Revive);
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
