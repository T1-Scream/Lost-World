using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Zombie
{
    public class Zombie : ZombieState
    {
        protected override void Awake()
        {
            base.Awake();
        }

        private void FixedUpdate()
        {
            if (!isDead) {
                InsightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
                InatkRange = Physics.CheckSphere(transform.position, atkRange, playerLayer);

                insideFOV = InFOV(transform, player, viewAngle, viewRadius);

                if (insideFOV && !AnimationIsPlaying(0,"Punch"))
                    ChasePlayer();
                else {
                    zombieAni.SetBool("isRunning", false);
                    isAttack = false;
                    timer = 0;
                }

                if (InsightRange && InatkRange && health > 0)
                    AttackPlayer();
                else
                    zombieAni.SetBool("isAttacking", false);
            }
        }
    }
}
