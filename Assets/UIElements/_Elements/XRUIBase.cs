using UnityEngine;

[RequireComponent(typeof(XRUIColors))]
public abstract class XRUIBase : MonoBehaviour
{
    public bool isEnabled = true;

    protected XRUIColors uiColors;
    protected AudioSource audioSource;

    protected virtual void OnValidate()
    {
        uiColors = GetComponent<XRUIColors>();
    }

    protected virtual void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        uiColors = GetComponent<XRUIColors>();
    }

    //TODO: Criar aqui todas as ações pra lidar com o audioSoruce, assim não precisa das referencias nas herancas
    //TODO: Criar um sistema de templates de cor
}
