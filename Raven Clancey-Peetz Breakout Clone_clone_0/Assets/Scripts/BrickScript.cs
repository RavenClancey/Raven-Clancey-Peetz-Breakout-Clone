using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BrickScript : MonoBehaviour
{
    [SerializeField] private bool isNetworked = true;   // bool to control check network or not
    [SerializeField] private Color brickColour; // Colour of the brick

    // Start is called before the first frame update
    void Start()
    {

        
        // Set the material colour to brick colour variable
        GetComponent<MeshRenderer>().material.color = brickColour; 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
