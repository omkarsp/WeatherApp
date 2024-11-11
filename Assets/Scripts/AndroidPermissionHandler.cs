using UnityEngine;
using UnityEngine.Android;
using System;
using System.Threading.Tasks;

public class AndroidPermissionHandler : MonoBehaviour
{
    private TaskCompletionSource<bool> permissionTCS;

    public async Task<bool> RequestLocationPermission()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            permissionTCS = new TaskCompletionSource<bool>();

            Permission.RequestUserPermission(Permission.FineLocation);

            // Register callback for permission result
            var callback = new PermissionCallbacks();
            callback.PermissionDenied += (string permission) =>
            {
                permissionTCS.SetResult(false);
            };
            callback.PermissionGranted += (string permission) =>
            {
                permissionTCS.SetResult(true);
            };

            // Wait for user response
            return await permissionTCS.Task;
        }
        return true;
#else
        // In editor or other platforms, just return true
        return true;
#endif
    }

    public void ShowLocationSettings()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.settings.LOCATION_SOURCE_SETTINGS");
            currentActivity.Call("startActivity", intent);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error opening location settings: {e.Message}");
        }
#else
        Debug.Log("Location settings can only be opened on Android devices");
#endif
    }
}