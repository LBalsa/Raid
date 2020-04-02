using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [System.Serializable]
    public class LootItem
    {
        public string name;
        public GameObject gameObject;

        // Weight of the item, the higher the more drop rate.
        public float dropWeight;

        // Actual probability of item being dropped.    
        [ReadOnly]
        public float dropPercent;

        // These values are assigned via LootDropTable script. They represent from which number to which number if selected, the item will be picked.
        [HideInInspector]
        public float probabilityRangeFrom;
        [HideInInspector]
        public float probabilityRangeTo;
    }
}