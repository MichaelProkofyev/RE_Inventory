using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour {
    public Vector2Int size;
    [SerializeField] protected SpriteRenderer[] sprites;

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

    private void Start()
    {
        foreach (var sprite in sprites)
        {
            originalColor = sprite.color;
            orignalScale = sprite.transform.localScale;
        }
    }
}
