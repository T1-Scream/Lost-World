using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Scripts.Weapon;

public class BotController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Animator botAni;
    public Transform bulletTrans;
    public BotBullet bullet;

    public int sightRange;
    public int atkRange;
    public bool InsightRange;
    public bool InattackRange;
    public int viewRadius;
    public LayerMask playerLayer;
    public LayerMask zombieLayer;
    public GameObject zombie;
    public int health;

    public int curAmmo;
    public int curMaxAmmo;
    public int Ammo = 30;
    public int MaxAmmo = 480;
    public float fireRate;
    public AudioSource shootSound;
    public AudioClip shootClip;
    public ImpactPrefabData impactPrefab;
    public ImpactAudioData impactAudio;
    public float bulletSpeed;
    public GameObject pool;

    float lastFireTime;
    AnimatorStateInfo gunStateInfo;
    IEnumerator reload;
    internal ObjectPool<BotBullet> bulletPool;

    private void Start()
    {
        curAmmo = Ammo;
        curMaxAmmo = MaxAmmo;
        reload = ReloadAmmo();

        bulletPool = new ObjectPool<BotBullet>(CreateBullet, bullet => { bullet.gameObject.SetActive(true); }, bullet => { bullet.gameObject.SetActive(false); }, bullet => {
            Destroy(bullet.gameObject);
        }, false, Ammo);
    }

    private BotBullet CreateBullet()
    {
        BotBullet b = Instantiate(bullet, bulletTrans.position, bulletTrans.rotation);
        b.transform.SetParent(pool.transform, true);
        b.transform.gameObject.SetActive(false);
        return b;
    }

    private void Update()   
    {
        InsightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        InattackRange = Physics.CheckSphere(transform.position, atkRange, zombieLayer);

        if (InsightRange) {
            ChasePlayer();
        }
        else {
            botAni.SetBool("isWalking", false);
        }

        if (InattackRange && InsightRange && curAmmo > 0) {
            //AttackZombie();
            FindClosestTarget();
        }
        else
        {
            botAni.SetBool("isShooting", false);
        }

        if (curAmmo == 0 && !AnimatorIsPlaying(1) && curMaxAmmo > 0)
        {
            Reload();
        }

        if (curMaxAmmo <= 0 || curAmmo < 0) {
            curMaxAmmo = 0;
            curAmmo = 0;
        }
    }

    private void ChasePlayer()
    {
        float distance = (player.position - transform.position).magnitude;
        int dis = (int)distance;

        if (dis == agent.stoppingDistance) {
            botAni.SetBool("isWalking", false);
            agent.isStopped = true;
        }
        else {
            botAni.SetBool("isWalking", true);
            agent.SetDestination(player.position);
            agent.isStopped = false;
        }
    }

    private void AttackZombie()
    {
        transform.LookAt(zombie.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        botAni.SetBool("isShooting", true);
        Shooting();
    }

    private void Shooting()
    {
        if (curAmmo > 0) {
            if (!isAllowShooting())
                return;

            curAmmo -= 1;
            shootSound.clip = shootClip;
            shootSound.PlayOneShot(shootClip);
            bulletPool.Get();
            lastFireTime = Time.time;
        }
        else
            return;
    }

    private void Reload()
    {
        botAni.SetLayerWeight(1, 1);
        botAni.SetTrigger("Reloading");

        if (reload != null)
        {
            StopCoroutine(reload);
            reload = null;
            reload = ReloadAmmo();
            StartCoroutine(reload);
        }
    }

    IEnumerator ReloadAmmo()
    {
        while (true)
        {
            yield return null;

            gunStateInfo = botAni.GetCurrentAnimatorStateInfo(1);

            if (gunStateInfo.IsTag("ReloadAmmo"))
            {
                if (gunStateInfo.normalizedTime >= 0.9f)
                {
                    int ammoCount = Ammo - curAmmo;
                    int Remain = curMaxAmmo - ammoCount;
                    if (Remain <= 0) {
                        curAmmo += curMaxAmmo;
                    }
                    else {
                        curAmmo = Ammo;
                    }

                    if (Ammo <= 0) {
                        curMaxAmmo = 0;
                    }
                    else {
                        curMaxAmmo = Remain;
                    }

                    if (gunStateInfo.normalizedTime >= 0.99f) {
                        botAni.SetLayerWeight(1, 0);
                        yield break;
                    }
                }
            }
        }
    }

    public GameObject FindClosestTarget()
    {
        GameObject[] entity;
        entity = GameObject.FindGameObjectsWithTag("Zombie");
        GameObject closest = null;
        float closeDistance = Mathf.Infinity;
        foreach (GameObject ent in entity) {
            Vector3 diff = ent.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < closeDistance) {
                closest = ent;
                closeDistance = curDistance;
            }
        }
        transform.LookAt(closest.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        botAni.SetBool("isShooting", true);
        Shooting();
        return closest;
    }

    public bool isAllowShooting()
    {
        return Time.time - lastFireTime > 1 / fireRate;
    }

    public bool AnimatorIsPlaying(int LayerIndex)
    {
        return botAni.GetCurrentAnimatorStateInfo(LayerIndex).length > botAni.GetCurrentAnimatorStateInfo(LayerIndex).normalizedTime;
    }
}
