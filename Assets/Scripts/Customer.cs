using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    public float moveSpeed;
    private float currentWaitTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWaitTime = .5f;
    }

    // Update is called once per frame
    void Update()
    {
       if(points.Count >  0)
        {
            MoveToPoint();
        } 
    }

    public void MoveToPoint()
    {
        Vector3 targetPosition = new Vector3(points[0].position.x, transform.position.y, points[0].position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        transform.LookAt(targetPosition);

        if(Vector3.Distance(transform.position, targetPosition) < .25f)
        {
            if(currentWaitTime > 0)
            {
                currentWaitTime -= Time.deltaTime;

                if(currentWaitTime <= 0)
                {
                    StartNextPoint();
                }
            }
        }
    }

    public void StartNextPoint()
    {
        if(points.Count > 0)
        {
            points.RemoveAt(0);

            if(points.Count > 0)
            {
                currentWaitTime = .5f;
            }
        }
    }
}


