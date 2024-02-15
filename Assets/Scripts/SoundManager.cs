using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance{ get; set;}
    
    public AudioSource shootingSound1911;
    public AudioSource reloadingSound1911;
    public AudioSource emptyMagazineSound1911;
     private void Awake()
      {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
      }
   
}
