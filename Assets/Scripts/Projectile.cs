using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile: MonoBehaviour
{
    [SerializeField] private float shootForce = 10;
    [SerializeField] private float bulletLifetime;

    public void Shoot(Vector3 direction)
    {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body == null) {
            Debug.LogError($"Rigid body of object {gameObject.name} was null");
            Destroy(gameObject);
        }
        transform.LookAt(transform.position+direction);
        body.AddForce(transform.forward * shootForce, ForceMode.Impulse);
        Destroy(gameObject, bulletLifetime);
    }

    public void OnTriggerEnter(Collider other) {
        //TODO: make damage here
        Debug.Log(other.gameObject.name);
        Destroy(gameObject);
    }
}

