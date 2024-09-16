using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera Camera;
    public float giveDamageOf = 5f;
    public float shootingRange = 70f;
    public float fireCharge = 15f;
    public Animator animator;
    public PlayerScript player;

    [Header("Rifle Ammunition and shooting")]
    private int maxAmmunition = 30;
    private int mag = 2;
    private int presentAmmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;
    private float nextTimeToShoot = 0f;


    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject impactEffect;
    public GameObject goreEffect;


    [Header("Sounds and UI")]
    [SerializeField] private GameObject AmmoOut;
    public AudioClip ReloadingSound;
    public AudioSource audioSource;
    [SerializeField] private AudioClip[] pistolSound;

    [Header("Bullet Properties")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletsPerShot;
    PlayerScript aim;

    [Header("Recoil")]
    [SerializeField] Transform recoilFollowPos;
    [SerializeField] float kickBackAmmount =-1;
    [SerializeField] float kickBackSpeed = 10, returnSpeed = 20;
    float currentRecoilPosition, finalRecoilPosition;

    private void Awake()
    {
        presentAmmunition = maxAmmunition;
    }
    void Start()
    {
       // aim = GetComponentInParent<PlayerScript>();
    }

    void Update()
    {
        if (setReloading)
            return;

        if(presentAmmunition == 0 && mag > 0 || Input.GetKey(KeyCode.R) && presentAmmunition < maxAmmunition && mag >= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToShoot)
        {
            animator.SetBool("Aim", true);
            animator.SetBool("Pistol", true);


            nextTimeToShoot = Time.time + 1f / fireCharge;
            Shoot();
        }
        else if (Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.Z))
        {

            animator.SetBool("Pistol", true);
            animator.SetBool("Aim", true);
            //animator.SetBool("Walking", true);
        }
        else if (Input.GetButton("Fire2") && Input.GetButtonDown("Fire1"))
        {

            animator.SetBool("Pistol", true);
            animator.SetBool("Aim", true);
            animator.SetBool("Walking", true);
        }

        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Pistol", true);
        }
        currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0, returnSpeed * Time.deltaTime);
        finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, kickBackSpeed * Time.deltaTime);
        recoilFollowPos.localPosition = new Vector3(0, 0, finalRecoilPosition);

        
    }
    public void TriggerRecoil() => currentRecoilPosition += kickBackAmmount;

    void Shoot()
    {
        if (mag == 0)
        {
            StartCoroutine(ShowAmmoOut());
            return;
        }
        presentAmmunition--;
        if (presentAmmunition == 0 && mag > 0)
        {
            mag--;
        }

        //Updating UI
        AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        AmmoCount.occurrence.UpdateMagText(mag);


        muzzleSpark.Play();
        PlayClip(audioSource, pistolSound);
        RaycastHit hitInfo;
        TriggerRecoil();
      /* barrelPos.LookAt(aim.aimPos);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }*/

        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitInfo, shootingRange, LayerMask.GetMask("Enemy")))
        {
            Debug.Log(hitInfo.transform.name);
            Objects objects = hitInfo.transform.GetComponent<Objects>();
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

            if (objects != null)
            {
                objects.ObjectHitDamage(giveDamageOf);
                GameObject impactGO = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGO, 1f);
            }
            else if(enemy != null)
            {
                enemy.enemyHitDamage(giveDamageOf);
                GameObject impactGO = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGO, 2f);
            }
            
        }

    }
    IEnumerator Reload()
    {
        player.PlayerSpeed = 0f;
       player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading...");
       animator.SetBool("Reloading", true );
        audioSource.PlayOneShot(ReloadingSound);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmunition = maxAmmunition;

       // if ( mag > 0)
        //{

            AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        //}

        player.PlayerSpeed = 1.9f;
       // player.playerSprint = 3;
        setReloading = false;
    }
    IEnumerator ShowAmmoOut()
    {
        AmmoOut.SetActive(true);
        yield return new WaitForSeconds(1);
        AmmoOut.SetActive(false);
    }
    private void PlayClip(AudioSource source, AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        int index = Random.Range(0, clips.Length);

        source.clip = clips[index];
        source.pitch = Random.Range(.85f, 1.4f);
        source.PlayOneShot(clips[index]);
    }
}
