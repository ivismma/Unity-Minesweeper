using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;
using System.Collections.Generic;

// classe da lógica do jogo e atributos
public partial class Game : MonoBehaviour {
    // flags e count's importantes:
    public int width;     // linhas       default: 16
    public int height;    // colunas      default: 16
    public int mineCount; // qtd. minas   default: 60
    private int tilesRevealed;
    private int tilesFlagged;
    private bool gameOver;

    // estruturas de dados essenciais:
    private Board board;            // grid (visual) do jogo
    private Cell[,] state;          // matriz de células

    // UI:
    private FlagCounterUI flagCounterUI;
    private InputHandler inputhandler;

    private void Awake() {
        board = GetComponentInChildren<Board>();
        inputhandler = new InputHandler();
        unrevealedCellsAround = new List<Cell>();

        if (flagCounterUI == null)
            flagCounterUI = FindObjectOfType<FlagCounterUI>();
    }

    private void Start() {
        NewGame();
    }

    private void NewGame() {
        // flags/counts:
        gameOver = false;
        tilesRevealed = 0;
        tilesFlagged = 0;
        previousLMB_cell = INVALID_CELL;
        previousMMB_cell = INVALID_CELL;

        // inicialização do grid:
        state = new Cell[width, height];
        GenerateCells();

        // visual:
        flagCounterUI.SetTotalMines(mineCount);
        flagCounterUI.SetMarkedFlags(tilesFlagged);

        board.Draw(state, gameOver);
    }

    private void Update() {
        HandleActions();
        HandleEffects();
        
        if (PlayerHasWon()) { Debug.Log("Você ganhou!"); gameOver = true; }
    }

    private void HandleActions() {
        if (inputhandler.CheckGameRestart()) {
            Start();
            inputhandler.Reset();
        }

        if (gameOver)
            return;
        
        if (inputhandler.HandleLeftClick())
            Reveal();
        else if (inputhandler.HandleMiddleClick())
            RevealAround();
        else if (inputhandler.HandleRightClick())
            Flag();
    }

    private void HandleEffects() {
        if (gameOver)
            return;
        
        UpdateLMB();
        UpdateMMB();
    }

    private Cell GetCell(int x, int y) {
        return isValidCoordinates(x, y) ? state[x, y] : new Cell();
    }

    private bool isValidCoordinates(int x, int y) {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    private bool PlayerHasWon() {
        return (tilesRevealed == width * height - mineCount);
    }
}