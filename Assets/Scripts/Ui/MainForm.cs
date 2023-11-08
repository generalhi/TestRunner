using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class MainForm : MonoBehaviour
    {
        public Button ButtonExit;
        public Button ButtonUp;
        public Button ButtonDown;

        public Action OnButtonUp;
        public Action OnButtonDown;

        private void Awake()
        {
            ButtonExit.onClick.AddListener(OnButtonExitHandler);
            ButtonUp.onClick.AddListener(OnButtonUpHandler);
            ButtonDown.onClick.AddListener(OnButtonDownHandler);
        }

        private void OnButtonExitHandler()
        {
            Application.Quit();
        }

        private void OnButtonUpHandler()
        {
            OnButtonUp?.Invoke();
        }

        private void OnButtonDownHandler()
        {
            OnButtonDown?.Invoke();
        }
    }
}
