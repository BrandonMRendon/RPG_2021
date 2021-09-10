using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    private Collider2D collider2d;
    GameObject invCursorImg, invSelectorImgLeft, invSelectorImgRight, pickedItem; //The object that will hold the sprite of the picked item
    RectTransform invCursor,invSelectorLeft, invSelectorRight; //cursor is the white arrow, selector is the green circle on quickbar only

    public GameObject[] inventoryArray = new GameObject[36]; //Player's inventory
    public GameObject[] inventorySprites, chestSprites; //the sprites of their inventory an chests

    private Animator anim;
    public Animator inventoryAnim, chestAnim;
    private Vector2 mouse;
    public Text keyText, itemNameText;
    public Sprite UIMask;
    public Container chest;

    public NavMeshAgent agent;
    Vector3 enemyTransform;
    bool enemyHit;
    public int multiplier, range;

    IItem lastUsed;
    bool wasActiveUse, activeUseStart;

    public int equippedIndexLeft, equippedIndexRight, cursorIndex, cursorHoldingIndex; //Index of the quick bar, index of the cursor currently, index of the item selected by cursor
    public GameObject EquippedItemLeft, EquippedItemRight; //Item selected by the Green Selector
    GameObject pickedCursorItem; //The item held by the cursor
    bool cursorHolding, isActiveBuffer; //is the cursor currently holding something?
    public int lightProtection;
    public Slider healthBar, ManaBar;
    bool attacking, isInventory, isChestInRange, isChest, isInChest, wasInChest; //is the attack animation still playing? is inventory open? is the chest within opening range? is a chest open?, is the cursor in a chest, was the current item picked up in the chest?
    public bool playerIsFrozen { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            SetHealth(getHealth() - collision.gameObject.GetComponent<DamageModifier>().damage);
        }
    }
    public void LoadChest(Container chest)
    {
        
        for(int i = 0; i < chestSprites.Length-1; i++)
        {
            if(chest.inventorySprites[i] == null)
            {
                chestSprites[i].GetComponent<Image>().sprite = UIMask;
            }
            else
            {
                chestSprites[i].GetComponent<Image>().sprite = chest.inventorySprites[i];
                if (chest.inventoryArray[i].GetComponent<IItem>().stack > 1)
                {
                    chestSprites[i].GetComponentInChildren<Text>().text = ""+chest.inventoryArray[i].GetComponent<IItem>().stackHolding;
                }
            }
        }
    }
    public void UnloadChest()
    {
        for (int i = 0; i < chestSprites.Length; i++)
        {
            chestSprites[i].GetComponent<Image>().sprite = UIMask;
            chestSprites[i].GetComponentInChildren<Text>().text = "";
        }
    }
    
    IEnumerator PlayerInvincible()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<Collider2D>() != null) Physics2D.IgnoreCollision(collider2d, enemies[i].GetComponent<Collider2D>(), true);
        }
        yield return new WaitForSeconds(2);
        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].GetComponent<Collider2D>() != null) Physics2D.IgnoreCollision(collider2d, enemies[i].GetComponent<Collider2D>(), false);
        }
    }
    public void Knockback(Transform enemy)
    {
        enemyHit = true;
        enemyTransform = enemy.position;
        agent.enabled = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;
        agent.enabled = false;
        range = 2;
        multiplier = 1;

        SetHealth(startingHealth);
        cursorHolding = false;
        collider2d = GetComponent<Collider2D>();
        wasActiveUse = false;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inventorySprites = GameObject.FindGameObjectsWithTag("InventorySprite");
        chestSprites = GameObject.FindGameObjectsWithTag("ChestSprite");
        playerIsFrozen = true;
        attacking = false;
        activeUseStart = false;
        isActiveBuffer = false;
        cursorIndex = 11;
        isInventory = false;
        itemNameText = GameObject.FindGameObjectWithTag("ItemText").GetComponent<Text>();
        pickedItem = GameObject.FindGameObjectWithTag("PickedItem");
        invSelectorImgLeft = GameObject.FindGameObjectsWithTag("InventorySelector")[1];
        invSelectorImgRight = GameObject.FindGameObjectsWithTag("InventorySelector")[0];
        invSelectorLeft = invSelectorImgLeft.GetComponent<RectTransform>();
        invSelectorRight = invSelectorImgRight.GetComponent<RectTransform>();
        invCursorImg = GameObject.FindGameObjectWithTag("InventoryCursor");
        invCursor = invCursorImg.GetComponent<RectTransform>();
        invCursorImg.SetActive(false);
        StartCoroutine("ManaRegen");
        equippedIndexLeft = 0;
        lightProtection = 0;
        equippedIndexLeft = 2;
        FixTheSpriteArray();
    }
    public bool AddItemToInv(GameObject item)
    {
        for (int i = 0; i < 36; i++)
        {
            if (inventoryArray[i] != null && inventoryArray[i].GetComponent<IItem>().itemName == item.GetComponent<IItem>().itemName)
            {
                if (inventoryArray[i].GetComponent<IItem>().stackHolding < item.GetComponent<IItem>().stack)
                {
                    inventoryArray[i].GetComponent<IItem>().stackHolding += 1;
                    inventorySprites[i].GetComponentInChildren<Text>().text = "" + inventoryArray[i].GetComponent<IItem>().stackHolding;
                    return true;
                }
                
            }
            if (inventoryArray[i] == null)
            {
                
                inventoryArray[i] = item;
                inventorySprites[i].GetComponent<Image>().sprite = item.GetComponent<IItem>().sprite;
                if (item.GetComponent<IItem>().stack > 1)
                {
                    inventorySprites[i].GetComponentInChildren<Text>().text = "" + inventoryArray[i].GetComponent<IItem>().stackHolding;
                }
                break;
            }
        }
        return false;
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

        GameObject[] Row3 = new GameObject[] { chestSprites[12], chestSprites[13], chestSprites[14], chestSprites[15], chestSprites[16], chestSprites[17] };
        GameObject[] Row2 = new GameObject[] { chestSprites[6], chestSprites[7], chestSprites[8], chestSprites[9], chestSprites[10], chestSprites[11] };
        for (int i = 6; i < 12; i++)
        {

            chestSprites[i] = Row3[i - 6];
        }
        for (int i = 12; i < 18; i++)
        {
            chestSprites[i] = Row2[i - 12];
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
            if (!activeUseStart && ManaBar.value != ManaBar.maxValue)
            {
                ManaBar.value += 5;
            }
            yield return new WaitForSeconds(.5f);
        }
        

    }
    IEnumerator ManaDegen(int manaCost)
    {
        yield return new WaitForSeconds(.5f);
        ManaBar.value -= manaCost;
        isActiveBuffer = false;
    }
    public void OpenChest(Container container)
    {
        chest = container;
        isChestInRange = true;
        LoadChest(chest);
    }
    public void CloseChest()
    {
        chest = null;
        isChestInRange = false;
        isInChest = false;
        UnloadChest();
        isInChest = false;
        cursorIndex = 11;
        invCursor.anchoredPosition = new Vector2(-181, 130);

    }
    //velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    public void MoveCursor(bool conditionOne, bool conditionTwo, int indexAdder, int xPosAdder, int yPosAdder)
    {
        if(conditionOne && conditionTwo)
        {
            invCursor.anchoredPosition = new Vector2(invCursor.anchoredPosition.x + xPosAdder, invCursor.anchoredPosition.y + yPosAdder);
            cursorIndex += indexAdder;
        }
        
    }

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

        /*-----------------INVENTORY MANAGEMENT -----------*/
        if (Input.GetButtonDown("Inventory"))  //Inventory button is pressed
        {
            if (cursorHolding) //If inventory was cloesd while holding an item
            {
                if (wasInChest)//If the item being held was in the chest, return item to chest at same location it was taken from
                {
                    if (pickedCursorItem.GetComponent<IItem>().stack > 1)
                    {
                        chestSprites[cursorHoldingIndex].GetComponentInChildren<Text>().text = "" + pickedCursorItem.GetComponent<IItem>().stackHolding;
                        pickedItem.GetComponentInChildren<Text>().text = "";
                    }

                    chest.inventoryArray[cursorHoldingIndex] = pickedCursorItem;
                    
                    chestSprites[cursorHoldingIndex].GetComponent<Image>().sprite = chest.inventorySprites[cursorHoldingIndex] = pickedCursorItem.GetComponent<IItem>().sprite;
                    pickedCursorItem = null;
                    pickedItem.GetComponent<Image>().sprite = UIMask;
                    cursorHolding = false;
                }
                else//If the item being held was in the inventory, return item to inventory at same location it was taken from
                {
                    if (pickedCursorItem.GetComponent<IItem>().stack > 1)
                    {
                        inventorySprites[cursorHoldingIndex].GetComponentInChildren<Text>().text = "" + pickedCursorItem.GetComponent<IItem>().stackHolding;
                        pickedItem.GetComponentInChildren<Text>().text = "";
                    }

                    inventoryArray[cursorHoldingIndex] = pickedCursorItem;
                    
                    inventorySprites[cursorHoldingIndex].GetComponent<Image>().sprite = pickedCursorItem.GetComponent<IItem>().sprite;
                    pickedCursorItem = null;
                    pickedItem.GetComponent<Image>().sprite = UIMask;
                    cursorHolding = false;
                }

            }
            inventoryAnim.SetTrigger(isInventory ? "Close" : "Open"); //If inventory is closed, play Open anim (vice versa)
            if (isChestInRange) //If there is a chest in range, Open/Close it
            {
                chestAnim.SetTrigger(isInventory ? "CloseChest" : "OpenChest");
                isChest = !isInventory;
                if (isInventory)
                {
                    isInChest = false;
                    cursorIndex = 11;
                    invCursor.anchoredPosition = new Vector2(-181, 130);
                }
            }
            if(!isChestInRange && isChest) //If player walks away from chest while still inside menu, close chest and menu
            {
                isChest = false;
                chestAnim.SetTrigger("CloseChest");
                isInChest = false;
                cursorIndex = 11;
                invCursor.anchoredPosition = new Vector2(-181, 130);
            }
            inventoryAnim.speed = 1;
            invCursorImg.SetActive(!isInventory);
            
            isInventory = !isInventory;
        }
        if (Input.GetButton("Shift")) //Adjust secondary selector on quickbar (RED)
        {
            if (Input.mouseScrollDelta.y > 0 && invSelectorRight.anchoredPosition.x < -195)
            {
                invSelectorRight.anchoredPosition = new Vector2(invSelectorRight.anchoredPosition.x + 34, invSelectorRight.anchoredPosition.y);
                equippedIndexRight += 1;

            }
            if (Input.mouseScrollDelta.y < 0 && invSelectorRight.anchoredPosition.x > -365)
            {
                invSelectorRight.anchoredPosition = new Vector2(invSelectorRight.anchoredPosition.x - 34, invSelectorRight.anchoredPosition.y);
                equippedIndexRight -= 1;
            }
        }
        else //Adjust primary selector on quickbar (GREEN)
        {
            if (Input.mouseScrollDelta.y > 0 && invSelectorLeft.anchoredPosition.x < -195)
            {
                invSelectorLeft.anchoredPosition = new Vector2(invSelectorLeft.anchoredPosition.x + 34, invSelectorLeft.anchoredPosition.y);
                equippedIndexLeft += 1;

            }
            if (Input.mouseScrollDelta.y < 0 && invSelectorLeft.anchoredPosition.x > -365)
            {
                invSelectorLeft.anchoredPosition = new Vector2(invSelectorLeft.anchoredPosition.x - 34, invSelectorLeft.anchoredPosition.y);
                equippedIndexLeft -= 1;
            }
        }
        
        if (isInventory) //if in inventory menu
        {
            if (isInChest) //if cursor is in the chest menu
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) && invCursor.anchoredPosition.x < -70) //if cursor moves from chest menu to inventory menu, adjust cursor
                {
                    isInChest = false;
                    cursorIndex = 11;
                    invCursor.anchoredPosition = new Vector2(-181, 130);
                    return;
                }
                MoveCursor(Input.GetKeyDown(KeyCode.RightArrow), invCursor.anchoredPosition.x < 90, 1, 34, 0);
                //If right arrow is pressed and the cursor is not at the edge boundary, then:
                //Cursor index moves by one (value used for referncing array), the image gets moved +34 x axiz, and +0 y axis

                MoveCursor(Input.GetKeyDown(KeyCode.LeftArrow) , invCursor.anchoredPosition.x > -70 ,- 1, -34, 0);
                MoveCursor(Input.GetKeyDown(KeyCode.UpArrow) , invCursor.anchoredPosition.y < 100 ,- 6, 0, 34 + (invCursor.anchoredPosition.y > 129 ? 10 : 0));//if it's about to move to the quickbar, it needs more added to the y axis
                MoveCursor(Input.GetKeyDown(KeyCode.DownArrow) , invCursor.anchoredPosition.y > 40,6, 0, -34 - (invCursor.anchoredPosition.y > 132 ? 10 : 0));
            }
            else //if cursor is in the inventory menu
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) && invCursor.anchoredPosition.x > -182 && isChest) //if cursor moves from inventory menu to chest menu, adjust cursor
                {
                    isInChest = true;
                    cursorIndex = 0;
                    invCursor.anchoredPosition = new Vector2(-77, 107.5f);
                    return;
                }
                MoveCursor(Input.GetKeyDown(KeyCode.RightArrow) , invCursor.anchoredPosition.x < -182, 1, 34, 0);
                MoveCursor(Input.GetKeyDown(KeyCode.LeftArrow) , invCursor.anchoredPosition.x > -350 , -1, -34, 0);
                MoveCursor(Input.GetKeyDown(KeyCode.UpArrow), invCursor.anchoredPosition.y < 132, -6, 0, +34 + (invCursor.anchoredPosition.y > 129 ? 10 : 0));
                MoveCursor(Input.GetKeyDown(KeyCode.DownArrow) , invCursor.anchoredPosition.y > -3,+6,0, -34 - (invCursor.anchoredPosition.y > 132 ? 10 : 0));
            }
            
            
            if(isInChest) itemNameText.text = chest.inventoryArray[cursorIndex] != null ? chest.inventoryArray[cursorIndex].GetComponent<IItem>().itemName : ""; //Display name of item in the chest to description bar
            else itemNameText.text = inventoryArray[cursorIndex] != null ? inventoryArray[cursorIndex].GetComponent<IItem>().itemName : ""; //Display name of item in the inventory to description bar
            if (Input.GetButtonDown("Submit")) //When pressing Cursor Select button
            {
                if (isInChest) //if cursor is in the chest menu
                {
                    if (!cursorHolding && chest.inventoryArray[cursorIndex] != null) //if the slot the cursor is on is not empty, take item into cursor's temp slot
                    {
                        pickedItem.GetComponent<Image>().sprite = chestSprites[cursorIndex].GetComponent<Image>().sprite;
                        chestSprites[cursorIndex].GetComponent<Image>().sprite = chest.inventorySprites[cursorIndex] = UIMask;
                        pickedCursorItem = chest.inventoryArray[cursorIndex];
                        chest.inventoryArray[cursorIndex] = null;
                        cursorHoldingIndex = cursorIndex;
                        cursorHolding = true;
                        wasInChest = true;

                        if (pickedCursorItem.GetComponent<IItem>().stack > 1)
                        {
                            pickedItem.GetComponentInChildren<Text>().text = "" + pickedCursorItem.GetComponent<IItem>().stackHolding;
                            chestSprites[cursorIndex].GetComponentInChildren<Text>().text = "";
                        }
                    }

                    else if (cursorHolding && chest.inventoryArray[cursorIndex] == null) //if the slot the cursor is on is empty, take item from cursor's temp slot and place into the selected slot
                    {
                        if (pickedCursorItem.GetComponent<IItem>().stack > 1)
                        {
                            chestSprites[cursorIndex].GetComponentInChildren<Text>().text = "" + pickedCursorItem.GetComponent<IItem>().stackHolding;
                            pickedItem.GetComponentInChildren<Text>().text = "";
                        }

                        chest.inventoryArray[cursorIndex] = pickedCursorItem;
                        chestSprites[cursorIndex].GetComponent<Image>().sprite = chest.inventorySprites[cursorIndex]= pickedCursorItem.GetComponent<IItem>().sprite;
                        pickedCursorItem = null;
                        pickedItem.GetComponent<Image>().sprite = UIMask;
                        cursorHolding = false;


                    }
                }
                else
                {
                    if (!cursorHolding && inventoryArray[cursorIndex] != null)//if the slot the cursor is on is not empty, take item into cursor's temp slot
                    {
                        pickedItem.GetComponent<Image>().sprite = inventorySprites[cursorIndex].GetComponent<Image>().sprite;
                        inventorySprites[cursorIndex].GetComponent<Image>().sprite = UIMask;
                        pickedCursorItem = inventoryArray[cursorIndex];
                        inventoryArray[cursorIndex] = null;
                        cursorHoldingIndex = cursorIndex;
                        cursorHolding = true;
                        wasInChest = false;
                        if (pickedCursorItem.GetComponent<IItem>().stack > 1)
                        {
                            inventorySprites[cursorIndex].GetComponentInChildren<Text>().text = "";
                            pickedItem.GetComponentInChildren<Text>().text = "" + pickedCursorItem.GetComponent<IItem>().stackHolding;
                        }
                    }

                    else if (cursorHolding && inventoryArray[cursorIndex] == null)//if the slot the cursor is on is empty, take item from cursor's temp slot and place into the selected slot
                    {
                        if (pickedCursorItem.GetComponent<IItem>().stack > 1)
                        {
                            inventorySprites[cursorIndex].GetComponentInChildren<Text>().text = "" + pickedCursorItem.GetComponent<IItem>().stackHolding;
                            pickedItem.GetComponentInChildren<Text>().text = "";
                        }

                        inventoryArray[cursorIndex] = pickedCursorItem;
                        inventorySprites[cursorIndex].GetComponent<Image>().sprite = pickedCursorItem.GetComponent<IItem>().sprite;
                        pickedCursorItem = null;
                        pickedItem.GetComponent<Image>().sprite = UIMask;
                        cursorHolding = false;
                    }
                }
                
            }

        }
        /* ---------------------- USE ITEMS---------------*/
        if(inventoryArray[equippedIndexLeft] != null)
        {
            if (inventoryArray[equippedIndexLeft].GetComponent<IItem>().isActivelyUsed)
            {
                if (Input.GetButtonDown("Fire1") && activeUseStart)
                {
                    lastUsed.Action(FacingToVector());
                    wasActiveUse = false;
                    lastUsed = null;
                    activeUseStart = false;
                    return;
                }
                if (Input.GetButtonDown("Fire1") && ManaBar.value >= inventoryArray[equippedIndexLeft].GetComponent<IItem>().usesMana && !activeUseStart)
                {
                    activeUseStart = true;
                    lastUsed = inventoryArray[equippedIndexLeft].GetComponent<IItem>();
                    wasActiveUse = true;
                    inventoryArray[equippedIndexLeft].GetComponent<IItem>().Action(FacingToVector());
                    StartCoroutine("ManaDegen", inventoryArray[equippedIndexLeft].GetComponent<IItem>().usesMana);
                }
                if (!isActiveBuffer && activeUseStart && ManaBar.value >= inventoryArray[equippedIndexLeft].GetComponent<IItem>().usesMana)
                {
                    isActiveBuffer = true;
                    StartCoroutine("ManaDegen", inventoryArray[equippedIndexLeft].GetComponent<IItem>().usesMana);
                }
                
                
            }
            else if (Input.GetButtonDown("Fire1") && ManaBar.value >= inventoryArray[equippedIndexLeft].GetComponent<IItem>().usesMana)
            {
                IItem itemLeft = inventoryArray[equippedIndexLeft].GetComponent<IItem>();
                itemLeft.Action(FacingToVector());
                ManaBar.value -= itemLeft.usesMana;
                if (itemLeft.stack > 1)
                {
                    itemLeft.stackHolding -= 1;
                    inventorySprites[equippedIndexLeft].GetComponentInChildren<Text>().text = ""+ itemLeft.stackHolding;
                    if (itemLeft.stackHolding == 0)
                    {
                        inventoryArray[equippedIndexLeft] = null;
                        inventorySprites[equippedIndexLeft].GetComponent<Image>().sprite = UIMask;
                        inventorySprites[equippedIndexLeft].GetComponentInChildren<Text>().text = "";
                    }
                }
            }
        } //Use item with Left Click
        if (inventoryArray[equippedIndexRight] != null)
        {
            if (inventoryArray[equippedIndexRight].GetComponent<IItem>().isActivelyUsed)
            {
                if (Input.GetButtonDown("Fire2") && activeUseStart)
                {
                    lastUsed.Action(FacingToVector());
                    wasActiveUse = false;
                    lastUsed = null;
                    activeUseStart = false;
                    return;
                }
                if (Input.GetButtonDown("Fire2") && ManaBar.value >= inventoryArray[equippedIndexRight].GetComponent<IItem>().usesMana && !activeUseStart)
                {
                    activeUseStart = true;
                    lastUsed = inventoryArray[equippedIndexRight].GetComponent<IItem>();
                    wasActiveUse = true;
                    inventoryArray[equippedIndexRight].GetComponent<IItem>().Action(FacingToVector());
                    StartCoroutine("ManaDegen", inventoryArray[equippedIndexRight].GetComponent<IItem>().usesMana);
                }
                if (!isActiveBuffer && activeUseStart && ManaBar.value >= inventoryArray[equippedIndexRight].GetComponent<IItem>().usesMana)
                {
                    isActiveBuffer = true;
                    StartCoroutine("ManaDegen", inventoryArray[equippedIndexRight].GetComponent<IItem>().usesMana);
                }
                

            }
            else if (Input.GetButtonDown("Fire2") && ManaBar.value >= inventoryArray[equippedIndexRight].GetComponent<IItem>().usesMana)
            {
                IItem itemLeft = inventoryArray[equippedIndexRight].GetComponent<IItem>();
                itemLeft.Action(FacingToVector());
                ManaBar.value -= itemLeft.usesMana;
                if (itemLeft.stack > 1)
                {
                    itemLeft.stackHolding -= 1;
                    inventorySprites[equippedIndexRight].GetComponentInChildren<Text>().text = "" + itemLeft.stackHolding;
                    if (itemLeft.stackHolding == 0)
                    {
                        inventoryArray[equippedIndexRight] = null;
                        inventorySprites[equippedIndexRight].GetComponent<Image>().sprite = UIMask;
                        inventorySprites[equippedIndexRight].GetComponentInChildren<Text>().text = "";
                    }
                }
            }
        }//Use item with Right Click
        if (wasActiveUse && !isActiveBuffer)
        {
            lastUsed.Action(FacingToVector());
            wasActiveUse = false;
            lastUsed = null;
            activeUseStart = false;
        } //Turn off active use Item
        /*-----------------------ENEMY KNOCKBACK ----------*/
        if (enemyHit)
        {

            Vector3 runTo = transform.position + ((transform.position - enemyTransform) * multiplier);
            float distance = Vector3.Distance(transform.position, enemyTransform);
            if (distance < range)
            {
                agent.SetDestination(runTo);
                agent.stoppingDistance = 0;
                agent.speed = 6;
            }
            else
            {
                agent.enabled = false;
                enemyHit = false;
            }
        }

    }
}
