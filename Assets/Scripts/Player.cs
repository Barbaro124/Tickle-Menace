using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CharacterController characterController;
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;
    private bool ShouldTickle => Input.GetMouseButtonDown(0) && closeToEnemy == true;

    [Header("Functional Options")]
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool tickleMode = true;

    [Header("Controls")]
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    /// <summary>
    /// [SerializeField] private MouseButton tickleButton = MouseButton.Left;
    /// </summary>

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float StandingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);

    [Header("Tickle Parameters")]
    private Vector2 tickleVector;
    private int tickleMeter;
    bool enemyTickled = false;
    //previous Vector
    //Current Vector


    public bool isCrouching;
    public bool isTickling;
    private bool duringCrouchAnimation;
    public bool closeToEnemy = true;
    Vector2 twoFrameOldPoint = Vector2.zero;
    Vector2 oneFrameOldPoint = Vector2.zero;
    Vector2 mouseMovementOne = Vector2.zero;
    

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
       if (canCrouch == true)
            {
            HandleCrouch();
            }
       //if (tickleMode == true)
            //{
            Tickle();
            //}
       // Debug.Log("Tickle Meter: " + tickleMeter);
    }


    private void HandleCrouch()
    {
        if (ShouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }

    }
    private IEnumerator CrouchStand()
    {
        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? StandingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //sanity check 
        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }


    private void HandleTickle()
    {
       //if (ShouldTickle)
        //{
            Tickle();
        //}
    }

    private void Tickle()
    {
        
        Vector2 oneFrameOldPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
       
        Vector2 mouseMovementOne = oneFrameOldPoint - twoFrameOldPoint;

        //Vector2 currentFramePoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Vector2 currentFramePoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        if (Vector2.Dot(twoFrameOldPoint, oneFrameOldPoint) < 0)
        {
            //tickleMeter += 10;
            twoFrameOldPoint = oneFrameOldPoint;
            if (Vector2.Dot(oneFrameOldPoint, currentFramePoint) <0)
            {

            }
        }
       
        if (tickleMeter >= 100)
        {
            tickleMeter = 100;
            enemyTickled = true; 
        }
    }

}
