using UnityEngine;

// Game.Generation.cs -> Funções que inicializam o jogo
// e tratam geração do campo minado.

public partial class Game : MonoBehaviour {
    private void GenerateCells() {
    for (int i = 0; i < width; ++i) {
        for (int j = 0; j < height; ++j) {
            Cell cell = new Cell();
            cell.pos = new Vector3Int(i, j, 0);
            cell.type = Cell.Type.Empty;
            cell.pressed = false;
            cell.number = 0;
            state[i, j] = cell;
        }
    }
}

    private void GenerateMines(Cell cell) {
        int firstClick_x = cell.pos.x,
            firstClick_y = cell.pos.y;

        for (int k = 0; k < mineCount; ++k) {
            int i = Random.Range(0, width);
            int j = Random.Range(0, height);
            cell = GetCell(i, j);

            while (state[i, j].type == Cell.Type.Mine || IsNearCell(cell, firstClick_x, firstClick_y)) {
                i = Random.Range(0, width);
                j = Random.Range(0, height);
                cell = GetCell(i, j);
            }

            state[i, j].type = Cell.Type.Mine;
        }
}

    // essa função impede que uma mina seja gerada na
    // célula do 1º clique ou ao redor dela.
    private bool IsNearCell(Cell cell, int pos_x, int pos_y) {
        return (pos_x >= cell.pos.x - 1 && pos_x <= cell.pos.x + 1) &&
               (pos_y >= cell.pos.y - 1 && pos_y <= cell.pos.y + 1);
    }

    private void GenerateNumbers() {
        // adjacências horizontais
        for (int x = 0; x < height; ++x) {
            for (int y = 0; y < width; ++y) {
                if (state[x, y].type == Cell.Type.Mine) {
                    if (y > 0 && state[x, y - 1].type != Cell.Type.Mine)
                        state[x, y - 1].number++;
                    if (y < width - 1 && state[x, y + 1].type != Cell.Type.Mine)
                        state[x, y + 1].number++;
                }
            }
        }

        // adjacências verticais
        for (int x = 0; x < height; ++x) {
            for (int y = 0; y < width; ++y) {
                if (state[x, y].type == Cell.Type.Mine) {
                    if (x > 0 && state[x - 1, y].type != Cell.Type.Mine)
                        state[x - 1, y].number++;
                    if (x < height - 1 && state[x + 1, y].type != Cell.Type.Mine)
                        state[x + 1, y].number++;
                }
            }
        }

        // diagonais (esq->dir)
        for (int x = 0; x < height; ++x) {
            for (int y = 0; y < width; ++y) {
                if (state[x, y].type == Cell.Type.Mine) {
                    if (x > 0 && y > 0 && state[x - 1, y - 1].type != Cell.Type.Mine)
                        state[x - 1, y - 1].number++;
                    if (x < height - 1 && y < width - 1 && state[x + 1, y + 1].type != Cell.Type.Mine)
                        state[x + 1, y + 1].number++;
                }
            }
        }

        // diagonais (dir->esq)
        for (int x = 0; x < height; ++x) {
            for (int y = 0; y < width; ++y) {
                if (state[x, y].type == Cell.Type.Mine) {
                    if (x > 0 && y < width - 1 && state[x - 1, y + 1].type != Cell.Type.Mine)
                        state[x - 1, y + 1].number++;
                    if (x < height - 1 && y > 0 && state[x + 1, y - 1].type != Cell.Type.Mine)
                        state[x + 1, y - 1].number++;
                }
            }
        }

        // atualizar tipo de células que são números
        for (int x = 0; x < height; ++x) {
            for (int y = 0; y < width; ++y) {
                if (state[x, y].type != Cell.Type.Mine && state[x, y].number > 0) {
                    state[x, y].type = Cell.Type.Number;
                }
            }
        }
    }
}