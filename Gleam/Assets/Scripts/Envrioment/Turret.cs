using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject prefab;
    public float spawnTime = 2f;
    public GameObject player;
    private float distance;
    public bool inrange;
    public float bullettime;




    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(player.transform.position , transform.position);
        if(distance<5f)
        {
            inrange = true;
            if(bullettime>= spawnTime)
            {
                SpawnBullet();
                bullettime = 0f;
            }
            bullettime += Time.deltaTime;
            //spawn bullet per sec 

        }
        else
            inrange = false;


    }
    public void SpawnBullet()
    {
        GameObject bullet = Instantiate(prefab, transform);
        bullet.GetComponent<TurretLaser>().PlayerDirection(player.transform);
         

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);

    }

}
//player ke pass aye dedict kre or shoot kre 

