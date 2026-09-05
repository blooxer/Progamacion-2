
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class PlayerController : MonoBehaviour
{
    
    // references variables
    PlayerInput playerinput;
    CharacterController controller;
    Animator animator;
  
    
    //variables in player input values
    Vector2 currentaMovementInput;  
    Vector3 currentbMovement;
    bool isMouseOrKeyboard { get { return playerinput.PlayerController.enabled; } }
    bool isMovementPressed;
    bool isAttacking;


    //Movement variables

    [SerializeField] float speed = 3f;
    float targetRotation = 0f; 
    float rotationVel;
    [SerializeField] float rotationSmootTime = 0.12f; // bt 0.0-0.3
 

    // camera variables
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
    }
    private void Start()
    {
        cinemachineCameraTarget = GameObject.FindGameObjectWithTag("CinemachineCameraTarget");
        _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
     
    }
    void Update()
    {
        handleAnimation();
        Move();
    }
    private void LateUpdate()
    {
        handleRotationCamera();
        
    }

    private void onAttackInput(InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton();
    }

    void onLookCamInput(InputAction.CallbackContext context)
    {
        movementCameraBasedInput = context.ReadValue<Vector2>();
  
    }
    void onMovementInput(InputAction.CallbackContext context)
    {
        currentaMovementInput = context.ReadValue<Vector2>();
        currentbMovement.x = currentaMovementInput.x;
        currentbMovement.z = currentaMovementInput.y ;
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
        
        //positionToLookAt.x = currentbMovement.x;
        //positionToLookAt.y = 0.0f;
        //positionToLookAt.z = currentbMovement.z;
        
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
            targetRotation = Mathf.Atan2(currentbMovement.x, currentbMovement.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y; // transform the movement into radians and then make them degrees and base them on the relative movement of the camera

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVel, rotationSmootTime); //Smoot rotation

            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime));
        }
    }

    void handleAnimation()
    {
        //get values from animator
        bool isRunning = animator.GetBool("IsMove");
        //bool isAttack = animator.GetBool("isAttack");

        if(isMovementPressed && ! isRunning)
        {
            animator.SetBool("IsMove", true);

        }else if (!isMovementPressed && isRunning)
        {
            animator.SetBool("IsMove", false);
        }

        if(isAttacking)
        {
            animator.SetBool("isAttacking", true);
          
        }else { animator.SetBool("isAttacking", false); }
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
}
