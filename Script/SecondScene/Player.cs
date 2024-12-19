
using System.Collections.Generic;
public class Player
{
    public List<Characters> pieceList = new List<Characters>();
    public int id;
    public bool OnTurn;
    

    public Player(int id)
    {
        this.id = id;
    }
}

