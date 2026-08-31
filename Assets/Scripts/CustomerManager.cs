using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<Customer> customersToSpawn = new List<Customer>();

    public float timeBetweenCustomers;
    private float spawnCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if(spawnCounter <= 0)
        {
            SpawnCustomer();
        }
    }

    public void SpawnCustomer()
    {
        Instantiate(customersToSpawn[0]);

        spawnCounter = timeBetweenCustomers * Random.Range(.75f, 1.25f);
    }
}
