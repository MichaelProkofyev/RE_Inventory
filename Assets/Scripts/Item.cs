using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public Vector2Int size;
    [SerializeField] SpriteRenderer[] itemSprites;

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

    public void SetSelected(bool isSelected)
    {
        foreach (var sprite in itemSprites)
        {
            sprite.color = isSelected ? Color.green : Color.white;
        }
    }

    public void SetPicked(bool isPicked)
    {
        foreach (var sprite in itemSprites)
        {
            sprite.color = isPicked ? Color.yellow : Color.white;
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
        Position += movementAmount;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void Start()
    {
    }
}
