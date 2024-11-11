# Unity Weather App

A Unity application that displays the current temperature based on user's location using Toast/Snackbar notifications.

## Features
- Gets user's current location (latitude/longitude)
- Fetches current temperature from Weather API
- Shows location and temperature using Toast/Snackbar notifications
- Works on both Android and iOS

## Dependencies
- [Unity Toast Snackbar Package](https://github.com/omkarsp/Unity-Toast-Snackbar-Package/releases/tag/v1.0)
- Unity 2020.3 or higher
- Android API Level 19+ (for Android builds)
- iOS 11.0+ (for iOS builds)

## Setup Instructions
1. Clone this repository
2. Open with Unity
3. Add Toast Snackbar package:
   - Open Package Manager (Window → Package Manager)
   - Click + → Add package from git URL
   - Enter: `https://github.com/omkarsp/Unity-Toast-Snackbar-Package`

## API Information
Using Open-Meteo API:
```
https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&timezone=IST&daily=temperature_2m_max
```

Sample response:
```json
{
    "latitude": 19.125,
    "longitude": 72.875,
    "timezone": "Asia/Calcutta",
    "daily": {
        "time": ["2022-11-29"],
        "temperature_2m_max": [32]
    }
}
```

## Building

### Android
1. Switch platform to Android (File → Build Settings → Android → Switch Platform)
2. Set minimum API level to 19
3. Add required permissions in Player Settings:
   - ACCESS_COARSE_LOCATION
   - ACCESS_FINE_LOCATION
   - INTERNET
4. Build APK (File → Build Settings → Build)

### iOS
1. Switch platform to iOS
2. Add required permissions in Player Settings:
   - Location Usage Description
   - Location When In Use Usage Description
3. Build Xcode project
4. Open in Xcode and run

## Testing
The project includes unit tests to verify functionality:
1. Open Test Runner (Window → General → Test Runner)
2. Click PlayMode tab
3. Click Run All

Tests cover:
- Location services
- Weather API integration
- Toast/Snackbar functionality

## Project Structure
```plaintext
Assets/
└── Scripts/
    ├── WeatherApp.asmdef
    ├── WeatherManager.cs     # Handles API calls
    ├── LocationManager.cs    # Handles location services
    └── WeatherAppController.cs   # Main controller
```

## Architecture
- Uses Singleton pattern for managers
- Event-based communication between components
- Platform-specific implementations for notifications
- Async/await for API calls