using UnityEngine;
using System.Collections;

public class GunnerWeaponSystem : MonoBehaviour
{
    public GameObject laserPrefab;
    public GameObject ship;

    public float laserDistance = 10f;
    public float laserTime = 1.0f;
    public float laserWidth = 1.0f;

    private float timeSinceFire = 0f;
    private bool firing;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !firing)
            ShootLaser();

        if (firing)
        {
            timeSinceFire += Time.deltaTime;
            if (timeSinceFire >= laserTime)
            {
                firing = false;
                //laser.SetPosition(1, transform.position);
                laserPrefab.SetActive(false);
            }
        }
    }

    void ShootLaser()
    {
        firing = true;
        timeSinceFire = 0;
        laserPrefab.GetComponent<Weapon_Laser>().CreateLaser(laserDistance, laserWidth, ship.transform.position, (ship.transform.up * laserDistance));
        laserPrefab.SetActive(true);
    }
}
