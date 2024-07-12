using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCanvas : MonoBehaviour
{

    private void Awake()
    {
        var Hpcanvas = FindObjectsOfType<HPCanvas>();

        if(Hpcanvas.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
