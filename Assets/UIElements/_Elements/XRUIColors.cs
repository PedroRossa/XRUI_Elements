using UnityEngine;

public class XRUIColors : MonoBehaviour
{
    [Tooltip("If don't have a target, apply on attached gameobject")]
    public Transform target;

    [Header("State Colors")]
    public Color32 normalColor = new Color32(10, 198, 242, 255);
    public Color32 nearColor = new Color32(27, 140, 175, 255);
    public Color32 touchColor = new Color32(17, 100, 128, 255);
    public Color32 selectColor = new Color32(0, 200, 78, 255);
    public Color32 disabledColor = new Color32(100, 100, 100, 255);

    private void OnValidate()
    {
        if (target == null)
            target = transform;

        //Check if the uicolor is used with a Feedback Color script. If it's, refresh the normal color on target
        XRFeedbackColor feedbackColor = GetComponent<XRFeedbackColor>();
        if (feedbackColor != null)
            feedbackColor.RefreshElementColor();
    }

    private void Awake()
    {
        if (target == null)
            target = transform;
    }
}
