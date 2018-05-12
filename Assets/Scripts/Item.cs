using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public enum State
    {
        IDLE,
        HOVERED_OVER,
        PICKED
    }

    [SerializeField] private Vector2Int size;
    [SerializeField] SpriteRenderer[] itemSprites;

    private Color originalColor;
    private Vector3 orignalScale;

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

    public void SetState(State state)
    {
        foreach (var sprite in itemSprites)
        {
            switch (state)
            {
                case State.IDLE:
                    sprite.color = originalColor;
                    sprite.transform.localScale = orignalScale;
                    break;
                case State.HOVERED_OVER:
                    sprite.color = Color.Lerp(originalColor, Color.white, .5f);
                    sprite.transform.localScale = orignalScale;
                    break;
                case State.PICKED:
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

    // Update is called once per frame
    void Update () {
		
	}

    private void Start()
    {
        foreach (var sprite in itemSprites)
        {
            originalColor = sprite.color;
            orignalScale = sprite.transform.localScale;
        }
    }
}
