using JetBrains.Annotations;
using UnityEngine;

public class HighlightChildren : MonoBehaviour
{
    [SerializeField]public Material baseMat;
    [SerializeField]public Material highlightMat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void highlightLines()
    {
        // Loop through all child renderers
        foreach (Transform child in transform)
        {
            // Ignore any text
            if (child.name == "Text") continue;
            // Change the texture to the highlight texture
            child.GetComponent<Renderer>().sharedMaterial = highlightMat;
        }
    }

    public void clearLines()
    {
        // Loop through all child renderers
        foreach (Transform child in transform)
        {
            // Ignore any text
            if (child.name == "Text") continue;
            // Change the texture back to the regular texture
            child.GetComponent<Renderer>().sharedMaterial = baseMat;
        }
    }
}
