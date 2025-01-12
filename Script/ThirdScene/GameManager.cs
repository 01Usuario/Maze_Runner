using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.PlasticSCM.Editor.WebApi;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public MazeGenerator mazeGenerator;
    public GameObject characterPrefab;
    public static int CantidadPasos;
    [HideInInspector]
    public Player currentPlayer;
    [HideInInspector]
    public Characters currentPiece;
    private int currentPlayerIndex = 0;
    private List<Player> players;
    private int indexCharacter; 
    public Button nextCharacter;
    public Button prevCharacter;
    public List<GameObject> traps = new List<GameObject>();
    public int turn;
    private bool newGameStarted= true;
    public Player Winner;
    
    #region InfoPanel
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Range;
    public TextMeshProUGUI Ability;
    public TextMeshProUGUI Time;
    public TextMeshProUGUI currentPlayerId;
    public Image imageCharacter;
   

    #endregion InfoPanel

void Awake()
{
    if(Instance==null){
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    Destroy(gameObject);

}
    void Start()
{
    players = SelectCharacter.Instance.players;
    
    currentPlayer = players[currentPlayerIndex];
    currentPiece = currentPlayer.pieceList[0];
    CantidadPasos=currentPiece.Range;
    if (newGameStarted) 
    { 
        ResetAllScriptableObjectValues(); 
    }
    StartCoroutine(SetUp());
}    
void ResetAllScriptableObjectValues() 
{ 
    Characters[] characters = Resources.FindObjectsOfTypeAll<Characters>();
     foreach (var character in characters) 
    { 
        character.ResetValue(); 
    } 
}

    void Update()
{
    DetectedPressKey();
    UpdateVisual();
   if(SceneManager.GetActiveScene().buildIndex==2)
    if(VictoryCondition(currentPlayer))
    SceneManager.LoadScene(3);
}
    public void UpdateVisual()
{
    if(Name!= null)
        Name.text = currentPiece.name;
    if (currentPlayerId != null)
        currentPlayerId.text = currentPlayer.id.ToString();
        
    if (Range != null)
        Range.text = CantidadPasos.ToString();
        
    if (Ability != null)
        Ability.text = currentPiece.abilityDescription;
        
    if (Time != null)
        Time.text = currentPiece.AbilityTime.ToString();
        
    if (imageCharacter != null)
        imageCharacter.sprite = currentPiece.characterImage;
}


    public IEnumerator SetUp()
{
    yield return new WaitUntil(() => mazeGenerator.created == true);
    PlacePiecesInBoxes();
}

void PlacePiecesInBoxes()
{
    List<(int, int)>[] cajas = { MazeGenerator.Caja1, MazeGenerator.Caja2,MazeGenerator.Caja3,MazeGenerator.Caja4 };

    for (int playerIndex = 0; playerIndex < SelectCharacter.Instance.players.Count; playerIndex++)
    {
        Player player = SelectCharacter.Instance.players[playerIndex];
        List<(int, int)> currentCaja = cajas[playerIndex];

        for (int i = 0; i < player.pieceList.Count; i++)
        {
            if (i < currentCaja.Count)
            {
                int x = currentCaja[i].Item1;
                int y = currentCaja[i].Item2;
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

    character.positionJ = x;
    character.positionI = y;

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
public void Move(int dx, int dy)
{
        nextCharacter.interactable = false;
        prevCharacter.interactable= false;
        int newX = currentPiece.positionI + dx;
        int newY = currentPiece.positionJ + dy;

        if (newX < 0 || newX >= MazeGenerator.dim || newY < 0 || newY >= MazeGenerator.dim || mazeGenerator.grid[newX, newY] == 1)
        {
            return;
        }

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
        if(VictoryCondition(currentPlayer))
        {
    
            foreach (var player in players)
            {       foreach(var pieces in player.pieceList)
                {
                pieces.Range = pieces.OriginalRange;
                pieces.AbilityTime = pieces.OriginalAbilityTime;
                pieces.UnderIceEffect = false;
                pieces.Inmunity= false;
                }
            }
    
            SceneManager.LoadScene(3);   
        }
        if (CantidadPasos == 0)
        {
           AdvanceTurn();
        }


}
#endregion

public void AdvanceTurn()
{ 
    if(currentPlayer.id==0){
        turn++;
        Debug.Log(turn);
        ReduceCooldown();
        
    }
    if(SceneManager.GetActiveScene().buildIndex==2)
    UpdateVisual();
    nextCharacter.interactable = true;
    prevCharacter.interactable= true;
    currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    currentPlayer = players[currentPlayerIndex];
    currentPiece = currentPlayer.pieceList[indexCharacter];
    CantidadPasos = currentPiece.Range;
}
 void ReduceCooldown() 
{
     foreach (var player in players) 
    { 
    
    foreach (var character in player.pieceList) 
    { 
        if (character.AbilityTime > 0) 
        {
             character.AbilityTime -= 1; 
             if(character.UnderIceEffect==true)
             character.Range=character.OriginalRange;
             character.Inmunity = false;
        } 
    } 
}
}
public void NextCharacter()
{ 
    indexCharacter =(indexCharacter+1)% currentPlayer.pieceList.Count;
    currentPiece = currentPlayer.pieceList[indexCharacter];
}
public void PreviousCharacter()
{ 
    indexCharacter = (indexCharacter-1 + currentPlayer.pieceList.Count) % currentPlayer.pieceList.Count;
    currentPiece = currentPlayer.pieceList[indexCharacter]; 
}

public  GameObject GetCharacterGameObject(Characters character)
{
    CharactersDisplay[] charactersDisplays = FindObjectsOfType<CharactersDisplay>();
    foreach (var display in charactersDisplays)
    {
        if (display.character == character)
        {
            return display.gameObject;
        }
    }
    return null;
}

    public bool VictoryCondition(Player currentPlayer)
    {
        int count =0;
        foreach(var character in currentPlayer.pieceList)
        {
             foreach(var position in mazeGenerator.CentroList )
            {
                if(character.positionI==position.Item1 && character.positionJ==position.Item2){
                    count++;
                    if(count==ConfigureOptions.pieceCount){
                        GameData.Instance.winnerPlayer =currentPlayer;
                        return true;
                    }
                }
                

            }
           
        }   
        return false;
    }

}
