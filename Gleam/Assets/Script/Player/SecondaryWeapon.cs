using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryWeapon : MonoBehaviour
{
    public float Focus;
    public float Intensity;
    public float Wavelength;
    public PlayerAttack Attack;
    void Start()
    {

    }
    void Update()
    {

    }
    public void Weapon()
    {
        if (Wavelength <= 0)
        {
            //Attack.hasSecondWeapon = false;
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float _damage)
    {
        Wavelength -= _damage;
    }
}