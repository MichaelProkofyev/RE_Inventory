using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState
{
    IDLE,
    HOVERED_OVER,
    PICKED
}

public struct ItemRect
{
    public Vector2Int position;
    public int width;
    public int height;

    public ItemRect(Vector2Int position, int width, int height)
    {
        this.position = position;
        this.width = width;
        this.height = height;
    }
}

public abstract class Item : EntityBase {

    public abstract void Use();

    protected bool flippedX = false;
    protected bool flippedY = false;

    public abstract ItemRect UseRange { get; }

    public virtual void SetState(ItemState state)
    {
        foreach (var sprite in sprites)
        {
            switch (state)
            {
                case ItemState.IDLE:
                    sprite.color = originalColor;
                    sprite.transform.localScale = orignalScale;
                    break;
                case ItemState.HOVERED_OVER:
                    sprite.color = Color.Lerp(originalColor, Color.white, .5f);
                    sprite.transform.localScale = orignalScale;
                    break;
                case ItemState.PICKED:
                    sprite.color = Color.white;
                    sprite.transform.localScale = orignalScale * 1.1f;
                    break;
            }
        }
    }

    public virtual void ToggleFlipX()
    {
        flippedX = !flippedX;
        Flip(flippedX, flippedY);
    }

    public virtual void ToggleFlipY()
    {
        flippedY = !flippedY;
        Flip(flippedX, flippedY);
    }

    public virtual void Flip(bool flipX, bool flipY)
    {
        flippedX = flipX;
        flippedY = flipY;
        foreach (var sprite in sprites)
        {
            sprite.flipX = flippedX;
            sprite.flipY = flippedY;
        }
    }
}
