using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientAutoInit : MonoBehaviour
{
    public NetworkManager _nm;

    private void Start()
    {
        _nm.StartClient();
    }
}
