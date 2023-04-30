using UnityEngine;

public class Ant : MonoBehaviour
{
    public float MaxSpeed;
    public float Speed;
    Vector2 Velocity;
    Vector2 ActualTarget;
    bool GotTarget;
    bool IsHaveFood;
    GameObject FoodHead;
    private void Start()
    {
        FoodHead = transform.GetChild(1).gameObject;
    }
    void Update()
    {
        CheckFood();
        Move();
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
        if (!GotTarget)
        {
            var food = FindFood();
            if (food != Vector2.zero)
            {
                ActualTarget = food;
                GotTarget = true;
            }

        }
        var acc = ActualTarget - (Vector2)transform.position;

        acc = acc.normalized * Speed;

        Velocity += acc * Time.deltaTime;
        Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed);
        var angle = Mathf.Atan2(Velocity.y, Velocity.x);

        var position = transform.position + ((Vector3)Velocity * Time.deltaTime);
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }

    public Vector2 FindFood()
    {
        Collider2D[] buffer = new Collider2D[10];
        Physics2D.OverlapCircleNonAlloc(transform.position, 5, buffer);
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
                    else
                    {
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 10);
    }
}
