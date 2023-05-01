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
                Cells[i, j] = new Cell(i, j);
            }
        }

        SetupParticle();
    }

    private void SetupParticle()
    {
        particle = GetComponent<ParticleSystem>();
        var main = particle.main;
        main.startSpeed = 0;
        main.maxParticles = 100000;
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

    public int GetPheromoneInCircle(Vector2 center, float radius, PheromoneType type)
    {
        var cell = GetCellBaseOnPosition(center);
        if (cell == null) return 0;

        var cells = GetNeighborsCells(cell.x, cell.y, cell);

        int sum = 0;
        foreach (var cellEntry in cells)
        {
            ClearEmptyPheromones(cellEntry.Pheromones);
            foreach (var pheromone in cellEntry.Pheromones)
            {
                if (pheromone.Type == type)
                {
                    if (Vector2.Distance(pheromone.Location, center) <= radius)
                    {
                        var time = DateTime.Now - pheromone.CreationTime;
                        sum += (int)(10000 - time.TotalMilliseconds) / 100;
                    }
                }
            }
        }

        return sum;
    }

    private void ClearEmptyPheromones(List<Pheromone> pheromones)
    {
        for (int i = 0; i < pheromones.Count - 1; i++)
        {
            if (pheromones[i].CreationTime.AddSeconds(settings.PheromoneLifeTime) < DateTime.Now)
            {
                pheromones.RemoveAt(i);
            }
        }
    }
    private Cell[] GetNeighborsCells(int x, int y) => GetNeighborsCells(x, y, null);
    private Cell[] GetNeighborsCells(int x, int y, Cell? cell)
    {
        List<Cell> cells = new List<Cell>();
        if (cell != null) cells.Add(cell);
        if (x > 0)
        {
            cells.Add(Cells[x - 1, y]);
        }
        if (x < settings.CELL_WIDTH - 1)
        {
            cells.Add(Cells[x + 1, y]);
        }
        if (y > 0)
        {
            cells.Add(Cells[x, y - 1]);
        }
        if (y < settings.CELL_HEIGHT - 1)
        {
            cells.Add(Cells[x, y + 1]);
        }
        if (x > 0 && y > 0)
        {
            cells.Add(Cells[x - 1, y - 1]);
        }
        if (x < settings.CELL_WIDTH - 1 && y < settings.CELL_HEIGHT - 1)
        {
            cells.Add(Cells[x + 1, y + 1]);
        }
        if (x > 0 && y < settings.CELL_HEIGHT - 1)
        {
            cells.Add(Cells[x - 1, y + 1]);
        }
        if (x < settings.CELL_WIDTH - 1 && y > 0)
        {
            cells.Add(Cells[x + 1, y - 1]);
        }
        return cells.ToArray();
    }

    public Cell? GetCellBaseOnPosition(Vector2 position)
    {
        var x = (int)((Mathf.Abs(settings.MapStart.x) + position.x) / settings.CellLength);
        var y = (int)((Mathf.Abs(settings.MapStart.y) + position.y) / settings.CellLength);

        if (x >= settings.CELL_WIDTH || y >= settings.CELL_HEIGHT || x < 0 || y < 0)
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
        public int x;
        public int y;
        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Pheromone
    {
        public PheromoneType Type;
        public Vector2 Location;
        public DateTime CreationTime;
    }
}
public enum PheromoneType
{
    Food,
    Home
}
