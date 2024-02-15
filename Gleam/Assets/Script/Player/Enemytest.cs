using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemytest : MonoBehaviour
{
    public bool isFlashed;

    public IEnumerator flashbang(float Duration)
    {
        isFlashed = true;
        yield return new WaitForSeconds(Duration);
        isFlashed = false;


    }

    void Start()
    {
        
    }

     
    void Update()
    {
        if(isFlashed)
        {
            Debug.Log(name + " enemyFlashed");
        }
    }
}
