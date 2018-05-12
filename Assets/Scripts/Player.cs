using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public enum SelectionState
{
    EMPTY,
    HOVERED_OVER,
    ITEM_PICKED
}

public class Player : MonoBehaviour {

    [SerializeField] private Item currentItem;
    [SerializeField] private SelectionState selectionState;

    [SerializeField] private Inventory inventory;

    private void Start()
    {
    }


    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleInputDirection(MoveDirection.UP);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleInputDirection(MoveDirection.DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleInputDirection(MoveDirection.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleInputDirection(MoveDirection.RIGHT);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            switch (selectionState)
            {
                case SelectionState.HOVERED_OVER:
                    selectionState = SelectionState.ITEM_PICKED;
                    currentItem.SetState(Item.State.PICKED);
                    break;
                case SelectionState.ITEM_PICKED:
                    var possibleOverlappingItem = inventory.Overlapping(currentItem);
                    if (possibleOverlappingItem != null)
                    {
                        currentItem.SetState(Item.State.IDLE);
                        currentItem = possibleOverlappingItem;
                        currentItem.SetState(Item.State.PICKED);
                    }
                    else
                    {
                        currentItem.SetState(Item.State.HOVERED_OVER);
                        selectionState = SelectionState.HOVERED_OVER;
                    }

                    inventory.UpdateItemsPositions();
                    break;
            }
        }
    }

    private void HandleInputDirection(MoveDirection direction)
    {
        switch (selectionState)
        {
            case SelectionState.EMPTY:
                Cursor.Instance.Move(direction);
                var itemAtCursor = inventory.At(Cursor.Instance.Position.x, Cursor.Instance.Position.y);
                if (itemAtCursor != null)
                {
                    itemAtCursor.SetState(Item.State.HOVERED_OVER);
                    selectionState = SelectionState.HOVERED_OVER;
                    currentItem = itemAtCursor;
                    Cursor.Instance.SetVisible(false);
                }
                break;
            case SelectionState.HOVERED_OVER:
                //TODO: Check if there is space for cursor

                switch (direction)
                {
                    case MoveDirection.UP:
                        Cursor.Instance.Position = new Vector2Int(currentItem.PPosition.x, currentItem.Rect.yMax + 1);
                        break;
                    case MoveDirection.DOWN:
                        Cursor.Instance.Position = new Vector2Int(currentItem.PPosition.x, currentItem.Rect.yMin - 1);
                        break;
                    case MoveDirection.LEFT:
                        Cursor.Instance.Position = new Vector2Int(currentItem.Rect.xMin - 1, currentItem.PPosition.y);
                        break;
                    case MoveDirection.RIGHT:
                        Cursor.Instance.Position = new Vector2Int(currentItem.Rect.xMax + 1, currentItem.PPosition.y);
                        break;
                }

                selectionState = SelectionState.EMPTY;
                Cursor.Instance.SetVisible(true);
                currentItem.SetState(Item.State.IDLE);
                currentItem = null;
                break;
            case SelectionState.ITEM_PICKED:
                //Check if item can be moved
                if (inventory.CanItemBeMoved(currentItem, direction))
                {
                    currentItem.Move(direction);
                }

                break;
        }
    }
}
