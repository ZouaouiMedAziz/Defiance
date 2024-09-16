using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import SceneManagement to reload the scene

public class PlayerScript : MonoBehaviour
{
    public GameObject gameOverCanvas, PauseMenuPannel;
    public GameObject AimCanvas;
    private bool _isAbleToMove = true;
    private Enemy_Zone_Manager currentZoneManager; // Reference to the current zone manager

    [Header("Player movement")]
    public float PlayerSpeed = 3f;
    public float playerSprint = 5f;
    [HideInInspector] public float hzInput, vInput;
    [HideInInspector] public Vector3 Dir;

    [HideInInspector] public float walkSpeed = 3, walkBackSpeed = 2;
    [HideInInspector] public float runSpeed = 7, runBackSpeed = 5;
    [HideInInspector] public float crouchSpeed = 2, crouchBackSpeed = 1;

    [Header("Player Health Things")]
    private float playerHealth = 120f;
    private float presentHealth;
    public HealthBar healthBar;
    public Image hp;
    public AudioClip playerHurtSound;
    public AudioSource audioSource;

    [Header("Player Script Cameras")]
    public Transform playerCamera;
    [SerializeField] float mouseSense = 1;
    [SerializeField] Transform camFollowPos;
    float xAxis, yAxis;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Jumping and Velocity")]
    public float jumpRange = 1f;
    Vector3 velocity;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;
    [SerializeField] float groundYOffset;
    Vector3 spherePos;

    public MovementBaseState currentState;

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();

    public Transform aimPos;
    [SerializeField] float aimSmoothSpeed = 20;
    [SerializeField] LayerMask aimMask;
        public Button replayButton; // Reference to the replay button

    void Start()
    {
             // Make sure to set the button's OnClick listener
        if (replayButton != null)
        {
            replayButton.onClick.AddListener(ReplayGame);
        }
        cC = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.GiveFullHealth(playerHealth);
        SwitchState(Idle);

        Enemy_Zone_Manager.OnEnemyZoneEnter += OnPlayerEnemyZoneEnter;
        Enemy_Zone_Manager.OnEnemyZoneComplete += OnPlayerEnemyZoneComplete;
    }

    private void OnDestroy()
    {
        Enemy_Zone_Manager.OnEnemyZoneEnter -= OnPlayerEnemyZoneEnter;
        Enemy_Zone_Manager.OnEnemyZoneComplete -= OnPlayerEnemyZoneComplete;
    }

    void Update()
    {
        if (_isAbleToMove)
        {
            // Only move and update when the CharacterController is enabled
            if (cC.enabled)
        {
            onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

            if (onSurface && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            velocity.y += gravity * Time.deltaTime;
            cC.Move(velocity * Time.deltaTime);

            animator.SetFloat("hzInput", hzInput);
            animator.SetFloat("vInput", vInput);
            if (hzInput == 0 && vInput == 0)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
            }

            Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCentre);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
                aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);

            currentState.UpdateState(this);

            xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
            yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
            yAxis = Mathf.Clamp(yAxis, -80, 80);

            PlayerMove();
            Gravity();
        }
        }
        else
        {
            // When the player is not able to move, explicitly stop walking/running animations
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
        }
        HPFiller();
        ColorChanger();
    }

    public void EnableMovement(bool enable)
    {
        _isAbleToMove = enable;
        Debug.Log("Player movement enabled: " + enable);
    }
    private void LateUpdate()
    {
        if (cC.enabled)
        {
            camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
        }
    }

    void PlayerMove()
    {
        // Do not allow movement if the player is blocked
        if (!_isAbleToMove) { return; }

        hzInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        Dir = transform.forward * vInput + transform.right * hzInput;

        // Calculate movement
        Vector3 move = Dir.normalized * PlayerSpeed * Time.deltaTime;

        // If the player is in an enemy zone, limit their movement within the zone
        if (currentZoneManager != null)
        {
            Vector3 newPosition = transform.position + move;
            Vector3 limitedPosition = currentZoneManager.LimitPositionToZone(newPosition);

            // Move character controller to the limited position
            Vector3 offset = limitedPosition - transform.position;
            cC.Move(offset);
        }
        else
        {
            // Move normally
            cC.Move(move);
        }
    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        return Physics.CheckSphere(spherePos, cC.radius - 0.05f, surfaceMask);
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        cC.Move(velocity * Time.deltaTime);
    }

    private void OnPlayerEnemyZoneEnter(Enemy_Zone_Manager zoneManager)
    {
        currentZoneManager = zoneManager;
        Debug.Log("Entered enemy zone: " + zoneManager.name);

        // Call the AudioManager method to switch to the enemy zone audio
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetAudioToEnemyZone();
        }
    }

    private void OnPlayerEnemyZoneComplete(Enemy_Zone_Manager zoneManager)
    {
        if (currentZoneManager == zoneManager)
        {
            currentZoneManager = null;
            Debug.Log("Enemy zone complete: " + zoneManager.name);

            // Call the AudioManager method to reset to the default audio
            if (AudioManager.instance != null)
            {
                AudioManager.instance.ResetAudioToDefault();
            }
        }
    }

    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);
        audioSource.PlayOneShot(playerHurtSound);

        if (presentHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            AimCanvas.SetActive(false);
            // Ensure the cursor is unlocked for interacting with the UI
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Optionally, you can destroy the player after a short delay or simply disable movement:
        _isAbleToMove = false;
            Time.timeScale = 0f;

    }

      public void ReplayGame()
    {
                            Time.timeScale = 1f;

        // Reload the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PauseMenuPannel.SetActive(false);

        // Optional: Reset the player health or other stats here if needed
        presentHealth = playerHealth;
    }
    public void Back()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }


    void HPFiller()
    {
        hp.fillAmount = Mathf.Lerp(hp.fillAmount, (presentHealth / playerHealth), 3f * Time.deltaTime);
    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (presentHealth / playerHealth));
        hp.color = healthColor;
    }

    public void DisablePlayerMovement()
{
    // Disable movement
    _isAbleToMove = false;

    // Optionally, disable the CharacterController to ensure no movement occurs
    if (cC != null)
    {
        cC.enabled = false;
    }

    // Stop any animation related to movement
    if (animator != null)
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
    }

    // Stop any other actions related to movement, such as camera input
    xAxis = 0;
    yAxis = 0;

    // Optionally, stop any audio related to player movement or actions
    if (audioSource != null && audioSource.isPlaying)
    {
        audioSource.Stop();
    }

    Debug.Log("Player movement has been disabled.");
}

}
