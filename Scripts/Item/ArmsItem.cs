using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Items
{
    public class ArmsItem : Items
    {
        public enum GunType
        {
            AK47,
            HandGun,
            Ammo,
            Medkit,
        }

        public GunType curGun;
        public string gunName;
    }
}
