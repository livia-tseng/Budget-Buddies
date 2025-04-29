using UnityEngine;
using UnityEngine.UI;

public class ButtonFixer : MonoBehaviour
{
    public Button loginButton;

    void Start()
    {
        // Ensure the button is interactable when the game starts
        if (loginButton != null)
        {
            loginButton.interactable = true;
            loginButton.enabled = true;
        }
    }
}
