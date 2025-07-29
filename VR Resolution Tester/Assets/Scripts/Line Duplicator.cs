using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using System;

public class LineDuplicator : MonoBehaviour
{

    // Line Group GameObjects to duplicate
    [SerializeField] public GameObject lineGroup;
    [SerializeField] public GameObject inverseLineGroup;
    // Scaling array, determines the number of duplicated line groups
    private double[] scalingFactors = { 1, 0.8, 0.64, 0.51, 0.41, 0.33, 0.26, 0.21, 0.17, 0.13, 0.11, 0.09, 0.07, 0.05, 0.04, 0.03 };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            // Flip the inverse flag
            useInverse = !useInverse;
        }
    }
}
