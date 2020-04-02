using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "LootTable", menuName = ("LootTable"))]
    public class LootTable : ScriptableObject
    {
        public List<LootItem> lootItems = new List<LootItem>();
        [ReadOnly]
        public float totalDropWeight;

        // Calculate drop percentage of items and prevents wrong input. 
        public void OnValidate()
        {
            // Check if table is empty.
            if (lootItems != null && lootItems.Count > 0)
            {
                float currentDropWeight = 0f;

                // Set weight range of items.
                foreach (LootItem item in lootItems)
                {
                    // Prevent negative weight, as it would break the table.
                    if (item.dropWeight < 0f)
                    {
                        Debug.LogError("Item weight must be a positive value.");
                        item.dropWeight = 0f;
                    }
                    else
                    {
                        // From range is the current total weight, To range that plus the items own weight;
                        item.probabilityRangeFrom = currentDropWeight;
                        currentDropWeight += item.dropWeight;
                        item.probabilityRangeTo = currentDropWeight;
                    }

                }

                // Increment total weight of table.
                totalDropWeight = currentDropWeight;

                // Calculate drop rate percentage.
                foreach (LootItem item in lootItems)
                {
                    item.dropPercent = ((item.dropWeight) / totalDropWeight) * 100;
                }

            }

        }

        // Return item.
        public GameObject DropLoot()
        {
            return PickLootDropItem().gameObject;
        }

        // Drop item.
        public void DropLoot(Vector3 position)
        {
            Instantiate(PickLootDropItem().gameObject, position, Quaternion.identity);
        }

        // Drop several items.
        public void DropLoot(int dropNumber, Vector3 position)
        {
            Vector3 pos = new Vector3();
            for (int i = 0; i < dropNumber; i++)
            {
                pos = new Vector3(position.x + Random.Range(0, 0.5f), position.y, position.z + Random.Range(0, 0.5f));
                Instantiate(PickLootDropItem().gameObject, pos, Quaternion.identity);
            }

        }

        protected LootItem PickLootDropItem()
        {
            // Random number from total weight.
            float rnd = Random.Range(0, totalDropWeight);

            foreach (LootItem item in lootItems)
            {
                // Check if it's within the item's range.
                if (rnd > item.probabilityRangeFrom && rnd < item.probabilityRangeTo)
                {
                    return item;
                }
            }

            // If no item could be found, return first one.
            Debug.LogError("Item missing");
            return lootItems[0];
        }
    }
}