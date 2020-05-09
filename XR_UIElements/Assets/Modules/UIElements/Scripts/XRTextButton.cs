using TMPro;
using UnityEngine;

public class XRTextButton : XRButton
{
    TextMeshProUGUI tmpField;

    [Header("Properties")]
    string txtValue;    

    //Runs only in editor
    private void OnValidate()
    {
        if(tmpField != null)
        {
            tmpField = GetComponentInChildren<TextMeshProUGUI>();
            tmpField.text = txtValue;
        }
        else
        {
            Debug.LogError("This script need to be applied in a XRTextButton Prefab.");
        }
    }
}
