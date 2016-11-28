using UnityEngine;
using System.Collections;

public class WeaponIcon : MonoBehaviour {

    public void SelectWaterGun() {
        GameManager.SelectWeapon(Weapon.Type.WATER_GUN);
    }

    public void SelectWaterTurret()
    {
        GameManager.SelectWeapon(Weapon.Type.WATER_TURRET);
    }

    public void SelectStickySoap()
    {
        GameManager.SelectWeapon(Weapon.Type.STICKY_SOAP);
    }

    public void SelectBucketCatapult()
    {
        GameManager.SelectWeapon(Weapon.Type.BUCKET_CATAPULT);
    }

}
