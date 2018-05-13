using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : SingletonComponent<Cursor> {

    [SerializeField] SpriteRenderer[] sprites;

    public Vector2Int Position
    {
        get
        {
            return new Vector2Int((int)transform.position.x, (int)transform.position.y);
        }
        set
        {
            if (value.x >= Inventory.Instance.slots.GetLength(0))
            {
                value.x = 0;
            }
            else if(value.x < 0)
            {
                value.x = Inventory.Instance.slots.GetLength(0) - 1;
            }

            if (value.y >= Inventory.Instance.slots.GetLength(1))
            {
                value.y = 0;
            }
            else if (value.y < 0)
            {
                value.y = Inventory.Instance.slots.GetLength(1) - 1;
            }

            transform.position = new Vector3Int( value.x, value.y, 0);
        }
    }

    private void Start()
    {
        Position = Vector2Int.zero;
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
        Position += movementAmount;
    }


    public void SetVisible(bool isVisible)
    {
        foreach (var sprite in sprites)
        {
            sprite.enabled = isVisible;
        }
    }
}
