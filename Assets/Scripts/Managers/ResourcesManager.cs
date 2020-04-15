using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Managers/Resources Manager")]
    public class ResourcesManager : ScriptableObject
    {
        public List<Item> allItems = new List<Item>();
        Dictionary<string, Item> itemDict = new Dictionary<string, Item>();

        public RoomVariable curRoom;

        public void Init()
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (!itemDict.ContainsKey(allItems[i].name))
                {
                    itemDict.Add(allItems[i].name, allItems[i]);
                }
                else
                {
                    Debug.LogError("There's two items named: " + allItems[i].name + " ! That's ILLEGAL !");
                }
            }
        }

        public Item GetItemInstance(string targetID)
        {
            Item defaultItem = GetItem(targetID);
            if (defaultItem != null)
            {
                Item newItem = Instantiate(defaultItem);
                newItem.name = defaultItem.name;
                return newItem;
            }
            else
            {
                Debug.LogError("Weapon with ID: " + targetID + " was not found!");
                return null;
            }
        }

        public ClothItem GetClothItem(string targetID)
        {
            return (ClothItem)GetItem(targetID);
        }

        Item GetItem(string targetID)
        {
            itemDict.TryGetValue(targetID, out Item retVal);
            if(retVal==null)
                Debug.LogError("Item with ID: " + targetID + " was not found!");
            return retVal;
        }
    }
}