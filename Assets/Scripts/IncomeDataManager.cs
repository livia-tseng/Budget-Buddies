using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  // Import SceneManagement

public class IncomeDataManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField monthlyIncomeInput;  // Input field for monthly income
    [SerializeField] private TextMeshProUGUI userGreetingText;  // To display user email or username

    void Start()
    {
        // Access the saved user data from UserDataManager Singleton
        if (UserDataManager.Instance != null)
        {
            string userName = UserDataManager.Instance.GetUserData().username; // Get the username
            DisplayUserGreeting(userName);
        }
    }

    // Display greeting with username or email
    private void DisplayUserGreeting(string userName)
    {
        if (userGreetingText != null)
        {
            userGreetingText.text = $"Hello, {userName}!";
        }
    }

    // Method to save income when the user submits it
    public void SaveIncome()
    {
        string incomeInput = monthlyIncomeInput.text;
        if (float.TryParse(incomeInput, out float income))
        {
            // Save the income in a data structure or process it as needed
            Debug.Log($"Income saved: ${income}");

            // Optionally, save the income to PlayerPrefs, a file, or a database
            // Example of saving to PlayerPrefs:
            // PlayerPrefs.SetFloat("MonthlyIncome", income);

            // After saving income, transition to Home Scene
            TransitionToHomeScene();
        }
        else
        {
            Debug.LogWarning("Invalid income entered!");
        }
    }

    // Method to handle scene transition to Home Scene
    private void TransitionToHomeScene()
    {
        // Use SceneManager to load the Home Scene
        SceneManager.LoadScene("HomeScene");  // Replace "HomeScene" with your actual scene name
    }
}

