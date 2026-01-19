using UnityEngine;

public class InputHandler {
    private InputHandler() { }
    private static InputHandler instance;

    public static InputHandler Instance {
        get { 
            if (instance == null){
                instance = new InputHandler();
            }
            return instance;
        }
    }

    public bool leftclick_pressed {
        get;
        private set;
    }
    public bool middleclick_pressed {
        get; 
        private set;
    }
    public bool r_pressed // não usado por enquanto.
    {
        get;
        private set;
    }

    public void Reset() {
        leftclick_pressed = false;
        middleclick_pressed = false;
        r_pressed = false;
    }
    
    public bool HandleRestartKey() {
        if (Input.GetKeyDown(KeyCode.R)) {
            r_pressed = true;
            Game.setSmile(SmileStatus.Hover);
        }

        if (r_pressed && Input.GetKeyUp(KeyCode.R))
            return true; // reiniciar jogo.

        return false;
    }

    public bool HandleLeftClick() {
        if (Input.GetMouseButtonDown(0))
        {
            leftclick_pressed = true;
            Game.setSmile(SmileStatus.Worried);
        }
        
        // soltou o botão:
        if (leftclick_pressed && Input.GetMouseButtonUp(0)) {
            leftclick_pressed = false;
            Game.setSmile(SmileStatus.Normal);
            return true; // clique foi completado
        }

        return false;
    }

    public bool HandleMiddleClick() {
        if (Input.GetMouseButtonDown(2)) {
            middleclick_pressed = true;
            Game.setSmile(SmileStatus.Worried);
        }

        if (middleclick_pressed && Input.GetMouseButtonUp(2)) {
            middleclick_pressed = false;
            Game.setSmile(SmileStatus.Normal);
            return true; // clique foi completado
        }
        return false;
    }

    public bool HandleRightClick() {
        return Input.GetMouseButtonDown(1);
    }
}