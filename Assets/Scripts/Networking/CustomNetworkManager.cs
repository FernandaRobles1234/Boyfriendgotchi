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
        NetworkServer.Spawn(serverBoyfriend);

    }
    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        Debug.Log("Client Ready");
    }

}