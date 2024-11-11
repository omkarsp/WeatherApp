using UnityEngine;
using System;
using System.Threading.Tasks;

namespace WeatherApp
{
    /// <summary>
    /// handles location permissions and get the user's latitude and longitude.
    /// </summary>
    public class LocationManager : MonoBehaviour
    {
        public static LocationManager Instance { get; private set; }

        private bool isInitialized = false;
        public bool IsLocationServiceEnabled
        {
            get
            {
#if UNITY_EDITOR
                return true; // Always return true in editor
#else
                return Input.location.isEnabledByUser;
#endif
            }
        }

        public event Action<string> OnLocationError;
        public event Action<LocationInfo> OnLocationUpdated;

        private AndroidPermissionHandler permissionHandler;

        // Editor simulation values
#if UNITY_EDITOR
        private float simulatedLatitude = 19.2f;//19.076f;  // Mumbai latitude
        private float simulatedLongitude = 77.31f;//72.877f; // Mumbai longitude
#endif

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                // Create AndroidPermissionHandler
                GameObject handlerObject = new GameObject("PermissionHandler");
                permissionHandler = handlerObject.AddComponent<AndroidPermissionHandler>();
                handlerObject.transform.SetParent(transform);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private async Task<bool> CheckAndRequestPermissions()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Request location permission using the local reference
            bool hasPermission = await permissionHandler.RequestLocationPermission();
            
            if (!hasPermission)
            {
                OnLocationError?.Invoke("Location permission denied");
                return false;
            }

            // Check if location is enabled in device settings
            if (!Input.location.isEnabledByUser)
            {
                // Show dialog to open location settings
                ShowLocationSettingsDialog();
                return false;
            }
#endif

            return true;
        }

        private void ShowLocationSettingsDialog()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (CustomDialog.Instance != null)
            {
                CustomDialog.Show("Location Required", 
                    "Please enable location services to use this app",
                    "Open Settings",
                    "Cancel",
                    () => permissionHandler.ShowLocationSettings(),
                    () => OnLocationError?.Invoke("Location services disabled")
                );
            }
            else
            {
                Debug.LogWarning("CustomDialog instance not found in scene");
                permissionHandler.ShowLocationSettings();
            }
#else
            Debug.Log("Location settings dialog is only available on Android devices");
#endif
        }

        public async Task InitializeLocationService()
        {
            if (!await CheckAndRequestPermissions())
            {
                return;
            }

#if UNITY_EDITOR
            // In Editor, just simulate the location
            isInitialized = true;
            GetLocation();
            return;
#endif

            Input.location.Start(1f, 1f);

            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                await Task.Delay(1000);
                maxWait--;
            }

            if (maxWait <= 0)
            {
                OnLocationError?.Invoke("Location service initialization timed out");
                Input.location.Stop();
                return;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                OnLocationError?.Invoke("Unable to determine device location");
                return;
            }

            isInitialized = true;
            GetLocation();
        }

        private void GetLocation()
        {
            if (!isInitialized) return;

#if UNITY_EDITOR
            // Start location service directly in editor mode
            Input.location.Start();

            // Create a simulated location by starting the service
            // This will create a real LocationInfo struct that we can modify
            if (Input.location.status == LocationServiceStatus.Running)
            {
                Debug.Log($"Simulating location: Lat {simulatedLatitude}, Long {simulatedLongitude}");
                // We'll just pass the coordinates directly to the weather manager
                // since we can't modify LocationInfo
                if (OnLocationUpdated != null)
                {
                    WeatherManager.Instance.FetchWeatherData(simulatedLatitude, simulatedLongitude);
                }
            }
#else
            LocationInfo locationInfo = Input.location.lastData;
            OnLocationUpdated?.Invoke(locationInfo);
#endif
        }

        public void SetSimulatedLocation(float latitude, float longitude)
        {
#if UNITY_EDITOR
            simulatedLatitude = latitude;
            simulatedLongitude = longitude;
            if (isInitialized)
            {
                GetLocation();
            }
#endif
        }

        private void OnDestroy()
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                Input.location.Stop();
            }
        }
    }
}