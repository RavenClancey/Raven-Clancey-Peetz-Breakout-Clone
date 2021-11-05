using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkBallScript : NetworkBehaviour
{

    // Private Variables

    [SerializeField] private float maxSpeed = 20;                       // the max speed the ball can go
    [SerializeField] private float startSpeed = 5;                      // the speed the ball starts at when shot 
    [SerializeField] [Range(1, 2)] private float speedModifier = 1.05f;  // modifer used on ball speed on collisions
    [SerializeField] private GameObject trailObject;                    //  trail particle object 
    [SerializeField] private GameObject sparkParticle;                  //  Spark Particle Prefab
    [SerializeField] private bool isNetworked = true;                   // bool to control check network or not
    [SerializeField] private int ballID = 0;                   // bool to control check network or not

    private float xSpeed = 0;           // current x speed of the ball
    private float ySpeed = 0;           // current y speed of the ball
  [SerializeField]  private bool ballActive = false;    //  bool for if the game is active
    private GameObject paddleObject;    // player object
    private GameObject gameManager;     //  game manager object 
    private GameObject networkManager;  //  network manager object 
    private GameObject audioManager;    //  audio manager object 

    private Rigidbody rb;               // rigid body attached to ball

    // Start is called before the first frame update
    void Start()
    {
        
        InitVars();
    }

    private void Awake()
    {
        InitVars();


        // name the ball synced over network
      if(gameObject.name != "Ball1")
        {
            if(GameObject.Find("Ball1") == null)
            {
                gameObject.name = "Ball1";
            }
            else
            {
                gameObject.name = "Ball2";
            }
           
        }

      
    }
    
    //function that loads variables this object needs;
    public void InitVars()
    {
        // fill varibles if null 

        if (paddleObject == null)
        {
            paddleObject = GameObject.FindGameObjectWithTag("Paddle");
        }
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager");
        }
        if (networkManager == null)
        {
            networkManager = GameObject.Find("NetworkManager");
        }
        if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager");
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

            // activate trail particle if ball is at max speed
            if (Mathf.Abs(ySpeed) >= maxSpeed)
            {
                trailObject.SetActive(true);
            }
            else
            {
                trailObject.SetActive(false);
            }

        }
        else
        {

            if (gameObject.name == "Ball1")
            {
                paddleObject = GameObject.Find("Player1");
            }
            else
            {
                paddleObject = GameObject.Find("Player2");
            }

         

            // keep ball above player paddle
            rb.MovePosition(new Vector3(paddleObject.transform.position.x, paddleObject.transform.position.y + 1.0f, 0));

            //if space is hit, shoot ball with speed of start speed between -45 and 45 degrees and activate ballActive bool
            
            if (Input.GetKeyDown(KeyCode.Space) && paddleObject.GetComponent<NetworkPlayerScript>().CheckIfLocal())
            {
                
                ySpeed = startSpeed;
                xSpeed = Random.Range(-startSpeed, startSpeed);

                paddleObject.GetComponent<ServerScript>().ServerBallActive(gameObject, true, ySpeed, xSpeed);
            }
            
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        //if Yspeed is less than max speed increase it on every collision
        if (Mathf.Abs(ySpeed) < maxSpeed)
        {

            xSpeed *= speedModifier;
            ySpeed *= speedModifier;
        }


        switch (collision.gameObject.tag)
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
                    // sets ball back to player paddle with server message
                    
                    paddleObject.GetComponent<ServerScript>().ServerBallActive(gameObject, false, 0, 0);
                    break;
                }

            case "Brick":
                {

                    Destroy(Instantiate(sparkParticle, collision.contacts[0].point, Quaternion.Euler(0, 180, 0)), 0.1f);
                   

                    //different check needed for top and bottom otherwise if you hit 2 bricks at once it * -1 twice and it doesnt flip             
                    // Bounce off bottom of brick
                    if (collision.contacts[0].normal.y < 0)
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

                    // play audio for breaking brick with saftey check

                    if (audioManager.transform.GetChild(0).GetComponent<AudioSource>() != null)
                    {
                        print("playing pop" + audioManager.transform.GetChild(0).GetComponent<AudioSource>().name);
                        audioManager.transform.GetChild(0).GetComponent<AudioSource>().Play();
                    }

                    // saftey check for if network ball or singleplayer
                    if (isNetworked)
                    {
                        NetworkDestroy(collision.gameObject);
                    }

                  

                    break;
                }

            case "Paddle":
                {
                    //flip Y speed & make X Speed equal the difference of ball pos and paddle pos.
                    ySpeed *= -1;

                    // This makes the ball bounce at a shaper angle at the edges of the paddle
                    xSpeed = transform.position.x - collision.gameObject.transform.position.x;
                    xSpeed *= Mathf.Abs((ySpeed / 3.0f));

                    // play audio for hitting paddle with saftey check
                    if (collision.gameObject.GetComponent<AudioSource>() != null)
                    {
                        collision.gameObject.GetComponent<AudioSource>().Play();
                    }

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

    void NetworkDestroy(GameObject Object)
    {
        //Get the NetworkIdentity assigned to the object
        NetworkIdentity id = Object.GetComponent<NetworkIdentity>();
        // Check if we successfully got the NetworkIdentity Component from our object, if not we return(essentially do nothing).
        if (id == null) return;
        // First check if the objects NetworkIdentity can be transferred, or if it is server only.

        // Do we already own this NetworkIdentity? If so, don't do anything.
        if (id.hasAuthority == false)
        {

            // If takeover was successful, we can now destroy our GameObject.
            // Add score 
           
            GameObject.Find("Player1").GetComponent<ServerScript>().AddServerScore(100);
            NetworkBehaviour.Destroy(Object);
            
        }
      
    }

    //returns ID of the ball
    public int GetBallID()
    {
        return (ballID);
    }

    // Sets the ballActive Variable
    public void SetBallAcitve(bool active)
    {
        ballActive = active;
    }

    // Sets the X & Y Speeds
    public void SetSpeeds(float x, float y)
    {
        xSpeed = x;
        ySpeed = y;
    }
}
     


