using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] float maxSpeed = 20;
    [SerializeField] float startSpeed = 5;
    [SerializeField] float xSpeed = 0;
    [SerializeField] float ySpeed = 0;
    [SerializeField][Range(1, 2)] float speedModifier = 1.05f;

    private bool ballActive = false;
    private GameObject paddleObject;

    [SerializeField] Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
     
        if(paddleObject == null)
        {
            paddleObject = GameObject.Find("Player");
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (ballActive)
        {
            // Add the x speed and y speed to the players current position 
            rb.MovePosition(new Vector3(transform.position.x + xSpeed * Time.deltaTime, transform.position.y + ySpeed * Time.deltaTime, 0));
        }
        else
        {
            rb.MovePosition(new Vector3(paddleObject.transform.position.x, paddleObject.transform.position.y + 1.0f, 0));

            if(Input.GetKeyDown(KeyCode.Space))
            {
                ySpeed = startSpeed;
                xSpeed = Random.Range(-startSpeed, startSpeed);
             
                ballActive = true;
            }
        }
      
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (Mathf.Abs(ySpeed) < maxSpeed )
        {
           
            xSpeed *= speedModifier;
            ySpeed *= speedModifier;
        }

        
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

            case "DeadZone":
                {
                    ballActive = false;
                    break;
                }

            case "Brick":
                {
                    // disable brick that is hit 
                    collision.gameObject.SetActive(false);

                    //different check needed for top and bottom otherwise if you hit 2 bricks at once it * -1 twice and it doesnt flip

                    print(collision.contacts[0].normal);
                    // Bounce off bottom of brick
                    if(collision.contacts[0].normal.y < 0 )
                    {
                        ySpeed = -Mathf.Abs(ySpeed);
                    }

                    // Bounce off top of brick
                    if (collision.contacts[0].normal.y > 0)
                    {
                        ySpeed = Mathf.Abs(ySpeed);
                    }

                    // Bounce off side of brick
                    if (collision.contacts[0].normal.x != -0)
                    {
                        xSpeed *= -1;
                    }

                    break;
                }

            case "Paddle":
                {
                    //flip Y speed & make X Speed equal the difference of ball pos and paddle pos.
                    ySpeed *= -1;
                    
                    // This makes the ball bounce at a shaper angle at the edges of the paddle
                    xSpeed = transform.position.x - collision.gameObject.transform.position.x;
                    xSpeed *= Mathf.Abs((ySpeed / 3.0f) );
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
