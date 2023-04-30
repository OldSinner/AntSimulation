using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public GameObject Food;
    // Start is called before the first frame update
    public int PheromoneCounter = 0;

    DateTime LastCheck = DateTime.MinValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DateTime.Now.AddSeconds(-1) > LastCheck)
        {
            Debug.Log(PheromoneCounter);
        }
        // Spawn Food
        //if(Input.GetMouseButtonDown(0))
        //{
        //    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Instantiate(Food,worldPosition,Quaternion.Euler(0,0,0));
        //}
        //Check Collission
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var coll = Physics2D.OverlapCircleAll(worldPosition, 1);
        //    Debug.Log(coll.Length);
        //    foreach(var coll2 in coll)
        //    {
        //        Debug.Log(coll2);
        //    }
        //}
    }
}
