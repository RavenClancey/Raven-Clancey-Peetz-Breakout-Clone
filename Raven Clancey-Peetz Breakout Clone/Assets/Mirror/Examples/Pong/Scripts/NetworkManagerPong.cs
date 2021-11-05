using UnityEngine;


namespace Mirror.Examples.Pong
{
    // Custom NetworkManager that simply assigns the correct racket positions when
    // spawning players. The built in RoundRobin spawn method wouldn't work after
    // someone reconnects (both players would be on the same side).
    [AddComponentMenu("")]
    public class NetworkManagerPong : NetworkManager
    {

        public int serverScore = 0;
        public Transform player1Spawn;
        public Transform player2Spawn;
        GameObject ball;
        GameObject ball2;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            // add player at correct spawn position
            Transform start = numPlayers == 0 ? player1Spawn : player2Spawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            
            NetworkServer.AddPlayerForConnection(conn, player);

            // spawn ball if two players
            if (numPlayers == 2)
            {
                ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
                
                NetworkServer.Spawn(ball);

                ball2 = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
               
                NetworkServer.Spawn(ball2);
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            // destroy ball
            if (ball != null)
                NetworkServer.Destroy(ball);

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }
  
    }
}
