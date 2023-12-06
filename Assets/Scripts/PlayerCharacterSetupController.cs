using System.Collections;
using System.Collections.Generic;
using AvocadoShark;
using Fusion;
using RobbieWagnerGames;
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
            InitializeAnimator(playerObject);
    }

    private void InitializeAnimator(NetworkObject playerObject)
    {
        UnitAnimator unitAnimator = playerObject.GetComponent<UnitAnimator>();

        unitAnimator.SelectedCharacterAnimator = characterSelectionController.currentSelectedCharacter.animator;
        animator.runtimeAnimatorController = characterSelectionController.currentSelectedCharacter.animator;
    }
}
