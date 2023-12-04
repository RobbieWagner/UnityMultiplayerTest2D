using System.Collections;
using System.Collections.Generic;
using AvocadoShark;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{

    [SerializeField] private List<AnimatorController> characterOptions;
    [SerializeField] private FusionConnection fusionConnection;
    [SerializeField] private Animator menuDisplayCharacter;

    [SerializeField] private Button selectNextButton;
    [SerializeField] private Button selectPreviousButton;

    [HideInInspector] public int currentCharacterIndex;
    [HideInInspector] public AnimatorController currentSelectedCharacter;

    private void Awake()
    {
        currentCharacterIndex = 0;
        currentSelectedCharacter = characterOptions[currentCharacterIndex];
        SetCurrentCharacter(currentSelectedCharacter);
    }

    private void OnEnable()
    {
        selectNextButton?.onClick.AddListener(() => ChangeCharacter(true));
        selectPreviousButton?.onClick.AddListener(() => ChangeCharacter(false));
    }

    private void OnDisable()
    {
        selectNextButton?.onClick.RemoveListener(() => ChangeCharacter(true));
        selectPreviousButton?.onClick.RemoveListener(() => ChangeCharacter(false));
    }

    private void ChangeCharacter(bool increment)
    {
        if(increment)
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characterOptions.Count;
        }
        else
        {
            currentCharacterIndex--;
            if(currentCharacterIndex < 0) currentCharacterIndex = characterOptions.Count-1;
        }

        currentSelectedCharacter = characterOptions[currentCharacterIndex];
        SetCurrentCharacter(currentSelectedCharacter);
    }

    private void SetCurrentCharacter(AnimatorController character)
    {
        menuDisplayCharacter.runtimeAnimatorController = character;
    }
}
