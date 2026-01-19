using UnityEngine;
using UnityEngine.UI;

public enum SmileStatus {
    Invalid,
    Worried,
    Normal,
    Dead,
    Winner,
    Hover
}

public partial class Game : MonoBehaviour {
    
    // Smile Face:
    [Header("SmileFace")]
    public Image smileImage;
    public Sprite smileNormal;   // neutra
    public Sprite smileHover;    // hover
    public Sprite smileDead;     // explodiu
    public Sprite smileWorried;  // pressionando
    public Sprite smileWinner;   // venceu o jogo
    public SmileStatus smileStatus { get; private set; }

    public static void setSmile(SmileStatus status) {
        Debug.Log("Setting smileface...");
        
        switch (status) {
            case SmileStatus.Worried:
                Instance.smileImage.sprite = Instance.smileWorried;
                Instance.smileStatus = SmileStatus.Worried;
                break;
            case SmileStatus.Normal:
                Instance.smileImage.sprite = Instance.smileNormal;
                Instance.smileStatus = SmileStatus.Normal;
                break;
            case SmileStatus.Dead:
                Instance.smileImage.sprite = Instance.smileDead;
                Instance.smileStatus = SmileStatus.Dead;
                break;
            case SmileStatus.Winner:
                Instance.smileImage.sprite = Instance.smileWinner;
                Instance.smileStatus = SmileStatus.Winner;
                break;
            case SmileStatus.Hover:
                Instance.smileImage.sprite = Instance.smileHover;
                Instance.smileStatus = SmileStatus.Hover;
                break;
        }
    }
}
