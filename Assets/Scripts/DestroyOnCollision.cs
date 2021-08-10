using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public GameObject bullet;
    public Transform playerTranform;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Projectile") && !collision.gameObject.CompareTag("ColliderProjectile") && !collision.gameObject.CompareTag("ColliderProjectile1"))
        {
            Destroy(bullet);
        }

    }
    private void Start()
    {
        playerTranform = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
    }
    private void Update()
    {
        if (Vector3.Distance(playerTranform.position, transform.position) > 200f)
        {
            Destroy(bullet);
        }
    }
}
