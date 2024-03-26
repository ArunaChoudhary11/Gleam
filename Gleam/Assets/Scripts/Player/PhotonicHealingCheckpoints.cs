using UnityEngine;

public class PhotonicHealingCheckpoints : MonoBehaviour
{
    public int greenPowerUpsCount;
    public float holdTimer;
    void Update()
    {
        if(greenPowerUpsCount >= 4)
        {
            if(Input.GetKey(KeyCode.E))
            {
                holdTimer += Time.deltaTime;
            }
        }

        if(Input.GetKeyUp(KeyCode.E))
        {
            holdTimer = 0;
        }

        if(holdTimer >= 1.5f)
        {
            SetCheckpoint();
            holdTimer = 0;
            greenPowerUpsCount -= 4;
        }
    }
    private void SetCheckpoint()
    {
        Debug.Log("CheckPoint Set");
    }
}