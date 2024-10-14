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
    private bool holdingOnObject = false;
    void Start()
    {
        Crosshair.color = defaultColor;
    }

    // Update is called once per frame
    void Update(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (holdingOnObject == false) {
            if (Physics.Raycast(ray, out hit, rayDistance)){
                if (hit.collider.CompareTag("Interactive")){
                    //Debug.Log("Interactive object");
                    Crosshair.color = interactColor;

                    if (Input.GetMouseButtonDown(0)){
                        holdingOnObject = true;
                        hit.collider.GetComponent<InteractiveObjects>().OnInteract();
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
                    holdingOnObject = false;
                }
        }
    }
}

