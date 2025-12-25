using ProjectCore.Events;
using ProjectCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.UI
{
    public class MainMenuView : UIPanelInAndOut
    {
        [Header("Buttons")]
        [SerializeField] private Button PlayButton;

        [Header("Events (Output)")]
        [SerializeField] private GameEventWithInt MainMenuViewClosed;

        private void OnEnable()
        {
            PlayButton.onClick.AddListener(OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            // Intent: "User wants to Play"
            SendCloseReason(UICloseReasons.Game);
        }

        private void SendCloseReason(UICloseReasons reason)
        {
            if (MainMenuViewClosed != null)
                MainMenuViewClosed.Invoke((int)reason);
        }
    }
}
