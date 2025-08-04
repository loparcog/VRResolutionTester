using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LineScaler : MonoBehaviour
{
    // Line Group GameObjects to duplicate
    [SerializeField] public GameObject lineGroup;
    // Actions to select a smaller or larger line pair
    [SerializeField] public InputActionReference increaseLineSizeAction;
    [SerializeField] public InputActionReference decreaseLineSizeAction;
    [SerializeField] public InputActionReference fineTuneAction;
    [SerializeField] public TextMeshProUGUI lineSizeText;
    // Current line scaling
    private float currentLineScale = 1;
    // Fine tune toggle
    private bool fineZoom = false;
    // Line group object to reference at runtime
    private GameObject runtimeLineGroup;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set up functions for the select action triggers
        increaseLineSizeAction.action.performed += IncreaseLPSize;
        decreaseLineSizeAction.action.performed += DecreaseLPSize;
        fineTuneAction.action.started += FineTuneEnabled;
        fineTuneAction.action.canceled += FineTuneDisabled;
        // Make the line group a real game object
        runtimeLineGroup = Instantiate(lineGroup);
        // Grab the last selected line scale
        currentLineScale = (float)LineManager.scalingFactors[LineManager.selectedIdx];
        // Update the current text
        updateLineSize();
    }
    void IncreaseLPSize(InputAction.CallbackContext context)
    {
        // Check for a fine zoom
        if (fineZoom)
        {
            currentLineScale += 0.01f;
        }
        else
        {
            currentLineScale += 0.1f;   
        }
        // Limit zoom out
        if (currentLineScale > 1f) currentLineScale = 1f;
        updateLineSize();
    }

    void DecreaseLPSize(InputAction.CallbackContext context)
    {
        if (fineZoom)
        {
            currentLineScale -= 0.01f;
        }
        else
        {
            currentLineScale -= 0.1f;   
        }
        // Limit zoom in
        if (currentLineScale < 0f) currentLineScale = 0f;
        updateLineSize();
    }

    void updateLineSize()
    {
        // Update the line size
        runtimeLineGroup.GetComponent<LinePairManager>().updateScaling(currentLineScale);
        // Also update the text
        lineSizeText.text = string.Format("Selected: {0:0.00}mm", currentLineScale * 5);
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
