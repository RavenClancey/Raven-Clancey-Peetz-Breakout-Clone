using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{

    [SerializeField] private Color brickColour;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = brickColour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
