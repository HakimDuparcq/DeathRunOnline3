using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairs : MonoBehaviour
{
    public Image ImgCrossLeft;
    public Image ImgCrossRight;

    public Color normalColor;
    public Color playerColor;

    public Animator CrossHairLeft;
    public Animator CrossHairRight;

    public void CrossHairsActivationAnim()
    {

            CrossHairLeft.SetTrigger("active");
            CrossHairRight.SetTrigger("active");
        
    }

    public void CrossHairsActivationColor(bool onPlayer)
    {
        if (onPlayer)
        {
            ImgCrossLeft.color = playerColor;
            ImgCrossRight.color = playerColor;
        }
        else
        {
            ImgCrossLeft.color = normalColor;
            ImgCrossRight.color = normalColor;
        }
    }
}
