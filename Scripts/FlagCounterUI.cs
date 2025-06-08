using UnityEngine;
using TMPro;

public class FlagCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    
    private int totalMines;
    private int markedFlags;

    public void SetTotalMines(int value) {
        totalMines = value;
        UpdateText(value);
    }

    public void SetMarkedFlags(int value) {
        markedFlags = value;
        UpdateText(value);
    }

    private void UpdateText(int value) {
        if(text != null)
            text.text = $"Minas: {markedFlags}/{totalMines}";
    }
}
