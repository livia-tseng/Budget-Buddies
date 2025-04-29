using UnityEngine;
using UnityEngine.UI;

public class DashboardManager : MonoBehaviour
{
    // UI elements (using UnityEngine.UI Text for legacy Text)
    public Text budgetText;
    public Text expensesText;
    public Text remainingText;
    public Button addExpenseButton;
    public InputField expenseInput;

    // Budget Bar (Slider or Image)
    public Slider budgetSlider;  // Use this if using a Slider for the budget bar
    // public Image budgetBar;  // Uncomment this if using an Image instead of a Slider

    // Budget variables
    private float totalIncome = 1000f;  // Example starting income
    private float totalExpenses = 0f;
    private float moneyLeft;

    void Start()
    {
        moneyLeft = totalIncome - totalExpenses;
        UpdateUI();

        // Add listener to the button to handle the event when clicked
        addExpenseButton.onClick.AddListener(OnAddExpense);
    }

    void OnAddExpense()
    {
        // Try to parse the input text to a float (expense)
        if (float.TryParse(expenseInput.text, out float expense))
        {
            totalExpenses += expense;
            moneyLeft = totalIncome - totalExpenses;
            UpdateUI();
        }
        else
        {
            Debug.LogError("Invalid expense input! Please enter a valid number.");
        }
    }

    void UpdateUI()
    {
        // Update the text components with current budget, expenses, and remaining balance
        budgetText.text = $"Total Income: ${totalIncome}";
        expensesText.text = $"Total Expenses: ${totalExpenses}";
        remainingText.text = $"Money Left: ${moneyLeft}";

        // Update the budget bar (Slider or Image)
        // If using Slider:
        budgetSlider.value = totalExpenses / totalIncome;  // Slider value updates based on expenses
        // If using Image:
        // budgetBar.fillAmount = totalExpenses / totalIncome;  // Image fillAmount updates based on expenses
    }
}
