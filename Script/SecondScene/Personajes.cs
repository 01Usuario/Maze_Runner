
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]

public class Personajes : ScriptableObject
{
    public Sprite CharacterImage;
    public Sprite plataform;
    public Faction faction;
    public Habilidad habilidad;
    bool hasKey = false;
    

}
 public enum Faction  {Shingeki_No_Kyojin,OnePiece, Naruto, Marvel };
 public enum Habilidad{Retroceso,Obstacilizar,MaxVision,TiroDoble};