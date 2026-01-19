using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Threading;
using System.Collections.Generic;

// classe da lógica do jogo e atributos
public partial class Game : MonoBehaviour {
    public static Game Instance { get; private set; }   

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
    private InputHandler ih;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            Debug.LogWarning("Another instance of Game found, destroying it...");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
        board = GetComponentInChildren<Board>();
        ih = InputHandler.Instance;
        unrevealedCellsAround = new List<Cell>();

        if (flagCounterUI == null)
            flagCounterUI = FindFirstObjectByType<FlagCounterUI>();
    }

    public void Start() {
        NewGame();
    }

    public void RestartGame() {
        NewGame();
    }

    private void NewGame() {
        ih.Reset();

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
        setSmile(SmileStatus.Normal);
        flagCounterUI.SetTotalMines(mineCount);
        flagCounterUI.SetMarkedFlags(tilesFlagged);

        board.Draw(state, gameOver);
    }

    private void Update() {
        HandleActions();
        HandleEffects();

        if (gameOver)
            return;

        if (PlayerHasWon()) { 
            Debug.Log("Você ganhou!");

            setSmile(SmileStatus.Winner);
            RevealUnflaggedMines();
			gameOver = true;
		}
    }

    private void HandleActions() {
        if (ih.HandleRestartKey())
            NewGame();

        if (gameOver)
            return;

        if (ih.HandleLeftClick())
            Reveal();
        else if (ih.HandleMiddleClick())
            RevealAround();
        else if (ih.HandleRightClick())
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