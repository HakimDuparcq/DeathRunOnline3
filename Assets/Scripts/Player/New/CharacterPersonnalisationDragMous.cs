using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPersonnalisationDragMous : MonoBehaviour
{
    public void OnMouseDrag()
    {

        transform.Rotate(Vector3.down, Input.GetAxis("Mouse X") * 1.6f);

    }
}
