using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("User Data UI Elements")]
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField usernameInputField;
    public TextMeshProUGUI errorMessageText;

    // Ensure the UserDataManager persists across scenes and is accessible
    private void Awake()
    {
        // Check if UserDataManager exists, if not, create a new one
        if (UserDataManager.Instance != null)
        {
            // Assign references to UserDataManager
            UserDataManager.Instance.AssignUIReferences(emailInputField, passwordInputField, usernameInputField, errorMessageText);
        }
        else
        {
            Debug.LogError("UserDataManager is not in the scene");
        }
    }
}