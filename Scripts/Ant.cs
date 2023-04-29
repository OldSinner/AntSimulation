using UnityEngine;

public class Ant : MonoBehaviour
{
    public float MaxSpeed;
    public float Speed;
    Vector2 Velocity;
    private void Start()
    {
    }
    void Update()
    {
        HaveFood();
        Move();
    }

    private void Move()
    {
        
        var target = FindFood();
        Debug.Log(target);
        var acc = target - (Vector2)transform.position;
        
        acc = acc.normalized * Speed;

        Velocity += acc * Time.deltaTime;
        Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed);
        var angle = Mathf.Atan2(Velocity.y, Velocity.x);

        var position = transform.position + ((Vector3)Velocity * Time.deltaTime);
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }

    void HaveFood()
    {
      
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
                if(col.CompareTag("Food"))
                {
                    Debug.Log("Found Something");
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 10);
    }
}
