using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1 : MonoBehaviour
{
    public ZombieHand1 zombieHand1;

    public int zombieDamage;


    private void Start()
    {
        zombieHand1.damage = zombieDamage;
    }

}
