using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public GameObject Food;
    public PheromoneMap map;

    public float InteractionRange;
    // Start is called before the first frame update

    DateTime LastCheck = DateTime.MinValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CheckCellid
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var cell = map.GetCellBaseOnPosition(worldPosition);
            Debug.Log(cell.x + " " + cell.y);
            Debug.Log(map.GetPheromoneInCircle(worldPosition, InteractionRange, PheromoneType.Home));
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            map.AddPheromone(worldPosition, PheromoneType.Home);
        }
        //CheckPheromones
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Debug.Log(map.GetPheromoneInCircle(worldPosition, 1, PheromoneType.Home));
        //    Debug.Log(map.GetPheromoneInCircle(worldPosition, 1, PheromoneType.Food));

        //}
        // Spawn Food
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Instantiate(Food, worldPosition, Quaternion.Euler(0, 0, 0));
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
    void OnDrawGizmos()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.DrawWireSphere(worldPosition, InteractionRange);
    }
}
