using UnityEngine;

public class Refract : MonoBehaviour
{
    public void DeviationEffect(Enemytest enemy)
    {
        if(enemy.isRefracted == false)
        {
            enemy.isRefracted = true;
            enemy.Invoke(nameof(enemy.DeviationEffectReset), 5);
        }
    }
}