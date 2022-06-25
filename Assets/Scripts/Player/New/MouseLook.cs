using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLook : MonoBehaviour
{
    public Camera Cam; 
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            Cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX);
        }



        if (Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked)  //Unlock cursor after pressing echap
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.None)
        {
            StartCoroutine(HideMouseOnClick());
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            StartCoroutine(HideMouseOnClick());
            Debug.Log("hide");
        }
        
    }

    IEnumerator HideMouseOnClick()
    {
        yield return new WaitForSeconds(0.1f);
        Cursor.lockState = CursorLockMode.Locked;
    }
    IEnumerator ShowMouseOnClick()
    {
        yield return new WaitForSeconds(0.1f);
        Cursor.lockState = CursorLockMode.None;
    }

}
