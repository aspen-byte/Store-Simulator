using UnityEngine;

public class BuyMenuController : MonoBehaviour
{
    public GameObject stockPanel, furniturePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenStockPannel()
    {
        stockPanel.SetActive(true);
        furniturePanel.SetActive(false);
    }

    public void OpenFurniturePannel()
    {
        stockPanel.SetActive(false);
        furniturePanel.SetActive(true);
    }
}
