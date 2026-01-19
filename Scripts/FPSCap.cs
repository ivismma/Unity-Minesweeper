using UnityEngine;

public class FPSCap : MonoBehaviour
{
    [SerializeField] private int frameRate;

    void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
    }
}
