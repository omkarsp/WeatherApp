using UnityEngine;

namespace ToastSnackbarPackage
{
    public class ToastSnackbarManager : MonoBehaviour
    {
        public static ToastSnackbarManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowMessage(string message)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            ShowAndroidToast(message);
#elif UNITY_IOS && !UNITY_EDITOR
            ShowIosSnackbar(message);
#else
            Debug.Log($"Message: {message}");
#endif
        }

        private void ShowAndroidToast(string message)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast"))
            {
                AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
                AndroidJavaObject toast = toastClass.CallStatic<AndroidJavaObject>("makeText", context, message, 0);
                toast.Call("show");
            }
        }

        private void ShowIosSnackbar(string message)
        {
#if UNITY_IOS && !UNITY_EDITOR
            ShowIosSnackbarNative(message);
#endif
        }

#if UNITY_IOS && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void ShowIosSnackbarNative(string message);
#endif
    }
}