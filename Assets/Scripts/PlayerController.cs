using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Entity
{
    [Header("Set in Inspector")]
    public float speed = 5;
    [Header("Set Dynamically")]
    public int dirHeld = -1; // Direction of the held movement key
    public enum walkingDirection  {RIGHT, UP, LEFT, DOWN }
    public int dirFacing = 1;
    private int numKeys = 0;
    private Rigidbody2D rigid;
    GameObject invCursorImg, invSelectorImg, pickedItem; //The object that will hold the sprite of the picked item
    RectTransform invCursor,invSelector; //cursor is the white arrow, selector is the green circle on quickbar only

    public IItem[] inventoryArray = new IItem[36]; //Player's inventory
    GameObject[] inventorySprites; //the sprites of their inventort
    private Animator anim;
    public Animator inventoryAnim;
    private Vector2 mouse;
    public Text keyText, itemNameText;
    public Sprite UIMask;

    private int equippedIndex, cursorIndex, cursorHoldingIndex; //Index of the quick bar, index of the cursor currently, index of the item selected by cursor
    public IItem EquippedItem; //Item selected by the Green Selector
    IItem pickedCursorItem; //The item held by the cursor
    bool cursorHolding; //is the cursor currently holding something?

    public Slider healthBar, ManaBar;
    bool attacking, isInventory; //is the attack animation still playing? is inventory open?
    public bool playerIsFrozen { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            SetHealth(getHealth() - collision.gameObject.GetComponent<DamageModifier>().damage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetHealth(startingHealth);
        cursorHolding = false;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inventorySprites = GameObject.FindGameObjectsWithTag("InventorySprite");
        playerIsFrozen = true;
        attacking = false;
        cursorIndex = 10;
        isInventory = false;
        itemNameText = GameObject.FindGameObjectWithTag("ItemText").GetComponent<Text>();
        pickedItem = GameObject.FindGameObjectWithTag("PickedItem");
        invSelectorImg = GameObject.FindGameObjectWithTag("InventorySelector");
        invSelector = invSelectorImg.GetComponent<RectTransform>();
        invCursorImg = GameObject.FindGameObjectWithTag("InventoryCursor");
        invCursor = invCursorImg.GetComponent<RectTransform>();
        invCursorImg.SetActive(false);
        StartCoroutine("ManaRegen");
        equippedIndex = 0;
        FixTheSpriteArray();
    }
    public void AddItemToInv(IItem item)
    {
        for (int i = 0; i < 36; i++)
        {
            if (inventoryArray[i] == null)
            {
                inventoryArray[i] = item;
                inventorySprites[i].GetComponent<Image>().sprite = item.sprite;
                break;
            }
        }
    }
    void FixTheSpriteArray()
    {
        GameObject[] QuickBar = new GameObject[] { inventorySprites[30], inventorySprites[31], inventorySprites[32], inventorySprites[33], inventorySprites[34], inventorySprites[35] }; 
        for (int i = 35; i > 5; i--){

            inventorySprites[i] = inventorySprites[i - 6];
        }
        for(int i = 0; i < 6; i++)
        {
            inventorySprites[i] = QuickBar[i];
        }
        
        
    }
    public void AttackOver()
    {
        attacking = false;
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
    IEnumerator ManaRegen()
    {
        while (true)
        {
            if (ManaBar.value != ManaBar.maxValue)
            {
                ManaBar.value += 5;
            }
            yield return new WaitForSeconds(.5f);
        }
        

    }
    //velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    // Update is called once per frame
    void Update()
    {
        if (playerIsFrozen)
        {
            if (dirHeld != -1)
            {
                dirFacing = dirHeld;
                anim.CrossFade("Player_Walk_" + dirHeld, 0);
            }
            
            anim.speed = 0;
            return;
        }
        healthBar.value = getHealth();
        
        dirHeld = -1;
        // Delete the four "if ( Input.GetKey..." lines that were here
        getDir();

        // Animation
        if (dirHeld == -1)
        {
            if (!attacking)
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
                /*
                if (Cursor.visible == true)
                {
                    getCursorDirection();
                }
                dirFacing = dirHeld;
                anim.CrossFade("Player_Attack_" + dirHeld, 0);
                anim.speed = 1;*/
            }
            
        }
        else
        {
            
            if (Cursor.visible == true)
            {
                getCursorDirection();
            }
            dirFacing = dirHeld;
            if (!attacking)
            {
                anim.CrossFade("Player_Walk_" + dirHeld, 0);
            }
            else
            {
                //anim.CrossFade("Player_Attack_" + dirHeld, 0);
            }
            anim.speed = 1;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Break();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            //set the PlayMode too stop
#else
        Application.Quit();
#endif
        }
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryAnim.SetTrigger(isInventory ? "Close" : "Open");
            inventoryAnim.speed = 1;
            invCursorImg.SetActive(!isInventory);
            if (cursorHolding)
            {
                inventoryArray[cursorHoldingIndex] = pickedCursorItem;
                pickedCursorItem = null;
                inventorySprites[cursorHoldingIndex].GetComponent<Image>().sprite = pickedCursorItem.sprite;
                pickedItem.GetComponent<Image>().sprite = UIMask;
                cursorHolding = false;
            }
            isInventory = !isInventory;
        }
        if (Input.mouseScrollDelta.y > 0 && invSelector.anchoredPosition.x < -195)
        {
            invSelector.anchoredPosition = new Vector2(invSelector.anchoredPosition.x + 34, invSelector.anchoredPosition.y);
            equippedIndex += 1;
            
        }
        if (Input.mouseScrollDelta.y < 0 && invSelector.anchoredPosition.x > -365)
        {
            invSelector.anchoredPosition = new Vector2(invSelector.anchoredPosition.x - 34, invSelector.anchoredPosition.y);
            equippedIndex -= 1;
        }
        if (isInventory)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && invCursor.anchoredPosition.x < -182)
            {
                invCursor.anchoredPosition = new Vector2(invCursor.anchoredPosition.x + 34, invCursor.anchoredPosition.y);
                cursorIndex += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && invCursor.anchoredPosition.x > -350)
            {
                invCursor.anchoredPosition = new Vector2(invCursor.anchoredPosition.x - 34, invCursor.anchoredPosition.y);
                cursorIndex -= 1;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && invCursor.anchoredPosition.y < 132)
            {
                invCursor.anchoredPosition = new Vector2(invCursor.anchoredPosition.x , invCursor.anchoredPosition.y + 34 + (invCursor.anchoredPosition.y > 129 ? 10:0));
                cursorIndex -= 6;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && invCursor.anchoredPosition.y > -3)
            {
                invCursor.anchoredPosition = new Vector2(invCursor.anchoredPosition.x, invCursor.anchoredPosition.y - 34 - (invCursor.anchoredPosition.y > 132 ? 10 : 0));
                cursorIndex += 6;
            }
            itemNameText.text = inventoryArray[cursorIndex] != null ? inventoryArray[cursorIndex].itemName : "";
            if (Input.GetButtonDown("Submit"))
            {
                if (!cursorHolding && inventoryArray[cursorIndex] != null)
                {
                    pickedItem.GetComponent<Image>().sprite = inventorySprites[cursorIndex].GetComponent<Image>().sprite;
                    inventorySprites[cursorIndex].GetComponent<Image>().sprite = UIMask;
                    pickedCursorItem = inventoryArray[cursorIndex];
                    inventoryArray[cursorIndex] = null;
                    cursorHoldingIndex = cursorIndex;
                    cursorHolding = true;
                }
                
                else if(cursorHolding && inventoryArray[cursorIndex] == null)
                {
                    inventoryArray[cursorIndex] = pickedCursorItem;
                    inventorySprites[cursorIndex].GetComponent<Image>().sprite = pickedCursorItem.sprite;
                    pickedCursorItem = null;
                    pickedItem.GetComponent<Image>().sprite = UIMask;
                    cursorHolding = false;
                }
            }

        }
        if (inventoryArray[equippedIndex] != null && Input.GetButtonDown("Fire2") && ManaBar.value>= inventoryArray[equippedIndex].usesMana)
        {
            inventoryArray[equippedIndex].Action(FacingToVector());
            //attacking = true;
            ManaBar.value -= inventoryArray[equippedIndex].usesMana;
        }

    }
}
