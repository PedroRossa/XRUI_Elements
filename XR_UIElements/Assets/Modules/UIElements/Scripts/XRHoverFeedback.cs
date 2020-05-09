using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRHoverFeedback : MonoBehaviour
{
    private List<string> ColliderTypes { get { return new List<string>() { "Box Collider", "Sphere Collider", "Capsule Collider" }; } }
    private List<string> ElementTypes { get { return new List<string>() { "Mesh Renderer", "Sprite Renderer" }; } }

    private string lastColliderType;

    [Dropdown("ColliderTypes")]
    public string colliderType;
    [Dropdown("ElementTypes")]
    public string elementType;

    [ShowIf("IsMeshRenderer")]
    public MeshRenderer meshHoverFeedback;
    [ShowIf("IsSpriteRenderer")]
    public SpriteRenderer spriteHoverFeedback;

    private bool IsMeshRenderer() { return elementType.Equals("Mesh Renderer"); }

    private bool IsSpriteRenderer() { return elementType.Equals("Sprite Renderer"); }

    public bool checkToUpdate = false;

    [Header("Feedback Properties")]
    public Color hoverColor = new Color(0.3f, 0.3f, 0.3f, 1);
    public float hoverScaleMultiplier = 1;

    private Color originalColor;

    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    private void ClearColliders()
    {
        foreach (Collider item in GetComponents<Collider>())
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(item);
            };
        }
    }

    private void AddColliderBySelectedType()
    {
        if (colliderType.Equals("Box Collider"))
        {
            BoxCollider bc = gameObject.AddComponent<BoxCollider>();
            bc.isTrigger = true;
        }
        else if (colliderType.Equals("Sphere Collider"))
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
        }
        else if (colliderType.Equals("Capsule Collider"))
        {
            CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
            cc.isTrigger = true;
        }
    }


    private void SetMeshFeedbackToHover()
    {
        if (meshHoverFeedback != null)
        {
            originalColor = meshHoverFeedback.sharedMaterial.color;
            meshHoverFeedback.sharedMaterial.color = hoverColor;

            meshHoverFeedback.transform.localScale *= hoverScaleMultiplier;
        }
    }

    private void SetMeshFeedbackToOriginal()
    {
        if (meshHoverFeedback != null)
        {
            meshHoverFeedback.sharedMaterial.color = originalColor;

            meshHoverFeedback.transform.localScale /= hoverScaleMultiplier;
        }
    }

    private void SetSpriteFeedbackToHover()
    {
        if (spriteHoverFeedback != null)
        {
            originalColor = spriteHoverFeedback.color;
            spriteHoverFeedback.color = hoverColor;

            spriteHoverFeedback.transform.localScale *= hoverScaleMultiplier;
        }
    }

    private void SetSpriteFeedbackToOriginal()
    {
        if (spriteHoverFeedback != null)
        {
            spriteHoverFeedback.color = originalColor;

            spriteHoverFeedback.transform.localScale /= hoverScaleMultiplier;
        }
    }

    private void Awake()
    {
        onHoverEnter.AddListener(HoverEnterFunction);
        onHoverExit.AddListener(HoverExitFunction);
    }

    private void HoverEnterFunction()
    {
        if (elementType.Equals("Mesh Renderer"))
            SetMeshFeedbackToHover();
        else if (elementType.Equals("Sprite Renderer"))
            SetSpriteFeedbackToHover();
    }

    private void HoverExitFunction()
    {
        if (elementType.Equals("Mesh Renderer"))
            SetMeshFeedbackToOriginal();
        else if (elementType.Equals("Sprite Renderer"))
            SetSpriteFeedbackToOriginal();
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(lastColliderType))
            return;

        if (!lastColliderType.Equals(colliderType))
        {
            lastColliderType = colliderType;
            ClearColliders();
            AddColliderBySelectedType();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            onHoverEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            
            onHoverExit?.Invoke();
        }
    }
}
