using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRotate : MonoBehaviour
{
    public Transform trans;
    public float speed = 10;
    public float rotationZ;
    public enum direction { left, up, right, down }
    public direction dir;
    // Start is called before the first frame update
    void Start()
    {

        trans = GetComponent<Transform>();
        trans.parent = GameObject.FindGameObjectWithTag("Player").transform;
        if (dir == direction.up)
        {
            trans.localEulerAngles = new Vector3(trans.localEulerAngles.x, trans.localEulerAngles.y, -90);
        }
        if (dir == direction.down)
        {
            trans.localEulerAngles = new Vector3(trans.localEulerAngles.x, trans.localEulerAngles.y, 90);
        }
        rotationZ = trans.localEulerAngles.z;
        if (dir == direction.left && rotationZ == 0) rotationZ = 360;
        
        
        

    }


    // Update is called once per frame
    void Update()
    {
        trans.Rotate(Vector3.forward, 45 * Time.deltaTime * speed);
        switch (dir)
        {
            case direction.right:
                if (trans.localEulerAngles.z > rotationZ + 180)
                {
                    Destroy(this.gameObject);
                }
                break;
            case direction.left:
                if (trans.localEulerAngles.z > rotationZ - 180)
                {
                    Destroy(this.gameObject);
                }
                break;
            case direction.up:
                if (trans.localEulerAngles.z > 100 && trans.localEulerAngles.z < 180)
                {
                    Destroy(this.gameObject);
                }
                break;
            case direction.down:

                if (trans.localEulerAngles.z > 275)
                {
                    Destroy(this.gameObject);
                }
                break;
        }


    }
}
