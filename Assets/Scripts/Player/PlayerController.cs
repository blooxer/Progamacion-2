
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController :  Character
{

    // animator hash
    int isAttackingHash = Animator.StringToHash("isAttacking");
    int isMoveHash = Animator.StringToHash("isMove");
    int isJumpHash = Animator.StringToHash("isJumping");

    // references variables
    PlayerInput playerinput;
    CharacterController controller;
    Animator animator;
  
    
    //variables in player input values
    Vector2 currentaMovementInput;  
    Vector3 currentMovement;
    bool isMouseOrKeyboard { get { return playerinput.PlayerController.enabled; } }
    bool isMovementPressed;
    bool isAttacking;

    //gravity variables
    float groundedGravity = -0.05f;
    float gravityAir = -9.8f;

    //Movement variables
    [Header("Movement variables")]
    [SerializeField] float speed = 6f;
    float targetRotation = 0f; 
    float rotationVel;
    [SerializeField] float rotationSmootTime = 0.12f; // bt 0.0-0.3
    // Jump Variables   
       [Header("Jump Variables")]
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 2.0f;
    float maxJumpTime = 0.6f;
    float fallMultipler = 2.50f;
    bool isJumping = false;
  
    
    //Life Variables


    // camera variables
    [Header("Camera variables")]
    [SerializeField]  Camera cam;
    bool lockCameraPosition = false;
     float _cinemachineTargetYaw;
     float _cinemachineTargetPitch;
    Vector3 movementCameraBasedInput;
    [SerializeField]  float topClamp = 70.0f; //"How far in degrees can you move the camera up"
    [SerializeField]  float bottomClamp = -30.0f; // "How far in degrees can you move the camera down"
    float cameraAngleOverride = 0.0f;
    [SerializeField] GameObject cinemachineCameraTarget;

    private void Awake()
    {
        playerinput = new PlayerInput();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        

        //player inputs callbacks
        playerinput.PlayerController.Move.started += onMovementInput;
        playerinput.PlayerController.Move.canceled += onMovementInput;
        playerinput.PlayerController.Move.performed += onMovementInput;
        playerinput.PlayerController.Attack.started += onAttackInput;
        playerinput.PlayerController.Attack.canceled += onAttackInput;
        playerinput.PlayerController.Look.started += onLookCamInput;
        playerinput.PlayerController.Look.canceled += onLookCamInput;
        playerinput.PlayerController.Look.performed += onLookCamInput;
        playerinput.PlayerController.Jump.started += onJumpInput;
        playerinput.PlayerController.Jump.canceled += onJumpInput;

        setupJumpVariables();
    }
    private void Start()
    {
        currentHealth = maxHealth;
        cinemachineCameraTarget = GameObject.FindGameObjectWithTag("CinemachineCameraTarget");
        _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
     
    }
    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2; // top time in parabolla (simmetrical jump)
        gravityAir = (-2 * maxJumpHeight) / MathF.Pow(timeToApex, 2); // calculate the gravity based in the maxjump height and time to apex
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
    void HandleJump()
    {
        if (!isJumping && controller.isGrounded && isJumpPressed)
        {
            isJumping = true;
            animator.SetBool(isJumpHash, true);
            currentMovement.y = initialJumpVelocity;
        }else if(!isJumpPressed && isJumping && controller.isGrounded || isJumpPressed && !isJumping )
        {
            isJumping= false;
            
        }
    }
   void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0  || !isJumpPressed;
       
        if(controller.isGrounded)
        {
           

            currentMovement.y = groundedGravity;
            animator.SetBool(isJumpHash, false);
            

        }
        else if (isFalling )
        {
            float previousYVel = currentMovement.y;
            float actualYVel = currentMovement.y + (gravityAir * fallMultipler * Time.deltaTime);
            float nextYVel = MathF.Max((previousYVel + actualYVel) * .5f,  -20.0f);
            currentMovement.y = nextYVel;
          
        }
        else 
        {
            float previousYVel = currentMovement.y;
            float actualYVel = currentMovement.y + (gravityAir * Time.deltaTime);
            float nextYVel = (previousYVel + actualYVel) * .5f;
            currentMovement.y = nextYVel;
        
            // Velcity Verlet for framme rate independet in jump or fall moment
        }
   
      
    }
    void Update()
    {
        HandleGravity();
        HandleJump();
        Move();
      
        handleAnimation();
 
    }
    private void LateUpdate()
    {
        handleRotationCamera();
        
    }

    private void onAttackInput(InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton();
    }
    private void onJumpInput(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    void onLookCamInput(InputAction.CallbackContext context)
    {
        movementCameraBasedInput = context.ReadValue<Vector2>();
  
    }
    void onMovementInput(InputAction.CallbackContext context)
    {
        currentaMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentaMovementInput.x;
        currentMovement.z = currentaMovementInput.y ;
        isMovementPressed = currentaMovementInput.x != 0 || currentaMovementInput.y != 0;

        
    }
    void handleRotationCamera()
    {

        if(!lockCameraPosition)
        {
            float deltaMult = isMouseOrKeyboard ? 1.0f : Time.deltaTime;
            _cinemachineTargetYaw += movementCameraBasedInput.x * deltaMult;
            _cinemachineTargetPitch += movementCameraBasedInput.y * deltaMult;
        }

        //clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue); // Horizontal rotation in degrees of camera in the GameObject camera
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp); // Vertical inclination in degrees of camera in the GameObject camera whit limits (btClamp && tpClamp)

        //Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        


        //Rotation based in where looking player

        //Vector3 positionToLookAt;
        
        //positionToLookAt.x = currentMovement.x;
        //positionToLookAt.y = 0.0f;
        //positionToLookAt.z = currentMovement.z;
        
        //Quaternion currentRotation = transform.rotation;

        //if(isMovementPressed)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
           
        //    transform.rotation = Quaternion.Slerp(currentRotation,targetRotation, RrotationFactorPerFrame);
        //}
    }

   
    void Move()
    {
      


        if (currentaMovementInput != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(currentMovement.x, currentMovement.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y; // transform the movement into radians and then make them degrees and base them on the relative movement of the camera

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVel, rotationSmootTime); //Smoot rotation

            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime) );
        }
        controller.Move(currentMovement * Time.deltaTime);
    }

    void handleAnimation()
    {
        //get values from animator
        bool isRunning = animator.GetBool(isMoveHash);
        //bool isAttack = animator.GetBool("isAttack");

        if(isMovementPressed && ! isRunning)
        {
            animator.SetBool(isMoveHash, true);
        

        }
        else if (!isMovementPressed && isRunning)
        {
            animator.SetBool(isMoveHash, false);
        }

        if(isAttacking && controller.isGrounded)
        {
            animator.SetBool(isAttackingHash, true);

        }
        else { animator.SetBool(isAttackingHash, false); }
    }

    private void OnEnable()
    {
        playerinput.PlayerController.Enable();
    }
    private void OnDisable()
    {
        playerinput.PlayerController.Disable();
    }


   
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        // Turn Around in degrees 
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    //public void TakeDamage(int dmg)
    //{
    //    currentHealth -= dmg;

    //    Debug.Log("Player recibió daño. Vida actual: " + currentHealth + "Daño recibido " + dmg);

    //    if (currentHealth <= 0)
    //    {
    //        Die();
    //    }
    //}

    //void Die()
    //{
    //    Debug.Log("Player murió");
    //}



}
