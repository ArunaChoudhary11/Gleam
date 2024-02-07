using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{

    float destoryDelay = 0.01f;
    public CoinCollect cm;


    void OnTriggerEnter2D(Collider2D other)

    {

        if (other.gameObject.tag == "Coin")
        {
            cm.CoinCount++;


            Destroy(other.gameObject);

        }



    }
}
