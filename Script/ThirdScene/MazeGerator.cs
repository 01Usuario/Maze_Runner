using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MazeGenerator : MonoBehaviour
{
   public static int dim= ConfigurarOpciones.dimensionLaberinto;
    public GameObject muroUIPrefab;
    public GameObject caminoPrefab;
    public RectTransform canvasRectTransform;
    private int[,] grid;
    private System.Random rand = new System.Random();

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

        // Crear el espacio de 2x2 en el centro
        int centroFila = dim / 2;
        int centroColumna = dim/ 2;
        grid[centroFila, centroColumna] = 0;
        grid[centroFila, centroColumna - 1] = 0;
        grid[centroFila - 1, centroColumna] = 0;
        grid[centroFila - 1, centroColumna - 1] = 0;
        
        //Crear las casillas
        grid[1,1]=0;
        grid[2,1]=0;
        grid[1,2]=0;
        grid[2,2]=0;

        grid[1,dim-2]=0;
        grid[1,dim-3]=0;
        grid[2,dim-3]=0;
        grid[2,dim-2]=0;

        grid[dim-2,1]=0;
        grid[dim-3,1]=0;
        grid[dim-3,2]=0;
        grid[dim-2,2]=0;

        grid[dim-2,dim-2]=0;
        grid[dim-2,dim-3]=0;
        grid[dim-3,dim-2]=0;
        grid[dim-3,dim-3]=0;

        // Generar el laberinto
        GenerarLaberinto(1, 1);
        // Instanciar el laberinto en el Canvas
        CrearLaberintoUI();
    }

    private void GenerarLaberinto(int fila, int columna)
    {
        var direcciones = new List<int[]> { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 0 } };
        Shuffle(direcciones);

        foreach (var dir in direcciones)
        {
            int nuevaFila = fila + dir[0] * 2;
            int nuevaColumna = columna + dir[1] * 2;

            if (EnRango(nuevaFila, nuevaColumna) && grid[nuevaFila, nuevaColumna] == 1)
            {
                grid[fila + dir[0], columna + dir[1]] = 0;
                grid[nuevaFila, nuevaColumna] = 0;
                GenerarLaberinto(nuevaFila, nuevaColumna);
            }
        }
    }

    private void Shuffle(List<int[]> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private bool EnRango(int fila, int columna)
    {
        return fila > 0 && fila < dim&& columna > 0 && columna <dim;
    }

    private void CrearLaberintoUI()
    {
        float cellWidth = canvasRectTransform.rect.width / dim;
        float cellHeight = canvasRectTransform.rect.height /dim;

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (grid[i, j] == 1)
                {
                    Vector2 posicion = new Vector2(j * cellWidth - canvasRectTransform.rect.width / 2 + cellWidth / 2, i * cellHeight - canvasRectTransform.rect.height / 2 + cellHeight / 2);
                    GameObject muro = Instantiate(muroUIPrefab, canvasRectTransform);
                    muro.GetComponent<RectTransform>().anchoredPosition = posicion;
                    muro.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                }
                else {
                    Vector2 posicion = new Vector2(j * cellWidth - canvasRectTransform.rect.width / 2 + cellWidth / 2, i * cellHeight - canvasRectTransform.rect.height / 2 + cellHeight / 2);
                    GameObject camino = Instantiate(caminoPrefab, canvasRectTransform);
                    camino.GetComponent<RectTransform>().anchoredPosition = posicion;
                    camino.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                }

                
            }
        }
    }
}
