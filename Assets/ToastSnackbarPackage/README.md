Download package: https://github.com/omkarsp/Unity-Toast-Snackbar-Package/releases/tag/v1.0

# Toast Snackbar Package

A Unity package for displaying Toast messages on Android and Snackbar notifications on iOS.

## Installation

### Via Unity Package Manager (Recommended)
1. Open the Package Manager window (Window → Package Manager)
2. Click the + button in the top-left corner
3. Select "Add package from git URL"
4. Enter the repository URL

### Via .unitypackage
1. Download the .unitypackage file
2. Import into your Unity project

## Basic Usage
1. Drag the `ToastManager` prefab from `Runtime/Prefabs` into your scene
   - You only need to do this once as the manager uses DontDestroyOnLoad
2. Show a message:
```csharp
ToastSnackbarManager.Instance.ShowMessage("Hello World!");
```

## Features
- Native Android Toast messages
- Native iOS Snackbar notifications
- Editor support for testing (shows Debug.Log)
- Easy to integrate

## Repository Structure
```plaintext
├── Runtime/
│   ├── ToastSnackbarPackage.asmdef
│   ├── ToastSnackbarManager.cs
│   └── Prefabs/
│       └── ToastManager.prefab
├── Tests/
│   ├── ToastSnackbarTests.asmdef
│   └── ToastSnackbarTests.cs
└── Samples~/
    └── DemoScene/
        ├── Scenes/
        │   └── ToastSnackbarDemo.unity
        └── Scripts/
            └── ToastSnackbarDemo.cs
```

## Testing
The package includes unit tests to verify functionality:

### Running Tests
1. Open Unity Test Runner (Window → General → Test Runner)
2. Select PlayMode tab
3. Click "Run All" or run specific tests

### Test Coverage
- ShowMessage functionality
- Null/empty message handling
- Singleton pattern validation
- Basic functionality tests

### Writing Additional Tests
Tests are located in the Tests folder. Add new test classes inheriting from `MonoBehaviour` and using the `[Test]` attribute.

## Example Scene
Check the Samples folder for a demo scene showing basic usage:
1. Basic message display
2. Input field for custom messages
3. Example implementation

## Requirements
- Unity 2019.1 or higher
- Android: API Level 19+
- iOS: iOS 11.0+

## Platform-Specific Details

### Android
Uses native Android Toast through AndroidJavaClass/AndroidJavaObject

### iOS
Uses native UIKit components through native bindings

### Unity Editor
Messages are logged to the console for testing

## Weather App Implementation
This package is used in the Weather App to display:
- Location updates
- Temperature information
- Error messages

## License
This package is licensed under the MIT License.