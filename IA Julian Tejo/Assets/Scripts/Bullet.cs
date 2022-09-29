using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float destroyTime = 3f;
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        Destroy(gameObject, destroyTime);
    }
}
