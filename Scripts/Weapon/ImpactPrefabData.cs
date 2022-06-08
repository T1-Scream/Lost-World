using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    [CreateAssetMenu(menuName = "FPS Sound/Impact Prefab Data")]
    public class ImpactPrefabData : ScriptableObject
    {
        public List<ImpactInfo> ImpactTag;
    }

    [System.Serializable]
    public class ImpactInfo {
        public string Tags;
        public List<GameObject> ImpactPrefab;
    }
}
