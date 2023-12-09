using System.Collections;
using System.Collections.Generic;
using AvocadoShark;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class TopDownCharacterMovement : NetworkBehaviour
{

    public PlayerStats playerStats;
    
    private INetworkInteractable currentInteractable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentInteractable = other.GetComponent<INetworkInteractable>();
        Debug.Log("canInteract");
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        currentInteractable?.Interact(playerStats);
    }
}
