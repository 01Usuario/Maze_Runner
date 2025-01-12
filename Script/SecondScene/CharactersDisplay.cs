using UnityEngine;
using UnityEngine.UI;

public class CharactersDisplay : MonoBehaviour
{
    public Characters character; // Referencia al personaje
    public Image CharacterImgen; // UI Image para mostrar el personaje

    void Start()
    {
        if (character != null)
        {
            CharacterImgen.sprite = character.characterImage; // Asigna la imagen del personaje a la UI
        }
    }
}

