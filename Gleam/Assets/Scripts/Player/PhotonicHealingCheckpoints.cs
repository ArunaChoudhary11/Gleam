using UnityEngine;

public class PhotonicHealingCheckpoints : MonoBehaviour
{
    public Inventory inventory;
    public bool CanCreateGreenZone;
    public ParticleSystem chgfh;

     public float holdTimer;
    void Update()
    {
        CanCreateGreenZone = inventory.collectables.greenCollectable == inventory.collectables.maxGreenCollect;

        if(CanCreateGreenZone)
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

        if(holdTimer >= 0.5f)
        {
            SetCheckpoint();
            holdTimer = 0;
        }
    }
    private void SetCheckpoint()
    {
        inventory.collectables.greenCollectable = 0;
        Debug.Log("CheckPoint Set");

    }
}