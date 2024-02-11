using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryWeapon : MonoBehaviour
{
    public float Focus;
    public float intensity;
    public float Wavelength;
    public playerAttack Attack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Weapon()
    {
        if (Wavelength <= 0)
        {
            Attack.hasSecondWeapon = false;
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float _damage)
    {
        Wavelength -= _damage;
    }
}
