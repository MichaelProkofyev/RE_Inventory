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
    ITEM_HIGHLIGHTED,
    ITEM_PICKED
}

public class Player : MonoBehaviour {

    [SerializeField] private Item currentItem;
    [SerializeField] private Item[,] inventory = new Item[5, 5];

    [SerializeField] private SelectionState selectionState;

    void Start ()
    {
        UpdateItemsPositions();
    }

    private void UpdateItemsPositions()
    {
        inventory = new Item[5, 5];

        foreach (var item in GameObject.FindObjectsOfType<Item>())
        {
            var itemPosition = item.Position;
            for (int x = itemPosition.x; x < itemPosition.x + item.size.x; x++)
            {
                for (int y = itemPosition.y; y < itemPosition.y + item.size.y; y++)
                {
                    inventory[x, y] = item;
                }
            }
        }
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
                case SelectionState.ITEM_HIGHLIGHTED:
                    selectionState = SelectionState.ITEM_PICKED;
                    currentItem.SetPicked(true);
                    break;
                case SelectionState.ITEM_PICKED:
                    selectionState = SelectionState.ITEM_HIGHLIGHTED;
                    currentItem.SetPicked(false);
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
                var itemAtCursor = inventory[Cursor.Instance.Position.x, Cursor.Instance.Position.y];
                if (itemAtCursor != null)
                {
                    itemAtCursor.SetSelected(true);
                    selectionState = SelectionState.ITEM_HIGHLIGHTED;
                    currentItem = itemAtCursor;
                    Cursor.Instance.SetVisible(false);
                }
                break;
            case SelectionState.ITEM_HIGHLIGHTED:
                //TODO: Check if there is space for cursor

                switch (direction)
                {
                    case MoveDirection.UP:
                        Cursor.Instance.Position = new Vector2Int(currentItem.Position.x, currentItem.Position.y + currentItem.size.y);
                        break;
                    case MoveDirection.DOWN:
                        Cursor.Instance.Position = new Vector2Int(currentItem.Position.x, currentItem.Position.y - 1);
                        break;
                    case MoveDirection.LEFT:
                        Cursor.Instance.Position = new Vector2Int(currentItem.Position.x - 1, currentItem.Position.y);
                        break;
                    case MoveDirection.RIGHT:
                        Cursor.Instance.Position = new Vector2Int(currentItem.Position.x + currentItem.size.x, currentItem.Position.y);
                        break;
                }

                selectionState = SelectionState.EMPTY;
                Cursor.Instance.SetVisible(true);
                currentItem.SetSelected(false);
                currentItem = null;
                break;
            case SelectionState.ITEM_PICKED:
                currentItem.Move(direction);
                UpdateItemsPositions();

                break;
        }
    }

    //private void SetPartSelected(PlayerPart newSelectedPart)
    //{
    //    if (newSelectedPart == selectedPart)
    //    {
    //        return;
    //    }

    //    if (selectedPart != null)
    //    {
    //        //Deselect prev part
    //        selectedPart.SetSelected(false);
    //    }
    //    selectedPart = newSelectedPart;
    //    selectedPart.SetSelected(true);
    //}
}
