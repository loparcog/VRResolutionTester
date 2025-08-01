using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    [SerializeField] public InputActionReference swapSceneAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add the action control
        swapSceneAction.action.performed += UpdateScene;
    }

    // Update is called once per frame
    void UpdateScene(InputAction.CallbackContext context)
    {
        // Load the next scene
        SceneManager.LoadScene(1);
        // Can also do
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
