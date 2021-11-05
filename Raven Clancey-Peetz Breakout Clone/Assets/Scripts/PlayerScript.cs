using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if(Input.GetKey(KeyCode.A) )
        {
            transform.position = new Vector3(transform.position.x - (moveSpeed * Time.deltaTime), transform.position.y, 0);
        }

        if (Input.GetKey(KeyCode.D) )
        {
            
            transform.position = new Vector3(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y, 0);
        }

        if(transform.position.x > 11.5f)
        {
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }
        if (transform.position.x < -11.5f)
        {
            transform.position = new Vector3(-11.5f, transform.position.y, 0);
        }

    }
}
