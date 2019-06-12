using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    float forceAmount = 1000f;
    float timeFromLastShoot;

    public void Shoot(float shootFreq)
    {
        if((timeFromLastShoot += Time.deltaTime) >= 1f/shootFreq)
        {
            InstantiateBullet();
            timeFromLastShoot = 0;
        }

    }

    public void Shoot()
    {
            InstantiateBullet();
    }

    private void InstantiateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(forceAmount * transform.forward);
    }
}
