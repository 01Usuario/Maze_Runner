using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName ="Trap")]
public class Trap : ScriptableObject
{
    public TrapType trapType;
    public Sprite ImageTrap;
   
}
public enum TrapType {Update,Ice,Hole,Teleport,Lava};
