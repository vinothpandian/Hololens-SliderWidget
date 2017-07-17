Hololens-Slider : 3D Slider widget for the Hololens
======
_Mini Project for Designing Interactive Systems, RWTH_

## Table of Contents
[Unity asset package](unity-asset-package)

[Getting Started](getting-started)
* [Importing Assets](importing-assets)
* [Setting up the scene for Hololens development](setting-up-the-scene-for-hololens-development)
* [Using the Holotoolkit Input and Cursor Manager](using-the-holotoolkit-input-and-cursor-manager)
* [Using the Hololens Slider widget](using-the-hololens-slider-widget)
* [Building and Deploying the HoloLens Application in Hololens Emulator](building-and-deploying-the-hololens-application-in-hololens-emulator)

## Unity asset package
Please find the recent Unity asset package in [Release folder](https://github.com/vinothpandian/Hololens-SliderWidget/tree/master/Release)

## Getting Started

### Importing Assets
 - Download the latest [Holotoolkit ](https://github.com/Microsoft/HoloToolkit-Unity/tree/master/External/Unitypackages) and [Hololens-Slider](https://github.com/vinothpandian/Hololens-SliderWidget/tree/master/Release) Unity asset packages.
 - Right click the **Assets** folder in the **Project** panel.
 - Click on **Import Package > Custom Package**.
 - Navigate to the project files you downloaded and click on **Holotoolkit-Unity-vX.unitypackage**.
 - Do the same as above and now select **Hololens-Slider.unitypackage**
 - Now you have imported all the necessary assets to use the Hololens slider widget

### Setting up the scene for Hololens development

 - Select **File > Save Scenes** and save your current scene
 - In menu bar select **HoloToolkit > Configure > Apply HoloLens Project Settings** and choose **Apply**
 - Unity will prompt for a  **Project reload**. Choose **Yes**
 - Wait till unity reloads the project
 - In menu bar select **HoloToolkit > Configure > Apply HoloLens Scene Settings** and choose **Apply**
 - Now you have successfully setup the Unity project for HoloLens development

### Using the Holotoolkit Input and Cursor Manager

 - In Project panel,
 - Type "inputmanager". Select the **prefab "InputManager"** and drag-drop it to the Scene.
 - Type "cursorwith". Select the **prefab "CursorWithFeedback"** and drag-drop it to the Scene.
 - Now you have successfully setup the Input and Cursor Manager of Holotoolkit

### Using the Hololens Slider widget

 - In Project panel,
 - Type "hololens-slider". Select the **prefab "Hololens-Slider"** and drag-drop it to the Scene.
 - Select the GameObject that should be connected with the Hololens Slider.
 - In the Inspector Panel, choose **Add Component**
 - Type "sliderconnection" and click on the "**Slider Connection" script** to add it.
 - Drag the **Hololens-Slider** from the Scene and drop it on the Slider option in **Slider Connection** script
 - Now in the GameObject script you can use **SliderConnection.Value** to get the Slider widge current value

### Building and Deploying the HoloLens Application in Hololens Emulator
 - Select **File > Build Settings** and select **Windows Store** option.
 - Choose **Switch platform** if not chosen already. Also select **Unity C# Projects** in Debugging.
 - Click **Build**.
 - Create a **New Folder** named "App".
 - Single click the **App Folder**.
 - Press **Select Folder**.
 - When Unity is done, a File Explorer window will appear.
 - Open the **App** folder in file explorer.
 - Open the generated **Visual Studio solution**
 - Using the top toolbar in Visual Studio, change the target from Debug to **Release** and from ARM to **X86**.
	 - Click on the arrow next to the Device button, and select **HoloLens Emulator**.
	 - Click **Debug -> Start Without debugging** or press **Ctrl + F5**.
	 - After some time the emulator will start with your project.
