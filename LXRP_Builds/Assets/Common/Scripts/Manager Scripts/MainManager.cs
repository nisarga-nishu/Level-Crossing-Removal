﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

// Singleton class
public class MainManager : MonoBehaviour
{
    // Singleton Members
    private static MainManager _instance;
    public static MainManager Instance { get { return _instance; } }

    GAMESTATE managerState;
    // Main selected character
    GameObject currentPlayerCharacter;
    public GameObject CurrentPlayerCharacter { get => currentPlayerCharacter; set => currentPlayerCharacter = value; }

    [SerializeField] WorldPlacementScript placementScript = null;

    [SerializeField] PlayerScript[] _arrayPlayers = null;

    [Space]
    [SerializeField] CarSpawnner carSpawnner = null;
    [SerializeField] CharacterSpawnner characterSpawnner = null;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        //StartCoroutine(PlayerSpawn());
        SetState(GAMESTATE.BEGIN);
    }

    // Function to change the state
    public void SetState(GAMESTATE newState)
    {
        if (managerState == newState)
        {
            return;
        }

        Debug.Log("Manager State changed from:  " + managerState + "  to:  " + newState);
        managerState = newState;
        HandleStateChangedEvent(managerState);
    }

    // Function to handle state changes
    void HandleStateChangedEvent(GAMESTATE state)
    {
        switch (state)
        {
            case GAMESTATE.BEGIN:
                InitializeGame();
                break;

            case GAMESTATE.PLACED:
                // Enable Game
                StartGame();
                break;
        }
    }

    private void InitializeGame()
    {
        if (Application.isEditor)
        {
            placementScript.SetState(ARSTATE.PLACEMENT);
            return;
        }

        placementScript.SetState(ARSTATE.TUTORIAL);
    }

    void StartGame()
    {
        //placementScript.enabled = false;

        StartCoroutine(PlayerSpawn());

        // Start Spawnning
        carSpawnner.StartCarSpawn();
        characterSpawnner.StartPedSpawn();
    }

    IEnumerator PlayerSpawn()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(5, 10));

        Debug.Log("Player Spawnner started");

        foreach (PlayerScript player in _arrayPlayers)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(10, 15));
            player.gameObject.SetActive(true);
        }
    }
}

