using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using UnityEditor.Search;


public class TrapManager : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public GameManager gameManager;

   
    void Start()
    {
        mazeGenerator = FindObjectOfType<MazeGenerator>();
        gameManager = FindObjectOfType<GameManager>();
    }

   
    public void ActivateTrapEffect(Trap trap, GameObject characterObject)
    {
        CharactersDisplay display = characterObject.GetComponent<CharactersDisplay>();
        Characters character = display.character;
        
        switch (trap.trapType)
        {      
            case TrapType.Ice:
                if(!character.Inmunity)
                ApplyIceEffect(characterObject);
                break;

            case TrapType.Hole:
                if(!character.Inmunity)
                ApplyHoleEffect();
                break;

            case TrapType.Teleport:
                if(!character.Inmunity)
                ApplyTeleportEffect(characterObject);
                break;
            case TrapType.Update:
                ApplyUpdateEffect(characterObject);
                break;
            case TrapType.Lava:
            ApplyLavaEffect(characterObject);
            break;
        }
        
    }

    private void  ApplyUpdateEffect(GameObject characterObject)
    {
        CharactersDisplay charactersDisplay= characterObject.GetComponent<CharactersDisplay>();
        Characters character = charactersDisplay.character;
        character.Range+=2;
        if(character.AbilityTime>0)
        character.AbilityTime-=1;    
        gameManager.UpdateVisual();

    }

public void ApplyTeleportEffect(GameObject characterObject)
{

    int x = Random.Range(2, ConfigureOptions.mazeDimension - 2);
    int y = Random.Range(2, ConfigureOptions.mazeDimension - 2);

    if (mazeGenerator.grid[x, y] == 0)
    {
        MoveCharacterToPosition(characterObject, x, y);
        Debug.Log($"Personaje teletransportado a ({x}, {y}).");
    }
    else
    {
        Debug.Log("Posición inválida para teletransporte, reintentando...");
        ApplyTeleportEffect(characterObject); // Reintentar si la posición no es válida.
    }
}
    private void ApplyIceEffect(GameObject Object)
    {
        
        CharactersDisplay charactersDisplay= Object.GetComponent<CharactersDisplay>();
        Characters character = charactersDisplay.character;
      if(!character.UnderIceEffect){
        character.UnderIceEffect = true;
        character.Range-=2;
        character.AbilityTime+=2;
        gameManager.UpdateVisual();
        if(GameManager.CantidadPasos>2){
            GameManager.CantidadPasos-=2;
        }
        else gameManager.AdvanceTurn();
        
        
      }

    }

    private void ApplyHoleEffect()
    {
        if(GameManager.CantidadPasos!=0)
        {
            gameManager.AdvanceTurn();
        }
        
    }
    public void ApplyLavaEffect(GameObject Object)
    {
       if( gameManager.currentPlayer.id==0)
       {
            MoveCharacterToPosition(Object,1,1);
       }
        if( gameManager.currentPlayer.id==1)
       {
            MoveCharacterToPosition(Object,MazeGenerator.dim-2,MazeGenerator.dim-2);
       }
        if( gameManager.currentPlayer.id==2)
       {
            MoveCharacterToPosition(Object,MazeGenerator.dim-2,1);
       }
        if( gameManager.currentPlayer.id==3)
       {
            MoveCharacterToPosition(Object,1,MazeGenerator.dim-2);
       }
    }

    
    
    public void MoveCharacterToPosition(GameObject characterObject, int x, int y)
{
    CharactersDisplay characterDisplay = characterObject.GetComponent<CharactersDisplay>();
    if (characterDisplay == null)
    {
        Debug.LogError("El GameObject no tiene el componente CharactersDisplay.");
        return;
    }

    Characters character = characterDisplay.character;
    if (character == null)
    {
        Debug.LogError("El CharactersDisplay no tiene un Characters asignado.");
        return;
    }

    // Verifica si la nueva posición es válida
    if (x < 0 || x >= MazeGenerator.dim || y < 0 || y >= MazeGenerator.dim || mazeGenerator.grid[x, y] == 1)
    {
        Debug.Log("Movimiento inválido: fuera de límites o hacia una pared.");
        return;
    }

    // Actualiza la cuadrícula
    mazeGenerator.grid[character.positionI, character.positionJ] = 0;
    mazeGenerator.grid[x, y] = 2;

    // Actualiza la posición del personaje
    character.positionI = x;
    character.positionJ = y;

    // Mueve el objeto en la interfaz de usuario
    RectTransform rectTransform = characterObject.GetComponent<RectTransform>();
    float cellWidth = mazeGenerator.WorkSpaceRectTransform.rect.width / MazeGenerator.dim;
    float cellHeight = mazeGenerator.WorkSpaceRectTransform.rect.height / MazeGenerator.dim;
    rectTransform.anchoredPosition = new Vector2(
        y * cellWidth - mazeGenerator.WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2,
        x * cellHeight - mazeGenerator.WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2
    );
}

   
}
