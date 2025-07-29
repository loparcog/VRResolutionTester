using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomManager : MonoBehaviour
{
    public InputActionReference triggerButton;
    public InputActionReference primaryButton;
    public MeshRenderer mesh;
    private bool defaultMeshState = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Subscribe to events
        triggerButton.action.started += TriggerButtonPressed;
        triggerButton.action.canceled += TriggerButtonReleased;
        primaryButton.action.started += PrimaryButtonPressed;
    }

    // Trigger button events to show/hide the screen
    void TriggerButtonPressed(InputAction.CallbackContext context)
    {
        mesh.enabled = !defaultMeshState;
    }

    void TriggerButtonReleased(InputAction.CallbackContext context)
    {
        mesh.enabled = defaultMeshState;
    }

    void PrimaryButtonPressed(InputAction.CallbackContext context)
    {
        // Enable/disable the mesh permanently
        defaultMeshState = !defaultMeshState;
        mesh.enabled = defaultMeshState;
    }

}
