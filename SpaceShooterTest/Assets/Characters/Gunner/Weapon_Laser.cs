using UnityEngine;
using System.Collections;

public class Weapon_Laser : MonoBehaviour
{
    public LayerMask layer;
    public GameObject secondRenderer;

    private float laserDistance;
    private float laserWidth;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 targetPos;

    private LineRenderer laser;

    private void Awake()
    {
        laser   = GetComponent<LineRenderer>();
    }

    public void CreateLaser(float distance, float width, Vector3 start, Vector3 end)       
    {
        laserDistance   = distance;
        laserWidth      = width;       
        startPos        = start;
        endPos          = end;
        AddLineRenderer();
        AddSecondLineRenderer();
        RaycastLaser();
    }

    // Create a line between point of fire and target point
    void AddLineRenderer()
    {
        laser.SetWidth(laserWidth, laserWidth);
        laser.SetPosition(0, startPos);
        targetPos = startPos + endPos;
        laser.SetPosition(1, targetPos);
    }

    void AddSecondLineRenderer()
    {
        LineRenderer laser2 = secondRenderer.GetComponent<LineRenderer>();
        laser2.transform.position = laser.transform.position;
        laser2.SetWidth(0.1f, 0.1f);
        laser2.SetColors(Color.blue, Color.blue);

        float xInc = targetPos.x - startPos.x;
        xInc = xInc / 20;

        float yMin, yMax;
        if(startPos.y > targetPos.y)
        {
            yMin = targetPos.y;
            yMax = startPos.y;
        }
        else
        {
            yMin = startPos.y;
            yMax = targetPos.y;
        }
        for (int i = 0; i < 20; i++)
        {

            Vector3 pos = new Vector3(i * xInc, Mathf.Sin(i + Time.time * Random.Range(yMin, yMax)), 0f);
            //Vector3 direction = targetPos - startPos;
            //pos = pos * direction;
            pos = pos + startPos;
            laser2.SetPosition(i, pos);
        }
    }

    // Raycast to see if enemy is hit
    void RaycastLaser()
    {
        Vector3 direction = targetPos - startPos;
        // Ensure that the layer mask is enemy layer
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(startPos.x, startPos.y),
           new Vector2(direction.x, direction.y), laserDistance, layer);
        if(hit.collider != null)
        {     
            print("hit");
        }
        else
        {
            print("miss");
        }
    }
}
