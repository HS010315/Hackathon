using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicBomb : MonoBehaviour
{
    public PlayerStateInfo playerStateInfo;

    private void Start()
    {
        playerStateInfo = FindObjectOfType<PlayerStateInfo>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerStateInfo.Hp = 0;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject,10f);
        }
    }
}
