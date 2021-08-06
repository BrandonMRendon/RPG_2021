using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Sprite cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public bool cursorVisible = true;
    void Start()
    {
        Cursor.SetCursor(cursorTexture.texture, hotSpot, cursorMode);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (cursorVisible)
            {
                Cursor.visible = false;
                cursorVisible = false;
            }
            else
            {
                Cursor.visible = true;
                cursorVisible = true;
            }
        }
    }


}
