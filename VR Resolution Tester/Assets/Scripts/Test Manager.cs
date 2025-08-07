using System.IO;
using NUnit.Framework.Constraints;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class TestManager : MonoBehaviour
{
    // Camera objects
    [SerializeField] public GameObject staticCamera;
    [SerializeField] public GameObject XRCamera;
    // Scene prefabs
    [SerializeField] public GameObject startScreen;
    [SerializeField] public GameObject dynamicScreen;
    [SerializeField] public GameObject endScreen;
    [SerializeField] public GameObject horizontalLP;
    [SerializeField] public Material highlightMaterial;
    // Controller input actions
    [SerializeField] public InputActionReference primaryButton;
    [SerializeField] public InputActionReference secondaryButton;
    [SerializeField] public InputActionReference triggerButton;
    [SerializeField] public InputActionReference joystickUp;
    [SerializeField] public InputActionReference joystickDown;
    [SerializeField] public bool logData = false;
    // Scenes, in order
    private string[] scenes = { "scene_start", "lp_horizontal", "lp_vertical", "lp_diagonal", "scene_dynamic",  "lp_horizontal", "lp_vertical", "lp_diagonal", "scene_end"};
    private int sceneIndex = 0;
    // Information on the current line scaling
    private float currentScale = 1;
    private bool fineZoom = false;
    private GameObject currentScene;
    // Data for screenshotting and file writing
    private string UUID = System.Guid.NewGuid().ToString();
    private string filePath = "VRRT Data\\VRRTData.txt";
    private string dirName = "VRRT Data";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create bindings for the primary buttons
        primaryButton.action.performed += NextScene;
        joystickUp.action.performed += IncreaseLPSize;
        joystickDown.action.performed += DecreaseLPSize;        
        triggerButton.action.started += FineTuneEnabled;
        triggerButton.action.canceled += FineTuneDisabled;
        // Initialize log information if needed
        if (logData)
        {
            // Make sure the screenshot folder and text document exists
            if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    fs.Close();
                }
            }
            // Write the UUID to the text file
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(UUID);
                sw.WriteLine("STATIC:");
            }
        }
        // Show the start screen
        currentScene = Instantiate(startScreen);
    }

    public void NextScene(InputAction.CallbackContext context)
    {

        // Iterate to the next scene if possible
        if (sceneIndex == scenes.Length - 1)
        {
            // End of scenes, don't progress
            return;
        }

        var sceneName = scenes[sceneIndex].Split("_");

        // Take a screenshot if in a line pair scene
        if (logData & sceneName[0] == "lp")
        {
            // Screenshot the current camera view
            ScreenCapture.CaptureScreenshot("VRRT Data\\" + UUID + "-" + sceneIndex + ".png");
            // Write the current data to the text document
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(sceneName[1] + ": " + currentScale.ToString("F3") + "mm");
            }
        }
        // Destroy the existing scene
        Destroy(currentScene);
        // Iterate the scene index
        sceneIndex += 1;
        sceneName = scenes[sceneIndex].Split("_");
        // Reset the scale
        currentScale = 1;
        // Set up the new scene
        if (sceneName[0] == "lp")
        {
            drawNewLPs(sceneName);
        }
        else
        {
            drawNewMenu(sceneName);
        }
    }

    private void drawNewLPs(string[] sceneName)
    {
        // Create the static lines
        // (Five line pairs with differenting sizes)
        currentScene = new GameObject();
        currentScene.name = "Line Pairs";
        for (int i = -2; i < 3; i++)
        {
            var linePair = Instantiate(horizontalLP);
            var scale = (float)(1 + (i * 0.1));
            linePair.transform.localScale = new Vector3(1, 1, scale);
            linePair.transform.Translate(0, 0, i * (scale * 8));
            // Highlight the middle pair
            if (i == 0)
            {
                foreach (Transform child in linePair.transform)
                {
                    child.GetComponent<Renderer>().sharedMaterial = highlightMaterial;
                }
            }
            linePair.transform.parent = currentScene.transform;
        }
        // Change the scene based on the setup
        switch (sceneName[1])
        {
            case "horizontal":
                // No rotation needed
                break;
            case "vertical":
                currentScene.transform.Rotate(0, 90, 0);
                break;
            case "diagonal":
                currentScene.transform.Rotate(0, 45, 0);
                break;
        }
    }

    private void drawNewMenu(string[] sceneName)
    {
        // Show the right menu based on the name
        switch (sceneName[1])
        {
            case "dynamic":
                // Show the menu and enable head tracking
                currentScene = Instantiate(dynamicScreen);
                staticCamera.SetActive(false);
                XRCamera.SetActive(true);
                // Also log this change
                if (logData)
                {
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.WriteLine("DYNAMIC:");
                    }
                }
                break;
            case "end":
                currentScene = Instantiate(endScreen);
                break;
        }
    }

    void IncreaseLPSize(InputAction.CallbackContext context)
    {
        // Make sure its a valid scene
        if (sceneIndex == 0 || sceneIndex == scenes.Length - 1) return;
        // Check for a fine zoom
        if (fineZoom)
        {
            currentScale += 0.01f;
        }
        else
        {
            currentScale += 0.1f;
        }
        // Limit zoom out
        if (currentScale > 1f) currentScale = 1f;
        currentScene.transform.localScale = new Vector3(1, 1, currentScale);
    }

    void DecreaseLPSize(InputAction.CallbackContext context)
    {
        if (sceneIndex == 0 || sceneIndex == scenes.Length - 1) return;
        if (fineZoom)
        {
            currentScale -= 0.01f;
        }
        else
        {
            currentScale -= 0.1f;
        }
        // Limit zoom in
        if (currentScale < 0f) currentScale = 0f;
        currentScene.transform.localScale = new Vector3(1, 1, currentScale);

    }
    
    void FineTuneEnabled(InputAction.CallbackContext context)
    {
        fineZoom = true;
    }
    void FineTuneDisabled(InputAction.CallbackContext context)
    {
        fineZoom = false;
    }

}
