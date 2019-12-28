using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Managers/Resources Manager")]
    public class ResourcesManager : ScriptableObject
    {
        public List<Item> allItems = new List<Item>();
        Dictionary<string, Item> itemDict = new Dictionary<string, Item>();

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
                    Debug.Log("There's two items named: " + allItems[i].name + " ! That's ILLEGAL !");
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
                Debug.Log("Weapon with ID: " + targetID + " was not found!");
                return null;
            }

        }

        Item GetItem(string targetID)
        {
            itemDict.TryGetValue(targetID, out Item retVal);
            return retVal;
        }
    }
}