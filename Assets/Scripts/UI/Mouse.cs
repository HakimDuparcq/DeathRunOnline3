using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public Texture2D mouseIcon;
    void Start()
    {
        Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.ForceSoftware);
    }

    
}
