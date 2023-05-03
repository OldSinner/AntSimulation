using System;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    private SimulationSettings settings;

    //Props
    public Helper helper;

    //Private
    Vector2 Velocity;
    Vector2 position;
    Vector2 desiredDirection;
    Vector2 TurnAroundPoint;


    bool IsHaveFood;
    bool IsTurnAround;
    GameObject FoodHead;
    GameObject Food;
    PheromoneMap map;
    float StartTurndAroundTime;
    float LastPheromoneSpawn;

    //Const

    private void Start()
    {
        settings = GameObject.Find("SimulationSettings").GetComponent<SimulationSettings>();
        FoodHead = transform.GetChild(1).gameObject;
        map = GameObject.Find("PheromoneMap").GetComponent<PheromoneMap>();
        helper = GameObject.Find("Helper").GetComponent<Helper>();
    }
    void Update()
    {
        MakeRandomTarget();
        MakePheromoneMovemnt();
        MakeFoodTarget();
        MakeTurnAround();
        Move();
        SpawnPheromone();
        CheckFood();
    }

    private void MakePheromoneMovemnt()
    {
        PheromoneType searchType = PheromoneType.Food;
        if(IsHaveFood)
        {
            searchType = PheromoneType.Home;
        }

        var leftPoint = transform.position + (transform.right * 1.5f) + transform.up;
        var rightPoint = transform.position + (transform.right * 1.5f) - transform.up;
        var centerPoint = transform.position + (transform.right * 2);

        var leftPheromone = map.GetPheromoneInCircle(leftPoint,settings.AntSensorRange, searchType);
        var rightPheromone = map.GetPheromoneInCircle(rightPoint, settings.AntSensorRange, searchType);
        var centerPheromone = map.GetPheromoneInCircle(centerPoint, settings.AntSensorRange, searchType);

        if(centerPheromone > leftPheromone && centerPheromone > rightPheromone)
        {
            desiredDirection = centerPoint-transform.position;
            Debug.Log("Going straight");
        }
        else if(leftPheromone > rightPheromone)
        {
            desiredDirection = leftPoint - transform.position;
            Debug.Log("Turning left");
        }
        else if (rightPheromone > leftPheromone)
        {
            desiredDirection = rightPoint - transform.position;
            Debug.Log("Turning Right");

        }
        if (centerPheromone == 0 && leftPheromone == 0 && rightPheromone == 0)
        {
            Debug.Log("No Pheromone");
        }
    }

    private void MakeTurnAround()
    {
        if(IsTurnAround)
        {
            if ((Time.time - StartTurndAroundTime) > settings.AntTurnAroundTime)
            {
                IsTurnAround = false;
                return;
            }
            desiredDirection = (TurnAroundPoint - (Vector2)transform.position).normalized;
        }
    }

    private void StartTurnAround()
    {
        IsTurnAround = true;
        StartTurndAroundTime = Time.time;
        TurnAroundPoint = transform.position - transform.right * 7 + transform.up * UnityEngine.Random.Range(-4,4);
    }

    private void Move()
    {

        var desiredVelocity = desiredDirection * settings.AntMaxSpeed;
        var steeringForce = (desiredVelocity - Velocity) * settings.AntSteerStrength;
        var acc = Vector2.ClampMagnitude(steeringForce, settings.AntSteerStrength);

        Velocity = Vector2.ClampMagnitude(Velocity + acc * Time.deltaTime, settings.AntMaxSpeed);
        position += Velocity * Time.deltaTime;

        float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = position;
    }

    private void MakeRandomTarget()
    {
        desiredDirection = (desiredDirection + UnityEngine.Random.insideUnitCircle * settings.AntWanderStrength).normalized;
    }
    private void MakeFoodTarget()
    {
        if (!IsHaveFood)
        {
            if (Food == null)
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
    }

    private void SpawnPheromone()
    {
        if (Time.time - LastPheromoneSpawn > .5)
        {
            if (IsHaveFood)
            {
                map.AddPheromone(transform.position, PheromoneType.Food);
            }
            else
            {
                map.AddPheromone(transform.position, PheromoneType.Home);

            }
            LastPheromoneSpawn = Time.time;
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
                StartTurnAround();
            }
        }
        if (!IsHaveFood)
        {
            if (collision.gameObject.tag == "Food")
            {
                IsHaveFood = true;
                Destroy(collision.gameObject);
                Food = null;
                StartTurnAround();
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
        Gizmos.DrawWireSphere(transform.position + (transform.right * 3) , 2);
        Gizmos.DrawWireSphere(transform.position + (transform.right * 2f) + transform.up * 2, 2f); // left
        Gizmos.DrawWireSphere(transform.position + (transform.right * 2f) - transform.up * 2, 2f); // right

    }

}
