using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    public Spawner spawner;
    public int spawnMaxCount;
    public int spawnIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            spawner.spawnMaxCount = spawnMaxCount;
            spawner.spawnIndex = spawnIndex;
            spawner.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            spawner.gameObject.SetActive(false);
    }
}
