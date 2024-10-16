using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoBackgroundController : MonoBehaviour
{
    public Text objectInfo;
    public RectTransform background;
    
    private void Update()
    {   
        background.gameObject.SetActive(true);
        Vector2 textSize = new Vector2(objectInfo.preferredWidth, objectInfo.preferredHeight);
        background.sizeDelta = textSize + new Vector2(20, 10);
    }
}

