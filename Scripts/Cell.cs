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

    public bool flagged;  // marcar como possível mina (bandeira)
    public bool revealed;
    public bool exploded;
    public bool pressed;  // está sendo pressionada (ainda não revelada)

    public bool Equals(Cell other) {
        return pos.Equals(other.pos) && type == other.type;
    }

    public override bool Equals(object obj) {
        if (obj is Cell other)
            return Equals(other);
        return false;
    }

    // necessário quando sobreescreve Equals():
    public override int GetHashCode() {
        unchecked {
            int hash = 17;
            hash = hash * 31 + pos.GetHashCode();
            hash = hash * 31 + type.GetHashCode();
            return hash;
        }
    }

    public static bool operator ==(Cell left, Cell right) {
        return left.Equals(right);
    }

    public static bool operator !=(Cell left, Cell right) {
        return !(left == right);
    }
}