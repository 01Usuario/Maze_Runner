using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public GameObject characterPrefab;

    void Start()
    {
        
        StartCoroutine(SetUp());
        

    }
    
    public IEnumerator SetUp()
    {
        yield return new WaitUntil(()=>mazeGenerator.created=true);
        PlacePiecesInBoxes();
    }
    void PlacePiecesInBoxes()
    {
        List<(int, int)>[] cajas = { MazeGenerator.Caja1, MazeGenerator.Caja2, MazeGenerator.Caja3, MazeGenerator.Caja4 };

        for (int playerIndex = 0; playerIndex < SelectCharacter.players.Count; playerIndex++)
        {
            Player player = SelectCharacter.players[playerIndex];
            List<(int, int)> currentCaja = cajas[playerIndex];

            

            for (int i = 0; i < player.pieceList.Count; i++)
            {
                if (i < currentCaja.Count)
                {
                    (int x, int y) = currentCaja[i];
                    PlaceCharacterAtPosition(player.pieceList[i], x, y);
                }
            }
        }
    }

    void PlaceCharacterAtPosition(Characters character, int x, int y)
{
    GameObject piece = Instantiate(characterPrefab);
    piece.transform.SetParent(mazeGenerator.WorkSpaceRectTransform, false);
    RectTransform rectTransform = piece.GetComponent<RectTransform>();
    float cellWidth = mazeGenerator.WorkSpaceRectTransform.rect.width / MazeGenerator.dim;
    float cellHeight = mazeGenerator.WorkSpaceRectTransform.rect.height / MazeGenerator.dim;

    // Calcula la posición centrada para la celda (x, y)
    Vector2 anchoredPosition = new Vector2(
        x * cellWidth - mazeGenerator.WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2,
        y * cellHeight - mazeGenerator.WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2
    );
    
    rectTransform.anchoredPosition = anchoredPosition;

    rectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);

    // Asigna la imagen del personaje al prefab
    Image imageComponent = piece.GetComponent<Image>();
    if (imageComponent != null)
    {
        imageComponent.sprite = character.characterImage;
    }

    // Configura el prefab con cualquier otra información necesaria del ScriptableObject
    CharactersDisplay characterDisplay = piece.GetComponent<CharactersDisplay>();
    if (characterDisplay != null)
    {
        characterDisplay.character = character;
    }
}

}