using UnityEngine;

[CreateAssetMenu(fileName = "XRUI_Prefabs", menuName = "XRUI_Elements/XRUI_Prefabs")]
public class XRUI_Prefabs : ScriptableObject
{
    //Set instance variable upon creation
    public XRUI_Prefabs()
    {
        Instance = this;
    }

    public static XRUI_Prefabs Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        Instance = Resources.Load<XRUI_Prefabs>("XRUI_Prefabs");
    }


    #region 2D Elements

    [Header("2D Element Prefabs")]
    public GameObject ButtonSprite2D;
    public GameObject ButtonText2D;
    public GameObject ProgressBar2D;
    public GameObject Toggle2D;

    #endregion

    #region 3D Elements

    [Header("3D Element Prefabs")]
    public GameObject ButtonSprite3D;
    public GameObject ButtonText3D;
    public GameObject ProgressBar3D;
    public GameObject Slider3D;
    public GameObject SliderBox3D;
    public GameObject Toggle3D;

    #endregion

    #region Other Elements

    [Header("Other Element Prefabs")]
    public GameObject Manipulable;
    public GameObject FeedbackColorSprite;
    public GameObject FeedbackColorMesh;
    public GameObject FeedbackColorOutline;
    public GameObject FeedbackSound;
    public GameObject FeedbackHaptics;

    #endregion

}

