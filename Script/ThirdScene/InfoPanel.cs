using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InfoPanel : MonoBehaviour
{
    public GameManager game;
     public Player currentPlayer;
    [HideInInspector]
    public Characters currentPiece;
    private int currentPlayerIndex;
    private List<Player> players;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Faction ;
    public TextMeshProUGUI Range;
    public TextMeshProUGUI Ability;
    public TextMeshProUGUI currentPlayerId;
    public Image imageCharacter;
    void Start()
    {
        players =SelectCharacter.Instance.players;
        currentPlayer = game.currentPlayer;
        currentPiece = game.currentPiece;
    }

   
    void Update()
    {
        currentPlayerId.text = currentPlayer.id.ToString();
        Name.text = currentPiece.name;
        Faction.text = currentPiece.faction.ToString();
        Range.text = currentPiece.Range.ToString();
        Ability.text = currentPiece.abilityDescription;
        imageCharacter.sprite =currentPiece.characterImage;   
    }
}
