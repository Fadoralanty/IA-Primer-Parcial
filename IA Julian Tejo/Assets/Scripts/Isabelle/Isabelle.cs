using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isabelle : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _turnSmoothTime = 0.1f;
    [SerializeField] Transform Camera;
    [SerializeField] bool _isDead = false;

    private float turnSmoothVelocity;
    public bool IsDead { get => _isDead; }

    public void Movement(Vector3 dir)
    {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += moveDir.normalized  * _speed * Time.deltaTime;
            //transform.rotation=Quaternion.RotateTowards(transform.rotation,)
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            _isDead = true;
        }
    }
}
