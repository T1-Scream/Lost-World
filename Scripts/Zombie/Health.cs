using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Zombie
{
    public class Health : MonoBehaviour
    {
        public int health = 100;

        public void TakeDamage(int damage)
        {
            health -= damage;
        }
    }
}
