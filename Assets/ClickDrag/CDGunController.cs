using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDGunController : MonoBehaviour
{
    public Transform weaponHold;
    public CDGun startingGun;
    CDGun equippedGun;

    private void Start()
    {
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    public void EquipGun(CDGun gunToEquip)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as CDGun;
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