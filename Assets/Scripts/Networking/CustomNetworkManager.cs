using Mirror;
using Mirror.Examples.Pong;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    GameObject serverBoyfriend;

    public override void OnStartServer()
    {
        base.OnStartServer();

        serverBoyfriend = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "ServerBoyfriend"));

    }
    public override void OnServerReady(NetworkConnectionToClient conn)
    {

        base.OnServerReady(conn);

        NetworkServer.Spawn(serverBoyfriend);

        Debug.Log("Client connected");
    }

}