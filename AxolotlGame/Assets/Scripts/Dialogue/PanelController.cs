using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    InputSystem_Actions action;

    private void Awake()
    {
        action = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        action.UI.Enable();

        action.UI.Submit.performed += OnSubmitPerformed;
    }

    private void OnDisable()
    {
        action.UI.Disable();

        action.UI.Submit.performed -= OnSubmitPerformed;
    }

    void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        DialogueManager.Instance.DisplayNextSentence();
    }
}
