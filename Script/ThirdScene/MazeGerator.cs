using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public static int dim = ConfigureOptions.mazeDimension;
    public GameObject muroUIPrefab;
    public GameObject caminoPrefab;
    public RectTransform WorkSpaceRectTransform;
    public int[,] grid;
    private System.Random rand = new System.Random();
    public static List<(int, int)> Caja1;
    public static List<(int, int)> Caja2;
    public static List<(int, int)> Caja3;
    public static List<(int, int)> Caja4;
    public int cellSize = 1;
    List<(int, int)> posicionTrampas = new List<(int, int)>();
    public bool created;

    void Awake()
    {
        Caja1 = new List<(int, int)> { (1, 1), (1, 2), (2, 1), (2, 2) };
        Caja2 = new List<(int, int)> { (dim - 2, dim - 2), (dim - 3, dim - 3), (dim - 2, dim - 3), (dim - 3, dim - 2) };
      
    }

    void Start()
    {
        grid = new int[dim, dim];

        // Iniciar el laberinto con paredes (1 = pared, 0 = camino)
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                grid[i, j] = 1;
            }
        }

        ReservarEspacios();
        GenerarLaberintoPrim();
        CrearLaberintoUI();
    }

    private void ReservarEspacios()
    {
        int centroFila = dim / 2;
        int centroColumna = dim / 2;

        grid[centroFila, centroColumna] = 2;
        grid[centroFila, centroColumna - 1] = 2;
        grid[centroFila - 1, centroColumna] = 2;
        grid[centroFila - 1, centroColumna - 1] = 2;

        foreach (var elemento in Caja1)
            grid[elemento.Item1, elemento.Item2] = 2;

        foreach (var elemento in Caja2)
            grid[elemento.Item1, elemento.Item2] = 2;
       
    }

    private void GenerarLaberintoPrim()
    {
        List<(int, int)> walls = new List<(int, int)>();
        int startX = 1, startY = 1;
        grid[startX, startY] = 0;
        walls.Add((startX - 1, startY));
        walls.Add((startX + 1, startY));
        walls.Add((startX, startY - 1));
        walls.Add((startX, startY + 1));

        while (walls.Count > 0)
        {
            int randIndex = rand.Next(walls.Count);
            (int x, int y) = walls[randIndex];
            walls.RemoveAt(randIndex);

            if (x > 0 && x < dim - 1 && y > 0 && y < dim - 1)
            {
                int surroundingPaths = 0;
                if (grid[x - 1, y] == 0) surroundingPaths++;
                if (grid[x + 1, y] == 0) surroundingPaths++;
                if (grid[x, y - 1] == 0) surroundingPaths++;
                if (grid[x, y + 1] == 0) surroundingPaths++;

                if (surroundingPaths == 1)
                {
                    grid[x, y] = 0;
                    walls.Add((x - 1, y));
                    walls.Add((x + 1, y));
                    walls.Add((x, y - 1));
                    walls.Add((x, y + 1));
                }
            }
        }
    }

    public void CrearLaberintoUI()
    {
        float cellWidth = WorkSpaceRectTransform.rect.width / dim;
        float cellHeight = WorkSpaceRectTransform.rect.height / dim;

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (grid[i, j] == 1)
                {
                    Vector2 posicion = new Vector2(j * cellWidth - WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2, (i) * cellHeight - WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2);
                    GameObject muro = Instantiate(muroUIPrefab, WorkSpaceRectTransform);
                    muro.GetComponent<RectTransform>().anchoredPosition = posicion;
                    muro.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                }
                else if (grid[i, j] == 2 || grid[i, j] == 0)
                {
                    Vector2 posicion = new Vector2(j * cellWidth - WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2, (i) * cellHeight - WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2);
                    GameObject camino = Instantiate(caminoPrefab, WorkSpaceRectTransform);
                    camino.GetComponent<RectTransform>().anchoredPosition = posicion;
                    camino.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                }
            }
        }
        created = true;
    }
}
