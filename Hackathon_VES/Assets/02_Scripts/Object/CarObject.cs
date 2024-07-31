using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObject : MonoBehaviour, IInteractable
{
    public Transform drivePosition;
    public PlayerStateInfo playerStateInfo;
    public GameObject driver;

    private bool inCar = false;
    private Vector3 interactPosition;

    public void Interact()
    {
        if (!inCar)
        {
            interactPosition = driver.transform.position;
            inCar = true;
            GoInCar();
        }
        else
        {
            inCar = false;
            GoOutCar();
        }
    }

    void GoInCar()
    {
        driver.transform.position = drivePosition.position;
        playerStateInfo.ChangeState(PlayerState.Sitting);

        PlayerController playerController = driver.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetMoveable(false);
        }
    }

    void GoOutCar()
    {
        driver.transform.position = interactPosition;
        playerStateInfo.ChangeState(PlayerState.Idle);

        PlayerController playerController = driver.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetMoveable(true);
        }
    }
}