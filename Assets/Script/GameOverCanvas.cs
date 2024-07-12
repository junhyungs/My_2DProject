using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour
{
   
    private void Awake()
    {
        var gameovercanvas = FindObjectsOfType<GameOverCanvas>();

        if(gameovercanvas.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
