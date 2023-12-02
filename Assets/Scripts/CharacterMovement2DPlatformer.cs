using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RobbieWagnerGames;
using System.Security.Cryptography;

public class CharacterMovement2DPlatformer : MonoBehaviour
{
    [SerializeField] private UnitAnimator unitAnimator;
    public bool canMove = true;
    [HideInInspector] public bool moving = false;
    private UnitAnimationState currentAnimationState;
    private PlayerInputActions inputActions;
    private Vector2 movementVector = Vector2.zero;
    [SerializeField] private float currentWalkSpeed;

    [SerializeField] private AudioSource footstepAudioSource;

    private Vector3 lastFramePos = Vector3.zero;

    private Rigidbody2D rb2d;
    [SerializeField] private float jumpForce;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate() 
    {

        if(canMove) 
        {
            transform.Translate(movementVector * currentWalkSpeed * Time.deltaTime);
        }

        lastFramePos = transform.position;

        if(moving && footstepAudioSource != null && !footstepAudioSource.isPlaying) 
        {
            PlayMovementSounds();
        }
    }

    private void OnMove(InputValue inputValue)
    {
        if(canMove)
        {
            float input = inputValue.Get<float>();

            if(movementVector.x != input && input != 0f)
            {
                if(input > 0) unitAnimator.ChangeAnimationState(UnitAnimationState.WalkRight);
                else unitAnimator.ChangeAnimationState(UnitAnimationState.WalkLeft);
                moving = true;
            }
            else
            {
                if(movementVector.x > 0)unitAnimator.ChangeAnimationState(UnitAnimationState.IdleRight);
                else unitAnimator.ChangeAnimationState(UnitAnimationState.IdleLeft);
                moving = false;
                if(footstepAudioSource != null) StopMovementSounds();
            }

            movementVector = new Vector2(input, movementVector.y);
        }
    }

    private void OnJump(InputValue inputValue)
    {
        rb2d.AddForce(Vector2.up * jumpForce);
        Debug.Log("jump");
    }

    private void OnDisable()
    {
        StopPlayer();
    }

    public void StopPlayer()
    {
        if(movementVector.x > 0) unitAnimator.ChangeAnimationState(UnitAnimationState.IdleRight);
        else unitAnimator.ChangeAnimationState(UnitAnimationState.IdleLeft);

        movementVector = Vector3.zero;
        moving = false;
        if(footstepAudioSource != null) StopMovementSounds();
    }

    public void CeasePlayerMovement()
    {
        canMove = false;
        StopPlayer();
    }

    public void PlayMovementSounds()
    {
        footstepAudioSource.Play();
    }

    public void StopMovementSounds()
    {
        footstepAudioSource.Stop();
    }
}
