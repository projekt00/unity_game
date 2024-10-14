using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    // Start is called before the first frame update
    public Image Crosshair;
    public Color defaultColor = new Color(27f / 255f, 20f / 255f, 200f / 255f, 0.8f);
    public Color interactColor = new Color(40f / 255f, 160f / 255f, 30f / 255f, 0.8f);
    public float rayDistance = 2f;
    private GameObject grabbedObj = null;
    void Start()
    {
        Crosshair.color = defaultColor;
    }

    // Update is called once per frame
    void Update(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (grabbedObj == null) {
            if (Physics.Raycast(ray, out hit, rayDistance)){
                if (hit.collider.CompareTag("Interactive")){
                    //Debug.Log("Interactive object");
                    Crosshair.color = interactColor;

                    if (Input.GetMouseButtonDown(0)){
                        grabbedObj = hit.collider.gameObject;
                        Physics.IgnoreCollision(grabbedObj.GetComponent<Collider>(), grabbedObj.GetComponent<Collider>(), true);
                        grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
                    }
                } else {
                    //Debug.Log("Object");
                    Crosshair.color = defaultColor;
                }
            } else {
                //Debug.Log("No object");
                Crosshair.color = defaultColor;
            }
        } else {
            Crosshair.color = defaultColor;
            if (Input.GetMouseButtonUp(0)){
                    grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
                    Physics.IgnoreCollision(grabbedObj.GetComponent<Collider>(), grabbedObj.GetComponent<Collider>(), false);
                    grabbedObj = null;
                }
        }
    }
    void FixedUpdate(){
        if (grabbedObj != null){
            MoveGrabbedObj();
        }
    }

    void MoveGrabbedObj(){
        Vector3 newPosition = Camera.main.transform.position + Camera.main.transform.forward * rayDistance;

        Renderer renderer = grabbedObj.GetComponent<Renderer>();
        Vector3 boxSize = renderer != null ? renderer.bounds.size : transform.localScale; // Get the box size for collision detection

        Collider[] hitColliders = Physics.OverlapBox(newPosition, boxSize / 2, Quaternion.identity); // Check for collisions
        foreach (Collider collider in hitColliders){
            if (collider.gameObject != grabbedObj){ // Stop moving if there is a collision with another object
                return; 
            }
        }

        Vector3 direction = newPosition - grabbedObj.transform.position;
        float distance = direction.magnitude;

        if (Physics.Raycast(grabbedObj.transform.position, direction.normalized, distance)){ //Stop moving if there is no collision in the new position but there is an object on a way to the new position
            return;
        }

        float t = Mathf.Clamp(Time.deltaTime * 10f, 0, 1);
        grabbedObj.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(grabbedObj.transform.position, newPosition, t));
    }
}

