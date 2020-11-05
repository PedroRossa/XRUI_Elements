using UnityEngine;

/// <summary>
/// Class of utilities for sprites of a SpriteRenderer
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpritesUtilities : MonoBehaviour
{
    /// <summary>
    /// Possible sprites to be used
    /// </summary>
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;
    private int spriteIndex = 0;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Set the sprite to the next one in sprites array
    /// </summary>
    public void changeSprite()
    {
        spriteIndex = (spriteIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
