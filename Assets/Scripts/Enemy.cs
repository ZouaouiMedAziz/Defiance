using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static Action<Enemy> OnEnemyDeath;

    [Header("Enemy Health and Damage")]
    private float enemyHealth = 120f;
    private float presentHealth;
    private float giveDamage = 5f;
    public Image enemyHp;


    [Header("Enemy Things")]
    public NavMeshAgent enemyAgent;
    public Transform lookPoint;
    public Camera shootingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;

    [Header("Enemy Guarding Var")]
    public GameObject[] walkPoints;
    int currentEnemyPosition = 0;
    public float enemySpeed;
    float walkingpointRadius = 2;

    [Header("Sounds and UI")]
    public AudioClip shootingSound;
    public AudioSource audioSource;
    [Header("Enemy Shooting Var")]
    public float timebtwShoot;
    bool previouslyShoot;


    [Header("Enemy Animation and Spark effect")]
    public Animator anim;
    public ParticleSystem muzzleSpark;

    [Header("Enemy mood/situation")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        presentHealth = enemyHealth;
        playerBody = GameObject.Find("Player").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()

    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);


        if (!playerInvisionRadius && !playerInshootingRadius) Guard();
        if (playerInvisionRadius && !playerInshootingRadius) Pursueplayer();
        if (playerInvisionRadius && playerInshootingRadius) ShootPlayer();

        HPFiller();
        ColorChanger();
    }
    private void Guard()
    {
        if (Vector3.Distance(walkPoints[currentEnemyPosition].transform.position, transform.position) < walkingpointRadius)
        {
            currentEnemyPosition = UnityEngine.Random.Range(0, walkPoints.Length);
            if (currentEnemyPosition >= walkPoints.Length)
            {
                currentEnemyPosition = 0;
            }
        }
       
         
        

        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentEnemyPosition].transform.position, Time.deltaTime * enemySpeed);
        //changing eneny facing
        transform.LookAt(walkPoints[currentEnemyPosition].transform.position);
    }
    private void Pursueplayer()
    {
        if (enemyAgent.SetDestination(playerBody.position))
        {
            //animations
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", true);
            anim.SetBool("Shoot", false);
            anim.SetBool("Die", false);


            //+ vision & shooting radius
            visionRadius = 30f;
            shootingRadius = 16f;
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Shoot", false);
            anim.SetBool("Die", true);
        }
    }
    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);

        transform.LookAt(lookPoint);

        if (!previouslyShoot)
        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootingSound);

            if (Physics.Raycast(shootingRaycastArea.transform.position, shootingRaycastArea.transform.forward, out var hit, shootingRadius, LayerMask.GetMask("Player")))
            {
                Debug.Log("Shooting" + hit.transform.name);
                PlayerScript playerBody = hit.transform.GetComponent<PlayerScript>();

                if(playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }
                anim.SetBool("Shoot", true);
                anim.SetBool("Walk", false);
                anim.SetBool("AimRun", false);
                anim.SetBool("Die", false);
            }

            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);
        }
    }
        private void ActiveShooting()
        {
            previouslyShoot = false;
        }

    public void enemyHitDamage (float takeDamage)
    {
        presentHealth -= takeDamage;

        //+ vision & shooting radius
        visionRadius = 40f;

        if (presentHealth <= 0)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Shoot", false);
            anim.SetBool("Die", true);

            enemyDie();
        }
    }

    private void enemyDie()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        playerInvisionRadius = false;
        playerInshootingRadius = false;
        Destroy(gameObject, 5.0f);
        OnEnemyDeath?.Invoke(this);
    }
    void HPFiller()
    {
        enemyHp.fillAmount = Mathf.Lerp(enemyHp.fillAmount, (presentHealth / enemyHealth), 3f * Time.deltaTime);
    }
    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (presentHealth / enemyHealth));
        enemyHp.color = healthColor;
    }
}