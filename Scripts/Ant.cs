using System;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    //Props
    public float WanderStrength; 
    public float MaxSpeed; 
    public float SteerStrength;
    public Helper helper;

    //Private
    Vector2 Velocity;
    Vector2 position;
    Vector2 desiredDirection;

    bool IsHaveFood;
    GameObject FoodHead;
    GameObject Food;
    PheromoneMap map;
    DateTime LastPheromoneSpawn = DateTime.MinValue;

    //Const

    private void Start()
    {
        FoodHead = transform.GetChild(1).gameObject;
        map = GameObject.Find("PheromoneMap").GetComponent<PheromoneMap>();
        helper = GameObject.Find("Helper").GetComponent<Helper>();
    }
    void Update()
    {
        MakeRandomTarget();
        MakeFoodTarget();
        Move();
        SpawnPheromone();
    }


    private void Move()
    {

        var desiredVelocity = desiredDirection * MaxSpeed;
        var steeringForce = (desiredVelocity - Velocity) * SteerStrength;
        var acc = Vector2.ClampMagnitude(steeringForce, SteerStrength);

        Velocity = Vector2.ClampMagnitude(Velocity + acc * Time.deltaTime, MaxSpeed);
        position += Velocity * Time.deltaTime;

        float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = position;
    }

    private void MakeRandomTarget()
    {
        desiredDirection = (desiredDirection + UnityEngine.Random.insideUnitCircle * WanderStrength).normalized;
    }
    private void MakeFoodTarget()
    {
        if(Food == null)
        {
            var food = FindFood();
            if (food != null)
            {
                desiredDirection = (food.transform.position - transform.position).normalized;
            }
        }
        else
        {
            desiredDirection = (Food.transform.position - transform.position).normalized;
        }
    }

    private void PheromonTarget()
    {
        var center = transform.position + transform.right * 2;
        var left = transform.position + transform.right * 2 + transform.up * 2 - transform.right * 0.5f;
        var right = transform.position + transform.right * 2 - transform.up * 2 - transform.right * 0.5f;
        var typeToSearch = IsHaveFood ? PheromoneType.Home : PheromoneType.Food;

        var leftPower = map.GetPheromoneInCircle(left, 1, typeToSearch);
        var centerPower = map.GetPheromoneInCircle(center, 1, typeToSearch);
        var rightPower = map.GetPheromoneInCircle(right, 1, typeToSearch);
    }
    private void SpawnPheromone()
    {
        if (LastPheromoneSpawn < DateTime.Now.AddMilliseconds(-500))
        {
            if (IsHaveFood)
            {
                map.AddPheromone(transform.position, PheromoneType.Food);
            }
            else
            {
                map.AddPheromone(transform.position, PheromoneType.Home);

            }
            LastPheromoneSpawn = DateTime.Now;
        }
    }

    public GameObject? FindFood()
    {
        Collider2D[] buffer = new Collider2D[20];
        Physics2D.OverlapCircleNonAlloc(transform.position, 10, buffer);
        Vector3 characterToCollider;
        float dot;
        foreach (var col in buffer)
        {
            if (col != null)
            {
                if (col.CompareTag("Food"))
                {
                    characterToCollider = (col.transform.position - transform.position).normalized;
                    dot = Vector3.Dot(characterToCollider, transform.right);
                    if (dot >= Mathf.Cos(55))
                    {
                        Food = col.gameObject;
                        return col.gameObject;
                    }

                }
            }
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsHaveFood)
        {
            if (collision.gameObject.tag == "Colony")
            {
                var colony = collision.gameObject.GetComponent<Colony>();
                colony.FoodCounter++;
                IsHaveFood = false;
            }
        }
        if (!IsHaveFood)
        {
            if (collision.gameObject.tag == "Food")
            {
                IsHaveFood = true;
                Destroy(collision.gameObject);
                Food = null;
                //TODO What do after get food
            }
        }
    }
    void CheckFood()
    {
        if (IsHaveFood)
        {
            FoodHead.SetActive(true);
        }
        else
        {
            FoodHead.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, desiredDirection);
    }

}
