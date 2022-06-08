using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    [CreateAssetMenu (menuName = "FPS Sound/Audio Data")]
    public class GunAudioData : ScriptableObject
    {
        public AudioClip shooting;
        public AudioClip ReloadLeft;
        public AudioClip ReloadOutOf;
    }
}
