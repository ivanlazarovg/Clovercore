using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Animations;
using System.Net;

public class Movement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;

    public float gravity = -9.18f;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;

    public Transform cam;

    [SerializeField]
    Vector3 velocity;

    [SerializeField]
    public float bobFrequency = 10.5f;

    [SerializeField]
    public float bobAmpltudeY = 0.09f;

    [SerializeField]
    public float bobAmplitudeX = 0.03f;

    [SerializeField]
    float breathFrequency = 2.2f;

    [SerializeField]
    float breathAmplitudeY = 0.05f;

    [SerializeField]
    float breathAmplitudeX = 0.015f;

    [SerializeField]
    float breathAmplitudeZ = 0.015f;

    [SerializeField]
    float crouchAmount = 1f;

    [SerializeField]
    public float speedAmount;
    
    public float slowDownSpeed = 11;

    [SerializeField]
    float flySpeedOrigin;

    [SerializeField]
    float dampingCoefficient;

    [SerializeField]
    float liftOffSpeed;

    float hoverInTime;

    public float hoverIntensityX;
    public float hoverIntensityY;
    public float hoverFrequency;
    public float hoverSpeed;

    public bool isFlying;

    public bool isInteracting;

    public bool isHovering;

    Vector3 camOrigin;

    Vector3 bobTargetPosition;

    Vector3 breathTargetPosition;

    float movementCounter;

    float idleCounter;

    public bool isCrouching = false;

    public bool isMoving;

    [SerializeField]
    float flySpeed;

    public bool speedbool;

    [HideInInspector]
    public bool canMove = true;


    [SerializeField] private Material cloudMat;
    [SerializeField] private float cloudCoverageTarget;
    float footsteptime = 0;
    public float lerpSpeed;

    public Vector3 move;

    private static Movement instance;

    public static Movement Instance
    {
        get 
        { 
            instance = FindAnyObjectByType<Movement>();
            return instance;
        }
    }

    #region Main Movement
    void Start()
    {
        camOrigin = cam.localPosition;
    }

    void Update()
    {
        if (canMove) 
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.F))
            {
                isFlying = !isFlying;
                velocity.y = 0;
            }

            if (speedbool)
            {
                speed = slowDownSpeed;
            }
            else
            {
                speed = speedAmount;
            }

            move = transform.right * x + transform.forward * z;

            velocity = Vector3.Lerp(velocity, Vector3.zero, dampingCoefficient * Time.deltaTime);
            transform.position += velocity * Time.deltaTime;

            velocity.y = gravity;

            controller.Move(move * speed * Time.deltaTime);
            velocity.x = 0f; velocity.z = 0f;

            controller.Move(velocity * Time.deltaTime);

        }

        if (move == new Vector3(0, 0, 0))
        {
            StandStill();
        }
        else
        {
            Move();
        }

        if (Input.GetKey(KeyCode.C))
        {
            Crouch(crouchAmount);
        }
        else
        {
            speed = speedAmount;
        }


    }

    #endregion

    void HeadBob(float zPoint, float x_intensity, float y_intensity)
    {
        bobTargetPosition = camOrigin + new Vector3(Mathf.Cos(zPoint)
        * x_intensity, Mathf.Sin(zPoint * 2) * y_intensity, 0f);

    }
    void Breathe(float zPoint, float x_intensity, float y_intensity, float z_intensity)
    {
        breathTargetPosition = camOrigin + new Vector3(Mathf.Cos(zPoint)
        * x_intensity, Mathf.Sin(zPoint) * y_intensity, Mathf.Sin(zPoint) * z_intensity);
        
    }

    void Move()
    {
        HeadBob(movementCounter, bobAmplitudeX, bobAmpltudeY);
        movementCounter += Time.deltaTime * bobFrequency;
        cam.localPosition = bobTargetPosition;
        isMoving = true;
    }

    public void StandStill()
    {
        dampingCoefficient = 6;
        Breathe(idleCounter, breathAmplitudeX, breathAmplitudeY, breathAmplitudeZ);
        idleCounter += Time.deltaTime * breathFrequency;
        cam.localPosition = breathTargetPosition;
        isMoving = false;
    }

    void Crouch(float amount)
    {
        bobTargetPosition = new Vector3(bobTargetPosition.x, bobTargetPosition.y - amount, bobTargetPosition.z);
        breathTargetPosition = new Vector3(breathTargetPosition.x, breathTargetPosition.y - amount, breathTargetPosition.z);
        speed = 2.7f;
        if (!isMoving)
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, breathTargetPosition, Time.deltaTime * 2f);
        }
        else
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, bobTargetPosition, Time.deltaTime * 2f);
        }

    }

    void Fly()
    {
        velocity += GetMovementVector() * flySpeed * Time.deltaTime;  
    }

    private void StaticFloating()
    {
        Vector3 hoverTargetPosition = camOrigin + new Vector3(Mathf.Cos(hoverInTime)
        * hoverIntensityX, Mathf.Sin(hoverInTime) * hoverIntensityY, 0f);
        hoverInTime += Time.deltaTime * hoverFrequency;
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, hoverTargetPosition, Time.deltaTime * hoverSpeed);
    }

    Vector3 GetMovementVector()
    {
        Vector3 moveInput = default;

        moveInput += cam.transform.forward;

        return moveInput;
    }

}

