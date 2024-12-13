using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public List<Factions> factions = new List<Factions>();
    public Image factionImageDisplay;
    public Transform charactersPanel; 
    public List<Button> characterButtons; 
    public Button nextFactionButton; 
    public Button previousFactionButton; 

    private int currentFactionIndex = 0;
    private int piecesToChoose;
    private int chosenPieces = 0;
    private int totalPlayers;
    public List<Player> players = new List<Player>(); 
    private int currentPlayerIndex = 0;
    public Player currentPlayer;

    void Start()
    {
        piecesToChoose = ConfigureOptions.pieceCount;
        totalPlayers = ConfigureOptions.playerCount;

        InitializePlayers();
        currentPlayer = players[currentPlayerIndex];
        DisplayFaction();
    }

     void InitializePlayers()
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            players.Add(new Player(i));
        }
    }

    void DisplayFaction()
    {
        // Update the faction image
        if (currentFactionIndex >= 0 && currentFactionIndex < factions.Count)
        {
            factionImageDisplay.sprite = factions[currentFactionIndex].factionImage;

            // Assign the faction's characters to buttons
            for (int i = 0; i < characterButtons.Count; i++)
            {
                if (i < factions[currentFactionIndex].factionCharacters.Count)
                {
                    var localIndex = i; // Capture local index
                    var imageComponent = characterButtons[localIndex].GetComponent<Image>();

                    if (imageComponent != null)
                    {
                        imageComponent.sprite = factions[currentFactionIndex].factionCharacters[localIndex].characterImage;
                        Characters character = factions[currentFactionIndex].factionCharacters[localIndex];
                        characterButtons[localIndex].onClick.RemoveAllListeners();
                        characterButtons[localIndex].onClick.AddListener(() => SelectCharacterForPlayer(character, characterButtons[localIndex]));
                        characterButtons[localIndex].gameObject.SetActive(true);
                    }
                }
                else
                {
                    characterButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void NextFaction()
    {
        if (chosenPieces == 0) // Allow faction change only if no pieces are chosen
        {
            currentFactionIndex = (currentFactionIndex + 1) % factions.Count;
            DisplayFaction();
        }
    }

    public void PreviousFaction()
    {
        if (chosenPieces == 0) // Allow faction change only if no pieces are chosen
        {
            currentFactionIndex = (currentFactionIndex - 1 + factions.Count) % factions.Count;
            DisplayFaction();
        }
    }


    void SelectCharacterForPlayer(Characters character, Button button)
    {
        if (currentPlayer == null || currentPlayer.pieceList == null)
        {
            Debug.LogError("Error: CurrentPlayer or its pieceList is not initialized.");
            return;
        }

        if (chosenPieces < piecesToChoose)
        {
            currentPlayer.pieceList.Add(character);
            button.gameObject.SetActive(false); // Hide the character button
            chosenPieces++;

            // Disable navigation buttons
            nextFactionButton.interactable = false;
            previousFactionButton.interactable = false;

            if (chosenPieces == piecesToChoose)
            {
                AdvanceTurn();
            }
        }
    }

void AdvanceTurn()
    {
        currentPlayerIndex++;

        if (currentPlayerIndex < players.Count)
        {
            currentPlayer = players[currentPlayerIndex];
            chosenPieces = 0;
            nextFactionButton.interactable = true;
            previousFactionButton.interactable = true;
            DisplayFaction();
        }
        else
        {
            SceneManager.LoadScene(2); // Proceed to the next scene when all players are done
        }
    }
}
