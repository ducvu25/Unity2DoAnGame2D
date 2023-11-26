using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInformation : MonoBehaviour
{
    [Header("------------Information--------------")]
    public float speed = 2;
    public float maxhp = 500;
    public float timeSpawn = 1f;
    public float timeIdle = 1f;
    public float delayFilip = 0.1f;
    public float delayHit = 1f;
    public float hp;
    public float distance = 5;
    private void Start()
    {
        hp = maxhp;
    }
    public bool Hit(float value)
    {
        hp -= value;
        return hp <= 0;
    }
}
