using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public Item[,] slots = new Item[10, 5];

    public void UpdateItemsPositions()
    {
        slots = new Item[10, 5];

        foreach (var item in GameObject.FindObjectsOfType<Item>())
        {
            var itemPosition = item.PPosition;
            for (int x = itemPosition.x; x <= item.Rect.xMax; x++)
            {
                for (int y = itemPosition.y; y <= item.Rect.yMax; y++)
                {
                    slots[x, y] = item;
                }
            }
        }
    }

    public Item At(int x, int y)
    {
        if (0 <= x && x < slots.GetLength(0) && 0 <= y && y < slots.GetLength(1))
        {
            return slots[x, y];
        }
        else
        {
            Debug.LogErrorFormat("Tried to get item from slot {0}:{1} out of bounds!", x, y);
            return null;
        }
    }

    public Item Overlapping(Item item)
    {
        for (int x = 0; x < slots.GetLength(0); x++)
        {
            for (int y = 0; y < slots.GetLength(1); y++)
            {
                Item slotItem = slots[x, y];

                if (slotItem != null && slotItem != item)
                {
                    bool yWithin = item.Rect.yMin <= y && item.Rect.yMax >= y;
                    bool xWithin = item.Rect.xMin <= x && item.Rect.xMax >= x;
                    if (yWithin && xWithin)
                    {
                        return slotItem;
                    }
                }

            }
        }
        return null;
    }

    public bool CanItemBeMoved(Item item, MoveDirection direction)
    {
        int width = slots.GetLength(0);
        int height = slots.GetLength(1);

        switch (direction)
        {
            case MoveDirection.UP:
                if (item.Rect.yMax == height - 1) return false;
                break;
            case MoveDirection.DOWN:
                if (item.Rect.yMin == 0) return false;
                break;
            case MoveDirection.LEFT:
                if (item.Rect.xMin == 0) return false;
                break;
            case MoveDirection.RIGHT:
                if (item.Rect.xMax == width - 1) return false;
                break;
        }
        return true;
    }

    void Start()
    {
        UpdateItemsPositions();
    }

}
