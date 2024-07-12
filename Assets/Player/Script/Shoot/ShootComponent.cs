using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootComponent : MonoBehaviour
{
    private bool shoot = true;
    public void Shoot()
    {
        if (shoot)
        {
            FireBallPool.Instance.UseFireBall(transform.position);
            shoot = false;
            StartCoroutine(ShootCoolTime());
        }
    }
    private IEnumerator ShootCoolTime()
    {
        yield return new WaitForSeconds(1.0f);
        shoot = true;
    }

}
