
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using System.Collections.Generic;

public class AbilityActivator : MonoBehaviour
{
    public Button activateAbilityButton;
    public GameObject obstaclePrefab;
    public GameManager gameManager;
    public MazeGenerator mazeGenerator;
    public TrapManager trapManager;
    public Characters currentCharacter;

    void Start()
    {
        activateAbilityButton.onClick.AddListener(ActivateCurrentCharacterAbility);
    }

    void Update()
    {
        currentCharacter = GameManager.Instance.currentPiece;
        HandleCooldown();
        CheckIfAbilityReady();
    }
    
    void HandleCooldown()
    {
        if (currentCharacter != null && currentCharacter.AbilityTime > 0)
        {
            activateAbilityButton.interactable = false;
        }
        else if (currentCharacter != null)
        {
            activateAbilityButton.interactable = true;
        }
    }

    void CheckIfAbilityReady()
    {
        if (currentCharacter != null && currentCharacter.AbilityTime <= 0)
        {
            activateAbilityButton.interactable = true;
        }
        else
        {
            activateAbilityButton.interactable = false;
        }
    }

    void ActivateCurrentCharacterAbility()
    {
        if (currentCharacter == null || currentCharacter.AbilityTime > 0)
        {
            return;
        }

        // Reset cooldown to original ability time
        currentCharacter.AbilityTime = currentCharacter.OriginalAbilityTime;
        Debug.Log("" + currentCharacter.AbilityTime);

        switch (currentCharacter.ability)
        {
            case Ability.Obstruct:
                Obstruct(currentCharacter);
                break;
            case Ability.Inmunity:
                Inmunity(currentCharacter);
                break;
            case Ability.Teleport:
                Teleport(currentCharacter);
                break;
            case Ability.ExtraMove:
                ExtraMove(currentCharacter);
                break;
            case Ability.ResetStatus:
                ResetStatus(currentCharacter);
                break;
        }
    }

    void Obstruct(Characters current)
    {
        current.AbilityTime = current.OriginalAbilityTime;
        int x, y;
        do
        {
            x = Random.Range(2, ConfigureOptions.mazeDimension - 2);
            y = Random.Range(2, ConfigureOptions.mazeDimension - 2);
        } while (mazeGenerator.grid[x, y] != 0);

        GameObject obstacle = Instantiate(obstaclePrefab, gameManager.mazeGenerator.WorkSpaceRectTransform);
        float cellWidth = gameManager.mazeGenerator.WorkSpaceRectTransform.rect.width / ConfigureOptions.mazeDimension;
        float cellHeight = gameManager.mazeGenerator.WorkSpaceRectTransform.rect.height / ConfigureOptions.mazeDimension;

        RectTransform rectTransform = obstacle.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(
            y * cellWidth - gameManager.mazeGenerator.WorkSpaceRectTransform.rect.width / 2 + cellWidth / 2,
            x * cellHeight - gameManager.mazeGenerator.WorkSpaceRectTransform.rect.height / 2 + cellHeight / 2
        );
        rectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);

        gameManager.mazeGenerator.grid[x, y] = 1;
    }

    void Teleport(Characters current)
    {
        current.AbilityTime = current.OriginalAbilityTime;

        GameObject characterObject = gameManager.GetCharacterGameObject(gameManager.currentPiece);
        if (characterObject != null)
        {
            trapManager.ApplyTeleportEffect(characterObject);
        }
    }

    void ExtraMove(Characters current)
    {
        current.AbilityTime = current.OriginalAbilityTime;
        GameManager.CantidadPasos += 2;
        current.Range+=2;
    }

    void ResetStatus(Characters current)
    {
        current.AbilityTime = current.OriginalAbilityTime;

        foreach (var piece in gameManager.currentPlayer.pieceList)
        {
            if(piece.Range<piece.OriginalRange){
                piece.Range = piece.OriginalRange;
            }
            
            piece.AbilityTime = piece.OriginalAbilityTime;
            piece.UnderIceEffect = false;
        }
    }

    void Inmunity(Characters current)
    {
        current.AbilityTime = current.OriginalAbilityTime;

        if (current != null)
        {
            current.Inmunity = true;
           
        }
    }
}
