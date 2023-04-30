using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PheromoneMap : MonoBehaviour
{
    public Cell[,] Cells;
    ParticleSystem particle;
    ParticleSystem.EmitParams particleSettings = new ParticleSystem.EmitParams();


    private SimulationSettings settings;
    void Start()
    {
        settings = GameObject.Find("SimulationSettings").GetComponent<SimulationSettings>();
        Cells = new Cell[settings.CELL_WIDTH, settings.CELL_HEIGHT];
        for (int i = 0; i < settings.CELL_WIDTH; i++)
        {
            for (int j = 0; j < settings.CELL_HEIGHT; j++)
            {
                Cells[i, j] = new Cell();
            }
        }

        SetupParticle();
    }

    private void SetupParticle()
    {
        particle = GetComponent<ParticleSystem>();
        var clf = particle.colorOverLifetime;
        clf.enabled = true;
        Gradient grad = new Gradient();

        grad.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.white, 1) };
        grad.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(0.0f, 1.0f) };
        clf.color = grad;

        particleSettings.startSize = settings.PheromoneSize;
        particleSettings.startLifetime = settings.PheromoneLifeTime;
        particleSettings.velocity = Vector3.zero;
    }

    public void AddPheromone(Vector2 location, PheromoneType type)
    {
        var cell = GetCellBaseOnPosition(location);
        if (cell == null) return;

        cell.Pheromones.Add(new Pheromone
        {
            CreationTime = DateTime.Now,
            Location = location,
            Type = type
        });
        particleSettings.startColor = type == PheromoneType.Food ? settings.FoodPheromoneColor : settings.HomePheromoneColor;
        particleSettings.position = location;
        particle.Emit(particleSettings, 1);
    }

    private void OnDrawGizmos()
    {
        if (settings == null)
        {
            settings = GameObject.Find("SimulationSettings").GetComponent<SimulationSettings>();
        }
        if (settings != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < settings.CELL_WIDTH; i++)
            {
                for (int j = 0; j < settings.CELL_HEIGHT; j++)
                {
                    Gizmos.DrawWireCube(new Vector2(settings.MapStart.x + settings.CellLength * i + settings.CellLength / 2, settings.MapStart.y + settings.CellLength * j + settings.CellLength / 2), new Vector2(settings.CellLength, settings.CellLength));
                }
            }
        }
    }

    public Cell? GetCellBaseOnPosition(Vector2 position)
    {
        var x = (int)((Mathf.Abs(settings.MapStart.x) + position.x) / settings.CellLength);
        var y = (int)((Mathf.Abs(settings.MapStart.y) + position.y) / settings.CellLength);

        if(x >= settings.CELL_WIDTH || y >= settings.CELL_HEIGHT || x < 0 || y < 0)
        {
            return null;
        }
        return Cells[x, y];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public class Cell
    {
        public List<Pheromone> Pheromones = new();
    }

    public class Pheromone
    {
        public PheromoneType Type;
        public Vector2 Location;
        public DateTime CreationTime;
    }

    public enum PheromoneType
    {
        Food,
        Home
    }
}
