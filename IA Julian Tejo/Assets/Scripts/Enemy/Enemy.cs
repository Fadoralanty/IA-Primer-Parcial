using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float walkSpeed = 5f;
    public float RunSpeed = 7f;

    //LineOfSight()
    public float range;
    public float angle;
    public LayerMask obstacleMask;
    public Isabelle isabelle;
    public Transform target;
    public bool IsplayerDetected;
    //Shoot()
    public GameObject bulletPrefab;
    public float ShootRange;
    int _lastFrame;
    public float ShootRate;
    public bool LineOfSight(Transform target)
    {
        int frame = Time.frameCount;
        if (_lastFrame != frame)
        {
            _lastFrame = frame;
            Vector3 diff = target.position - transform.position;
            float Distance = diff.magnitude;
            if (Distance > range) return IsplayerDetected = false;

            float angleToTarget = Vector3.Angle(diff, transform.forward);
            if (angleToTarget > angle / 2) return IsplayerDetected = false;

            if (Physics.Raycast(transform.position, diff.normalized, Distance, obstacleMask)) return IsplayerDetected = false;
            IsplayerDetected = true;
        }
        return IsplayerDetected;
    }
    public void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
    private void Update()
    {
        //if (LineOfSight(target))
        //{
        //    Debug.Log("in view");
        //}
        //else
        //{
        //    Debug.Log("out of view");
        //}
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * range);
    }
}
