
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faction")]
public class Factions : ScriptableObject
{
    public Sprite factionImage;
    public List<Characters> factionCharacters = new List<Characters>();
}
