using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; private set; }

    [Header("User Data UI References")]
    private TMP_InputField emailInputField;
    private TMP_InputField passwordInputField;
    private TMP_InputField usernameInputField;
    private TextMeshProUGUI errorMessageText;

    private UserData currentUserData;

    // List to store the user's purchases
    private List<PurchaseData> purchaseHistory = new List<PurchaseData>();

    [System.Serializable]
    public class UserData
    {
        public string username;
        public string email;
        public string password;
        public string lastPurchaseCategory; // Added to store the last purchase category
        public string lastPurchaseSprite;   // Added to store the last purchase sprite
    }

    [System.Serializable]
    public class PurchaseData
    {
        public string purchaseType;
        public float amount;
        public System.DateTime date;
        public string spriteName; // Store the exact sprite name
        public float posX; // X position of the sprite
        public float posY; // Y position of the sprite

        public PurchaseData(string type, float amt, string sprite, float x, float y)
        {
            purchaseType = type;
            amount = amt;
            date = System.DateTime.Now;
            spriteName = sprite;
            posX = x;
            posY = y;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentUserData = new UserData();
        LoadUserData();
        LoadPurchaseHistory(); // Load purchase history when the app starts
    }

    public void AssignUIReferences(TMP_InputField email, TMP_InputField password, TMP_InputField username, TextMeshProUGUI errorMessage)
    {
        emailInputField = email;
        passwordInputField = password;
        usernameInputField = username;
        errorMessageText = errorMessage;
    }

    public string GetEmailInput()
    {
        if (emailInputField != null && !string.IsNullOrEmpty(emailInputField.text))
        return emailInputField.text;

        if (!string.IsNullOrEmpty(currentUserData.email))
            return currentUserData.email;

        // Last fallback: load from PlayerPrefs (in case currentUserData wasn't set yet)
        return PlayerPrefs.GetString("UserEmail", "Unknown User");
    }

    public string GetPasswordInput()
    {
        return passwordInputField != null && !string.IsNullOrEmpty(passwordInputField.text)
            ? passwordInputField.text
            : currentUserData.password;
    }

    public string GetUsernameInput()
    {
        return usernameInputField != null && !string.IsNullOrEmpty(usernameInputField.text)
            ? usernameInputField.text
            : currentUserData.username;
    }

    public bool ValidateInputs()
    {
        if (string.IsNullOrEmpty(GetEmailInput()))
        {
            ShowErrorMessage("Email is required");
            return false;
        }

        if (string.IsNullOrEmpty(GetPasswordInput()) || GetPasswordInput().Length < 6)
        {
            ShowErrorMessage("Password must be at least 6 characters");
            return false;
        }

        if (string.IsNullOrEmpty(GetUsernameInput()))
        {
            ShowErrorMessage("Username is required");
            return false;
        }

        return true;
    }

    public bool SaveUserData()
    {
        if (!ValidateInputs()) return false;

        currentUserData.email = GetEmailInput();
        currentUserData.password = GetPasswordInput();
        currentUserData.username = GetUsernameInput();

        PlayerPrefs.SetString("UserEmail", currentUserData.email);
        PlayerPrefs.SetString("UserPassword", currentUserData.password);
        PlayerPrefs.SetString("Username", currentUserData.username);
        PlayerPrefs.Save();

        Debug.Log($"✅ User Data Saved: {currentUserData.username}, {currentUserData.email}");
        ClearErrorMessage();
        return true;
    }

    private void LoadUserData()
    {
        currentUserData.email = PlayerPrefs.GetString("UserEmail", "");
        currentUserData.password = PlayerPrefs.GetString("UserPassword", "");
        currentUserData.username = PlayerPrefs.GetString("Username", "");
        currentUserData.lastPurchaseCategory = PlayerPrefs.GetString("LastPurchaseCategory", ""); // Load last category
        currentUserData.lastPurchaseSprite = PlayerPrefs.GetString("LastPurchaseSprite", ""); // Load last sprite

        Debug.Log("🔄 User Data Loaded Successfully");
    }

    private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
            errorMessageText.text = message;
        Debug.LogWarning("⚠ " + message);
    }

    private void ClearErrorMessage()
    {
        if (errorMessageText != null)
            errorMessageText.text = "";
    }

    // ✅ This method is added for handling scene-specific logic
    public void OnSceneLoaded(string sceneName)
    {
        if (sceneName == "HomeScene")
        {
            // Example of finding the HomePageManager when HomeScene is loaded
            HomePageManager homePageManager = FindObjectOfType<HomePageManager>();

            if (homePageManager != null)
            {
                Debug.Log("✅ HomePageManager found in scene.");
                // Now you can perform any actions needed with HomePageManager
            }
            else
            {
                Debug.LogWarning("⚠ HomePageManager not found in HomeScene.");
            }
        }
    }

    public UserData GetUserData()
    {
        return currentUserData;
    }

    public void DebugPrintStoredCredentials()
    {
        string storedEmail = PlayerPrefs.GetString("UserEmail", "No email found");
        string storedPassword = PlayerPrefs.GetString("UserPassword", "No password found");
        string storedUsername = PlayerPrefs.GetString("Username", "No username found");

        Debug.Log($"📌 Stored Email: {storedEmail}");
        Debug.Log($"📌 Stored Username: {storedUsername}");
        Debug.Log($"📌 Stored Password: {storedPassword}");
    }

    // Save a purchase associated with the user
    public void SavePurchase(string purchaseType, float amount, string spriteName, Vector2 position)
{
    string userEmail = currentUserData.email; // Get the current user's email or any unique identifier
    PurchaseData newPurchase = new PurchaseData(purchaseType, amount, spriteName, position.x, position.y);
    purchaseHistory.Add(newPurchase);

    // Save the purchase details
    int purchaseIndex = purchaseHistory.Count - 1;
    PlayerPrefs.SetString($"{userEmail}_PurchaseType_{purchaseHistory.Count - 1}", purchaseHistory[purchaseHistory.Count - 1].purchaseType);
    PlayerPrefs.SetFloat($"{userEmail}_PurchaseAmount_{purchaseHistory.Count - 1}", purchaseHistory[purchaseHistory.Count - 1].amount);
    PlayerPrefs.SetString($"{userEmail}_PurchaseDate_{purchaseHistory.Count - 1}", purchaseHistory[purchaseHistory.Count - 1].date.ToString());
    PlayerPrefs.SetString($"{userEmail}_PurchaseSprite_{purchaseIndex}", newPurchase.spriteName);
    PlayerPrefs.SetFloat($"{userEmail}_PurchasePosX_{purchaseIndex}", newPurchase.posX);
    PlayerPrefs.SetFloat($"{userEmail}_PurchasePosY_{purchaseIndex}", newPurchase.posY);
    PlayerPrefs.SetInt($"{userEmail}_PurchaseCount", purchaseHistory.Count);  // Store the count of purchases for this user

    // Update the last purchase category and sprite in the UserData
    currentUserData.lastPurchaseCategory = purchaseType;
    currentUserData.lastPurchaseSprite = spriteName;

    // Save the updated user data
    PlayerPrefs.SetString("LastPurchaseCategory", purchaseType);
    PlayerPrefs.SetString("LastPurchaseSprite", spriteName);
    PlayerPrefs.Save();

    // Optionally, save user data if needed
    SaveUserData();

    Debug.Log($"✅ Purchase Saved: {purchaseType} - ${amount:N2}, Sprite: {spriteName} at ({position.x}, {position.y})");

}


    // Load the purchase history for the current user
    public void LoadPurchaseHistory()
    {
        string userEmail = currentUserData.email; // Get the current user's email
        int purchaseCount = PlayerPrefs.GetInt($"{userEmail}_PurchaseCount", 0);
        purchaseHistory.Clear();

        for (int i = 0; i < purchaseCount; i++)
        {
            string type = PlayerPrefs.GetString($"{userEmail}_PurchaseType_{i}");
            float amount = PlayerPrefs.GetFloat($"{userEmail}_PurchaseAmount_{i}");
            System.DateTime date = System.DateTime.Parse(PlayerPrefs.GetString($"{userEmail}_PurchaseDate_{i}"));
            string spriteName = PlayerPrefs.GetString($"{userEmail}_PurchaseSprite_{i}", "");
            float posX = PlayerPrefs.GetFloat($"{userEmail}_PurchasePosX_{i}", 0);
            float posY = PlayerPrefs.GetFloat($"{userEmail}_PurchasePosY_{i}", 0);

            PurchaseData purchase = new PurchaseData(type, amount, spriteName, posX, posY)
            {
                date = date
            };
            purchaseHistory.Add(purchase);
        }

        Debug.Log($"🔄 Loaded {purchaseHistory.Count} purchases for {userEmail}.");
    }

    // Return all purchase data for reference
    public List<PurchaseData> GetPurchaseHistory()
    {
        return purchaseHistory;
    }
}