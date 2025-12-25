using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

namespace ProjectCore.UI
{
    public class LoadingToolTip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TooltipText;
        [SerializeField] private string[] ToolTips;

        private void Start()
        {
            //Temp Script to Randomly Show ToolTip at Start: Replace with your own
            int randomTextIndex = Random.Range(0, ToolTips.Length);
            TooltipText.text = ToolTips[randomTextIndex];
        }

    }
}
