using UnityEngine;
using UnityEngine.Tilemaps;


// classe c/ métodos de desenhar, gerenciar e mapear células do jogo
public class Board : MonoBehaviour {
    public Tilemap tilemap {
        get;
        private set;
    }

    public Tile
        tileUnknown,
        tileEmpty,
        tileMine,
        tileExploded,
        tileFlag,
        tileWrongFlag,
        tileNum1,
        tileNum2,
        tileNum3,
        tileNum4,
        tileNum5,
        tileNum6,
        tileNum7,
        tileNum8;

    private void Awake() {
        tilemap = GetComponent<Tilemap>();
    }

    private Tile GetNumberTile(Cell cell) {
        switch (cell.number) {
            case 1: return tileNum1;
            case 2: return tileNum2;
            case 3: return tileNum3;
            case 4: return tileNum4;
            case 5: return tileNum5;
            case 6: return tileNum6;
            case 7: return tileNum7;
            case 8: return tileNum8;
            default:
                return null; // <-- não deveria acontecer em nenhuma hipótese.
        }
    }

    public void Draw(Cell[,] state, bool gameOver) {
        int width = state.GetLength(0);  // linhas
        int height = state.GetLength(1); // cols.

        for (int i = 0; i < width; ++i) {
            for (int j = 0; j < height; ++j) {
                Cell cell = state[i, j];
                tilemap.SetTile(cell.pos, GetTile(cell, gameOver));
            }
        }
    }

    private Tile GetRevealedTile(Cell cell) {
        switch (cell.type) {
            case Cell.Type.Empty:
                return tileEmpty;
            case Cell.Type.Mine:
                return (cell.exploded) ? tileExploded: tileMine;
            case Cell.Type.Number:
                return GetNumberTile(cell);
            default:
                return null; // <-- não deveria acontecer.
        }
    }

    private Tile GetTile(Cell cell, bool gameOver) {
        // ao explodir, se tiver flags em lugares errados, exibe-as como flags verdes:
        if (gameOver && cell.type != Cell.Type.Mine && cell.flagged == true)
            return tileWrongFlag;
        
        if (cell.revealed)
            return GetRevealedTile(cell);
        else if (cell.flagged)
            return tileFlag;
        else
            return tileUnknown;
    }
    
}