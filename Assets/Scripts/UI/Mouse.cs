using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse : MonoBehaviour
{
    public Texture2D mouseIcon;
    void Start()
    {
        Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            AudioManager.instance.Play("Click");
        }
    }
}
