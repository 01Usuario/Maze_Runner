using System.Collections.Generic;

public class Player
{
    public List<Characters> pieceList = new List<Characters>(); // Lista de piezas del jugador
    public int id; // Identificador del jugador
    public Player(int id)
    {
        this.id = id;
    }
}

