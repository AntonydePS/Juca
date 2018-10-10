using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource AudioS;

    #region SFX
    [Header("SFX")]

    public AudioClip clickMenu;
    public AudioClip pickupHP;
    public AudioClip useHP;
    public AudioClip hitSFX;
    public AudioClip jucaAttack1;
    public AudioClip jucaAttack2;
    public AudioClip jucaDamage;
    public AudioClip jucaDeath;
    public AudioClip gameOver;
    public AudioClip hitMini;
    public AudioClip hitTank;

    #endregion

    void Start () {
        AudioS = GetComponent<AudioSource>();
    }

    void PlayJucaDeath()
    {
        AudioS.PlayOneShot(jucaDeath, 0.3f);
    }

    void PlayJucaDMG()
    {
        AudioS.PlayOneShot(jucaDamage, 0.3f);
    }


    void PlayMenuClick()
    {
        AudioS.PlayOneShot(clickMenu, 0.3f);
        Debug.Log("InsertClickSoundHere");
    }

    void PlayJucaAttack1()
    {
        AudioS.PlayOneShot(jucaAttack1, 0.3f);
    }

    void PlayJucaAttack2()
    {
        AudioS.PlayOneShot(jucaAttack2, 0.3f);
    }

    void PlayPickupHP()
    {
        AudioS.PlayOneShot(pickupHP, 0.3f);
    }

    void PlayUseHP()
    {
        AudioS.PlayOneShot(useHP, 0.3f);
    }

    void PlayHitSFX()
    {
        AudioS.PlayOneShot(hitSFX, 0.3f);
    }

    void PlayGameOver()
    {
        AudioS.PlayOneShot(gameOver, 0.3f);
    }

    void PlayMiniHit()
    {
        AudioS.PlayOneShot(hitMini, 0.3f);
        Debug.Log("minihit");
    }

    void PlayTankHit()
    {
        AudioS.PlayOneShot(hitTank, 0.3f);
        Debug.Log("tankhit");
    }

}
