using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private FieldOfView View;
    void Start()
    {
        View  = GetComponent<FieldOfView>();
    }
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            FlashBangs();
        }
    }
    private void FlashBangs()
    {
        View.FindVisibleTargets();
        for (int i = 0; i < View.visibleTargets.Count; i++)
        {
            Enemytest enemy = View.visibleTargets[i].GetComponent<Enemytest>();
            StartCoroutine (enemy.flashbang(2));
        }
    }    
}