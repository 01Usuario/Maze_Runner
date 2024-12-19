using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class Characters : ScriptableObject
{
    public Sprite characterImage;
    public Faction faction;
    public Ability ability;
    public string abilityDescription;
    public int Range;
    public int AbilityTime;

    [HideInInspector]
    public Player Owner;
    [HideInInspector]
    public int positionI;
    [HideInInspector]
    public int  positionJ;
}

public enum Faction { Shingeki_No_Kyojin, OnePiece, Naruto, Marvel,None };
public enum Ability { Obstruct,DetectTrap,InvalidTrap,ExtraMove };
