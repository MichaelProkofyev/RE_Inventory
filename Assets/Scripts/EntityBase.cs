using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour, System.IComparable<EntityBase>
{
    public Vector2Int size;
    [SerializeField] protected SpriteRenderer[] sprites;
    [SerializeField] protected ObjectShake[] objectShakers;

    protected Color originalColor;
    protected Vector3 orignalScale;

    public Vector2Int Position
    {
        get
        {
            return new Vector2Int((int)transform.position.x, (int)transform.position.y);
        }
        set
        {
            transform.position = new Vector3Int(value.x, value.y, 0);
        }
    }

    public int yMax
    {
        get { return Position.y + size.y - 1; }
    }

    public int yMin
    {
        get { return Position.y; }
    }

    public int xMax
    {
        get { return Position.x + size.x - 1; }
    }

    public int xMin
    {
        get { return Position.x; }
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

    public int CompareTo(EntityBase other)
    {
        if (Position.x.CompareTo(other.Position.x) != 0)
        {
            return Position.x.CompareTo(other.Position.x);
        }
        else
        {
            return Position.y.CompareTo(other.Position.y);
        }
    }

    protected virtual void Start()
    {
        foreach (var sprite in sprites)
        {
            originalColor = sprite.color;
            orignalScale = sprite.transform.localScale;
        }

        objectShakers = GetComponentsInChildren<ObjectShake>();
    }
}
