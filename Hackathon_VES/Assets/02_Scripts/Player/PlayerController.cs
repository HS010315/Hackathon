using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStateInfo playerStateInfo;
    Rigidbody rb;
    [Header("Rotate")]
    public float mouseSpeed;
    float yRotation;
    float xRotation;
    Camera cam;
    [Header("Move")]
    public float moveSpeed;
    float h;
    float v;
    private bool isRunning = false;
    private bool isCrouching = false;
    private bool isMoving = false;
    private bool isMoveable = true;
    private bool isDie = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerStateInfo = GetComponent<PlayerStateInfo>();
        cam = Camera.main;
        playerStateInfo.ChangeState(PlayerState.Idle);  
    }

    void Update()
    {
        if(playerStateInfo.Hp <= 0)
        {
            isDie = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if(!isDie)
        {
            Rotate();
            if (isMoveable)
            {
                Move();
            }
        }
    }

    void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -50f, 50f);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.forward * v + transform.right * h;
        Vector3 velocity = moveDirection.normalized * moveSpeed;

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (h != 0 || v != 0)
        {
            if (!isMoving)
            {
                isMoving = true;
                if (!isRunning && !isCrouching)
                {
                    playerStateInfo.ChangeState(PlayerState.Walking);
                }
            }
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                if (!isRunning && !isCrouching)
                {
                    playerStateInfo.ChangeState(PlayerState.Idle);
                }
            }
        }

        if (!isCrouching && Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
            moveSpeed *= 2;
            playerStateInfo.ChangeState(PlayerState.Running);
        }
        if (!isCrouching && isRunning && Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            moveSpeed /= 2;
            if (isMoving)
            {
                playerStateInfo.ChangeState(PlayerState.Walking);
            }
            else
            {
                playerStateInfo.ChangeState(PlayerState.Idle);
            }
        }

        if (!isRunning && Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = true;
            moveSpeed /= 2;
            playerStateInfo.ChangeState(PlayerState.Crouching);
        }
        if (!isRunning && isCrouching && Input.GetKeyUp(KeyCode.C))
        {
            moveSpeed *= 2;
            if (isMoving)
            {
                playerStateInfo.ChangeState(PlayerState.Walking);
            }
            else
            {
                playerStateInfo.ChangeState(PlayerState.Idle);
            }
            isCrouching = false;
        }
    }
    public void SetMoveable(bool value)
    {
        isMoveable = value;
    }
}