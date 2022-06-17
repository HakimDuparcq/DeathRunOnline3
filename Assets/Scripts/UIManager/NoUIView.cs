using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoUIView : View
{
    public override void Initialize()
    {

    }

    public void Update()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            ViewManager.Show<EscapeMenuView>();
        }
    }
}
