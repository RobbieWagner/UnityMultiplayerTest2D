using System.Collections;
using System.Collections.Generic;
using AvocadoShark;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterOption
{
    public NetworkObject characterPrefab;
    public RuntimeAnimatorController animator;
    public Sprite characterVisual;
}

public class CharacterSelectionController : MonoBehaviour
{

    [SerializeField] private List<CharacterOption> characterOptions;
    [SerializeField] private FusionConnection fusionConnection;
    [SerializeField] private Image menuDisplayCharacter;

    [SerializeField] private Button selectNextButton;
    [SerializeField] private Button selectPreviousButton;

    [HideInInspector] public int currentCharacterIndex;
    [HideInInspector] public CharacterOption currentSelectedCharacter;

    private void Awake()
    {
        currentCharacterIndex = 0;
        currentSelectedCharacter = characterOptions[currentCharacterIndex];
        SetCurrentCharacter(currentSelectedCharacter.characterVisual);
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
        fusionConnection.playerPrefab = currentSelectedCharacter.characterPrefab.gameObject;
        SetCurrentCharacter(currentSelectedCharacter.characterVisual);
    }

    private void SetCurrentCharacter(Sprite character)
    {
        menuDisplayCharacter.sprite = character;
    }
}
