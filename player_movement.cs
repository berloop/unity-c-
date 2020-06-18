using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInput : MonoBehaviour
{
    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;
    public Animator playerAnimator;
    public float Speed;
    public float allowPlayerRotation;
    public Camera cam;
    public CharacterController controller;
    public bool isGrounded;
    private float verticalVel;
    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        InputMagnitude();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            //grounding the character..
            verticalVel -= 2;
        }

        moveVector = new Vector3 (0, verticalVel, 0);
        controller.Move(moveVector);
    }
    void PlayerMoveAndRotation()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if(blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);

        }
    }
    void InputMagnitude()
    {
        //calculating the input vectors..
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        playerAnimator.SetFloat("InputZ", InputZ, 0.0f, Time.deltaTime * 2f);
        playerAnimator.SetFloat("InputX", InputX, 0.0f, Time.deltaTime * 2f);

        //calculate magnitude of the input..
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;
        //the square magnitude....

        //physically moving the player..

        if(Speed > allowPlayerRotation)
        {
            playerAnimator.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
            PlayerMoveAndRotation();
        }
        else if(Speed < allowPlayerRotation)
        {
            playerAnimator.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
        }
    }
}
