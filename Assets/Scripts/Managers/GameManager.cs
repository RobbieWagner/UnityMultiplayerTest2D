using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AvocadoShark;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public HashSet<PlayerStats> playersInGame;
    public static GameManager Instance {get; private set;}
    
    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

        playersInGame = FindObjectsOfType<PlayerStats>().ToHashSet();
    }

    public void AddNewPlayer(PlayerStats newPlayer)
    {
        playersInGame.Add(newPlayer);
        OnPlayerJoinGame(newPlayer);
    }
    
    public delegate void OnPlayerJoinGameDelegate(PlayerStats playerStats);
    public event OnPlayerJoinGameDelegate OnPlayerJoinGame;
}
