using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    [CreateAssetMenu(menuName = "FPS Sound/Impact Audio Data")]
    public class ImpactAudioData : ScriptableObject
    {
        public List<ImpactTags> impactTags;
    }

    [System.Serializable]
    public class ImpactTags
    {
        public string Tag;
        public List<AudioClip> ImpactAudio;
    }
}
