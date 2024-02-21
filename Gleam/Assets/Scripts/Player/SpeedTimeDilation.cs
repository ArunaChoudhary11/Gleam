using UnityEngine;

public class SpeedTimeDilation : MonoBehaviour
{
    public static SpeedTimeDilation Instance;
    public bool TimeDilation;
    public float globalSpeedDeltaTime;
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }
    void Update()
    {
        if(TimeDilation == true)
        {
            globalSpeedDeltaTime = .1f;
        }
        else
        {
            globalSpeedDeltaTime = 1f;
        }
    }
}