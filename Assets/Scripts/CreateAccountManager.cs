using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateAccountManager : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField firstNameInput;
    [SerializeField] private TMP_InputField lastNameInput;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private Button createAccountButton;
    [SerializeField] private Button loginButton;  // Add reference to Login Button
    [SerializeField] private SceneChanger sceneChanger;  // Reference to SceneChanger script

    private void Start()
    {
        // Set up listeners for the buttons
        if (createAccountButton != null)
        {
            createAccountButton.onClick.AddListener(OnCreateAccountClick);
        }

        if (loginButton != null)
        {
            loginButton.onClick.AddListener(OnLoginButtonClick);
        }
    }

    // Called when the "Create Account" button is clicked
    private void OnCreateAccountClick()
    {
        // Clear previous error message
        if (errorMessageText != null)
        {
            errorMessageText.text = "";
        }

        // Validate inputs and attempt to save user data
        if (ValidateInputs())
        {
            string firstName = firstNameInput.text;
            string lastName = lastNameInput.text;
            string email = emailInputField.text;
            string password = passwordInputField.text;

            Debug.Log($"Account Creation: {firstName} {lastName}, Email: {email}");

            // After successful validation, save user data and transition to the next scene
            if (SaveUserData(firstName, lastName, email, password))
            {
                // Load the MonthlyIncomeScreen or another scene
                sceneChanger.LoadScene("MonthlyIncomeScreen");
            }
        }
    }

    // Called when the "Login" button is clicked
    private void OnLoginButtonClick()
    {
        // Load the LoginScene when the login button is pressed
        sceneChanger.LoadScene("LoginScene");
    }

    // Method to validate user inputs
    private bool ValidateInputs()
    {
        bool isValid = true;

        // Validate first name
        if (string.IsNullOrEmpty(firstNameInput.text))
        {
            ShowErrorMessage("First name is required.");
            isValid = false;
        }

        // Validate last name
        if (string.IsNullOrEmpty(lastNameInput.text))
        {
            ShowErrorMessage("Last name is required.");
            isValid = false;
        }

        // Validate email
        if (string.IsNullOrEmpty(emailInputField.text))
        {
            ShowErrorMessage("Email is required.");
            isValid = false;
        }

        // Validate password (minimum 6 characters)
        if (string.IsNullOrEmpty(passwordInputField.text) || passwordInputField.text.Length < 6)
        {
            ShowErrorMessage("Password must be at least 6 characters.");
            isValid = false;
        }

        return isValid;
    }

    // Method to save user data (using PlayerPrefs for now)
    private bool SaveUserData(string firstName, string lastName, string email, string password)
    {
        // Save data to PlayerPrefs (email and password)
        PlayerPrefs.SetString("UserEmail", email);
        PlayerPrefs.SetString("UserPassword", password);
        PlayerPrefs.SetString("FirstName", firstName);
        PlayerPrefs.SetString("LastName", lastName);
        PlayerPrefs.Save();  // Save to persist the data

        Debug.Log($"Saved user data: {firstName} {lastName}, {email}, {password}");

        // If everything is valid, return true
        return true;
    }

    // Method to show error messages
    private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
        }
        Debug.LogWarning(message);  // Log to the console as well
    }
}
