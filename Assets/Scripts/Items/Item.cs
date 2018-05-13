using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState
{
    IDLE,
    HOVERED_OVER,
    PICKED
}

public abstract class Item : EntityBase {

    public abstract void Use();

    public void SetState(ItemState state)
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

    public void Move(MoveDirection direction)
    {
        Vector2Int movementAmount = Vector2Int.zero;
        switch (direction)
        {
            case MoveDirection.UP:
                movementAmount = Vector2Int.up;
                break;
            case MoveDirection.DOWN:
                movementAmount = Vector2Int.down;
                break;
            case MoveDirection.LEFT:
                movementAmount = Vector2Int.left;
                break;
            case MoveDirection.RIGHT:
                movementAmount = Vector2Int.right;
                break;
        }
        PPosition += movementAmount;
    }
}
