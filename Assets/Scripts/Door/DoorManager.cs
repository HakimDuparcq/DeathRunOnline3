using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class DoorManager : NetworkBehaviour
{
    public static DoorManager instance;
    public Door[] Doors;
    void Start()
    {
        instance = this;
    }

    [ClientRpc]
    public void RpcOnOpenDoor()
    {
        OpenDoorNumber(1);
    }

    public void OpenDoorNumber(int number)
    {
        for (int i = 0; i < Doors.Length; i++)
         {
             if (number==Doors[i].DoorNumber)
             {
                 Doors[i].lanimator.SetBool("open", true);
             }
         }

        AudioManager.instance.Play("DoorOpen");
    }

    public void CloseDoorNumber(int number)
    {
        for (int i = 0; i < Doors.Length; i++)
        {
            if (number == Doors[i].DoorNumber)
            {
                Doors[i].lanimator.SetBool("open", false);
            }
        }

        AudioManager.instance.Play("DoorOpen");

    }



}
