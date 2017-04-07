using UnityEngine;
using System.Collections;

public class GunnerWeaponSystem : MonoBehaviour
{

    public float laserDistance = 10f;
    public float laserTime = 1.0f;

    private LineRenderer laser;

    private float timeSinceFire = 0f;
    private bool firing;

    void Start()
    {
        laser = GetComponent<LineRenderer>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !firing)
            Shoot();

        if(firing)
        {
            timeSinceFire += Time.deltaTime;
            if(timeSinceFire >= laserTime)
            {
                firing = false;
                laser.SetPosition(1, transform.position);
            }
        }
    }

    void Shoot()
    {
        firing = true;
        timeSinceFire = 0;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.up * laserDistance);
    }
}
