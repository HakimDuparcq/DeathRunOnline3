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
    private void Update()
    {
        
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
    }
}
