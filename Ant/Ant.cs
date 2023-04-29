using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ant : MonoBehaviour
{
    public float MaxSpeed;
    public float MaxSteerPower;
    public float SteerPower;
    public float Speed;
    Vector2 Velocity;
    Vector2 Position;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        var Acceleration = (Vector2)(target - transform.position).normalized * (Speed * Time.deltaTime);
        Acceleration = Vector2.ClampMagnitude(Acceleration * SteerPower, MaxSteerPower);
        Velocity += Acceleration;
        Vector2.ClampMagnitude(Velocity, MaxSpeed);
        Position += Velocity * Time.deltaTime;

        transform.SetPositionAndRotation(Position, Quaternion.Euler(0, 0, 0));
    }
}
