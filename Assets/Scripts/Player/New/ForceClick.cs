using UnityEngine;
using UnityEngine.EventSystems;

public class ForceClick : StandaloneInputModule
{
    [SerializeField] private RectTransform fakeCursor = null;

    private float moveSpeed = 5f;

    public void ClickAt(Vector2 pos, bool pressed)
    {
        Input.simulateMouseWithTouches = true;
        var pointerData = GetTouchPointerEventData(new Touch()
        {
            position = pos,
        }, out bool b, out bool bb);

        ProcessTouchPress(pointerData, pressed, !pressed);
    }


    public void Update()
    {
        OnForceClick();
    }

    public void OnForceClick()
    {
        ClickAt(fakeCursor.position, true);
        
    }

}