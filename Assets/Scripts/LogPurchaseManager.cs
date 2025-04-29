using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LogPurchaseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Dropdown purchaseTypeDropdown;
    [SerializeField] private Button logPurchaseButton;
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private TMP_InputField purchaseAmountInput; // InputField for the purchase amount

    [Header("Prefab Arrays (for different categories)")]
    [SerializeField] private GameObject[] foodPrefabs;
    [SerializeField] private GameObject[] travelPrefabs;
    [SerializeField] private GameObject[] clothingPrefabs;
    [SerializeField] private GameObject[] entertainmentPrefabs;
    [SerializeField] private GameObject[] otherPrefabs;

    private void Start()
    {
        if (logPurchaseButton == null)
        {
            Debug.LogError("❌ Log Purchase Button is not assigned in the Inspector.");
            return;
        }
        if (purchaseTypeDropdown == null)
        {
            Debug.LogError("❌ Purchase Type Dropdown is not assigned in the Inspector.");
            return;
        }
        if (purchaseAmountInput == null)
        {
            Debug.LogError("❌ Purchase Amount Input Field is not assigned in the Inspector.");
            return;
        }
//        logPurchaseButton.onClick.RemoveAllListeners();
//        logPurchaseButton.onClick.AddListener(OnLogPurchaseButtonClick);
    }

    private GameObject GetPrefabForCategory(string category)
    {
        GameObject[] selectedArray = category switch
        {
            "Food" => foodPrefabs,
            "Travel" => travelPrefabs,
            "Clothing" => clothingPrefabs,
            "Entertainment" => entertainmentPrefabs,
            "Other" => otherPrefabs,
            _ => null
        };

        return (selectedArray != null && selectedArray.Length > 0) ? selectedArray[Random.Range(0, selectedArray.Length)] : null;
    }

    public void OnLogPurchaseButtonClick()
    {
        Debug.Log("Button Clicked!");
        errorMessageText.text = "";

        // Get selected category from dropdown
        string selectedCategory = purchaseTypeDropdown.options[purchaseTypeDropdown.value].text;
//        Debug.Log("this works");
        if (selectedCategory == "Select Category")  // Ensure this matches your dropdown's default text
        {
            ShowErrorMessage("Please select a valid purchase category.");
            return;
        }
//        Debug.Log("ok this does too");
        // Get a prefab based on the category
        GameObject selectedPrefab = GetPrefabForCategory(selectedCategory);
        Debug.Log("name: " + selectedPrefab.name);
        if (selectedPrefab == null)
        {
            ShowErrorMessage($"No prefab available for the selected category: {selectedCategory}");
            return;
        }
//        Debug.Log("and this");
        // Validate purchase amount
        if (!float.TryParse(purchaseAmountInput.text, out float purchaseAmount) || purchaseAmount <= 0)
        {
            ShowErrorMessage("Please enter a valid amount greater than 0.");
            return;
        }
//        Debug.Log("yes this too");
        // Generate a random position for the sprite
//        RectTransform canvasRect = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
//        float maxX = canvasRect.rect.width / 2f - 100f; // 100px margin
//        float maxY = canvasRect.rect.height / 2f - 200f; // 200px margin
//        Vector2 randomPosition = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));
        Vector2 randomPosition = new Vector2(Random.Range(-250f, 250f), Random.Range(-0f, 500f));
//        Debug.Log("yep");
        // Save the purchase data
        UserDataManager.Instance.SavePurchase(selectedCategory, purchaseAmount, selectedPrefab.name, randomPosition);
//        Instantiate(selectedPrefab, Vector3.zero, Quaternion.identity);
//        Debug.Log("oke doke");
        // Load the home scene
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (canvas != null)
        {
            GameObject newUIObject = Instantiate(selectedPrefab, Vector3.zero, Quaternion.identity);
            newUIObject.transform.SetParent(canvas.transform, false);
            newUIObject.GetComponent<RectTransform>().anchoredPosition = randomPosition;
            newUIObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Canvas not found. Prefab not instantiated.");
        }
        Debug.Log("attempting to load home screen");
        SceneManager.LoadScene("HomeScene");
    }

    private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
        }
        else
        {
            Debug.LogError($"❌ Error: {message}");
        }
    }
}
