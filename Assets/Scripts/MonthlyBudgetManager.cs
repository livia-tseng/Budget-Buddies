using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MonthlyBudgetInputManager : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField monthlyBudgetInput;
    [SerializeField] private TextMeshProUGUI errorMessageText;

    [Header("Buttons")]
    [SerializeField] private Button saveBudgetButton;

    [Header("User Data")]
    [SerializeField] private UserDataManager userDataManager;

    private void Start()
    {
        // Add listener to save budget button
        if (saveBudgetButton != null)
        {
            saveBudgetButton.onClick.AddListener(SaveMonthlyBudget);
        }
    }

    public void SaveMonthlyBudget()
    {
        // Clear previous error message
        if (errorMessageText != null)
            errorMessageText.text = "";

        // Validate budget input
        if (ValidateBudgetInput())
        {
            // Parse the budget value
            float monthlyBudget = float.Parse(monthlyBudgetInput.text);

            // Example storage (adjust as needed)
            string userEmail = userDataManager != null ? userDataManager.GetEmailInput() : "Unknown User";
            Debug.Log($"Monthly Budget Saved: ${monthlyBudget:N2} for user: {userEmail}");

            // Optional: Show success message
            if (errorMessageText != null)
            {
                errorMessageText.text = "Budget saved successfully!";
            }

            // Add scene transition after a short delay
            Invoke("LoadHomePage", 1.0f); // 1 second delay
        }
    }

    private bool ValidateBudgetInput()
    {
        // Check if input field exists
        if (monthlyBudgetInput == null)
        {
            ShowErrorMessage("Budget input field is missing");
            return false;
        }

        // Check if input is empty
        if (string.IsNullOrEmpty(monthlyBudgetInput.text))
        {
            ShowErrorMessage("Please enter your monthly budget");
            return false;
        }

        // Try parsing the input as a float
        if (!float.TryParse(monthlyBudgetInput.text, out float budget))
        {
            ShowErrorMessage("Please enter a valid number");
            return false;
        }

        // Check for negative budget
        if (budget < 0)
        {
            ShowErrorMessage("Budget cannot be negative");
            return false;
        }

        return true;
    }

    private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
        }
        Debug.LogWarning(message);
    }

    private void LoadHomePage()
    {
        SceneManager.LoadScene("HomeScene"); // Ensure this scene name matches your actual homepage scene
    }
}
