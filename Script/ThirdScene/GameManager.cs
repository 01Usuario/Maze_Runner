using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class GameManager : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public GameObject characterPrefab;
    public int CantidadPasos;
    [HideInInspector]
    public Player currentPlayer;
    [HideInInspector]
    public Characters currentPiece;
    private int currentPlayerIndex = 0;
    private List<Player> players;
    private int indexCharacter; 
    public Button nextCharacter;
    public Button prevCharacter;
    
    #region InfoPanel
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Faction ;
    public TextMeshProUGUI Range;
    public TextMeshProUGUI Ability;
    public TextMeshProUGUI currentPlayerId;
    public Image imageCharacter;

    #endregion InfoPanel

    void Start()
{
    Debug.Log("Iniciando GameManager...");
    players = SelectCharacter.Instance.players;
    currentPlayer = players[currentPlayerIndex];
    currentPiece = currentPlayer.pieceList[0];
    CantidadPasos=currentPiece.Range;
    Debug.Log($"Primer jugador: {currentPlayer.id}, Primer personaje: {currentPiece.name}");
    StartCoroutine(SetUp());
}

void Update()
{
    DetectedPressKey();
    UpdateVisual();
}
void UpdateVisual()
    {
        currentPlayerId.text = currentPlayer.id.ToString();
        Name.text = currentPiece.name;
        Faction.text = currentPiece.faction.ToString();
        Range.text = currentPiece.Range.ToString();
        Ability.text = currentPiece.abilityDescription;
        imageCharacter.sprite =currentPiece.characterImage;
    }

public IEnumerator SetUp()
{
    Debug.Log("Esperando a que el laberinto esté creado...");
    yield return new WaitUntil(() => mazeGenerator.created == true);
    Debug.Log("Laberinto creado. Colocando personajes en las cajas.");
    PlacePiecesInBoxes();
}

void PlacePiecesInBoxes()
{
    Debug.Log("Colocando personajes en sus cajas iniciales...");
    List<(int, int)>[] cajas = { MazeGenerator.Caja1, MazeGenerator.Caja2, MazeGenerator.Caja3, MazeGenerator.Caja4 };

    for (int playerIndex = 0; playerIndex < SelectCharacter.Instance.players.Count; playerIndex++)
    {
        Player player = SelectCharacter.Instance.players[playerIndex];
        List<(int, int)> currentCaja = cajas[playerIndex];

        Debug.Log($"Colocando personajes del jugador {player.id} en la caja {playerIndex + 1}.");
        for (int i = 0; i < player.pieceList.Count; i++)
        {
            if (i < currentCaja.Count)
            {
                (int x, int y) = currentCaja[i];
                Debug.Log($"Colocando personaje {player.pieceList[i].name} en posición ({x}, {y}).");
                PlaceCharacterAtPosition(player.pieceList[i], x, y);
            }
        }
    }
}

void PlaceCharacterAtPosition(Characters character, int x, int y)
{
    Debug.Log($"Instanciando personaje {character.name} en posición ({x}, {y}).");
    GameObject piece = Instantiate(characterPrefab);
    piece.transform.SetParent(mazeGenerator.WorkSpaceRectTransform, false);
    RectTransform rectTransform = piece.GetComponent<RectTransform>();
    float cellWidth = mazeGenerator.WorkSpaceRectTransform.rect.width / MazeGenerator.dim;
    float cellHeight = mazeGenerator.WorkSpaceRectTransform.rect.height / MazeGenerator.dim;

    Vector2 anchoredPosition = new Vector2(
        x * cellWidth - mazeGenerator.WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2,
        y * cellHeight - mazeGenerator.WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2
    );

    rectTransform.anchoredPosition = anchoredPosition;
    rectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);

    Image imageComponent = piece.GetComponent<Image>();
    if (imageComponent != null)
    {
        imageComponent.sprite = character.characterImage;
    }

    character.positionJ = y;
    character.positionI = x;

    CharactersDisplay characterDisplay = piece.GetComponent<CharactersDisplay>();
    if (characterDisplay != null)
    {
        characterDisplay.character = character;
    }
}

void DetectedPressKey()
{
    if (Input.GetKeyDown(KeyCode.UpArrow)) { Debug.Log("Tecla 'Arriba' presionada."); Move(1, 0); }
    if (Input.GetKeyDown(KeyCode.DownArrow)) { Debug.Log("Tecla 'Abajo' presionada."); Move(-1, 0); }
    if (Input.GetKeyDown(KeyCode.LeftArrow)) { Debug.Log("Tecla 'Izquierda' presionada."); Move(0, -1); }
    if (Input.GetKeyDown(KeyCode.RightArrow)) { Debug.Log("Tecla 'Derecha' presionada."); Move(0, 1); }
    if(Input.GetKeyDown(KeyCode.P)){Debug.Log("Cambiando de turno");AdvanceTurn();}
}
#region Logic Move
void Move(int dx, int dy)
{
    nextCharacter.interactable = false;
    prevCharacter.interactable= false;
    Debug.Log($"Intentando mover el personaje {currentPiece.name}...");
    int newX = currentPiece.positionI + dx;
    int newY = currentPiece.positionJ + dy;

    if (newX < 0 || newX >= MazeGenerator.dim || newY < 0 || newY >= MazeGenerator.dim || mazeGenerator.grid[newX, newY] == 1)
    {
        Debug.Log("Movimiento inválido: fuera de límites o hacia una pared.");
        return;
    }

    Debug.Log($"Movimiento válido. Nueva posición: ({newX}, {newY}).");
    GameObject piece = GetCharacterGameObject(currentPiece);

    mazeGenerator.grid[currentPiece.positionI, currentPiece.positionJ] = 0;
    mazeGenerator.grid[newX, newY] = 2;

    currentPiece.positionI = newX;
    currentPiece.positionJ = newY;

    RectTransform rectTransform = piece.GetComponent<RectTransform>();
    float cellWidth = mazeGenerator.WorkSpaceRectTransform.rect.width / MazeGenerator.dim;
    float cellHeight = mazeGenerator.WorkSpaceRectTransform.rect.height / MazeGenerator.dim;
    rectTransform.anchoredPosition = new Vector2(
        newY * cellWidth - mazeGenerator.WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2,
        newX * cellHeight - mazeGenerator.WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2
    );

    CantidadPasos--;
    Debug.Log($"Pasos restantes: {CantidadPasos}");

    if (CantidadPasos == 0)
    {
        Debug.Log("Pasos agotados. Cambiando turno...");
        AdvanceTurn();
    }
}
#endregion

void AdvanceTurn()
{
    nextCharacter.interactable = true;
    prevCharacter.interactable= true;
    Debug.Log("Cambiando turno...");
    currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    currentPlayer = players[currentPlayerIndex];
    currentPiece = currentPlayer.pieceList[0];
    CantidadPasos = currentPiece.Range;

    Debug.Log($"Turno del jugador: {currentPlayer.id}. Personaje seleccionado: {currentPiece.name}");
}
public void NextCharacter()
{ 
    currentPiece = currentPlayer.pieceList[(indexCharacter+1)% currentPlayer.pieceList.Count];
}
public void PreviousCharacter()
{ 
    indexCharacter = (indexCharacter-1 + currentPlayer.pieceList.Count) % currentPlayer.pieceList.Count;
    currentPiece = currentPlayer.pieceList[indexCharacter];
}


GameObject GetCharacterGameObject(Characters character)
{
    Debug.Log($"Buscando GameObject del personaje {character.name}...");
    CharactersDisplay[] charactersDisplays = FindObjectsOfType<CharactersDisplay>();
    foreach (var display in charactersDisplays)
    {
        if (display.character == character)
        {
            Debug.Log($"GameObject encontrado para {character.name}.");
            return display.gameObject;
        }
    }
    Debug.LogError($"No se encontró el GameObject del personaje {character.name}.");
    return null;
}


}