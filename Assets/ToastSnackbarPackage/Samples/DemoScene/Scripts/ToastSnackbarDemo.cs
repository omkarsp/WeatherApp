using UnityEngine;
using UnityEngine.UI;

namespace ToastSnackbarPackage.Demo
{
    public class ToastSnackbarDemo : MonoBehaviour
    {
        [SerializeField] private Button messageButton;
        [SerializeField] private InputField messageInput;  // Changed to regular InputField

        private void Start()
        {
            // Create ToastManager if it doesn't exist
            if (ToastSnackbarManager.Instance == null)
            {
                GameObject managerObject = new GameObject("Toast Manager");
                managerObject.AddComponent<ToastSnackbarManager>();
            }

            if (messageButton != null)
            {
                messageButton.onClick.AddListener(ShowMessage);
            }
        }

        private void ShowMessage()
        {
            if (ToastSnackbarManager.Instance == null)
            {
                Debug.LogError("ToastSnackbarManager instance not found!");
                return;
            }

            if (messageInput != null && !string.IsNullOrEmpty(messageInput.text))
            {
                ToastSnackbarManager.Instance.ShowMessage(messageInput.text);
            }
            else
            {
                ToastSnackbarManager.Instance.ShowMessage("Test Message");
            }
        }

        private void OnDestroy()
        {
            if (messageButton != null)
            {
                messageButton.onClick.RemoveListener(ShowMessage);
            }
        }
    }
}