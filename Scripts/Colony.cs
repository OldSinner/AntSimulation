using System;
using System.Collections;
using UnityEngine;

public class Colony : MonoBehaviour
{
    public int NumberOfAnts;
    public GameObject Ant;

    void Start()
    {
        if (NumberOfAnts > 0)
        {
            Vector2 center = transform.position;
            for (int i = 0; i < NumberOfAnts; i++)
            {
                int a = i * 360 / NumberOfAnts;
                var pos = RandomCircle(center, 1.0f, a);
                var target = RandomCircle(center, 20f, a);

                var obj = Instantiate(Ant, pos, Quaternion.identity, transform);
                var ant = obj.GetComponent<Ant>();
                ant.ActualTarget = target;
                ant.GotTarget = true;
            }
        }

    }
    void Update()
    {
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

