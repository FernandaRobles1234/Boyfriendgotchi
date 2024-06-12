using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{

    private BoyfriendObject[] _BoyfriendObjects;
    private BoyfriendObject _BoyfriendObject;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Debug Running");
    }

    // Update is called once per frame
    void Update()
    {
        _BoyfriendObjects = FindObjectsOfType<BoyfriendObject>();

        if (_BoyfriendObjects.Length == 0)
        {
            Debug.Log("No boyfriend object.");
        }
        else
        {
            foreach (var boyfriend in _BoyfriendObjects)
            {
                Debug.Log("Boyfriend object transform: " + boyfriend.transform.position);
            }
        }
    }
}
