using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera Camera;
    public float giveDamageOf = 10f;
    public float shootingRange = 100f;
    public float fireCharge = 15f;
    public Animator animator;
    public PlayerScript player;

    [Header("Rifle Ammunition and shooting")]
    private int maxAmmunition = 45;
    public int mag = 90;
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
    [SerializeField] private AudioClip[] rifleSound;

    [Header("Bullet Properties")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletsPerShot;

    [Header("Recoil")]
    [SerializeField] Transform recoilFollowPos;
    [SerializeField] float kickBackAmmount =-1;
    [SerializeField] float kickBackSpeed = 10, returnSpeed = 20;
    float currentRecoilPosition, finalRecoilPosition;
    public float RecoilY;
    public float RecoilX;
    public float currentRecoilXpos;
    public float currentRecoilYpos;

    [Header("Mouse")]
    [HideInInspector] public float zRotation;
    private float camYvelocity, camXvelocity;
    [HideInInspector] public float currentXRotation;
    [HideInInspector] public float wantedXRotation;
    [HideInInspector] public float currentYRotation;
    [HideInInspector] public float wantedYRotation;





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
        if (mag < 0)
        {
            mag = 0;
        }
        if (setReloading)
            return;

        if(presentAmmunition == 0 && mag > 0 || Input.GetKey(KeyCode.R) && presentAmmunition < maxAmmunition && mag >= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetKey(KeyCode.R) && mag == 0 && presentAmmunition == 0)
        {
            StartCoroutine(ShowAmmoOut());
            return;
        }
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            animator.SetBool("Aim", true);
            animator.SetBool("Rifle", true);


            nextTimeToShoot = Time.time + 1f / fireCharge;
           

            Shoot();

        }
        else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.Z))
        {

            animator.SetBool("Rifle", true);
            animator.SetBool("Aim", true);
            animator.SetBool("Walking", true);
        }
        else if (Input.GetButton("Fire2") && Input.GetButton("Fire1"))
        {

            animator.SetBool("Rifle", true);
            animator.SetBool("Aim", true);
            animator.SetBool("Walking", true);
        }

        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Rifle", true);
        }
        currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0, returnSpeed * Time.deltaTime);
        finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, kickBackSpeed * Time.deltaTime);
        recoilFollowPos.localPosition = new Vector3(0, 0, finalRecoilPosition);
        CamControl();
        if (animator.GetBool("Aim") == true && animator.GetBool("Running") == false && animator.GetBool("Walking") == false)
        {
            RecoilX = .001f;
            RecoilY = .001f;
        }
        else if (animator.GetBool("Running") == true)
        {
            RecoilX = 0.02f;
            RecoilY = 2.5f;
        }
        else if (animator.GetBool("Rifle") == true && animator.GetBool("Aim") == false 
            || animator.GetBool("Pistol") == true && animator.GetBool("Aim") == false
             )
        {
            RecoilX = 0.00001f;
            RecoilY = .5f;
        }
    }
    public void TriggerRecoil() => currentRecoilPosition += kickBackAmmount;
    
    public void RecoilMatch()
    {
        currentRecoilXpos = ((Random.value - .5f) / 2) * RecoilX;
        currentRecoilYpos = ((Random.value - .5f) / 2) * RecoilY;
        wantedXRotation -= currentRecoilXpos;
        wantedYRotation -= Mathf.Abs(currentRecoilYpos); 
    }
    void CamControl()
    {
        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref camYvelocity, Time.deltaTime);
        currentXRotation = Mathf.SmoothDamp(currentXRotation, wantedXRotation, ref camXvelocity, Time.deltaTime);
        if (animator.GetBool("Aim") == true && animator.GetBool("Running") == false && animator.GetBool("Walking") == false)
        {
            currentYRotation = 0;
            currentXRotation = 0;
        }
        Camera.transform.localRotation = Quaternion.Euler(currentYRotation, currentXRotation, zRotation);
    }
    void Shoot()
    {
        if (mag == 0 && presentAmmunition == 0)
        {
            StartCoroutine(ShowAmmoOut());
            return;
        }
        if (presentAmmunition > 0)
        {
            presentAmmunition--;
        }
     
        if (presentAmmunition == 0 && mag > 0)
        {
            AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
            AmmoCount.occurrence.UpdateMagText(mag);
            StartCoroutine(Reload());
            return;
        }

        //Updating UI
        AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        AmmoCount.occurrence.UpdateMagText(mag);


        muzzleSpark.Play();
        PlayClip(audioSource, rifleSound);
        RaycastHit hitInfo;

        TriggerRecoil();

        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward , out hitInfo, shootingRange, LayerMask.GetMask("Enemy")))
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
            RecoilMatch();
            
        }

    }
    IEnumerator Reload()
    {
        player.PlayerSpeed = 0f;
       player.playerSprint = 0f;
        if (mag > 0)
        {
            setReloading = true;
        Debug.Log("Reloading...");
       animator.SetBool("Reloading", true );
        audioSource.PlayOneShot(ReloadingSound);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        if ((mag - maxAmmunition + presentAmmunition) >= 0)
            {
                mag = mag - maxAmmunition + presentAmmunition;
                presentAmmunition = maxAmmunition;
            }
        else 
            {
                presentAmmunition += mag;
                mag = 0;
                
            }
        AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        AmmoCount.occurrence.UpdateMagText(mag);
        }

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
