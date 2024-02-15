using System.Collections;
using System.Collections.Generic;
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
        if(Powerups == powerups.Red )
        {
            RedPowerUp();
        }
        else if(Powerups == powerups.Blue )
        {
            BluePowerUp();
        }
        else if (Powerups == powerups.Green )
        {
            GreenPowerUp();
        }
   }
    private void RedPowerUp()
    {

    }
    private void BluePowerUp()
    {

    }
    private void GreenPowerUp()
    {

    }
}