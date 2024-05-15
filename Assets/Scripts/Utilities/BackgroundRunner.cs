using UnityEngine;

public class BackgroundRunner : MonoBehaviour
{
    void Awake()
    {
        // This makes the game not stop running when the focus is lost
        Application.runInBackground = true;
    }
}