using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Interacting,
    Sleeping,
    Crouching,
    Sitting,
    Falling,
    Dying
}

public enum PlayInfo
{
    Hp,
    Hunger,
    Fatigue,
    Contamination,
    Panic
}

public class PlayerStateInfo : MonoBehaviour
{
    public int hp;                  //체력
    public int hunger;              //배고픔
    public int fatigue;             //피로
    public int contamination;       //오염
    public int panic;               //공황

    private PlayerState currentState;
    public PlayerState CurrentState
    {
        get { return currentState; }
        private set { currentState = value; }
    }
    //public Animator animator;
    public bool isSleeping = false;
    public PlayerController playerController;
    public GameTimer gameTimer;
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = Mathf.Clamp(value, 0, 100);
            if (hp == 0)
            {
                CurrentState = PlayerState.Dying;
            }
        }
    }

    public int Hunger
    {
        get { return hunger; }
        set
        {
            hunger = Mathf.Clamp(value, 0, 100);
            CheckPlayInfoValues();
        }
    }

    public int Fatigue
    {
        get { return fatigue; }
        set
        {
            fatigue = Mathf.Clamp(value, 0, 100);
            CheckPlayInfoValues();
        }
    }

    public int Contamination
    {
        get { return contamination; }
        set
        {
            contamination = Mathf.Clamp(value, 0, 100);
            CheckPlayInfoValues();
        }
    }

    public int Panic
    {
        get { return panic; }
        set
        {
            panic = Mathf.Clamp(value, 0, 100);
            CheckPlayInfoValues();
        }
    }

    void Start()
    {
        Hp = 100;
        Hunger = 50;
        Fatigue = 50;
        Contamination = 20;
        Panic = 20;
        CurrentState = PlayerState.Idle;
    }

    public void ChangeState(PlayerState newState)
    {
        if (!isSleeping || newState == PlayerState.Dying) // 잠자는 중이 아니거나 Dying 상태로 변경할 때만 상태 변경 허용
        {
            currentState = newState;
            //UpdateAnimator(currentState);
        }
        if(newState == PlayerState.Sleeping)
        {
            isSleeping = true;
        }
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }

    public void WakeUp()
    {
        if (isSleeping)
        {
            isSleeping = false;
            ChangeState(PlayerState.Idle);
            playerController.SetMoveable(true);
        }
    }

    /*private void UpdateAnimator(PlayerState newState)
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isInteracting", false);
        animator.SetBool("isSleeping", false);
        animator.SetBool("isCrouching", false);
        animator.SetBool("isSitting", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isDying", false);

        switch (newState)
        {
            case PlayerState.Walking:
                animator.SetBool("isWalking", true);
                break;
            case PlayerState.Running:
                animator.SetBool("isRunning", true);
                break;
            case PlayerState.Interacting:
                animator.SetBool("isInteracting", true);
                break;
            case PlayerState.Sleeping:
                animator.SetBool("isSleeping", true);
                isSleeping = true; // 잠자는 상태로 변경 시 플래그 설정
                break;
            case PlayerState.Crouching:
                animator.SetBool("isCrouching", true);
                break;
            case PlayerState.Dying:
                animator.SetBool("isDying", true);
                break;
            case PlayerState.Sitting:
                animator.SetBool("isSitting", true);
                break;
            case PlayerState.Falling:
                animator.SetBool("isFalling", true);
                break;
            default:
                animator.SetBool("isIdle", true);
                break;
        }
    }*/

    private void CheckPlayInfoValues()
    {
        if (Fatigue == 100)
        {
            Fatigue = 0;
            gameTimer.SpendHours(16);
            Hp -= 10;
        }
        else if(Panic == 100)
        {
            gameTimer.SpendHours(4);
            Hp -= 20;
            Panic -= 30;
        }
        else if(Hp == 0)
        {
            ChangeState(PlayerState.Dying);
        }
    }
    public void TakeDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("Player took " + damage + " damage. Current HP: " + Hp);
    }
}