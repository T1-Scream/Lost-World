using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClear : MonoBehaviour
{
    public GameObject LevelClearUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            LevelClearUI.SetActive(true);
            FindObjectOfType<GameManager>().UnLockMouse();
            FindObjectOfType<Spawner>().gameObject.SetActive(false);
        }
    }
}
