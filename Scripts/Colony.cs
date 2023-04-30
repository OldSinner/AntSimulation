using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Colony : MonoBehaviour
{
    public int NumberOfAnts;
    public int PerSpawn;
    public GameObject Ant;

    DateTime LastSpawn = DateTime.MinValue;
    void Start()
    {
        
        
    }
    void Update()
    {
        if(NumberOfAnts > 0)
        {
            if (DateTime.Now.AddMilliseconds(-500) > LastSpawn)
            {
                Vector2 center = transform.position;
                for (int i = 0; i < PerSpawn; i++)
                {
                    int a = i * 360 / PerSpawn;
                    var pos = RandomCircle(center, 1.0f, a);
                    var target = RandomCircle(center, 20f, a);

                    var obj = Instantiate(Ant, pos, Quaternion.identity);
                    var ant = obj.GetComponent<Ant>();
                    ant.ActualTarget = target;
                    ant.GotTarget = true;
                }
                NumberOfAnts -= PerSpawn;
            }
        }
    }
   
    Vector2 RandomCircle(Vector3 center, float radius, int a)
    {
        float ang = a;
        Vector2 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}

