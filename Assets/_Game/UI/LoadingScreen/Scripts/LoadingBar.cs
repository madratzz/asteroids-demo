using UnityEngine;
using UnityEngine.UI;
using ProjectCore.Variables;
using TMPro;

namespace ProjectCore.UI
{
    public class LoadingBar : MonoBehaviour
    {
        [SerializeField] private Image LoadingBarImage;
        [SerializeField] private Image LoadingBackgroundImage;

        [SerializeField] private Float SceneLoadingProgress;

        private float _currentProgress;
        private float _newProgress;

        public void HideLoading()
        {
            Destroy(gameObject, 0.5f);
        }

        private void Start()
        {
            LoadingBarImage.fillAmount = 0;
        }

        private void Update()
        {
            _newProgress = Mathf.Clamp01(SceneLoadingProgress.GetValue());
            if (!(_currentProgress < _newProgress)) return;
            
            _currentProgress = _newProgress;
            LoadingBarImage.fillAmount = _currentProgress;
        }
    }
}