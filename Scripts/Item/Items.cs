using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Items
{
    public abstract class Items : MonoBehaviour
    {
        public enum ItemType
        {
            FireArms,
            Others
        }

        public ItemType curItem;
        public int Item_ID;
    }
}
