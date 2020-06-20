using TMPro;
using UnityEngine;

public class XRTextButton : XRButton
{
    private TextMeshPro tmpField;

    [Header("Properties")]
    public string txtValue;
    public int fontSize = 20;

    //Runs only in editor
    protected override void OnValidate()
    {
        base.OnValidate();

        if (tmpField == null)
            tmpField = GetComponentInChildren<TextMeshPro>();

        if (tmpField != null)
        {
            tmpField.text = txtValue;
            tmpField.fontSize = fontSize;
        }
        else
        {
            Debug.LogError("This script need to be applied in a XRTextButton Prefab.");
        }
    }

    private void Awake()
    {
        if (tmpField == null)
            tmpField = GetComponentInChildren<TextMeshPro>();

        tmpField.text = txtValue;
        tmpField.fontSize = fontSize;
    }
}
