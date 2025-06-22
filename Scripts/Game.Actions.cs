using UnityEngine;
using System.Collections.Generic;

public partial class Game : MonoBehaviour {

    private void Reveal() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged)
            return;

        // primeiro clique: gera minas e n�meros
        if (tilesRevealed == 0) {
            GenerateMines(cell);
            GenerateNumbers();
        }

        switch (cell.type) {
            case Cell.Type.Mine:
                Explode(cell);
                break;
            case Cell.Type.Empty:
                Spread(cell);
                break;
            default:
                cell.revealed = true;
                tilesRevealed++;
                state[cellPos.x, cellPos.y] = cell;
                break;
        }
        board.Draw(state, gameOver);
    }

    private void RevealAround() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);
        if (cell.revealed == false)
            return;

        List<Cell> toReveal = new List<Cell>(); // c�lulas ao redor a serem reveladas
        int flagsFound = 0;

        for (int x = cellPos.x - 1; x <= cellPos.x + 1; ++x) {
            for (int y = cellPos.y - 1; y <= cellPos.y + 1; ++y) {
                if (!(x >= 0 && x < width && y >= 0 && y < height))
                    continue;

                Cell currentCell = GetCell(x, y);
                if ((x == cellPos.x && y == cellPos.y) || currentCell.revealed)
                    continue;
                if (currentCell.flagged) {
                    flagsFound++;
                    continue;
                }
                toReveal.Add(currentCell);
            }
        }

        if (flagsFound < cell.number) // n�o pode dar Reveal Around!
            return;

        foreach (Cell currentCell in toReveal) {
            if (currentCell.type == Cell.Type.Mine)
                Explode(currentCell);
            else
                Spread(currentCell);
        }

        board.Draw(state, gameOver);
    }

    // espalhar todos os campos vazios pr�ximos recursivamente
    private void Spread(Cell cell) {
        if (state[cell.pos.x, cell.pos.y].revealed || cell.type == Cell.Type.Mine)
            return;
        if (cell.type == Cell.Type.Invalid)
            return;

        tilesRevealed++;
        cell.revealed = true;
        cell.flagged = false; // evita bug ao marcar antes do jogo come�ar mostrando bandeira verde nesses
        state[cell.pos.x, cell.pos.y] = cell;

        if (cell.type == Cell.Type.Empty) {
            // verif. adjac�ncias (lados) de uma por uma:
            Spread(GetCell(cell.pos.x - 1, cell.pos.y));
            Spread(GetCell(cell.pos.x + 1, cell.pos.y));
            Spread(GetCell(cell.pos.x, cell.pos.y - 1));
            Spread(GetCell(cell.pos.x, cell.pos.y + 1));
            Spread(GetCell(cell.pos.x + 1, cell.pos.y + 1));
            Spread(GetCell(cell.pos.x + 1, cell.pos.y - 1));
            Spread(GetCell(cell.pos.x - 1, cell.pos.y + 1));
            Spread(GetCell(cell.pos.x - 1, cell.pos.y - 1));
        }
    }

    private void Flag() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);

        // clicou fora da �rea do jogo:
        if (cell.type == Cell.Type.Invalid || cell.revealed || cell.pressed)
            return;

        if (cell.flagged) {
            cell.flagged = false;
            tilesFlagged--;
        }
        else {
            cell.flagged = true;
            tilesFlagged++;
        }
        flagCounterUI.SetMarkedFlags(tilesFlagged);

        state[cellPos.x, cellPos.y] = cell;
        board.Draw(state, gameOver);
    }

    private void Explode(Cell cell) {
        gameOver = true;

        cell.revealed = true;
        cell.exploded = true;
        state[cell.pos.x, cell.pos.y] = cell;

        // exibir todas os campos n�o revelados do jogo antes de terminar:
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                cell = state[x, y];

                if (cell.type == Cell.Type.Mine && cell.flagged == false) {
                    cell.revealed = true;
                    state[x, y] = cell;
                }
            }
        }
    }
}
