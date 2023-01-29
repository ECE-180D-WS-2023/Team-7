using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DistributePlayerID : NetworkBehaviour
{

    private IDictionary<int, int> PlayerIDtoConnID = new Dictionary<int, int>();


    private void Start()
    {
        PlayerIDtoConnID.Add(1, -1);
        PlayerIDtoConnID.Add(2, -1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }


    public (int, int) ReturnPlayerID()
    {

        int connIDToAdd = -1;
        foreach (var connection in NetworkServer.connections)
        {
           if (PlayerIDtoConnID[1] != connection.Key && PlayerIDtoConnID[2] != connection.Key)
            {
                connIDToAdd = connection.Key;
            }
        }
        foreach (var playerIDtoConnIDPair in PlayerIDtoConnID)
        {
            if (playerIDtoConnIDPair.Value == -1)
            {
                PlayerIDtoConnID[playerIDtoConnIDPair.Key] = connIDToAdd;

                foreach (var debug in PlayerIDtoConnID)
                {
                    Debug.Log("Server: " + debug.Key + " -- " + debug.Value);
                }

                return (playerIDtoConnIDPair.Key, PlayerIDtoConnID[playerIDtoConnIDPair.Key]);
            }
        }
        return (-1, -1);
    }
}
