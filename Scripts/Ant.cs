using System;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    //Props
    public float MaxSpeed;
    public float Speed;
    public float WanderStrength;

    public GameObject HomePheromone;
    public GameObject FoodPheromone;


    //Private
    Vector2 Velocity;
    Vector2 ActualTarget;
    bool GotTarget;
    bool IsTargetFood;
    bool IsHaveFood;
    GameObject FoodHead;
    DateTime LastPheromoneSpawn = DateTime.MinValue;

    //Const
    private const int WARDEN_DISTANT_THRESHOLD = 2;

    private void Start()
    {
        FoodHead = transform.GetChild(1).gameObject;
    }
    void Update()
    {
        CheckFood();
        FindTarget();
        Move();
        CreatePheromone();
    }

    private void CreatePheromone()
    {
        if (DateTime.Now.AddSeconds(-1) > LastPheromoneSpawn)
        {
            if (IsHaveFood)
            {
                Instantiate(FoodPheromone, transform.position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                Instantiate(HomePheromone, transform.position, Quaternion.Euler(0, 0, 0));
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
        if (Vector2.Distance(transform.position, ActualTarget) < WARDEN_DISTANT_THRESHOLD && !IsTargetFood)
        {
            GotTarget = false;
        }

        if (!GotTarget)
        {

            ActualTarget = ((Vector2)transform.position + UnityEngine.Random.insideUnitCircle * WanderStrength);
            IsTargetFood = false;
            Debug.Log(ActualTarget);
            GotTarget = true;

        }

        if (!IsTargetFood)
        {
            var food = FindFood();
            if (food != Vector2.zero)
            {
                ActualTarget = food;
                IsTargetFood = true;
            }
        }
    }

    public Vector2 FindFood()
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
                        return col.transform.position;
                    }

                }
            }
        }
        return Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            IsHaveFood = true;
            GotTarget = false;
            Destroy(collision.gameObject);
        }
    }


    void OnDrawGizmos()
    {
        float angle = -110.0f;
        float rayRange = 10.0f;
        float halfFOV = angle / 2.0f;
        float coneDirection = 0;

        Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
        Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.forward);

        Vector3 upRayDirection = upRayRotation * transform.right * rayRange;
        Vector3 downRayDirection = downRayRotation * transform.right * rayRange;

        Gizmos.DrawRay(transform.position, upRayDirection);
        Gizmos.DrawRay(transform.position, downRayDirection);
        Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(ActualTarget, 2);
    }
}
