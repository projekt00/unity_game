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

    public Text ObjectInfo;
    public RectTransform background;
    public float rayDistance = 2f;
    private bool holdingOnObject = false;

    private UniversalObjectController hittedObj;
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
                    hittedObj = hit.collider.GetComponent<UniversalObjectController>();
                    background.gameObject.SetActive(true);
                    ObjectInfo.text = hittedObj.mouseHoverInfo;
                    Crosshair.color = interactColor;

                    if (Input.GetMouseButtonDown(0)){
                        if (hit.collider.GetComponent<UniversalObjectController>().interactionType == UniversalObjectController.InteractionType.Clickable){
                            hittedObj.onClick.Invoke();
                        } else {
                            holdingOnObject = true;
                            hittedObj.onHoldStart.Invoke();
                        }
                    }
                } else {
                    //Debug.Log("Object");
                    background.gameObject.SetActive(false);
                    ObjectInfo.text = "";
                    Crosshair.color = defaultColor;
                }
            } else {
                //Debug.Log("No object");
                background.gameObject.SetActive(false);
                ObjectInfo.text = "";
                Crosshair.color = defaultColor;
            }
        } else {
            background.gameObject.SetActive(false);
            ObjectInfo.text = "";
            Crosshair.color = defaultColor;
            if (Input.GetMouseButtonUp(0)){
                    holdingOnObject = false;
                    hittedObj.onHoldEnd.Invoke();
                    hittedObj = null;
                } else {
                    hittedObj.onHold.Invoke();
                }
        }
    }
}

