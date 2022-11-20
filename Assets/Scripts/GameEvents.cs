using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{

    public static GameEvents current;

    void Awake()
    {
        current = this;    
    }

    public event Action<int> onDoorwayTriggerEnter;

    public void DoorwayTriggerEnter(int id) {
        if (onDoorwayTriggerEnter != null) {
            onDoorwayTriggerEnter(id);
        }
    }

    public event Action<int> onDoorwayTriggerClose;
    public void DoorwayTriggerClose(int id) {
        if (onDoorwayTriggerClose != null) {
            onDoorwayTriggerClose(id);
        }
    }


}
