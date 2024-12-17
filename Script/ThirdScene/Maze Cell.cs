using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName ="Trap")]
public class MazeCell :ScriptableObject
{
   public bool isLava;
    public bool isHole;
    public bool isTeleport;
    public bool isIce;
    public Sprite Image;
   

}

