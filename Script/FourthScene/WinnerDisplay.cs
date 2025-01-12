using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinnerDisplay : MonoBehaviour
{
    public TextMeshProUGUI winnerText; // Texto para mostrar el ID del jugador ganador
    public List<Image> characterImages; // Array de imágenes para los personajes
    
    void Start()
    {
        if (GameData.Instance.winnerPlayer != null)
        {
            Player winner = GameData.Instance.winnerPlayer;
            winnerText.text = winner.id.ToString();

            // Mostrar las imágenes de los personajes
            for (int i = 0; i < characterImages.Count; i++)
            {
                if (i < winner.pieceList.Count)
                {
                characterImages[i].sprite = winner.pieceList[i].characterImage;
                characterImages[i].gameObject.SetActive(true);
                }
                
               else
                {
                    characterImages[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
