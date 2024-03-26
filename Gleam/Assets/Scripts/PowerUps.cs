using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public enum powerups
    {
        Red , Blue , Green
    }
    public powerups Powerups;
    private bool isCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Inventory>(out var player))
        {
            switch(Powerups)
            {
                case powerups.Red:
                    RedPowerUp(player);
                break;

                case powerups.Blue:
                    BluePowerUp(player);
                break;

                case powerups.Green:
                    GreenPowerUp(player);
                break;
            }

            if(isCollected == true)
                Destroy(gameObject);
        }
   }
    private void RedPowerUp(Inventory player)
    {
        isCollected = true;
    }
    private void BluePowerUp(Inventory player)
    {
      isCollected = true;
    }
    private void GreenPowerUp(Inventory player)
    {
        if(player.collectables.greenCollectable < player.collectables.maxGreenCollect)
        {
            player.collectables.greenCollectable++;
            isCollected = true;
        }
    }

}