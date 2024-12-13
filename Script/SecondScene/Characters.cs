using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class Characters : ScriptableObject
{
    public Sprite characterImage;
    public Sprite platform;
    public Faction faction;
    public Ability ability;
    public bool hasKey = false;
}

public enum Faction { Shingeki_No_Kyojin, OnePiece, Naruto, Marvel,None };
public enum Ability { Backstep, Obstruct, MaxVision, DoubleShot,ActiveTrap };
