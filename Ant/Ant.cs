using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ant : MonoBehaviour
{
    public float MaxSpeed;
    public float Speed;
    Vector2 Velocity;

    void Start()
    {
    }

    void Update()
    {
        var target = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var acc = target - (Vector2)transform.position;

        acc = acc.normalized * Speed;

        Velocity += acc * Time.deltaTime;
        Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed);
        var angle = Mathf.Atan2(Velocity.y, Velocity.x);
        Debug.Log(angle);

        var position = transform.position + ((Vector3)Velocity * Time.deltaTime);
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }
}
