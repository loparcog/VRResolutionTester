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
    [SerializeField] public TextMeshProUGUI lineSizeText;
    // Current line scaling
    private float currentLineScale = 1;
    private GameObject runtimeLineGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set up functions for the select action triggers
        increaseLineSizeAction.action.performed += IncreaseLPSize;
        decreaseLineSizeAction.action.performed += DecreaseLPSize;
        // Make the line group a real game object
        runtimeLineGroup = Instantiate(lineGroup);
        // Update the current text
        updateLineSize();
    }
    void IncreaseLPSize(InputAction.CallbackContext context)
    {
        if (currentLineScale == 1f) return;
        currentLineScale += 0.1f;
        updateLineSize();
    }

    void DecreaseLPSize(InputAction.CallbackContext context)
    {
        currentLineScale -= 0.1f;
        updateLineSize();
    }

    void updateLineSize()
    {
        // Update the line size
        runtimeLineGroup.GetComponent<LinePairManager>().updateScaling(currentLineScale);
        // Also update the text
        lineSizeText.text = string.Format("Selected: {0:0.00}mm", currentLineScale * 5);
    }
}
