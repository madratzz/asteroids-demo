using ProjectCore.Events;
using TMPro;
using UnityEngine;
using ProjectCore.Variables;

namespace ProjectGame.Features.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class IntVariableBinder : MonoBehaviour
    {
        [Header("Data Source")]
        [SerializeField] private Int DataVariable;
        [SerializeField] private GameEvent ValueChangedEvent;
        
        [Header("Formatting")]
        [SerializeField] private string Format = "{0}"; // e.g. "Score: {0}" or "Lives: {0}"

        private TextMeshProUGUI _targetText;

        private void Awake()
        {
            _targetText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            if (DataVariable == null) return;
            
            ValueChangedEvent.Handler+=(UpdateUI);
            UpdateUI();
        }

        private void OnDisable()
        {
            if (DataVariable == null) return;
            
            ValueChangedEvent.Handler-=(UpdateUI);
        }

        private void UpdateUI()
        {
            _targetText.text = string.Format(Format, DataVariable.GetValue());
        }
    }
}