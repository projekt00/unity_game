using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float rayDistance = 2f;
    private bool isGrabbed = false;
    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        if (isGrabbed){
            if(Input.GetMouseButtonUp(0)){
                isGrabbed = false;
                rb.isKinematic = false;
            }
        }
    }
    void FixedUpdate(){
        if (isGrabbed == true){
            MoveGrabbedObj();
        }
    }
    public void OnInteract(){
        isGrabbed = true;
        rb.isKinematic = true;
    }

    public void MoveGrabbedObj(){
        Vector3 newPosition = Camera.main.transform.position + Camera.main.transform.forward * rayDistance;

        Renderer renderer = GetComponent<Renderer>();
        Vector3 boxSize = renderer != null ? renderer.bounds.size : transform.localScale; // Get the box size for collision detection

        
        Collider[] hitColliders = Physics.OverlapBox(newPosition, boxSize / 2, Quaternion.identity); // Check for collisions
        foreach (Collider collider in hitColliders){
            if (collider.gameObject != this.gameObject){ // Stop moving if there is a collision with another object
                //Debug.Log("First collison");
                return; 
            }
        }

        Vector3 direction = newPosition - transform.position;
        float distance = direction.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, distance)){ //Stop moving if there is no collision in the new position but there is an object on a way to the new position
            //Debug.Log("Second collision");
            if (hit.collider.gameObject != this.gameObject){
                return;
            }
        }

        float t = Mathf.Clamp(Time.deltaTime * 10f, 0, 1);
        GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(transform.position, newPosition, t));
    }
}
