using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
public class Weapon : MonoBehaviour
{
    

    public bool isWeapon;
    
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    //Burst
    public int bulletsPerBurst = 3;
    public int currentBurst;

    //Spread Intensity
    public float SpreadIntensity; 

    public GameObject muzzleEffect;
    private Animator animator;



    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletspawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 4f;


    //Reloading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;
    public enum ShootingMode
    {
        Single, 
        Burst, 
        Auto
    }
    public enum WeaponModel
    {
        Pistol1911,
        HK16
    }
    public WeaponModel weaponModel;
    public ShootingMode currentShootingMode;
    private void Awake()
    {
        readyToShoot = true;
        currentBurst = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;

    
    }
    // Update is called once per frame
    void Update()
    {   //Mouse0 = LeftMouseKey

     if(isWeapon) 
     {
        if(bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSound1911.Play();
        }
        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single
                || currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
        {
            Reload();
        }

        if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <=0)
        {
            Reload();
        }    

        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            currentBurst = bulletsPerBurst;
            FireWeapon();

            }

            
        {
            

        }

     }  
        
    }

    private void FireWeapon()
    {
        
        bulletsLeft --;
        muzzleEffect.GetComponent<ParticleSystem>().Play();        
        animator.SetTrigger("RECOIL");
        SoundManager.Instance.shootingSound1911.Play();

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletspawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //Destroying the bullet after it has been shot (4 seconds later)
        StartCoroutine(DestroyBulletAfterTIme(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;

        }
        if (currentShootingMode == ShootingMode.Burst && currentBurst > 1)
        {
            currentBurst --;
            Invoke("FireWeapon", shootingDelay);
        }
        
    
    }
    private void Reload()
    {

        SoundManager.Instance.reloadingSound1911.Play();
        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);

    }
    public void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
 
    }
    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else 
        {
            targetPoint = ray.GetPoint(100);

        }
        Vector3 direction = targetPoint - bulletspawn.position;

        float x = UnityEngine.Random.Range(-SpreadIntensity, SpreadIntensity);
        float y = UnityEngine.Random.Range(-SpreadIntensity, SpreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTIme(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

}

