using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HomePageManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider budgetProgressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Button logPurchaseButton;
    [SerializeField] private TextMeshProUGUI monthlyBudgetText;
    [SerializeField] private TextMeshProUGUI amountSpentText;

    [Header("Otter and Purchased Items")]
    [SerializeField] private Transform otterTransform;
    [SerializeField] private GameObject itemPrefab;  // Prefab for purchased items (icons)

    [Header("Sprite Arrays (for different categories)")]
    [SerializeField] private GameObject[] foodSpritesGO;  // GameObjects for food sprites
    [SerializeField] private GameObject[] travelSpritesGO;  // GameObjects for travel sprites
    [SerializeField] private GameObject[] clothingSpritesGO;  // GameObjects for clothing sprites
    [SerializeField] private GameObject[] entertainmentSpritesGO;  // GameObjects for entertainment sprites
    [SerializeField] private GameObject[] otherSpritesGO;  // GameObjects for other sprites

    private float monthlyBudget = 1000f;
    private float amountSpent = 0f;

    private void Start()
    {
        // Initialize UI
        if (monthlyBudgetText != null) monthlyBudgetText.text = $"Monthly Budget: ${monthlyBudget:N2}";
        if (amountSpentText != null) amountSpentText.text = $"Amount Spent: ${amountSpent:N2}";

        // Set up budget progress bar
        if (budgetProgressBar != null)
        {
            budgetProgressBar.maxValue = monthlyBudget;
            budgetProgressBar.value = amountSpent;
        }

        // Check if the PurchaseManager exists in the scene
        if (PurchaseManager.Instance == null)
        {
            Debug.LogWarning("PurchaseManager.Instance not found. Make sure it exists in your scene.");
        }
        else
        {
            // Load any previously purchased items
            DisplayPurchasedItems();
        }

        // Load saved spending data if available
        LoadSpendingData();

        if (logPurchaseButton != null)
        {
            logPurchaseButton.onClick.RemoveAllListeners();
            logPurchaseButton.onClick.AddListener(OnLogPurchaseClick);
        }
        else
        {
            Debug.LogError("Log Purchase Button is not assigned in the Inspector.");
        }
    }

    private void LoadSpendingData()
    {
        // Example of loading from PlayerPrefs - implement based on your data storage method
        if (PlayerPrefs.HasKey("AmountSpent"))
        {
            amountSpent = PlayerPrefs.GetFloat("AmountSpent");
            UpdateUIValues();
        }
    }

    private void UpdateUIValues()
    {
        if (budgetProgressBar != null)
        {
            budgetProgressBar.value = amountSpent;
        }

        if (progressText != null)
        {
            float progressPercentage = (amountSpent / monthlyBudget) * 100f;
            progressText.text = $"{progressPercentage:N1}% spent";
        }

        if (amountSpentText != null)
        {
            amountSpentText.text = $"Amount Spent: ${amountSpent:N2}";
        }
    }
    private HashSet<string> instantiatedItems = new HashSet<string>();
    // Retrieve the last selected category and display a random sprite from that category
    private void DisplayPurchasedItems()
    {
    // Load previously purchased items
    List<UserDataManager.PurchaseData> purchasedItems = UserDataManager.Instance.GetPurchaseHistory();

    // Dictionary to store all sprite GameObjects (make sure they are assigned in the Inspector)
    Dictionary<string, GameObject[]> categorySprites = new Dictionary<string, GameObject[]>
    {
        { "Food", foodSpritesGO },
        { "Travel", travelSpritesGO },
        { "Clothing", clothingSpritesGO },
        { "Entertainment", entertainmentSpritesGO },
        { "Other", otherSpritesGO }
    };

    foreach (var purchase in purchasedItems)
    {
        string itemKey = $"{purchase.purchaseType}_{purchase.spriteName}";
        if (instantiatedItems.Contains(itemKey))
        {
            continue;
        }
        if (categorySprites.TryGetValue(purchase.purchaseType, out GameObject[] spriteGameObjects))
        {
            // Find the prefab that matches the name stored in the purchase data
            GameObject chosenPrefabGO = System.Array.Find(spriteGameObjects, sprite => sprite.name == purchase.spriteName);

            if (chosenPrefabGO != null)
            {
                Debug.Log($"✅ Found prefab: {chosenPrefabGO.name}, instantiating it.");

                // Instantiate the prefab at the specified position
//                GameObject instantiatedObject = Instantiate(chosenPrefabGO, new Vector3(purchase.posX, purchase.posY, 0), Quaternion.identity);
                GameObject instantiatedObject = Instantiate(chosenPrefabGO, new Vector3(367, 1737), Quaternion.identity);
                instantiatedObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
                Debug.Log("instantiated: " + chosenPrefabGO.name);
                // Scale the instantiated object
//                instantiatedObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                // Optionally, you can parent the instantiated object to the otterTransform (or other parent object)
//                instantiatedObject.transform.parent = otterTransform;

                // Activate the instantiated GameObject (this is usually not necessary because Instantiate makes it active by default)
                instantiatedObject.SetActive(true);
                instantiatedItems.Add(itemKey);
            }
            else
            {
                Debug.LogError($"❌ No matching prefab found for {purchase.spriteName} in {purchase.purchaseType}");
            }
        }
    }
}


    // Example: Update the progress bar when a purchase is made
    public void UpdateProgressBar(float spentAmount)
    {
        amountSpent += spentAmount;
        PlayerPrefs.SetFloat("AmountSpent", amountSpent);
        PlayerPrefs.Save();
        UpdateUIValues();
    }

    // Redirect to Log Purchase Page
    private void OnLogPurchaseClick()
    {
        try
        {
            SceneManager.LoadScene("LogPurchaseScene");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load LogPurchaseScene: {e.Message}");
        }
    }
}
