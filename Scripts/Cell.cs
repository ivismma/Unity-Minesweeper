using UnityEngine;

// estrutura da célula
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

    public bool flagged; // marcar como possível mina (bandeira)
    public bool revealed;
    public bool exploded;
}