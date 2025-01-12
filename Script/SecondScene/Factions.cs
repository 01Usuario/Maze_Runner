using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Faction")]
public class Factions : ScriptableObject
{
    public Sprite factionImage; // Imagen de la facción
    public List<Characters> factionCharacters = new List<Characters>(); // Lista de personajes en la facción
}
