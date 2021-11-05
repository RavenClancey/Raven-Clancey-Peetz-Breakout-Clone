using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerScript : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;    // Speed the player moves at
    [SerializeField] private Rigidbody rb;              // Rigid Body attached to the player


    // Start is called before the first frame update
    void Start()
    {


        //find and attach rigid body if not done already
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        // Name player object, Synced over server
        if (gameObject.name != "Player1")
        {
            if (GameObject.Find("Player1") == null)
            {
                gameObject.name = "Player1";
            }
            else
            {
                gameObject.name = "Player2";
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (isLocalPlayer)
        {
            // Move left if A is pushed
            if (Input.GetKey(KeyCode.A))
            {
                rb.MovePosition(new Vector3(transform.position.x - (moveSpeed * Time.deltaTime), transform.position.y, 0));
            }
            // Move Right if D is pushed
            if (Input.GetKey(KeyCode.D))
            {
                rb.MovePosition(new Vector3(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y, 0));
            }
        }


        //lock player position inside screen boundary
        if (transform.position.x > 14.5f)
        {

            rb.MovePosition(new Vector3(14.5f, transform.position.y, 0));
        }
        if (transform.position.x < -14.5f)
        {
            rb.MovePosition(new Vector3(-14.5f, transform.position.y, 0));
        }


        // stop all forces on player
        rb.velocity = new Vector3(0, 0, 0);

    }

    //function that returns if this object is the local player
    public bool CheckIfLocal()
    {
        return (isLocalPlayer);
    }
}
