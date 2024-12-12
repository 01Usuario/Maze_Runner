using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PersonajesDisplay : MonoBehaviour
{
    public Personajes character;
    public Image Plataform;
    public Image CharacterImgen;

    void Start(){
        if(character != null)
        {
            CharacterImgen.sprite = character.CharacterImage;
            Plataform.sprite = character.plataform;       
        }
    }
}
