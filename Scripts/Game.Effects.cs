using UnityEngine;
using System.Collections.Generic;

public partial class Game : MonoBehaviour
{
    private Cell previousLMB_cell; // �ltima c�lula pressionada
    private Cell previousMMB_cell; // �ltima c�lula pressionada com clique do meio
    private readonly Cell INVALID_CELL = new Cell();

    private List<Cell> unrevealedCellsAround;
    // essa �ltima lista serve pra guardar as c�lulas que est�o sendo exibidas quando
    // se pressiona MMB a fim de recuper�-las de volta quando o clique do meio for solto.

    private void UpdateLMB() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);

        // isso evita torrar o CPU e GPU atualizando a tela freneticamente:
        if (cell.Equals(previousLMB_cell))
            return;

        // caso em que o mouse est� fora do campo de jogo:
        if (cell.type == Cell.Type.Invalid) {
            // caso em que havia uma c�lula sendo pressionada antes disso:
            if (previousLMB_cell.type != Cell.Type.Invalid) {
                // necess�rio apagar o efeito de pressionamento dela:
                Cell updatedCell = state[previousLMB_cell.pos.x, previousLMB_cell.pos.y];
                updatedCell.pressed = false;
                state[previousLMB_cell.pos.x, previousLMB_cell.pos.y] = updatedCell;
                board.tilemap.SetTile(updatedCell.pos, board.GetTile(updatedCell, gameOver));
                previousLMB_cell = new Cell();
            }
            return;
        }

        // LMB est� sendo pressionado
        if (inputhandler.leftclick_pressed) {
            cell.pressed = true;
            state[cell.pos.x, cell.pos.y] = cell;
            if (previousLMB_cell != new Cell() && !previousLMB_cell.Equals(cell)) {
                Cell updatedCell = state[previousLMB_cell.pos.x, previousLMB_cell.pos.y];
                updatedCell.pressed = false;
                state[previousLMB_cell.pos.x, previousLMB_cell.pos.y] = updatedCell;
                board.tilemap.SetTile(updatedCell.pos, board.GetTile(updatedCell, gameOver));
            }
            previousLMB_cell = cell;
            board.tilemap.SetTile(cell.pos, board.GetTile(cell, gameOver));
        }
    }

    private void UpdateMMB() {
        // MMB n�o est� sendo pressionado:
        if (!inputhandler.middleclick_pressed) {
            if (unrevealedCellsAround.Count > 0) {
                // limpando lista ap�s soltar bot�o (MMB):
                foreach (Cell currentCell in unrevealedCellsAround)
                    state[currentCell.pos.x, currentCell.pos.y].pressed = false;

                unrevealedCellsAround.Clear();
                board.Draw(state, gameOver); // atualiza tela
            }
            previousMMB_cell = new Cell();
            return;
        }
        else { // MMB est� sendo pressionado:
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
            Cell cell = GetCell(cellPos.x, cellPos.y);
            bool validCell = true;

            if (cell.type == Cell.Type.Invalid)
                validCell = false;

            if (validCell) {
                if (cell.Equals(previousMMB_cell))
                    return;
            }
            else if (previousMMB_cell == new Cell()) {
                previousMMB_cell = new Cell();
                return;
            }

            if (unrevealedCellsAround.Count > 0) {
                foreach (Cell currentCell in unrevealedCellsAround)
                    state[currentCell.pos.x, currentCell.pos.y].pressed = false;
                unrevealedCellsAround.Clear();
            }

            if (validCell) {
                // Adicionando na estrutura c�lulas pr�ximas:
                for (int x = cellPos.x - 1; x <= cellPos.x + 1; ++x) {
                    for (int y = cellPos.y - 1; y <= cellPos.y + 1; ++y) {
                        if (!(x >= 0 && x < width && y >= 0 && y < height))
                            continue;

                        Cell currentCell = state[x, y];
                        if (!currentCell.revealed)
                            unrevealedCellsAround.Add(currentCell);
                        state[x, y].pressed = true;
                    }
                }
            }
            previousMMB_cell = cell; // atualiza c�lula anterior
            board.Draw(state, gameOver); // atualiza tela
        }
    }
}
