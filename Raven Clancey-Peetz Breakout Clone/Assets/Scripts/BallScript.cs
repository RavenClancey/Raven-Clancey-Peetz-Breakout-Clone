using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    // Private Variables

    [SerializeField] private float maxSpeed = 20;                       // the max speed the ball can go
    [SerializeField] private float startSpeed = 5;                      // the speed the ball starts at when shot 
    [SerializeField][Range(1, 2)] private float speedModifier = 1.05f;  // modifer used on ball speed on collisions


    private float xSpeed = 0;           // current x speed of the ball
    private float ySpeed = 0;           // current y speed of the ball
    private bool ballActive = false;    //  bool for if the game is active
    private GameObject paddleObject;    // player object
    private GameObject GameManager;     //  game manager object 
    private Rigidbody rb;               // rigid body attached to ball

    // Start is called before the first frame update
    void Start()
    {

        // fill varibles if null 
     
        if(paddleObject == null)
        {
            paddleObject = GameObject.Find("Player");
        }
        if (GameManager == null)
        {
            GameManager = GameObject.Find("GameManager");
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
            // keep ball above player paddle
            rb.MovePosition(new Vector3(paddleObject.transform.position.x, paddleObject.transform.position.y + 1.0f, 0));

            //if space is hit, shoot ball with speed of start speed between -45 and 45 degrees and activate ballActive bool
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
        //if Yspeed is less than max speed increase it on every collision
        if (Mathf.Abs(ySpeed) < maxSpeed )
        {
           
            xSpeed *= speedModifier;
            ySpeed *= speedModifier;
        }

        
        switch(collision.gameObject.tag)
        {
            case "Wall":
                {
                    // flip xspeed if wall is hit
                    xSpeed *= -1;
                    break;
                }

            case "Roof":
                {
                    // flip yspeed if roof it hit
                    ySpeed *= -1;
                    break;
                }

            case "DeadZone":
                {
                    //sets ball back to player paddle
                    ballActive = false;
                    break;
                }

            case "Brick":
                {
                    // disable brick that is hit 
                    collision.gameObject.SetActive(false);

                    // Add score 
                    GameManager.GetComponent<GameManagerScript>().AddScore(100);

                    //different check needed for top and bottom otherwise if you hit 2 bricks at once it * -1 twice and it doesnt flip             
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
                    //if collide with something unexpected print to console
                    print("UNEXPECTED HIT WITH OBJECT " + collision.gameObject.name + " AT POSITION (" + collision.transform.position + ")");
                    break;
                }
        }
    }
}
