using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts.Weapon
{
    public abstract class FireArms : MonoBehaviour,IWeapon
    {
        public Camera GunCamera;

        public Transform MuzzlePoint;
        public Transform CasingPoint;

        public ParticleSystem MuzzleParticle;
        public ParticleSystem CasingParticle;

        public AudioSource ShootingAudio;
        public AudioSource ReloadAudio;
        public GunAudioData audioData;
        public ImpactAudioData ImpactClip;

        public Bullet bullet;
        public int bulletSize;
        public ImpactPrefabData ImpactPrefab;

        public float fireRate;
        public int Ammo = 30;
        public int MaxAmmo = 120;

        public float spreadAngle;

        internal int curAmmo;
        internal int curMaxAmmo;
        protected float lastFireTime;

        internal Animator GunAni;
        protected AnimatorStateInfo GunStateInfo;

        protected float OriginFOV;
        protected bool isAiming;
        protected bool shootingTrigger;

        internal float shootTime;

        private IEnumerator isAim;

        internal ObjectPool<Bullet> bulletPool;
        [SerializeField] 
        public GameObject pool;

        protected virtual void Awake()
        {
            curAmmo = Ammo;
            curMaxAmmo = MaxAmmo;
            GunAni = GetComponent<Animator>();
            isAim = AimClose();

            bulletPool = new ObjectPool<Bullet>(CreateBullet, bullet => { bullet.gameObject.SetActive(true); },bullet => { bullet.gameObject.SetActive(false); }, bullet=> { Destroy(bullet.gameObject);
            }, false, bulletSize);

            SpawnBullet();
        }

        private Bullet CreateBullet()
        {
            Bullet b = Instantiate(bullet, MuzzlePoint.position, MuzzlePoint.rotation);
            b.transform.SetParent(pool.transform, true);
            b.transform.gameObject.SetActive(false);
            return b;
        }

        private void SpawnBullet()
        {
            for (int i = 0; i < bulletSize; i++) {
                var b = bulletPool.Get();
                bulletPool.Release(b);
            }
        }

        public void DoAttack()
        {
            isShooting();
        }

        internal void Aim(bool aim)
        {
            isAiming = aim;
            GunAni.SetBool("Aim", isAiming);
            

            if (isAim == null) {
                isAim = AimClose();
                StartCoroutine(isAim);
            }
            else {
                StopCoroutine(isAim);
                isAim = null;
                isAim = AimClose();
                StartCoroutine(isAim);
            }
        }

        // spread inside circle
        protected Vector3 CalcSpread()
        {
            float spread = spreadAngle / GunCamera.fieldOfView;
            return spread * UnityEngine.Random.insideUnitCircle;
        }

        protected bool isAllowShooting()
        {
            return Time.time - lastFireTime > 1 / fireRate;
        }

        protected IEnumerator ReloadAniEnd()
        {
            while (true) {
                yield return null;
                GunStateInfo = GunAni.GetCurrentAnimatorStateInfo(2);
                if (GunStateInfo.IsTag("ReloadAmmo")) {
                    if (GunStateInfo.normalizedTime >= 0.9f) {
                        int ammoCount = Ammo - curAmmo;
                        int Remain = curMaxAmmo - ammoCount;
                        if (Remain <= 0)
                            curAmmo += curMaxAmmo;
                        else
                            curAmmo = Ammo;

                        if (Ammo <= 0)
                            curMaxAmmo = 0;
                        else
                            curMaxAmmo = Remain;

                        yield break;
                    }
                }
            }
        }

        protected IEnumerator AimClose()
        {
            while (true) {
                yield return null;
                float curFov = 0;
                GunCamera.fieldOfView = Mathf.SmoothDamp(GunCamera.fieldOfView, isAiming && !isPlaying(2) ? 45 : OriginFOV, ref curFov, Time.deltaTime * 3);
            }
        }

        internal bool isPlaying(int LayerIndex)
        {
            return GunAni.GetCurrentAnimatorStateInfo(LayerIndex).length > GunAni.GetCurrentAnimatorStateInfo(LayerIndex).normalizedTime;
        }

        internal void shootTriggerOn()
        {
            DoAttack();
            shootingTrigger = true;
        }

        internal void shootTriggerOff()
        {
            shootingTrigger = false;
        }

        internal void reloadAmmo()
        {
            Reload();
        }

        protected abstract void isShooting();
        protected abstract void Reload();
    }
}
