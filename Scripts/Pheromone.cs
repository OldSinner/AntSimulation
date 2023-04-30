using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone : MonoBehaviour
{
    public PheromoneType type;
    float Strength;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Strength = 20;
        if(type == PheromoneType.ToFood)
        {
            spriteRenderer.color = new Color(255, 0, 0, 20);
        }
        else
        {
            spriteRenderer.color = new Color(0, 0, 255, 20);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ClearStrength();
        ColorPheromone();
        ValidateStrength();
    }

    void ColorPheromone()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, 0, spriteRenderer.color.b, Strength > 0 ? Strength / 255 : 0); 
    }
    void ClearStrength()
    {
        Strength -= 0.5f * Time.deltaTime;
    }
    void ValidateStrength()
    {
        if (Strength <= 0)
        { 
            Destroy(gameObject);
        }
    }

    public void AddStrength()
    {
        Strength += 10;
        if(Strength > 255)
        {
            Strength = 255;
        }
    }
}
