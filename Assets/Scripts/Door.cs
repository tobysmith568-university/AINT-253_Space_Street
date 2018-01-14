﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    PlayerMovement player;

    public void DoorHiss()
    {
        player.DoorHiss();
    }
}