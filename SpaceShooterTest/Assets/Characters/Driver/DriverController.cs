using UnityEngine;
using System.Collections;

public class DriverController : MonoBehaviour
{
    public float forwardSpeed;
    public float sideSpeed;
    public float rotateSpeed;
    public float forwardMultiplier = 2f;

    private bool moveLeft;
    private bool moveRight;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            moveRight = true;
        else if (Input.GetKeyUp(KeyCode.D))
            moveRight = false;

        if (Input.GetKeyDown(KeyCode.A))
            moveLeft = true;
        else if (Input.GetKeyUp(KeyCode.A))
            moveLeft = false;

        if (moveLeft && !moveRight)
        {
            GetComponent<Rigidbody2D>().AddForce(-transform.right * sideSpeed);
            GetComponent<Rigidbody2D>().AddForce(transform.up * forwardSpeed);
            transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
        }

        if (moveRight && !moveLeft)
        {
            GetComponent<Rigidbody2D>().AddForce(transform.right * sideSpeed);
            GetComponent<Rigidbody2D>().AddForce(transform.up * forwardSpeed);
            transform.Rotate(Vector3.forward * -Time.deltaTime * rotateSpeed);
        }

        if (moveLeft && moveRight)
            GetComponent<Rigidbody2D>().AddForce(transform.up * forwardSpeed * forwardMultiplier);

    }
}
