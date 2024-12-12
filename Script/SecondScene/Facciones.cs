using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Faccion")]
public class Facciones : ScriptableObject
{
   public Sprite FaccionImage;
    public List<Personajes> faccion = new List<Personajes>();
}
   
