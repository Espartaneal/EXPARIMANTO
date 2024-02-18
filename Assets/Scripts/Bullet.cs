using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" + collision.gameObject.name + "!");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<Zombie1>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
    void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject hole = Instantiate(
            GlobalReference.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
    }
   
}
