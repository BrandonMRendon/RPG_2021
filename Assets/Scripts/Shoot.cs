using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour, IItem
{
    public GameObject bulletPrefab;
    public Transform parent;
    public float fireSpeed = 20;
    public int Damage { get; set; }


    public void Action(Vector2 directionFacing)
    {
        
            // Instantiate the projectile at the position and rotation of this transform
        GameObject projectile;
        projectile = Instantiate(bulletPrefab, transform.position + (Vector3)directionFacing, transform.rotation);
        Collider2D col1 = projectile.GetComponent<Collider2D>();
        if (GameObject.FindGameObjectWithTag("ColliderProjectile") != null)
        {
            Collider2D col2 = GameObject.FindGameObjectWithTag("ColliderProjectile").GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(col1, col2);
        }
        if (GameObject.FindGameObjectWithTag("ColliderProjectile1") != null)
        {
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("ColliderProjectile1").GetComponent<Collider2D>());
        }
       
        
        if (Cursor.visible == true)
        {

            Vector2 mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 direction =  mouse - new Vector2(playerScreenPoint.x, playerScreenPoint.y);
            
            direction.Normalize();
            direction *= new Vector2(1, -1);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * fireSpeed*2);
            projectile.GetComponent<Rigidbody2D>().AddTorque(20);
        }
        else
        {
            projectile.GetComponent<Rigidbody2D>().AddForce(directionFacing * fireSpeed*2);
            projectile.GetComponent<Rigidbody2D>().AddTorque(20*5);
        }
        //projectile.GetComponent<Rigidbody>().AddForce(transform.forward * shootingStrength, ForceMode.Impulse);
        // Give the cloned object an initial velocity along the current
        // object's Z axis
        
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    
}
