using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReGame : MonoBehaviour
{
    private void Awake()
    {
        var Regame = FindObjectsOfType<ReGame>();

        if (Regame.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
