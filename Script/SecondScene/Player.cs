
using System.Collections.Generic;
public class Player
{
    public List<Characters> pieceList = new List<Characters>(); // Initialize the list directly
    public int id;
    public bool isTurn;

    public Player(int id)
    {
        this.id = id;
    }
}

