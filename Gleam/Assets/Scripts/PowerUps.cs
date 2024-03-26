using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public enum powerups
    {
        Red , Blue , Green
    }
    public powerups Powerups;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerMove>(out var player))
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

            Destroy(gameObject);
        }
   }
    private void RedPowerUp(PlayerMove player)
    {

    }
    private void BluePowerUp(PlayerMove player)
    {

    }
    private void GreenPowerUp(PlayerMove player)
    {
        if(player.GetComponent<PhotonicHealingCheckpoints>().greenPowerUpsCount < 4)
        {
            player.GetComponent<PhotonicHealingCheckpoints>().greenPowerUpsCount++;
        }
    }
}