using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 5;
    [Header("Set Dynamically")]
    public int dirHeld = -1; // Direction of the held movement key
    public enum walkingDirection  {RIGHT, UP, LEFT, DOWN }
    public int dirFacing = 1;
    private int numKeys = 0;
    private Rigidbody2D rigid;

    public IItem Equipped1,Equipped2;
    private Animator anim;
    private Vector2 mouse;
    public Text keyText;



    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void getCursorDirection()
    {
        mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if(Mathf.Abs(mouse.x - playerScreenPoint.x) > Mathf.Abs(mouse.y - playerScreenPoint.y))
        {
            if (mouse.x > playerScreenPoint.x)
            {

                dirHeld = (int)walkingDirection.RIGHT;
            }
            else
            {
                dirHeld = (int)walkingDirection.LEFT;
            }
        }
        else
        {
            if (mouse.y > playerScreenPoint.y)
            {

                dirHeld = (int)walkingDirection.DOWN;
            }
            else
            {
                dirHeld = (int)walkingDirection.UP;
            }
        }
        
    }
    public void getKey()
    {
        numKeys += 1;
        keyText.text = "x " + numKeys;
    }
    public bool takeKey()
    {
        if (numKeys > 0)
        {
            numKeys -= 1;
            keyText.text = "x " + numKeys;
            return true;
        }
        else
        {
            return false;
        }
    }
    Vector2 FacingToVector()
    {
        switch (dirFacing)
        {
            case 0:
                return Vector2.right;
            case 1:
                return Vector2.up;
            case 2:
                return Vector2.left;
            case 3:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }
    void getDir()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rigid.velocity = new Vector2(x, y) * speed;
        if (x == 0 && y == 0) return;
        if (x > y)
        {
            if (x > 0)
            {
                dirHeld = (int)walkingDirection.RIGHT;
            }
            else
            {
                dirHeld = (int)walkingDirection.DOWN;
            }
        }
        else
        {
            if (y > 0)
            {
                dirHeld = (int)walkingDirection.UP;
            }
            else
            {
                dirHeld = (int)walkingDirection.LEFT;
            }
        }
        
    }
    //velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    // Update is called once per frame
    void Update()
    {
        
        dirHeld = -1;
        // Delete the four "if ( Input.GetKey..." lines that were here
        getDir();

        // Animation
        if (dirHeld == -1)
        {
            if (Cursor.visible == true)
            {
                getCursorDirection();
                dirFacing = dirHeld;
                anim.CrossFade("Player_Walk_" + dirHeld, 0);
            }
            
            anim.speed = 0;
            
        }
        else
        {
            
            if (Cursor.visible == true)
            {
                getCursorDirection();
            }
            dirFacing = dirHeld;
            anim.CrossFade("Player_Walk_" + dirHeld, 0); 
            anim.speed = 1;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Break();
        }
        if (Equipped2 != null && Input.GetButtonDown("Fire2"))
        {
            Equipped2.Action(FacingToVector());
        }
        if (Equipped1 != null && Input.GetButtonDown("Fire1"))
        {
            Equipped1.Action(FacingToVector());
        }

    }
}
