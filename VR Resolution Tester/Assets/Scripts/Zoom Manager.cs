using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomManager : MonoBehaviour
{
    public InputActionReference primaryButton;
    public MeshRenderer mesh;
    private bool zoomEnabled = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        primaryButton.action.started += PrimaryButtonPressed;
    }

    void PrimaryButtonPressed(InputAction.CallbackContext context)
    {
        // Enable/disable the mesh permanently
        zoomEnabled = !zoomEnabled;
        mesh.enabled = zoomEnabled;
    }

}
