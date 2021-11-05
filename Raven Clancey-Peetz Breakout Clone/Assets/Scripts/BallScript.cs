using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] float xSpeed = 0;
    [SerializeField] float ySpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        xSpeed = 0;
        ySpeed = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Add the x speed and y speed to the players current position 
        transform.position = new Vector3(transform.position.x + xSpeed, transform.position.y + ySpeed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {

        switch(collision.gameObject.tag)
        {
            case "Wall":
                {
                    xSpeed *= -1;
                    break;
                }

            case "Roof":
                {
                    ySpeed *= -1;
                    break;
                }

            case "Paddle":
                {
                    ySpeed *= -1;
                    break;
                }
            default:
                {

                    print("UNEXPECTED HIT WITH OBJECT " + collision.gameObject.name + " AT POSITION (" + collision.transform.position + ")");
                    break;
                }
        }
    }
}
