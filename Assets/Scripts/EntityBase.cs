using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour {
    [SerializeField] protected Vector2Int size;
    [SerializeField] protected SpriteRenderer[] sprites;

    protected Color originalColor;
    protected Vector3 orignalScale;

    public Vector2Int PPosition
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

    public RectInt Rect
    {
        get
        {
            Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            RectInt rect = new RectInt(position, size - Vector2Int.one);
            return rect;
        }
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
