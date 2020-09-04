using UnityEngine;

public class SpritesUtilities : MonoBehaviour
{
    public Sprite[] sprites;

    private XRUI_3DButtonSprite Button_mainSprite;
    private int index = 0;

    private void Start()
    {
        Button_mainSprite = gameObject.GetComponent<XRUI_3DButtonSprite>();
    }
    public void changeSprite()
    {
        if (Button_mainSprite.canActiveButton)
        {
            Button_mainSprite.iconSprite.sprite = sprites[index];
            index = (index + 1) % sprites.Length;
        }
    }
}
