
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

public class Player : SingletonComponent<Player> {

    [SerializeField] private Item currentItem;
    [SerializeField] private SelectionState selectionState;

    [SerializeField] private Inventory inventory;

    private void Start()
    {
    }

    public Item CurrentlyHoldingItem()
    {
        if (selectionState == SelectionState.ITEM_PICKED && currentItem != null)
        {
            return currentItem;
        }
        return null;
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

        if (currentItem != null)
        {
            if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                currentItem.ToggleFlipY();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                currentItem.ToggleFlipX();
            }
        }

        System.Func<Item> dropItem = () =>
        {
            Item droppedItem = currentItem;

            var possibleOverlappingItem = inventory.Overlapping(currentItem);
            if (possibleOverlappingItem != null)
            {
                currentItem.SetState(ItemState.IDLE);
                currentItem = possibleOverlappingItem;
                currentItem.SetState(ItemState.PICKED);
            }
            else
            {
                currentItem.SetState(ItemState.HOVERED_OVER);
                selectionState = SelectionState.HOVERED_OVER;
            }

            inventory.UpdateItemsPositions();
            return droppedItem;
            //inventory.PrintInventoryContents();
        };

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            switch (selectionState)
            {
                case SelectionState.HOVERED_OVER:
                    selectionState = SelectionState.ITEM_PICKED;
                    currentItem.SetState(ItemState.PICKED);
                    Inventory.Instance.ShowInteractionRangeTilesAt(currentItem.UseRange);
                    break;
                case SelectionState.ITEM_PICKED:
                    Inventory.Instance.HideInteractionRangeTiles();
                    dropItem();
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            switch (selectionState)
            {
                case SelectionState.HOVERED_OVER:
                    currentItem.Use();
                    break;
                case SelectionState.ITEM_PICKED:
                    var droppedItem = dropItem();
                    droppedItem.Use();
                    break;
            }
        }
    }

    private void HandleInputDirection(MoveDirection direction)
    {
        System.Func<bool> tryHoverOverItem = () =>
        {
            var itemAtCursor = inventory.At(Cursor.Instance.Position.x, Cursor.Instance.Position.y) as Item;
            if (itemAtCursor != null)
            {
                itemAtCursor.SetState(ItemState.HOVERED_OVER);
                selectionState = SelectionState.HOVERED_OVER;
                currentItem = itemAtCursor;
                Cursor.Instance.SetVisible(false);
                return true;
            }
            return false;
        };

        switch (selectionState)
        {
            case SelectionState.EMPTY:
                Cursor.Instance.Move(direction);
                tryHoverOverItem();
                break;
            case SelectionState.HOVERED_OVER:
                switch (direction)
                {
                    case MoveDirection.UP:
                        Cursor.Instance.Position = new Vector2Int(currentItem.Position.x, currentItem.yMax + 1);
                        break;
                    case MoveDirection.DOWN:
                        Cursor.Instance.Position = new Vector2Int(currentItem.Position.x, currentItem.yMin - 1);
                        break;
                    case MoveDirection.LEFT:
                        Cursor.Instance.Position = new Vector2Int(currentItem.xMin - 1, currentItem.Position.y);
                        break;
                    case MoveDirection.RIGHT:
                        Cursor.Instance.Position = new Vector2Int(currentItem.xMax + 1, currentItem.Position.y);
                        break;
                }

                //Deselect currently hovered over item
                currentItem.SetState(ItemState.IDLE);
                if (tryHoverOverItem() == false)
                {
                    selectionState = SelectionState.EMPTY;
                    Cursor.Instance.SetVisible(true);
                    currentItem = null;
                }

                break;
            case SelectionState.ITEM_PICKED:
                //Check if item can be moved
                if (inventory.CanItemBeMoved(currentItem, direction))
                {
                    currentItem.Move(direction);
                    Inventory.Instance.ShowInteractionRangeTilesAt(currentItem.UseRange);
                }

                break;
        }
    }
}
