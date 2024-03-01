using UnityEngine;

public class AttackReflect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2.5f);
    }
    void Update()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * 5);
    }
}

/*
    Tutorial fight
    Lose Fight
    Dead - not taking power because weak
    cutscene bwn council
    recieved powers
    cave section - blue clan
*/