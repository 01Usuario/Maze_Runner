using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public static SelectCharacter Instance { get; private set; }
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
    public  List<Player> players = new List<Player>(); 
    private int currentPlayerIndex = 0;
    public Player currentPlayer;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir este objeto entre escenas.
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
                    var imageComponent = characterButtons[i].GetComponent<Image>();
                    var index=i;
                    if (imageComponent != null)
                    {
                        imageComponent.sprite = factions[currentFactionIndex].factionCharacters[index].characterImage;
                        Characters character = factions[currentFactionIndex].factionCharacters[index];
                        characterButtons[index].onClick.RemoveAllListeners();
                        characterButtons[index].onClick.AddListener(() => SelectCharacterForPlayer(character, characterButtons[index]));
                        characterButtons[index].gameObject.SetActive(true);
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
            return;
        }

        if (chosenPieces < piecesToChoose)
        {
            
            currentPlayer.pieceList.Add(character);
            button.gameObject.SetActive(false); // Hide the character button
            chosenPieces++;

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