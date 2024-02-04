using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{

    public int CoinCount;
    public TextMeshProUGUI cointext;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cointext.text = "Coin:" + CoinCount.ToString();
    }
}
