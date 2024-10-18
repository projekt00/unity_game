using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

public class Dismantle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectAfterDismantling = null;
    public GameObject dismantleEffect;
    public string requiredTool = null;
    private bool requiredObjects = true;
    void Start()
    {
        if (objectAfterDismantling == null){
            requiredObjects = false;
        }
    }

    // Update is called once per frame
    public void onClick(){
        bool dismantlingPossible = true;
        if (requiredTool != null && requiredObjects){
            bool isInInventory = InventoryController.instance.isInInventory("Basic Inventory", requiredTool);
            if (isInInventory == false){
                dismantlingPossible = false;
            }
        }
        if (dismantlingPossible){
            gameObject.SetActive(false);
            dismantleEffect.GetComponent<ParticleSystem>().Play();
            objectAfterDismantling.SetActive(true);
        }
    }
}
