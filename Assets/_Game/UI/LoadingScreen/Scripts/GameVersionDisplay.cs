using TMPro;
using UnityEngine;

namespace ProjectCore.UI.ProjectCore.UI
{
    public class GameVersionDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI VersionText;
        [SerializeField] private string Prefix = "v";

        private void Start()
        {
            if (VersionText != null)
            {
                VersionText.text = Prefix + Application.version;
            }
        }
    }
}