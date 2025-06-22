using UnityEngine;

public class InputHandler
{
    public bool leftclick_pressed {
        get;
        private set;
    }
    public bool middleclick_pressed {
        get; 
        private set;
    }

    public void Reset() {
        leftclick_pressed = false;
        middleclick_pressed = false;
    }

    public bool CheckGameRestart() {
        return Input.GetKey(KeyCode.R);
    }

    public bool HandleLeftClick() {
        if (Input.GetMouseButtonDown(0))
            leftclick_pressed = true;
        
        // soltou o botão:
        if (leftclick_pressed && Input.GetMouseButtonUp(0)) {
            leftclick_pressed = false;
            return true; // clique foi completado
        }

        return false;
    }

    public bool HandleMiddleClick() {
        if (Input.GetMouseButtonDown(2))
            middleclick_pressed = true;

        if (middleclick_pressed && Input.GetMouseButtonUp(2)) {
            middleclick_pressed = false;
            return true; // clique foi completado
        }
        return false;
    }

    public bool HandleRightClick() {
        return Input.GetMouseButtonDown(1);
    }
}
