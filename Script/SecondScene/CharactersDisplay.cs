
using UnityEngine.UI;
using UnityEngine;

public class CharactersDisplay : MonoBehaviour
{
    public Characters character;
    public Image CharacterImgen;

    void Start(){
        if(character != null)
        {
            CharacterImgen.sprite = character.characterImage;
        }
    }
}
