using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public GameObject Food;
    public PheromoneMap map;
    // Start is called before the first frame update

    DateTime LastCheck = DateTime.MinValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CheckCellX
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(map.GetCellBaseOnPosition(worldPosition));
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
