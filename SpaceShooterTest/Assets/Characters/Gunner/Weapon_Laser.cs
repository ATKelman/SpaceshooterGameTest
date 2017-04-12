using UnityEngine;
using System.Collections;

public class Weapon_Laser : MonoBehaviour
{
    public LayerMask layer;
    public GameObject secondRenderer;

    public Vector2 MinMaxSecondLaser;

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

    public void AddSecondLineRenderer()
    {
        LineRenderer laser2 = secondRenderer.GetComponent<LineRenderer>();
        laser2.transform.position = laser.transform.position;
        laser2.SetWidth(0.1f, 0.1f);
        laser2.SetColors(Color.blue, Color.blue);

        bool useX = true;
        if (Mathf.Abs(targetPos.y - startPos.y) > Mathf.Abs(targetPos.x - startPos.x))
            useX = false;
        else if (Mathf.Abs(targetPos.y - startPos.y) < Mathf.Abs(targetPos.x - startPos.x))
            useX = true;

        float xDistance, yDistance;
        xDistance = targetPos.x - startPos.x;
        yDistance = targetPos.y - startPos.y;

        float xInc = xDistance / 20;
        float yInc = yDistance / 20;

        for (int i = 0; i < 20; i++)
        {
            Vector3 pos;
            if(useX)
                pos = new Vector3(i * xInc, Mathf.Sin(i + Time.time * Random.Range(MinMaxSecondLaser.x, MinMaxSecondLaser.y)) + (i * yInc), 0f);
            else
                pos = new Vector3(Mathf.Sin(i + Time.time * Random.Range(MinMaxSecondLaser.x, MinMaxSecondLaser.y)) + (i * xInc) , i * yInc, 0f);
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
        //if(hit.collider != null)
        //{     
        //    print("hit");
        //}
        //else
        //{
        //    print("miss");
        //}
    }
}
