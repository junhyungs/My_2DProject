using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] public Transform Player;
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector2 movedir = Player.transform.position - transform.position;
        Vector2 MonsterMove = movedir * Time.deltaTime;
        transform.Translate(MonsterMove);
    }
}
