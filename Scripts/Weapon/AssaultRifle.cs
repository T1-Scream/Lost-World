using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{ 
    public class AssaultRifle : FireArms
    {
        private IEnumerator reloadCheck;
        //private IEnumerator isAim;
        private PlayerMouse mouse;

        protected override void Awake()
        {
            base.Awake();
            reloadCheck = ReloadAniEnd();
            //isAim = AimClose();
            OriginFOV = GunCamera.fieldOfView;
            mouse = FindObjectOfType<PlayerMouse>();
        }

        protected override void isShooting()
        {
            if (curAmmo > 0) {
                if (!isAllowShooting()) return;

                MuzzleParticle.Play();
                curAmmo -= 1;
                GunAni.Play("fire@assault_rifle_01", isAiming ? 1 : 0, 0);
                ShootingAudio.clip = audioData.shooting;
                ShootingAudio.Play();
                CreateBullet();
                CasingParticle.Play();
                // if (!firstBullet)
                mouse.FiringRecoil();
                lastFireTime = Time.time;
            }
            else
                return;
        }

        protected override void Reload()
        {
            GunAni.SetLayerWeight(2, 1);
            GunAni.SetTrigger(curAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");
            ReloadAudio.clip = curAmmo > 0 ? audioData.ReloadLeft : audioData.ReloadOutOf;
            ReloadAudio.Play();
            if (reloadCheck == null) {
                reloadCheck = ReloadAniEnd();
                StartCoroutine(reloadCheck);
            }
            else {
                StopCoroutine(reloadCheck);
                reloadCheck = null;
                reloadCheck = ReloadAniEnd();
                StartCoroutine(reloadCheck);
            }
        }
        protected void CreateBullet()
        {
            bulletPool.Get();
        }
    }
}
