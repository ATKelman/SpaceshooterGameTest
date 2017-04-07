using UnityEngine;
using System.Collections;

public class GunnerWeaponSystem : MonoBehaviour
{
    public Transform start;

    public float laserDistance = 10f;
    public float laserTime = 1.0f;
    public float laserWidth = 1.0f;

    private LineRenderer laser;

    private float timeSinceFire = 0f;
    private bool firing;

    public Vector3 startPos;
    public Vector3 targetPos;

    void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.SetWidth(laserWidth, laserWidth);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !firing)
            Shoot();

        //if(firing)
        //{
        //    timeSinceFire += Time.deltaTime;
        //    if(timeSinceFire >= laserTime)
        //    {
        //        firing = false;
        //        laser.SetPosition(1, transform.position);
        //    }
        //}
    }

    void Shoot()
    {
        firing = true;
        timeSinceFire = 0;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.up * laserDistance);
        targetPos = transform.up * laserDistance;
        AddCollider();
    }

    void AddCollider()
    {
        BoxCollider col = new GameObject("Collider").AddComponent<BoxCollider>();
        col.transform.parent = laser.transform;
        col.size = new Vector3(laserDistance, laserWidth, 1f);
        Vector3 midPoint = (start.position + targetPos) / 2;
        col.transform.position = midPoint;

        float angle = (Mathf.Abs(startPos.y - targetPos.y) / Mathf.Abs(startPos.x - targetPos.x));
        if((startPos.y < targetPos.y && startPos.x > targetPos.x) || (targetPos.y < startPos.y && targetPos.x > startPos.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);
    }
}
