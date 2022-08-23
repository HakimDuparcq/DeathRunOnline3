using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableLink : MonoBehaviour
{
    public string link = "https://youtu.be/52-gpawpsFA?t=643";
    public void OpenLink()
    {
        Application.OpenURL(link);
    }
}
