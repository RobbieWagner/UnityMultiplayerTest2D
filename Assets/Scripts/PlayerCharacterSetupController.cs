using System.Collections;
using System.Collections.Generic;
using AvocadoShark;
using Fusion;
using UnityEngine;

public class PlayerCharacterSetupController : MonoBehaviour
{
    public Animator animator;
    public FusionConnection fusionConnection;
    public CharacterSelectionController characterSelectionController;

    public void SetupCharacter(NetworkObject playerObject)
    {
        animator = playerObject.GetComponent<Animator>();
        if(animator != null)
            InitializeAnimator();
    }

    private void InitializeAnimator()
    {
        animator.runtimeAnimatorController = characterSelectionController.currentSelectedCharacter;
    }
}
