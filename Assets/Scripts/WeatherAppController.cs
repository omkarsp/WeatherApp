using UnityEngine;
using UnityEngine.UI;
using System;
using ToastSnackbarPackage;  // Add this to use the Toast package

namespace WeatherApp
{
    public class WeatherAppController : MonoBehaviour
    {
        [SerializeField] private Button refreshButton;
        
        private LocationManager locationManager;
        private WeatherManager weatherManager;
        
        private async void Start()
        {
            // Create ToastManager if it doesn't exist
            if (ToastSnackbarManager.Instance == null)
            {
                GameObject managerObject = new GameObject("Toast Manager");
                managerObject.AddComponent<ToastSnackbarManager>();
            }

            locationManager = LocationManager.Instance;
            weatherManager = WeatherManager.Instance;
            
            // Set up event listeners
            locationManager.OnLocationUpdated += HandleLocationUpdated;
            locationManager.OnLocationError += HandleLocationError;
            weatherManager.OnWeatherDataReceived += HandleWeatherDataReceived;
            weatherManager.OnWeatherError += HandleWeatherError;
            
            // Set up refresh button
            if (refreshButton != null)
            {
                refreshButton.onClick.AddListener(RefreshWeatherData);
            }
            
            // Initialize location service
            await locationManager.InitializeLocationService();
        }

        private async void HandleLocationUpdated(LocationInfo location)
        {
            string locationMessage = $"Location: {location.latitude:F2}°, {location.longitude:F2}°";
            ToastSnackbarManager.Instance.ShowMessage(locationMessage);
            
            await weatherManager.FetchWeatherData(location.latitude, location.longitude);
        }

        private void HandleWeatherDataReceived(WeatherResponse weatherData)
        {
            float currentTemp = weatherData.daily.temperature_2m_max[0];
            string weatherMessage = $"Current Temperature: {currentTemp}°C";
            ToastSnackbarManager.Instance.ShowMessage(weatherMessage);
        }

        private void HandleLocationError(string error)
        {
            ToastSnackbarManager.Instance.ShowMessage($"Location Error: {error}");
        }

        private void HandleWeatherError(string error)
        {
            ToastSnackbarManager.Instance.ShowMessage($"Weather Error: {error}");
        }

        private async void RefreshWeatherData()
        {
            if (locationManager.IsLocationServiceEnabled)
            {
                await locationManager.InitializeLocationService();
            }
            else
            {
                ToastSnackbarManager.Instance.ShowMessage("Please enable location services");
            }
        }

        private void OnDestroy()
        {
            if (locationManager != null)
            {
                locationManager.OnLocationUpdated -= HandleLocationUpdated;
                locationManager.OnLocationError -= HandleLocationError;
            }
            
            if (weatherManager != null)
            {
                weatherManager.OnWeatherDataReceived -= HandleWeatherDataReceived;
                weatherManager.OnWeatherError -= HandleWeatherError;
            }

            if (refreshButton != null)
            {
                refreshButton.onClick.RemoveListener(RefreshWeatherData);
            }
        }
    }
}