using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    PlayerMovement player;

    private void Start()
    {
        Invoke("OneSecond", 1.2f);
    }

    bool isOneSecondIn = false;
    bool firstTrigger = true;

    public void FirstAirlockHiss()
    {
        if (!firstTrigger)
            player.DoorHiss();
    }

    public void DoorHiss()
    {
        if (firstTrigger)
            firstTrigger = false;

        if (isOneSecondIn)
            player.DoorHiss();
    }

    private void OneSecond()
    {
        isOneSecondIn = true;
    }
}