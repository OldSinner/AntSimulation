using System;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    //Props
    public float MaxSpeed;
    public float Speed;
    public float WanderStrength;

    public Helper helper;

    public Vector2 ActualTarget;
    public bool GotTarget;

    //Private
    Vector2 Velocity;
    bool IsTargetFood;
    bool IsHaveFood;
    GameObject FoodHead;
    GameObject Food;
     PheromoneMap map;
    DateTime LastPheromoneSpawn = DateTime.MinValue;

    //Const
    private const int WARDEN_DISTANT_THRESHOLD = 2;

    private void Start()
    {
        FoodHead = transform.GetChild(1).gameObject;
        map = GameObject.Find("PheromoneMap").GetComponent<PheromoneMap>();
        helper = GameObject.Find("Helper").GetComponent<Helper>();
    }
    void Update()
    {
        CheckFood();
        FindTarget();
        Move();
        SpawnPheromone();
    }

    private void SpawnPheromone()
    {
        if (LastPheromoneSpawn < DateTime.Now.AddMilliseconds(-500))
        {
            if (IsHaveFood)
            {
                map.AddPheromone(transform.position, PheromoneMap.PheromoneType.Food);
            }
            else
            {
                map.AddPheromone(transform.position, PheromoneMap.PheromoneType.Home);

            }
            LastPheromoneSpawn = DateTime.Now;
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
    private void Move()
    {

        var acc = ActualTarget - (Vector2)transform.position;

        acc = acc.normalized * Speed;

        Velocity += acc * Time.deltaTime;
        Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed);
        var angle = Mathf.Atan2(Velocity.y, Velocity.x);

        var position = transform.position + ((Vector3)Velocity * Time.deltaTime);
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }

    private void FindTarget()
    {
        if (IsTargetFood && Food == null)
        {
            GotTarget = false;
        }
        if (Vector2.Distance(transform.position, ActualTarget) < WARDEN_DISTANT_THRESHOLD && !IsTargetFood)
        {
            GotTarget = false;
        }

        if (!GotTarget)
        {

            ActualTarget = ((Vector2)transform.position + UnityEngine.Random.insideUnitCircle * WanderStrength);
            IsTargetFood = false;
            GotTarget = true;

        }

        if (!IsTargetFood && !IsHaveFood)
        {
            var food = FindFood();
            if (food != null)
            {
                ActualTarget = food.transform.position;
                IsTargetFood = true;
                Food = food;
            }
        }
    }

    public GameObject? FindFood()
    {
        Collider2D[] buffer = new Collider2D[10];
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
                        return col.gameObject;
                    }

                }
            }
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsHaveFood)
        {
            if (collision.gameObject.tag == "Food")
            {
                IsHaveFood = true;
                Destroy(collision.gameObject);
                var randomness = UnityEngine.Random.Range(-2, 2);
                ActualTarget = transform.position + new Vector3(randomness, randomness) - transform.right * 5;
                IsTargetFood = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ActualTarget, 1);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position + transform.right * 2, .8f);
        Gizmos.DrawWireSphere(transform.position + transform.right * 2 + transform.up * 2 - transform.right * 0.5f, .8f);
        Gizmos.DrawWireSphere(transform.position + transform.right * 2 - transform.up * 2 - transform.right * 0.5f, .8f);
    }

}
