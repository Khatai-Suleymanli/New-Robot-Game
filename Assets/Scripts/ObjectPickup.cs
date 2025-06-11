using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public float pickUpRange = 10f;
    public KeyCode pickupKey = KeyCode.E;
    public Transform TargetPosition; // where to keep object after picking it up
    private GameObject pickupTarget;




    void Start()
    {
        //FlashLight.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(pickupKey)) {
            if (pickupTarget == null)
            {
                //PICKUP
                PickUpObject();
            }
            else { 
                //drop object
            }
        
        }
    }

    void PickUpObject() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(1f, 1f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange)) {
            if (hit.collider.CompareTag("Pickup")) {
                pickupTarget = hit.collider.gameObject;
                pickupTarget.transform.position = TargetPosition.position;
                pickupTarget.transform.parent = TargetPosition;
            }
        
        }
    
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.gameObject.SetActive(false);
                //FlashLight.SetActive(true);
            }
        }
    }
}