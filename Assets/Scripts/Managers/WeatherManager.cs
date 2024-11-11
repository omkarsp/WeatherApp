using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp
{
    public class WeatherManager : MonoBehaviour
    {
        public static WeatherManager Instance { get; private set; }
        private const string API_BASE_URL = "https://api.open-meteo.com/v1/forecast";

        public event Action<WeatherResponse> OnWeatherDataReceived;
        public event Action<string> OnWeatherError;

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

        public void OnLocationUpdated(LocationInfo location)
        {
            FetchWeatherData(location.latitude, location.longitude);
        }

        public async Task FetchWeatherData(float latitude, float longitude)
        {
            string url = $"{API_BASE_URL}?latitude={latitude}&longitude={longitude}&timezone=IST&daily=temperature_2m_max";
            Debug.Log($"Fetching weather data for: Lat {latitude}, Long {longitude}");

            try
            {
                using (UnityWebRequest request = UnityWebRequest.Get(url))
                {
                    var operation = request.SendWebRequest();

                    while (!operation.isDone)
                        await Task.Yield();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        string jsonResponse = request.downloadHandler.text;

                        //Debug.Log(jsonResponse);

                        WeatherResponse weatherData = JsonConvert.DeserializeObject<WeatherResponse>(jsonResponse);
                        OnWeatherDataReceived?.Invoke(weatherData);
                    }
                    else
                    {
                        string errorMessage = $"Weather API Error: {request.error}";
                        Debug.LogError(errorMessage);
                        OnWeatherError?.Invoke(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error fetching weather data: {ex.Message}";
                Debug.LogError(errorMessage);
                OnWeatherError?.Invoke(errorMessage);
            }
        }
    }
}