using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputAction : MonoBehaviour
{
    public InputActionReference customButton;
    public MeshRenderer mesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customButton.action.started += ButtonPressed;
        customButton.action.canceled += ButtonReleased;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ButtonPressed(InputAction.CallbackContext context)
    {
        mesh.enabled = false;
    }

    void ButtonReleased(InputAction.CallbackContext context)
    {
        mesh.enabled = true;
    }
}
