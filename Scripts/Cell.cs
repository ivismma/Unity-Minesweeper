using UnityEngine;

// estrutura da c�lula
public struct Cell {
    public enum Type {
        Invalid,
        Number,
        Mine,
        Empty,
    }

    public int number;

    public Vector3Int pos;
    public Type type;

    public bool flagged; // marcar como poss�vel mina (bandeira)
    public bool revealed;
    public bool exploded;
}