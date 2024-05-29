using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStart : MonoBehaviour
{
    public MyNetworkManager _NetworkManager;

    // Start is called before the first frame update
    void Start()
    {
        _NetworkManager.StartServer();
    }

}
