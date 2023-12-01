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

    [SerializeField][Range(-1, 0)] private float wallPushback = -.08f;
    private HashSet<Collider2D> colliders;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        colliders = new HashSet<Collider2D>();
    }

    private void LateUpdate() 
    {

        if(canMove) 
        {
            transform.Translate(movementVector * currentWalkSpeed * Time.deltaTime);
            foreach(Collider2D collider in colliders)
                transform.position = Vector2.MoveTowards(transform.position, collider.transform.position, wallPushback);
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("new collision");
        colliders.Add(other.collider);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        colliders.Remove(other.collider);
    }
}
