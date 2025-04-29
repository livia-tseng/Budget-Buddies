using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class CreateAccountScreenManager : MonoBehaviour
{
    // Method to handle login button click
    public void OnLoginButtonClicked()
    {
        // Load the login scene (replace "LoginScene" with your actual login scene name)
        SceneManager.LoadScene("LoginScene");
    }

    // Method to handle new account button click
    public void OnNewAccountButtonClicked()
    {
        // Load the create account scene (replace "CreateAccountScene" with your actual scene name)
        SceneManager.LoadScene("CreateAccountScene");
    }
}
