using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
     GameObject topWall;

     [SerializeField]
     GameObject bottomWall;

     [SerializeField]
     GameObject leftWall;

     [SerializeField]  
      GameObject rightWall;

     public enum Directions{
        Top,
        Bottom,
        Left,
        Right
     }

     Dictionary<Directions,GameObject> walls = new Dictionary<Directions,GameObject>();

     public Vector2Int Index{
        get;
        set;

     }
     public bool visited{get;set;}= false;
     Dictionary<Directions,bool> dirFlags = new Dictionary<Directions,bool>();
    void Start()
    {
        walls[Directions.Top]=topWall;
        walls[Directions.Bottom]=bottomWall;
        walls[Directions.Left]=leftWall;
        walls[Directions.Right]=rightWall;
    }

    private void SetActive(Directions dir, bool flags){
        walls[dir].SetActive(flags);
    }
    private void SetDirFlag(Directions dir, bool flag){
        dirFlags[dir]=flag;
        SetActive(dir,flag);
    }
    
}
