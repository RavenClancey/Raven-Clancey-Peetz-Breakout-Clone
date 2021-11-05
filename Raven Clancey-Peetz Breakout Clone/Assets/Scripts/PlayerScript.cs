using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //find and attach rigid body if not done already
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if(Input.GetKey(KeyCode.A) )
        { 
            rb.MovePosition(new Vector3(transform.position.x - (moveSpeed * Time.deltaTime), transform.position.y, 0));
        }

        if (Input.GetKey(KeyCode.D) )
        {          
            rb.MovePosition(new Vector3(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y, 0));
        }

        if(transform.position.x > 14.5f)
        {
            
            rb.MovePosition(new Vector3(14.5f, transform.position.y, 0));
        }
        if (transform.position.x < -14.5f)
        {
            rb.MovePosition(new Vector3(-14.5f, transform.position.y, 0));
        }

    }
}
