﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SingletonComponent<Inventory> {

    public enum ScanDirection
    {
        NONE,
        LEFT_RIGHT,
        RIGHT_LEFT
    }

    [SerializeField] private GameObject interactionRangeTilePrefab;
    [SerializeField] private List<GameObject> currentInteractionRangeTiles;
    [SerializeField] private Text capacityTitle;

    public EntityBase[,] slots = new EntityBase[inventotyWidth, inventotyHeight];

    private const int inventotyWidth = 10;
    private const int inventotyHeight = 5;


    //Should picked items be omitted ?
    public void UpdateItemsPositions()
    {
        slots = new EntityBase[inventotyWidth, inventotyHeight];
        Item possibleHoldedItem = Player.Instance.CurrentlyHoldingItem();

        foreach (var entity in GameObject.FindObjectsOfType<EntityBase>())
        {
            if (possibleHoldedItem == entity)
            {
                //Don't keep the position of currently holded item in inventory (so overlapping items won't overwrite each other)
                continue;
            }
            var possibleDamagable = entity as IDamagable;
            bool justDiedEntity = possibleDamagable != null && possibleDamagable.IsAlive() == false;
            if (justDiedEntity)
            {
                //Entity that died this frame & didn't get destroyed yet should be skipped
                continue;
            }
            var itemPosition = entity.Position;
            for (int x = itemPosition.x; x <= entity.xMax; x++)
            {
                for (int y = itemPosition.y; y <= entity.yMax; y++)
                {
                    slots[x, y] = entity;
                }
            }
        }

        //Update invetory copacity
        {
            int filledAmount = 0;
            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    filledAmount++;
                }
            }
            //Count the item thats not in the slots, but currently holding
            if (possibleHoldedItem != null)
            {
                filledAmount += possibleHoldedItem.size.x * possibleHoldedItem.size.y;
            }
            capacityTitle.text = $"{filledAmount}/{inventotyWidth * inventotyHeight}";
        }
    }

    public EntityBase At(int x, int y)
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

    public List<T> AtRect<T>(Vector2Int position, int width, int height, ScanDirection scanDirection) where T : EntityBase
    {
        //TODO: Use scan direction to sort the entries (LINQ?)
        List<T> foundEntities = new List<T>();
        for (int x = position.x; x < position.x + width && x < slots.GetLength(0); x++)
        {
            for (int y = position.y; y < position.y + height && y < slots.GetLength(1); y++)
            {
                T slotEntity = slots[x, y] as T;

                if (slotEntity != null && foundEntities.Contains(slotEntity) == false)
                {
                    foundEntities.Add(slotEntity);
                }

            }
        }

        return foundEntities;
    }

    public void ShowInteractionRangeTilesAt(ItemRect useRect)
    {
        HideInteractionRangeTiles();
        for (int x = useRect.position.x; x < useRect.position.x + useRect.width && x < slots.GetLength(0); x++)
        {
            for (int y = useRect.position.y; y < useRect.position.y + useRect.height && y < slots.GetLength(1); y++)
            {
                var interactionRangeTile = SimplePool.Spawn(interactionRangeTilePrefab, new Vector3Int(x, y, 0), Quaternion.identity);
                currentInteractionRangeTiles.Add(interactionRangeTile);
            }
        }
    }

    public void HideInteractionRangeTiles()
    {
        foreach (var tile in currentInteractionRangeTiles)
        {
            SimplePool.Despawn(tile);
        }
        currentInteractionRangeTiles.Clear();
    }

    public Item Overlapping(Item item)
    {
        for (int x = 0; x < slots.GetLength(0); x++)
        {
            for (int y = 0; y < slots.GetLength(1); y++)
            {
                Item slotItem = slots[x, y] as Item;

                if (slotItem != null && slotItem != item)
                {
                    bool xWithin = item.xMin <= x && x <= item.xMax;
                    bool yWithin = item.yMin <= y && y <= item.yMax;
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
                if (item.yMax == height - 1) return false;
                for (int x = item.Position.x; x <= item.xMax; x++)
                {
                    var possibleEnemyBlocker = At(x, item.yMax + 1) as Enemy;
                    if (possibleEnemyBlocker != null) return false;
                }
                break;
            case MoveDirection.DOWN:
                if (item.yMin == 0) return false;
                for (int x = item.Position.x; x <= item.xMax; x++)
                {
                    var possibleEnemyBlocker = At(x, item.yMin - 1) as Enemy;
                    if (possibleEnemyBlocker != null) return false;
                }
                break;
            case MoveDirection.LEFT:
                if (item.xMin == 0) return false;
                for (int y = item.Position.y; y <= item.yMax; y++)
                {
                    var possibleEnemyBlocker = At(item.xMin - 1, y) as Enemy;
                    if (possibleEnemyBlocker != null) return false;
                }
                break;
            case MoveDirection.RIGHT:
                if (item.xMax == width - 1) return false;
                for (int y = item.Position.y; y <= item.yMax; y++)
                {
                    var possibleEnemyBlocker = At(item.xMax + 1, y) as Enemy;
                    if (possibleEnemyBlocker != null) return false;
                }
                break;
        }
        return true;
    }

    void Start()
    {
        UpdateItemsPositions();
        Player.Instance.OnPlayerAction += UpdateItemsPositions;
    }

    void Update()
    {
        //Debug purposes
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintInventoryContents();
        }
    }

    public void PrintInventoryContents()
    {
        string inventoryDescription = string.Empty;
        for (int y = slots.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < slots.GetLength(0); x++)
            {
                EntityBase slotEntity = slots[x, y] as EntityBase;
                if (slotEntity != null)
                {
                    inventoryDescription += slotEntity.gameObject.name + " ";
                }
                else
                {
                    inventoryDescription += "0 ";
                }
            }
            inventoryDescription += "\n";
        }
        print(inventoryDescription);
    }
}
