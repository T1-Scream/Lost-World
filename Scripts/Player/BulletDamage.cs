using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Zombie;

public class BulletDamage : MonoBehaviour
{
    private Transform bulletTrans;
    // prefire
    private Vector3 prefPosition;

    bool isDead;

    private void Start()
    {
        bulletTrans = transform;
        prefPosition = bulletTrans.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Physics.Raycast(prefPosition, (bulletTrans.position - prefPosition).normalized,
            out RaycastHit hit, (bulletTrans.position - prefPosition).magnitude))
        {
            if (hit.collider.tag == "ZombieHead")
            {
                switch(hit.collider.gameObject.layer) {
                    case 12:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(80, 90));
                        break;
                    case 13:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(80, 90));
                        break;
                    case 14:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(80, 90));
                        break;
                    case 15:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(80, 90));
                        break;
                    case 16:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(80, 90));
                        break;
                    case 17:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(80, 90));
                        break;
                    default:
                        break;
                }
            }
            else if (hit.collider.tag == "ZombieBody")
            {
                switch (hit.collider.gameObject.layer) {
                    case 12:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(20, 23));
                        break;
                    case 13:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(20, 23));
                        break;
                    case 14:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(20, 23));
                        break;
                    case 15:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(20, 23));
                        break;
                    case 16:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(20, 23));
                        break;
                    case 17:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(20, 23));
                        break;
                    default:
                        break;
                }
            }
            else if (hit.collider.tag == "ZombieArm")
            {
                switch (hit.collider.gameObject.layer) {
                    case 12:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(10, 12));
                        break;
                    case 13:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(10, 12));
                        break;
                    case 14:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(10, 12));
                        break;
                    case 15:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(10, 12));
                        break;
                    case 16:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(10, 12));
                        break;
                    case 17:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(10, 12));
                        break;
                    default:
                        break;
                }
            }
            else if (hit.collider.tag == "ZombieLeg")
            {
                switch (hit.collider.gameObject.layer) {
                    case 12:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(9, 10));
                        break;
                    case 13:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(9, 10));
                        break;
                    case 14:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(9, 10));
                        break;
                    case 15:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(9, 10));
                        break;
                    case 16:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(9, 10));
                        break;
                    case 17:
                        hit.collider.gameObject.GetComponentInParent<Zombie>().TakeDamage(Random.Range(9, 10));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
