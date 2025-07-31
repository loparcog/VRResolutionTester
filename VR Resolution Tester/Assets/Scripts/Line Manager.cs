using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class LineManager : MonoBehaviour
{
    // Line Group GameObjects to duplicate
    [SerializeField] public GameObject lineGroup;
    [SerializeField] public GameObject inverseLineGroup;
    // Actions to select a smaller or larger line pair
    [SerializeField] public InputActionReference selectSmallerAction;
    [SerializeField] public InputActionReference selectLargerAction;
    [SerializeField] public TextMeshPro selectedText;
    // Scaling array, determines the number of duplicated line groups
    private double[] scalingFactors = { 1, 0.8, 0.64, 0.51, 0.41, 0.33, 0.26, 0.21, 0.17, 0.13, 0.11, 0.09, 0.07, 0.05, 0.04, 0.03 };
    // Object to hold references to newly created line group GameObjects
    private Dictionary<double, GameObject> lineGroupDict = new Dictionary<double, GameObject>();
    // Currently selected index
    private int highlightIdx = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set up functions for the select action triggers
        selectSmallerAction.action.performed += SelectNextLP;
        selectLargerAction.action.performed += SelectLastLP;
        // Create the loop to duplicate over the scaling factors
        bool useInverse = false;
        foreach (double f in scalingFactors)
        {
            GameObject newLineGroup;
            // Determine which copy we're using
            if (useInverse)
            {
                newLineGroup = Instantiate(inverseLineGroup);
            }
            else
            {
                newLineGroup = Instantiate(lineGroup);
            }
            // Scale the copy by the current factor
            newLineGroup.transform.localScale = new Vector3((float)f, (float)f, (float)f);
            // Update the text to reflect the new size
            newLineGroup.GetNamedChild("Text").GetComponent<TextMeshPro>().text = string.Format("{0:0.00}", 5 * f);
            // Add this to the list
            lineGroupDict.Add(f, newLineGroup);
            // Flip the inverse flag
            useInverse = !useInverse;
        }
        // Set the largest line to be highlighted
        highlightLP();
    }
    void SelectNextLP(InputAction.CallbackContext context)
    {
        if (highlightIdx == scalingFactors.Length - 1) return;
        // Set the current LP to stop being highlighted
        lineGroupDict[scalingFactors[highlightIdx]].GetComponent<HighlightChildren>().clearLines();
        // Set the next LP to be highlighted
        highlightIdx += 1;
        highlightLP();
    }

    void SelectLastLP(InputAction.CallbackContext context)
    {
        if (highlightIdx == 0) return;
        // Set the current LP to stop being highlighted
        lineGroupDict[scalingFactors[highlightIdx]].GetComponent<HighlightChildren>().clearLines();
        // Set the previous LP to be highlighted
        highlightIdx -= 1;
        highlightLP();
    }

    void highlightLP()
    {
        // Highlight the current line
        lineGroupDict[scalingFactors[highlightIdx]].GetComponent<HighlightChildren>().highlightLines();
        // Update the text to reflect this change (TODO)
    }
}
