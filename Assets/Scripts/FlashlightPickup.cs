using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPickup : MonoBehaviour
{
    public GameObject FlashlightOnPlayer;

    void Start()
    {
        FlashlightOnPlayer.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.gameObject.SetActive(false);
                FlashlightOnPlayer.SetActive(true);
            }
        }
    }
}
