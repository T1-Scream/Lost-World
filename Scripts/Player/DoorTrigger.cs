using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator doorAni;
    public bool openDoor;
    public bool clear;

    private void OnTriggerEnter(Collider other)
    {
        if (clear) {
            if (other.gameObject.CompareTag("Player") && !openDoor && FindObjectOfType<Spawner>().allReleased) {
                doorAni.Play("OpenDoor");
                openDoor = true;
            }
        }
        else {
            if (other.gameObject.CompareTag("Player") && !openDoor) {
                doorAni.Play("OpenDoor");
                openDoor = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && openDoor) {
            doorAni.Play("CloseDoor");
            gameObject.SetActive(false);
        }
    }
}
