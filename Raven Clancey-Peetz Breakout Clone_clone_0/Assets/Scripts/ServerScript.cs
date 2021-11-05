using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerScript : NetworkBehaviour
{
    [SerializeField] private int serverScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    // Function that sends a command to the server to run the Update client scores function on all clients
    [Command]
    public void AddServerScore(int score)
    {
        UpdateClientScores(score);

    }

    [ClientRpc]
    public void UpdateClientScores(int score)
    {

        GameObject.Find("GameManager").GetComponent<GameManagerScript>().AddScore(score);
    }

    // Function that sends a command to the server to run the Server Ball Active function on all clients
    [Command]
    public void ServerBallActive(GameObject ball, bool active, float yspd, float xspd)
    {
        UpdateClientBall(ball, active, yspd, xspd);

    }
    [ClientRpc]
    public void UpdateClientBall(GameObject ball, bool active, float yspd, float xspd)
    {

        ball.GetComponent<NetworkBallScript>().SetBallAcitve(active);
        ball.GetComponent<NetworkBallScript>().SetSpeeds(xspd, yspd);
    }
}
