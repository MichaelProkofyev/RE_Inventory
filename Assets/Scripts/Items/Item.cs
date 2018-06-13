using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState
{
    IDLE,
    HOVERED_OVER,
    PICKED
}

[Serializable]
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

    [SerializeField] private ItemRect useRange;

    public ItemRect UseRange
    { get
        {
            return new ItemRect(Position + useRange.position, useRange.width, useRange.height);
        }
    }

    public abstract void Use();

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
}
