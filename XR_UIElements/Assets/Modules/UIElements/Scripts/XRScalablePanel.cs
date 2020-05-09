using UnityEngine;

public class XRScalablePanel : MonoBehaviour
{
    [Header("Internal Properties")]
    public XRDragableElement[] dragElements;
    public XRScalableElement[] scalableElements;

    ////Runs only in editor
    //private void OnValidate()
    //{
    //    foreach (Transform item in dragElements)
    //    {
    //        item.GetComponent<XRDragableElement>();

    //        Vector3 scale = item.localScale;

    //        BoxCollider bxCollider = item.GetComponent<BoxCollider>();
    //        bxCollider.size = new Vector3(scale.x + dragAreaSize, dragAreaSize, dragAreaSize);
    //    }
    //}

    private void Awake()
    {
        dragElements = GetComponentsInChildren<XRDragableElement>();
        scalableElements = GetComponentsInChildren<XRScalableElement>();
    }
}
