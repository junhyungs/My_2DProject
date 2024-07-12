using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireBallPool : Singleton<FireBallPool>
{
    public static FireBallPool instance;
    [SerializeField] GameObject FireBall;
    [SerializeField] int InitFireBallCount;

    private Queue<GameObject>FireBallQueue = new Queue<GameObject>();
    void Start()
    {
        CreateFireBall(InitFireBallCount);
    }
    public void CreateFireBall(int InitFireBallCount)
    {
        for (int i = 0; i < InitFireBallCount; i++)
        {
            GameObject fireBall = Instantiate(FireBall, transform);
            fireBall.SetActive(false);
            FireBallQueue.Enqueue(fireBall);
        }
    }

    public void UseFireBall(Vector3 shootPosition)
    {
        if(FireBallQueue.Count == 0)
        {
            CreateFireBall(1);
        }

        GameObject fireBall = FireBallQueue.Dequeue();     

        fireBall.transform.position = shootPosition;
        fireBall.transform.parent = null;
        fireBall.SetActive(true);
    }

    public void ReturnFireBall(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = transform;
        FireBallQueue.Enqueue(gameObject);
    }
    
}
