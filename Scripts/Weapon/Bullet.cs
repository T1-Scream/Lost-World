using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Zombie;

namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float bulletSpeed;
        public ImpactAudioData impactAudio;
        public ImpactPrefabData impactPrefab;
        public AssaultRifle assaultRifle;
        [SerializeField] private ZombieState zombie;

        private Transform bulletTrans;
        // prefire
        private Vector3 prefPosition;

        private void Start()
        {   
            bulletTrans = transform;
            prefPosition = bulletTrans.position;
        }

        private void Update()
        {
            prefPosition = bulletTrans.position;
            bulletTrans.Translate(0, 0, bulletSpeed * Time.deltaTime);

            if (Physics.Raycast(prefPosition, (bulletTrans.position - prefPosition).normalized,
                    out RaycastHit hit, (bulletTrans.position - prefPosition).magnitude)) {
                var impactTags = impactPrefab.ImpactTag.Find((impact) => {
                        return impact.Tags.Equals(hit.collider.tag);
                });

                if (hit.collider)
                    zombie = hit.collider.gameObject.GetComponent<ZombieState>();
                GameObject hitObject = impactTags.ImpactPrefab[0];

                GameObject bulletEffect = Instantiate(hitObject, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up)) as GameObject;
                bulletEffect.transform.SetParent(hit.collider.transform);
                if (hit.collider.transform.root.gameObject.name != "Zombie" || !zombie.isDead) {
                    // find tags && play clip at hit point
                    var findTags = impactAudio.impactTags.Find((impact) => {
                        return impact.Tag.Equals(hit.collider.tag);
                    });
                    int length = findTags.ImpactAudio.Count;
                    AudioClip clip = findTags.ImpactAudio[0];
                    AudioSource.PlayClipAtPoint(clip, hit.point);
                }

                if (hit.collider) {
                    assaultRifle.bulletPool.Release(this);
                    Destroy(bulletEffect, 5);
                }
            }
        }
    }
}
