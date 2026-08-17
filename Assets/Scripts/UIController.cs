using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject updatePricePannel;

    public TMP_Text basePriceText, currentPriceText;

    public TMP_InputField priceInputField;

    private StockInfo activeStockInfo;

    public TMP_Text moneyText;

    public GameObject buyMenueScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.tabKey.wasPressedThisFrame)
        {
            OpenCloseBuyMenu();
        }
    }

    public void OpenUpdatePrice(StockInfo stockToUpdate)
    {
        updatePricePannel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;

        basePriceText.text = "$" + stockToUpdate.price.ToString("F2");
        currentPriceText.text = "$" + stockToUpdate.currentPrice.ToString("F2");

        activeStockInfo = stockToUpdate;

        priceInputField.text = stockToUpdate.currentPrice.ToString();
    }

    public void CloseUpdatePrice()
    {
        updatePricePannel?.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ApplyPriceUpdate()
    {
            activeStockInfo.currentPrice = float.Parse(priceInputField.text);

            currentPriceText.text = "$" + activeStockInfo.currentPrice.ToString("F2");

            StockInfoController.instance.UpdatePrice(activeStockInfo.name, activeStockInfo.currentPrice);

            CloseUpdatePrice();
       
    }

    public void UpdateMoney(float currentMoney)
    {
        moneyText.text = "$" + currentMoney.ToString("F2");
    }

    public void OpenCloseBuyMenu()
    {
        if(buyMenueScreen.activeSelf == false)
        {
            buyMenueScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
        } else
        {
            buyMenueScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
