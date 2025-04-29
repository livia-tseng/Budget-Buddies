using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro namespace
using System.Collections.Generic;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInput;  // Updated to TMP_InputField
    public TMP_InputField passwordInput;  // Updated to TMP_InputField
    public TextMeshProUGUI errorMessageText;  // Updated to TextMeshProUGUI
    public Button loginButton;
    public SceneChanger sceneChanger; // Reference to SceneChanger script

    private void Start()
    {
        // Ensure button is assigned and add the listener
        if (loginButton != null)
        {
            loginButton.onClick.AddListener(AttemptLogin);
        }
        else
        {
            Debug.LogError("Login Button is not assigned!");
        }

        // Ensure the error message text is initially hidden
        if (errorMessageText != null)
        {
            errorMessageText.gameObject.SetActive(false);
        }
    }

    public void AttemptLogin()
    {
        // Clear previous error messages
        ResetErrorMessage();

        // Validate inputs
        if (!ValidateInputs()) return;

        string email = emailInput.text;
        string password = passwordInput.text;

        // Load stored credentials from PlayerPrefs
        string storedEmail = PlayerPrefs.GetString("UserEmail", "");
        string storedPassword = PlayerPrefs.GetString("UserPassword", "");

        // Check if email and password match the stored credentials
        if (storedEmail == email && storedPassword == password)
        {
            Debug.Log("Login Successful! Redirecting to Homepage...");
            if (sceneChanger != null)
            {
                sceneChanger.LoadScene("HomeScene");
            }
            else
            {
                Debug.LogError("SceneChanger is not assigned!");
            }
        }
        else
        {
            ShowErrorMessage("Invalid email or password. Try again.");
        }
    }

    private bool ValidateInputs()
    {
        bool isValid = true;
        string errorMessage = "";

        if (string.IsNullOrEmpty(emailInput.text))
        {
            errorMessage += "Email is required.\n";
            isValid = false;
        }
        if (string.IsNullOrEmpty(passwordInput.text))
        {
            errorMessage += "Password is required.\n";
            isValid = false;
        }

        if (!isValid)
        {
            ShowErrorMessage(errorMessage);
        }

        return isValid;
    }

    private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
            errorMessageText.gameObject.SetActive(true);
        }
        Debug.LogWarning(message);
    }

    // Helper method to reset error message state
    private void ResetErrorMessage()
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = "";
            errorMessageText.gameObject.SetActive(false);
        }
    }
}
