using UnityEngine;
using TMPro; // Add this import
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IncomeInputManager : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField monthlyIncomeInput; // Change to TMP_InputField
    [SerializeField] private TextMeshProUGUI errorMessageText; // Change to TextMeshProUGUI

    [Header("Buttons")]
    [SerializeField] private Button saveIncomeButton;

    [Header("User Data")]
    [SerializeField] private UserDataManager userDataManager;

    private void Start()
    {
        // Add listener to save income button
        if (saveIncomeButton != null)
        {
            saveIncomeButton.onClick.AddListener(SaveMonthlyIncome);
        }
    }

    public void SaveMonthlyIncome()
    {
        // Clear previous error message
        if (errorMessageText != null)
            errorMessageText.text = "";

        // Validate income input
        if (ValidateIncomeInput())
        {
            // Parse the income value
            float monthlyIncome = float.Parse(monthlyIncomeInput.text);

            // Example storage (adjust as needed)
            string userEmail = userDataManager != null ? userDataManager.GetEmailInput() : "Unknown User";
            Debug.Log($"Monthly Income Saved: ${monthlyIncome:N2} for user: {userEmail}");

            // Optional: Show success message
            if (errorMessageText != null)
            {
                errorMessageText.text = "Income saved successfully!";
            }

            // Add scene transition after a short delay
            Invoke("LoadMonthlyBudgetScene", 1.0f); // 1 second delay
        }
    }

    private bool ValidateIncomeInput()
    {
        // Check if input field exists
        if (monthlyIncomeInput == null)
        {
            ShowErrorMessage("Income input field is missing");
            return false;
        }

        // Check if input is empty
        if (string.IsNullOrEmpty(monthlyIncomeInput.text))
        {
            ShowErrorMessage("Please enter your monthly income");
            return false;
        }

        // Try parsing the input as a float
        if (!float.TryParse(monthlyIncomeInput.text, out float income))
        {
            ShowErrorMessage("Please enter a valid number");
            return false;
        }

        // Check for negative income
        if (income < 0)
        {
            ShowErrorMessage("Income cannot be negative");
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

    private void LoadMonthlyBudgetScene()
    {
        SceneManager.LoadScene("MonthlyBudgetScene"); // Make sure this scene name is correct
    }
}
