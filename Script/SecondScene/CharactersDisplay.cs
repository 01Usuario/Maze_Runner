using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharactersDisplay : MonoBehaviour
{
    public Characters character;
    public Image Plataform;
    public Image CharacterImgen;

    void Start(){
        if(character != null)
        {
            CharacterImgen.sprite = character.characterImage;
            Plataform.sprite = character.platform;       
        }
    }
}
