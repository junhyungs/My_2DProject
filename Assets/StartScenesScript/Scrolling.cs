using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public float Speed = 5.0f;
    
    void Update()
    {
        transform.Translate(Vector3.left *  Speed * Time.deltaTime);
    }
}
