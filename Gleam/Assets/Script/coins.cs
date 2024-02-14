using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    float destoryDelay = 0.01f;
    public CoinCollection cm;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            cm.CoinCount++;
            Destroy(other.gameObject);
        }
    }
}