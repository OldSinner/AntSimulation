using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationSettings : MonoBehaviour
{
    [Header("Map")]
    public Vector2 MapStart;
    public int CellLength;
    public int CELL_WIDTH;
    public int CELL_HEIGHT;

    [Header("Pheromone")]
    public float PheromoneLifeTime;
    public Color FoodPheromoneColor;
    public Color HomePheromoneColor;
    public float PheromoneSize;
}
