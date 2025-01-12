using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class Characters : ScriptableObject
{
    public Sprite characterImage; // Imagen del personaje
    public Faction faction; // Facción del personaje
    public Ability ability; // Habilidad del personaje
    public string abilityDescription; // Descripción de la habilidad
    public int OriginalRange; // Rango original de la habilidad
    public int Range; // Rango actual de la habilidad
    public int AbilityTime=0; // Tiempo de la habilidad
    public int OriginalAbilityTime; // Tiempo original de la habilidad
    
    public int positionI; // Posición I del personaje
    public int positionJ; // Posición J del personaje

    public bool UnderIceEffect = false; // Efecto de hielo
     public bool Inmunity = false;

     
     public void ResetValue(){
        Range = OriginalRange;
        AbilityTime = 0;
        UnderIceEffect = false;
        Inmunity = false;
        positionI = 0;
        positionJ=0;
     }
}

public enum Faction { Shingeki_No_Kyojin, OnePiece, Naruto, Marvel }; // Facciones
public enum Ability { Obstruct, ResetStatus,Teleport, ExtraMove, Inmunity}; // Habilidades

