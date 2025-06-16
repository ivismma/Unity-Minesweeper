using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;
using System.Collections.Generic;


// classe da lógica do jogo e atributos
public class Game : MonoBehaviour {
    public int width;     // linhas       default: 16
    public int height;    // colunas      default: 16
    public int mineCount; // qtd. minas   default: 42
    
    private int tilesRevealed;
    private int tilesFlagged;

    private Board board;
    private Cell[,] state;
    private bool gameOver = false;

    private FlagCounterUI flagCounterUI;

    private void Awake() {
        board = GetComponentInChildren<Board>();
        if (flagCounterUI == null)
            flagCounterUI = FindObjectOfType<FlagCounterUI>();
    }

    private void NewGame() {
        // flags/counts:
        gameOver = false;
        tilesRevealed = 0;
        tilesFlagged = 0;

        // inicialização do grid:
        state = new Cell[width, height];
        GenerateCells();

        // visual:
        flagCounterUI.SetTotalMines(mineCount);
        flagCounterUI.SetMarkedFlags(tilesFlagged);
        
        board.Draw(state, gameOver);
    }

    private void Start() {
        NewGame();
    }

    private void GenerateCells() {
        for (int i = 0; i < width; ++i) {
            for (int j = 0; j < height; ++j) {
                Cell cell = new Cell();
                cell.pos = new Vector3Int(i, j, 0);
                cell.type = Cell.Type.Empty;
                cell.number = 0;
                state[i, j] = cell;
            }
        }
    }

    // essa função impede que uma mina seja gerada na
    // célula do 1º clique ou ao redor dela.
    private bool IsNearCell(Cell cell, int pos_x, int pos_y) {
        return (pos_x >= cell.pos.x - 1 && pos_x <= cell.pos.x + 1) &&
               (pos_y >= cell.pos.y - 1 && pos_y <= cell.pos.y + 1);
    }

    private void GenerateMines(Cell cell) {
        int firstClick_x = cell.pos.x,
            firstClick_y = cell.pos.y;

        for(int k = 0; k < mineCount; ++k) {
            int i = Random.Range(0, width);
            int j = Random.Range(0, height);
            cell = GetCell(i, j);

            while (state[i,j].type == Cell.Type.Mine || IsNearCell(cell, firstClick_x, firstClick_y)) {
                i = Random.Range(0, width);
                j = Random.Range(0, height);
                cell = GetCell(i, j);
            }

            state[i, j].type = Cell.Type.Mine;
        }
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

    private Cell GetCell(int x, int y) {
        return isValidCoordinates(x, y) ? state[x, y] : new Cell();
    }

    private bool isValidCoordinates(int x, int y) {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    private void Update() {
        if (Input.GetKey(KeyCode.R))
            Start();

        if (gameOver)
            return;

        if (Input.GetMouseButtonDown(1)) // RMB
            Flag();
        else {
            if (Input.GetMouseButtonDown(0)) // LMB
                Reveal();
            else if (Input.GetMouseButtonDown(2)) // MMB
                RevealAround();
            
            if (PlayerHasWon()) {
                Debug.Log("Você ganhou!");
                gameOver = true;
            }
        }
    }

    private void Flag() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);

        // clicou fora da área do jogo:
        if (cell.type == Cell.Type.Invalid || cell.revealed)
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

    private void RevealAround() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);
        if (cell.revealed == false)
            return;
        
        List<Cell> toReveal = new List<Cell>(); // células ao redor a serem reveladas
        int flagsFound = 0;

        for(int x = cellPos.x-1; x <= cellPos.x+1; ++x) {
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

        if(flagsFound < cell.number) // não pode dar Reveal Around!
            return;

        foreach(Cell currentCell in toReveal) {
            if (currentCell.type == Cell.Type.Mine)
                Explode(currentCell);
            else
                Spread(currentCell);
        }
        
        board.Draw(state, gameOver);
    }

    private void Reveal() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged)
            return;

        // primeiro clique: gera minas e números
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

    // espalhar todos os campos vazios próximos recursivamente
    private void Spread(Cell cell) {
        if (state[cell.pos.x, cell.pos.y].revealed || cell.type == Cell.Type.Mine)
            return;
        if (cell.type == Cell.Type.Invalid)
            return;

        tilesRevealed++;
        cell.revealed = true;
        state[cell.pos.x, cell.pos.y] = cell;

        if (cell.type == Cell.Type.Empty) {
            // verif. adjacências (lados) de uma por uma:
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

    private void Explode(Cell cell) {
        gameOver = true;

        cell.revealed = true;
        cell.exploded = true;
        state[cell.pos.x, cell.pos.y] = cell;

        // exibir todas os campos não revelados do jogo antes de terminar:
        for(int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                cell = state[x, y];

                if (cell.type == Cell.Type.Mine && cell.flagged == false) {
                    cell.revealed = true;
                    state[x, y] = cell;
                }
            }
        }
    }

    private bool PlayerHasWon() {
        return (tilesRevealed == width * height - mineCount);
    }
}