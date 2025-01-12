using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    public static int dim = ConfigureOptions.mazeDimension;
    public int centro ;
    public GameObject muroUIPrefab;
    public GameObject caminoPrefab;
    public GameObject centroPrefab;
    public GameObject trapPrefab; 
    public List<Trap> possibleTraps; 
    public RectTransform WorkSpaceRectTransform;
    public int[,] grid;
    private System.Random rand = new System.Random();
    public static List<(int, int)> Caja1;
    public static List<(int, int)> Caja2;
    public static List<(int,int)> Caja3;
    public static List<(int,int)> Caja4;
    private HashSet<(int, int)> posicionesOcupadas = new HashSet<(int, int)>();
    public List<(int,int)> CentroList ;
    public bool created;

    void Awake()
    {
        centro = dim/2;
        Caja1 = new List<(int, int)> { (2,1), (1,2), (2,2),(1,1) };
        Caja2 = new List<(int, int)> { (dim - 2, dim - 2), (dim - 3, dim - 3), (dim - 2, dim - 3), (dim - 3, dim - 2) };
        Caja3 = new List<(int, int)>{(dim-2,1),(dim-2,2),(dim-3,1),(dim-3,2)};
        Caja4 = new List<(int, int)> {(1,dim-2),(1,dim-3),(2,dim-2),(2,dim-3)};
        CentroList = new List<(int, int)>{(centro-1,centro-1),(centro,centro-1),(centro-1,centro),(centro,centro)};
    }

    void Start()
    {
        
        Debug.Log(dim);
        Debug.Log(centro);
        
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
        GenerateMaze(1,1); 
         CrearLaberintoUI(); 
         foreach(var trap in possibleTraps)
        {
            ColocarTrampas(trap);
            
            ColocarTrampas(trap);
        } 
       // CrearLaberintoUI();
        
        
    }

    private void ReservarEspacios()
    {
       foreach(var elemento in CentroList)
            grid[elemento.Item1, elemento.Item2] = 4; 
        foreach (var elemento in Caja1)
            grid[elemento.Item1, elemento.Item2] = 3;
        foreach (var elemento in Caja2)
            grid[elemento.Item1, elemento.Item2] = 3;
        foreach(var elemento in Caja3)
            grid[elemento.Item1,elemento.Item2]=3;
        foreach (var elemento in Caja4)
            grid[elemento.Item1,elemento.Item2]=3;

    }

    void GenerateMaze(int x, int y) { 
        int[] dx = { -1, 1, 0, 0 }; // Direcciones posibles en el eje x 
        int[] dy = { 0, 0, -1, 1 }; // Direcciones posibles en el eje y 
        grid[x, y] = 0; // Establecer el punto de partida como camino 

        // Mezclar las direcciones para crear caminos aleatorios 
        for (int i = 0; i < dx.Length; i++) { 
            int randomIndex = rand.Next(i, dx.Length); 
            int tempX = dx[i]; 
            int tempY = dy[i]; 
            dx[i] = dx[randomIndex]; 
            dy[i] = dy[randomIndex]; 
            dx[randomIndex] = tempX; 
            dy[randomIndex] = tempY; 
            } // Probar cada dirección 
            for (int i = 0; i < dx.Length; i++) 
            { int newX = x + dx[i] * 2; 
            int newY = y + dy[i] * 2;
            if (IsInBounds(newX, newY) && grid[newX, newY] == 1) 
            { 
                grid[x + dx[i], y + dy[i]] = 0; // Abrir el camino entre las celdas 
                GenerateMaze(newX, newY); // Llamada recursiva para continuar abriendo caminos 
            } 
        } 
    } 
        bool IsInBounds(int x, int y) 
        { 
            return x > 0 && x < dim - 1 && y > 0 && y < dim - 1;
        }

    private void ColocarTrampas(Trap trap)
    {
    
            int x = rand.Next(3, dim - 3);
            int y = rand.Next(3, dim - 3);

            if (grid[x, y] == 0 && !posicionesOcupadas.Contains((x, y)))
            {
                posicionesOcupadas.Add((x, y));
                // Instancia el prefab de la trampa
                GameObject trapObject = Instantiate(trapPrefab);
                trapObject.transform.SetParent(WorkSpaceRectTransform, false);
                RectTransform rectTransform = trapObject.GetComponent<RectTransform>();
                float cellWidth = WorkSpaceRectTransform.rect.width / dim;
                float cellHeight = WorkSpaceRectTransform.rect.height / dim;
                // Calcula la posición centrada para la celda (x, y)
                Vector2 anchoredPosition = new Vector2(
                    y * cellWidth - WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2,
                    x * cellHeight - WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2
                );
                grid[x,y]=2;
                rectTransform.anchoredPosition = anchoredPosition;
                rectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);

                Image imageComponent =trapObject.GetComponent<Image>();
                 if (imageComponent != null)
                {
                    imageComponent.sprite = trap.ImageTrap;
                }

                // Cargar la información del ScriptableObject en el prefab instanciado
                TrapDisplay trapController = trapObject.GetComponent<TrapDisplay >();
                if (trapController != null)
                {
                    trapController.trap = trap;
                }

            }
            else
            {
                ColocarTrampas(trap);
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
                if (grid[i, j] ==1 )
                {
                    Vector2 posicion = new Vector2(j * cellWidth - WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2, (i) * cellHeight - WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2);
                    GameObject muro = Instantiate(muroUIPrefab, WorkSpaceRectTransform);
                    muro.GetComponent<RectTransform>().anchoredPosition = posicion;
                    muro.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                }
                if ( grid[i, j] ==0|| grid[i,j]==3)
                {
                    Vector2 posicion = new Vector2(j * cellWidth - WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2, (i) * cellHeight - WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2);
                    GameObject camino = Instantiate(caminoPrefab, WorkSpaceRectTransform);
                    camino.GetComponent<RectTransform>().anchoredPosition = posicion;
                    camino.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                }
                if(grid[i,j]==4){
                    Vector2 posicion = new Vector2(j * cellWidth - WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2, (i) * cellHeight - WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2);
                    GameObject camino = Instantiate(centroPrefab, WorkSpaceRectTransform);
                    camino.GetComponent<RectTransform>().anchoredPosition = posicion;
                    camino.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                }
                
            }
        }
        created = true;
    }
}
