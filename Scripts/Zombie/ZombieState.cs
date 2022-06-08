using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

namespace Scripts.Zombie
{
    public abstract class ZombieState : MonoBehaviour
    {
        public Transform player;
        public LayerMask playerLayer;
        public Animator zombieAni;
        public PlayerMovement playerState;
        public LayerMask ignoreLayer;

        //A*
        [Header("A*")]
        public float nextWayPointDistance;
        private Path path;
        private Seeker seeker;
        private Rigidbody rb;
        private int curWayPoint = 0;
        private float lastRepath = float.NegativeInfinity;
        private float repathRate = 0.5f;

        //chase && attack
        [Header("Statu")]
        public int health;
        public int atkDamage;
        internal bool isAttack;
        private float time = 0f;
        public float attackDelayTime = 3f;

        public float sightRange, atkRange;
        public bool InsightRange, InatkRange;

        public float speed;
        public float rotateSpeed;
        public float velocity = -1f;

        //FOV
        [Header("FOV")]
        public float viewRadius;
        public float viewAngle;

        protected bool insideFOV;

        protected float timer = 0;

        public bool isDead
        { get { return health <= 0; } }
        public bool deadAni;
        public bool released;

        private bool attack;

        //protected void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawWireSphere(transform.position, viewRadius);

        //    Vector3 fovLine1 = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward * viewRadius;
        //    Vector3 fovLine2 = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward * viewRadius;
        //    Gizmos.color = Color.cyan;
        //    Gizmos.DrawRay(transform.position, fovLine1);
        //    Gizmos.DrawRay(transform.position, fovLine2);

        //    if (!insideFOV)
        //        Gizmos.color = Color.black;
        //    else
        //        Gizmos.color = Color.red;

        //    Gizmos.DrawRay(transform.position + Vector3.up, (player.position + -new Vector3(0, -.4f, 0) + Vector3.down - transform.position).normalized * viewRadius);
        //}

        protected virtual void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            playerState = player.GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody>();
            seeker = GetComponent<Seeker>();
            zombieAni = GetComponent<Animator>();
        }

        protected void OnEnable()
        {
            released = false; //pool
        }

        protected void UpdatePlayerPath()
        {
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }

        protected void OnPathComplete(Path p)
        {
            if (!p.error) {
                path = p;
                curWayPoint = 1;
            }
        }

        internal void TakeDamage(int damage)
        {
            health -= damage;

            if (isDead && !deadAni) {
                zombieAni.SetTrigger("Dead");
                deadAni = true;
            }
        }

        protected void ChasePlayer()
        {
            if (Time.time > lastRepath + repathRate && seeker.IsDone()) {
                lastRepath = Time.time;
                UpdatePlayerPath();
            }

            if (path == null) return;

            if (curWayPoint >= path.vectorPath.Count) return;

            Vector3 direction = (path.vectorPath[curWayPoint] - rb.position).normalized;
            Vector3 force = direction * speed * Time.fixedDeltaTime;
            rb.velocity = new Vector3(force.x, velocity, force.z);

            zombieAni.SetBool("isRunning", true);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            float distance = Vector3.Distance(rb.position, path.vectorPath[curWayPoint]);
            if (distance < nextWayPointDistance) {
                curWayPoint++;
                return;
            }
        }

        protected void AttackPlayer()
        {
            time += 1f * Time.deltaTime;
            transform.LookAt(player);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            isAttack = true;
            zombieAni.SetBool("isAttacking", true);
            if (time >= attackDelayTime) {
                playerState.SetHealthBar(atkDamage);
                time = 0f;
                playerState.GameOver();
            }
        }

        public bool isPlaying(int LayerIndex) {
            return zombieAni.GetCurrentAnimatorStateInfo(LayerIndex).length > zombieAni.GetCurrentAnimatorStateInfo(LayerIndex).normalizedTime;
        }

        public bool AnimationIsPlaying(int LayerIndex, string animationName)
        {
            return isPlaying(LayerIndex) && zombieAni.GetCurrentAnimatorStateInfo(LayerIndex).IsName(animationName);
        }

        protected bool InFOV(Transform ai, Transform Target, float angle, float radius)
        {
            Vector3 dir = (Target.position - ai.position).normalized;
            dir.y *= 0;

            RaycastHit hit;
            int layerMask = ~ignoreLayer;
            // Raycast(eyePosition, direction, hit, distance)
            if (Physics.Raycast(ai.position + Vector3.up, (Target.position + -new Vector3(0, -.4f, 0) + Vector3.down - ai.position).normalized, out hit, radius, layerMask)) {
                if (hit.collider.gameObject.tag == "Player") {
                    // calc target inside angle
                    float eyeAngle = Vector3.Angle(ai.forward, dir);
                    if (eyeAngle <= angle)
                        return true;
                }
            }

            return false;
        }
    }
}