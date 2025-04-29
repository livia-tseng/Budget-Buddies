using UnityEngine;
using System.Collections.Generic;  // Needed for List<T>

public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager Instance { get; private set; }

    [Header("Item Prefabs")]
    public GameObject itemPrefab;  // The prefab for the items
    public Transform otterIsland;  // The parent where the items will be placed around the otter

    private List<GameObject> purchasedItems = new List<GameObject>();  // Store all instantiated items

    private void Awake()
    {
        // Ensure that there's only one instance of PurchaseManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Prevents PurchaseManager from being destroyed between scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to log a purchase and instantiate a new item
    public void LogPurchase(Sprite[] spriteArray)
    {
        // Ensure valid sprite array, prefab, and otter island are set
        if (spriteArray == null || spriteArray.Length == 0 || itemPrefab == null || otterIsland == null)
        {
            Debug.LogWarning("LogPurchase failed due to missing references.");
            return;
        }

        // Pick a random sprite from the selected array
        int randomIndex = Random.Range(0, spriteArray.Length);
        Sprite selectedSprite = spriteArray[randomIndex];

        // Call AddPurchaseToHomeScene to handle instantiation
        AddPurchaseToHomeScene(selectedSprite);
    }

    // Methods to log different types of purchases
    public void LogFoodPurchase(Sprite[] foodSprites) => LogPurchase(foodSprites);
    public void LogTravelPurchase(Sprite[] travelSprites) => LogPurchase(travelSprites);
    public void LogClothingPurchase(Sprite[] clothingSprites) => LogPurchase(clothingSprites);
    public void LogEntertainmentPurchase(Sprite[] entertainmentSprites) => LogPurchase(entertainmentSprites);
    public void LogOtherPurchase(Sprite[] otherSprites) => LogPurchase(otherSprites);

    // Adds a purchased item to the Home Scene
    public void AddPurchaseToHomeScene(Sprite selectedSprite)
    {
        if (selectedSprite == null || itemPrefab == null || otterIsland == null)
        {
            Debug.LogWarning("AddPurchaseToHomeScene failed due to missing references.");
            return;
        }

        GameObject newItem = Instantiate(itemPrefab, otterIsland);
        newItem.transform.localPosition = new Vector3(
            Random.Range(-1.5f, 1.5f),
            Random.Range(-1.5f, 1.5f),
            0
        );
        newItem.transform.localScale = Vector3.one * 0.3f;

        SpriteRenderer renderer = newItem.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sprite = selectedSprite;
        }

        purchasedItems.Add(newItem);
    }
}


