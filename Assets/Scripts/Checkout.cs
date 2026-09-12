using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Checkout : MonoBehaviour
{
    public static Checkout instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text priceText;
    public GameObject checkoutScreen;

    public Transform queuePoint;

    public List<Customer> customersInQueue = new List<Customer>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowPrice(45.32f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPrice(float priceTotal)
    {
        checkoutScreen.SetActive(true);

        priceText.text = "$" + priceTotal.ToString("F2");
    }

    public void HidePrice()
    {
        checkoutScreen.SetActive(false);
    }

    public void CheckoutCustomer()
    {
        HidePrice();
    }

    public void AddCustomerToQueue(Customer newCust)
    {
        customersInQueue.Add(newCust);

        updateQueue();
    }

    public void updateQueue()
    {
        for(int i = 0;  i < customersInQueue.Count; i++)
        {
            customersInQueue[i].UpdateQueuePoint(queuePoint.position + (queuePoint.forward * i * .5f));
        }
    }
}
