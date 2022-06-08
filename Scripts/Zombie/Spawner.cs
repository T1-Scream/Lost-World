using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Scripts.Zombie;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int spawnMaxCount = 15;
    public ZombieState[] zombie;
    public SpawnAreaData[] spawnArea;
    public float radius;
    public int spawnIndex = 0;
    [HideInInspector] public bool allReleased;

    [Header("Misc")]
    public GameObject areaClearText;
    private bool showOnceTimeEachArea;

    private GameObject pool;
    private bool allGet;

    [HideInInspector] public ObjectPool<ZombieState> zombiePool;

    private void Awake()
    {
        pool = transform.Find("ZombiePool").gameObject;
    }

    private void OnEnable()
    {
        zombiePool = new ObjectPool<ZombieState>(CreateZombie, zombie => zombie.gameObject.SetActive(true), Release,
    zombie => Destroy(zombie.gameObject), false, spawnMaxCount, spawnMaxCount);

        showOnceTimeEachArea = false;

        InvokeRepeating(nameof(ReleaseZombie), 5f, 5f);
        //InvokeRepeating(nameof(SpawnZombie), 10f, 10f);
        SpawnZombie();
        ResetZombie(); //reset first
        SpawnZombie();
    }

    private ZombieState CreateZombie()
    {
        ZombieState z = Instantiate(zombie[Random.Range(0, zombie.Length - 1)], GetArea(), Quaternion.identity);
        z.transform.SetParent(pool.transform, true);

        return z;
    }

    private Vector3 GetArea()
    {
        spawnArea[spawnIndex].center = new Vector3(spawnArea[spawnIndex].centerTrans.position.x, spawnArea[spawnIndex].centerTrans.position.y, spawnArea[spawnIndex].centerTrans.position.z);

        Vector3 pos = spawnArea[spawnIndex].center + new Vector3(Random.Range(-spawnArea[spawnIndex].size.x / 2, spawnArea[spawnIndex].size.x / 2),
            0, Random.Range(-spawnArea[spawnIndex].size.y / 2, spawnArea[spawnIndex].size.y / 2));

        return pos;
    }

    private bool CheckValidSpawnPosition(ZombieState zombie)
    {
        Collider[] col = Physics.OverlapSphere(zombie.transform.position + Vector3.up, 1f);
        foreach(Collider c in col) {
            if (c.CompareTag("Obstacle"))
                return false;
        }
        return true;
    }

    private void Release(ZombieState z)
    {
        z.gameObject.SetActive(false);
        z.deadAni = false;
        z.health = 100;
        z.zombieAni.Rebind();
        z.transform.position = GetArea();
        
        for (int i = 0; i < 100; i++) {
            if (!CheckValidSpawnPosition(z))
                z.transform.position = GetArea();
            else
                break;
        }
    }

    private void SpawnZombie()
    {
        if (zombiePool.CountInactive == spawnMaxCount) {
            allReleased = false;
            allGet = false;
        }

        for (int i = 0; i < spawnMaxCount; i++) {
            if (!allReleased && !allGet) {
                zombiePool.Get();
            }
        }


        for (int i = 0; i < spawnMaxCount; i++) {
            if (pool.transform.childCount < spawnMaxCount)
                zombiePool.Get();
            else
                break;
        }
        allGet = true;
    }

    private void ResetZombie()
    {
        for (int i = 0; i < spawnMaxCount; i++) {
            ZombieState z = pool.transform.GetChild(i).GetComponent<ZombieState>();
            zombiePool.Release(z);
            z.released = true;
        }

        //if (zombiePool.CountInactive == spawnMaxCount)
        //    allReleased = true;
    }

    private void ReleaseZombie()
    {
        if (gameObject.activeInHierarchy) {
            for (int i = 0; i < spawnMaxCount; i++) {
                ZombieState z = pool.transform.GetChild(i).GetComponent<ZombieState>();
                if (z.deadAni && !z.released) {
                    if (!z.isPlaying(0)) {
                        zombiePool.Release(z);
                        z.released = true;
                    }
                }
            }

            if (zombiePool.CountInactive == spawnMaxCount)
                allReleased = true;

            if (!showOnceTimeEachArea && allReleased) { // show clear text once time
                StartCoroutine(ShowClearTips());
                showOnceTimeEachArea = true;
            }
        }
    }

    IEnumerator ShowClearTips()
    {
        yield return new WaitForSeconds(0.2f);
        areaClearText.SetActive(true);
        yield return new WaitForSeconds(2f);
        areaClearText.SetActive(false);
        StopCoroutine(ShowClearTips());
    }

    private void FixedUpdate()
    {
        if (spawnMaxCount < pool.transform.childCount)
            Destroy(pool.transform.GetChild(spawnMaxCount).gameObject);
        else
            return;
    }
}
