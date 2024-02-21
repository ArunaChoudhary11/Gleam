using UnityEngine;

public class NightVision : MonoBehaviour
{
    public bool isUsingNightVision;
    void Update()
    {
        GetNightVision();
    }
    private void GetNightVision()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            if(isUsingNightVision)
            {
                Debug.Log("Night Vision Enabled");
                return;
            }

            isUsingNightVision = false;
            Debug.Log("Night Vision Disabled");
        }
    }
}