using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MazeGenerator : MonoBehaviour
{
   public static int dim= ConfigureOptions.mazeDimension;
    public GameObject muroUIPrefab;
    public GameObject caminoPrefab;
    public RectTransform WorkSpaceRectTransform;
    private int[,] grid;
    private System.Random rand = new System.Random();
    public List <MazeCell> trap = new List<MazeCell>();
    public static List<(int,int)> Caja1;
    public static List<(int,int)> Caja2;
    public static List<(int,int)> Caja3;
    public static List<(int,int)> Caja4;
    public static int cellSize=1;
    List<(int,int)> posicionTrampas = new List<(int,int)> ();
    public bool created;

    void Awake(){
        Caja1= new List<(int, int)>{(1,1),(1,2),(2,1),(2,2)};
        Caja2= new List<(int, int)>{(1,dim-2),(1,dim-3),(2,dim-2),(2,dim-3)};
        Caja3= new List<(int, int)>{(dim-2,1),(dim-3,1),(dim-3,2),(dim-2,2)};
        Caja4= new List<(int, int)>{(dim-2,dim-2),(dim-3,dim-3),(dim-2,dim-3),(dim-3,dim-2)};   
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
        // Generar el laberinto
        GenerarLaberinto(1, 1);
        // Instanciar el laberinto en el Canvas
        CrearLaberintoUI();
        GenerarTrampas();
        created = true;
    }

     private void ReservarEspacios(){
        // Crear el espacio de 2x2 en el centro
        int centroFila = dim / 2;
        int centroColumna = dim/ 2;

        grid[centroFila, centroColumna] = 2;
        grid[centroFila, centroColumna - 1] = 2;
        grid[centroFila - 1, centroColumna] = 2;
        grid[centroFila - 1, centroColumna - 1] = 2;
        
       foreach(var elemento in Caja1)
       grid[elemento.Item1, elemento.Item2] = 2;

       foreach(var elemento in Caja2)
       grid[elemento.Item1, elemento.Item2] = 2;

    foreach(var elemento in Caja3)
       grid[elemento.Item1, elemento.Item2] = 2;

    foreach(var elemento in Caja4)
       grid[elemento.Item1, elemento.Item2] = 2;

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


     private void GenerarTrampas()
     {
        List<(int,int)>position=positionTraps();

        
        
     }
     private List<(int,int)> positionTraps(){
        for (int i = 0;i<10;i++)
        {
            int random1=UnityEngine.Random.Range(0,ConfigureOptions.mazeDimension);
            int random2=UnityEngine.Random.Range(0,ConfigureOptions.mazeDimension);
            if(grid[random1,random2]==0)
            {
                posicionTrampas.Add((random1,random2));
            }

        }
        return posicionTrampas;

     }
    private void CrearLaberintoUI()
    {
        float cellWidth = WorkSpaceRectTransform.rect.width / dim;
        float cellHeight = WorkSpaceRectTransform.rect.height /dim;

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (grid[i, j] == 1)
                {
                    Vector2 posicion = new Vector2(j * cellWidth - WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2, i * cellHeight - WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2);
                    GameObject muro = Instantiate(muroUIPrefab, WorkSpaceRectTransform);
                    muro.GetComponent<RectTransform>().anchoredPosition = posicion;
                    muro.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                }
                
                else if( grid[i,j]==2 || grid[i,j]==0) {
                    Vector2 posicion = new Vector2(j * cellWidth - WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2, i * cellHeight - WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2);
                    GameObject camino = Instantiate(caminoPrefab, WorkSpaceRectTransform);
                    camino.GetComponent<RectTransform>().anchoredPosition = posicion;
                    camino.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                }

                
            }
        }
    }
}
