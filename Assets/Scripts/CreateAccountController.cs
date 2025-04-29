using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI; // Make sure this is added for UI Button support
using TMPro; // For TextMeshPro support

public class CreateAccountController : MonoBehaviour
{
    [SerializeField] private UserDataManager userDataManager;  // Reference to the UserDataManager Singleton
    [SerializeField] private SceneChanger sceneChanger;  // Reference to SceneChanger script
    [SerializeField] private Button loginButton;  // Reference to the Login button (UI Button)

    // This method will be called when the "Create Account" button is pressed
    public void OnCreateAccountButtonPressed()
    {
        // Save the user data
        bool isSaved = userDataManager.SaveUserData();

        if (isSaved)
        {
            // If the data was saved successfully, proceed to the MonthlyIncomeScreen
            Debug.Log("Account created successfully. Proceeding to MonthlyIncomeScreen.");
            sceneChanger.LoadScene("MonthlyIncomeScreen"); // Changed to MonthlyIncomeScreen
        }
        else
        {
            // If the data was not saved successfully, display an error message
            Debug.LogWarning("Account creation failed. Check input fields.");
        }
    }

    // This method will be called when the "Login" button is pressed
    public void OnLoginButtonPressed()
    {
        // Load the LoginScene when the login button is pressed
        sceneChanger.LoadScene("LoginScene");
    }
}
