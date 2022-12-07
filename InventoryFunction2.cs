using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryItem : MonoBehaviour
{   
    
  private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
  public List<InventoryItem> inventory { get; private set; }

    private void Awake()
    {
        inventory = new Lis<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out inventoryItem value))
        {
            value.RemoveFromStack();

            if(value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
    }

    public void Add(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out, inventoryItem value))
        {
            value.AddToStack();
        }
        else
        { 
            inventoryItem newItem = new inventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
    } 

  public InventoryItemData data { get; private set; }
  public int stackSize { get; private set; }

  public InventoryItem(InventoryItemData source)
    {
        data = source;
        AddToStack();
    }
    public void AddToStack()
    {
        stackSize++;
    }
    public void RemoveFromStack()
    {
        stackSize--;
    }
}

