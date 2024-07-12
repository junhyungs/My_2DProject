using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField]
    Transform target;

    private void Update()
    {
        if (target == null)
            return;

        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z - 10.0f);
    }
}
