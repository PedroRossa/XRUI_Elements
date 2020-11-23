using UnityEditor;
using UnityEngine;

public class XRUI_MenuBar : MonoBehaviour
{
    #region 2D Elements

    [MenuItem("GameObject/XRUI_Elements/2DElements/Button Sprite 2D", false, 0)]
    static void ButtonSprite2D()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.ButtonSprite2D) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/2DElements/Button Sprite 2D", true)]
    static bool ValidateButtonSprite2D()
    {
        GameObject go = XRUI_Prefabs.Instance.ButtonSprite2D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/2DElements/Button Text 2D", false, 0)]
    static void ButtonText2D()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.ButtonText2D) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/2DElements/Button Text 2D", true)]
    static bool ValidateButtonText2D()
    {
        GameObject go = XRUI_Prefabs.Instance.ButtonText2D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/2DElements/Progress Bar 2D", false, 0)]
    static void ButtonProgressBar2D()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.ProgressBar2D) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/2DElements/Progress Bar 2D", true)]
    static bool ValidateProgressBar2D()
    {
        GameObject go = XRUI_Prefabs.Instance.ProgressBar2D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/2DElements/Toggle 2D", false, 0)]
    static void Toggle2D()
    {
        GameObject go = (PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.Toggle2D)) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
        //Selection.activeObject = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.Toggle2D );
    }

    [MenuItem("GameObject/XRUI_Elements/2DElements/Toggle 2D", true)]
    static bool ValidateToggle2D()
    {
        GameObject go = XRUI_Prefabs.Instance.Toggle2D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    #endregion

    #region 3D Elements

    [MenuItem("GameObject/XRUI_Elements/3DElements/ButtonSprite 3D", false, 0)]
    static void ButtonSprite3D()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.ButtonSprite3D) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;

    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/ButtonSprite 3D", true)]
    static bool ValidateButtonSprite3D()
    {
        GameObject go = XRUI_Prefabs.Instance.ButtonSprite3D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/ButtonText 3D", false, 0)]
    static void ButtonText3D()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.ButtonText3D) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;

    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/ButtonText 3D", true)]
    static bool ValidateButtonText3D()
    {
        GameObject go = XRUI_Prefabs.Instance.ButtonSprite3D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/ProgressBar 3D", false, 0)]
    static void ProgressBar3D()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.ProgressBar3D) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;

    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/ProgressBar 3D", true)]
    static bool ValidateProgressBar3D()
    {
        GameObject go = XRUI_Prefabs.Instance.ProgressBar3D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/Slider 3D", false, 0)]
    static void Slider3D()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.Slider3D) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/Slider 3D", true)]
    static bool ValidateSlider3D()
    {
        GameObject go = XRUI_Prefabs.Instance.Slider3D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/SliderBox 3D", false, 0)]
    static void SliderBox3D()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.SliderBox3D) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/SliderBox 3D", true)]
    static bool ValidateSliderBox3D()
    {
        GameObject go = XRUI_Prefabs.Instance.SliderBox3D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/Toggle 3D", false, 0)]
    static void Toggle3D()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.Toggle3D) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;

    }

    [MenuItem("GameObject/XRUI_Elements/3DElements/Toggle 3D", true)]
    static bool ValidateToggle3D()
    {
        GameObject go = XRUI_Prefabs.Instance.Toggle3D;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    #endregion

    #region General Elements

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Manipulable", false, 0)]
    static void Manipulable()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.Manipulable) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Manipulable", true)]
    static bool ValidateManipulable()
    {
        GameObject go = XRUI_Prefabs.Instance.Manipulable;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Color Sprite", false, 0)]
    static void FeedbackColorSprite()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.FeedbackColorSprite) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Color Sprite", true)]
    static bool ValidateFeedbackColorSprite()
    {
        GameObject go = XRUI_Prefabs.Instance.FeedbackColorSprite;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Color Mesh", false, 0)]
    static void FeedbackColorMesh()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.FeedbackColorMesh) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Color Mesh", true)]
    static bool ValidateFeedbackColorMesh()
    {
        GameObject go = XRUI_Prefabs.Instance.FeedbackColorMesh;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Color Outline", false, 0)]
    static void FeedbackColorOutline()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.FeedbackColorOutline) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Color Outline", true)]
    static bool ValidateFeedbackColorOutline()
    {
        GameObject go = XRUI_Prefabs.Instance.FeedbackColorOutline;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Sound", false, 0)]
    static void FeedbackSound()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.FeedbackSound) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Sound", true)]
    static bool ValidateFeedbackSound()
    {
        GameObject go = XRUI_Prefabs.Instance.FeedbackSound;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Haptics", false, 0)]
    static void FeedbackHaptics()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(XRUI_Prefabs.Instance.FeedbackHaptics) as GameObject;
        go.transform.parent = Selection.activeTransform;
        go.transform.localPosition = Vector3.zero;
    }

    [MenuItem("GameObject/XRUI_Elements/GeneralElements/Feedback Haptics", true)]
    static bool ValidateFeedbackHaptics()
    {
        GameObject go = XRUI_Prefabs.Instance.FeedbackHaptics;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    #endregion
}
