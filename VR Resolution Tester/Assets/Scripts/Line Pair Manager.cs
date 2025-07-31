using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class LinePairManager : MonoBehaviour
{
    [SerializeField] public Material baseMat;
    [SerializeField] public Material highlightMat;
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

    public void updateScaling(float scale)
    {
        // Update the scale
        transform.localScale = new Vector3(scale, 0, scale);
        // Update the text
        transform.GetChild(6).GetComponent<TextMeshPro>().text = string.Format("{0:0.00}", 5 * scale);
    }
}
