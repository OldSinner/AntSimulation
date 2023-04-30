using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject FoodObject;
    public int Radius;
    public int NumberOfFood;
    void Start()
    {
        for (int i = 0; i < NumberOfFood; i++)
        {
            var xRandom = Random.Range(-Radius, Radius);
            var yRandom = Random.Range(-Radius, Radius);
            var pos = new Vector2(transform.position.x + xRandom, transform.position.y + yRandom);
            Instantiate(FoodObject, pos, Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
