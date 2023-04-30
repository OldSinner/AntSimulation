using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone : MonoBehaviour
{
    public PheromoneType type;
    public Helper helper;
    float Strength;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        helper = GameObject.Find("Helper").GetComponent<Helper>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Strength = 100;
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
        float alpha = 0;
        if (Strength > 255) alpha = 1;
        else if (Strength <= 0) alpha = 0;
        else alpha = Strength / 255;
        spriteRenderer.color = new Color(spriteRenderer.color.r, 0, spriteRenderer.color.b, alpha); 
    }
    void ClearStrength()
    {
        Strength -= 4 * Time.deltaTime;
    }
    void ValidateStrength()
    {
        if (Strength <= 0)
        {
            helper.PheromoneCounter--;
            Destroy(gameObject);
        }
    }

    public void AddStrength()
    {
        Strength += 20;
        
    }
}
