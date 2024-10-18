using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

public class TakeToInventory : MonoBehaviour
{   
    public string inventoryName = "Basic Inventory";
    public string itemName;
    public void onClick(){
        InventoryController.instance.AddItem(inventoryName, itemName);
        gameObject.SetActive(false);
    }
}
