using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class TestManager : MonoBehaviour
{
    // Scene Prefabs
    [SerializeField] public GameObject startScreen;
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
    private string[] scenes = { "start", "lp_horizontal", "lp_vertical", "lp_diagonal" };
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
        // Make sure the screenshot folder and text document exists
        if (logData)
        {
            if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    fs.Close();
                }
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
            // End of scenes, close the game
            return;
        }

        // Take a screenshot if needed
        if (logData & sceneIndex > 0)
        {
            // Screenshot the current camera view
            ScreenCapture.CaptureScreenshot("VRRT Data\\" + UUID + "-" + sceneIndex + ".png");
            // Write the current data to the text document
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(scenes[sceneIndex].Split("_")[1] + ": " + currentScale + "mm");
            }
        }
        // Store the data 
        // Destroy the existing scene
        Destroy(currentScene);

        sceneIndex += 1;
        // Reset the scale
        currentScale = 1;
        // Set up the new scene
        drawNewLPs();
    }

    private void drawNewLPs()
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
        switch (scenes[sceneIndex].Split("_")[1])
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

    void IncreaseLPSize(InputAction.CallbackContext context)
    {
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
