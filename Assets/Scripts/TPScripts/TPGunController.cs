using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPGunController : MonoBehaviour
{
    public Transform weaponHold;
    public ThirdPersonGun startingGun;
    ThirdPersonGun equippedGun;

    private void Start()
    {
        if (startingGun != null){
            EquipGun(startingGun);
        }
    }

    public void EquipGun(ThirdPersonGun gunToEquip)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as ThirdPersonGun;
        equippedGun.transform.parent = weaponHold;
    }
    public void Shoot()
    {
        if (equippedGun != null)
        {
            equippedGun.Shoot();
        }
    }
}