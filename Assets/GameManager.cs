using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    [SerializeField] public Canvas introCanvas;
    [SerializeField] public Canvas gameOverCanvas;
    [SerializeField] public TMP_Text gameOverScoreBoard;
    [SerializeField] public TMP_Text inGameScoreBoard;
    [SerializeField] public Canvas pauseCanvas;
    [SerializeField] public Canvas scoreCanvas;
    [SerializeField] public Spawner spawner;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public AudioClip music;
    [SerializeField] public AudioClip gameOverClip;
    public int score = 0;
    public bool gameOver = false;
    private AudioSource audioSource;
    

    // Use this for initialization
    void Start () {

        initializeGame();
        audioSource = GetComponent<AudioSource>();
    }

    private void initializeGame()
    {
        introCanvas.gameObject.SetActive(true);
        gameOverCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(false);
        spawner.on = false;
        score = 0;
        inGameScoreBoard.SetText("Score: " + score);
        gameOver = true;
        if (!GameObject.FindGameObjectWithTag("Player")) Instantiate(playerPrefab, new Vector3(1.5f, 0.5f, 0f), Quaternion.identity);

        MemorySpace[] memorySpaces = GameObject.FindObjectsOfType<MemorySpace>();
        for (int i = 0; i < memorySpaces.Length; i++)
        {
            memorySpaces[i].Unlock();
            memorySpaces[i].UnOccupySpace();
        }
    }

    // Update is called once per frame
    void Update () {
        if (!spawner.canSpawn && !gameOver) {
            RegisterDeath();
            gameOver = true;
        }

        if (Input.GetKeyDown("escape") && !gameOver) {
            TogglePause();
        }
	}

    public void StartGame() {
        introCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(true);
        gameOver = false;
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.volume = 0.2f;
        audioSource.Play();

        MemorySpace[] memorySpaces = GameObject.FindObjectsOfType<MemorySpace>();
        for (int i = 0; i < memorySpaces.Length; i++)
        {
            memorySpaces[i].Unlock();
            memorySpaces[i].UnOccupySpace();
            memorySpaces[i].UnHighlight();
        }
        spawner.on = true;
    }

    public void RegisterDeath() {
        PlayerMovement player = GameObject.FindObjectOfType<PlayerMovement>();
        if (player != null) player.Kill();
        StartCoroutine("EndGame");
    }

    public IEnumerator EndGame() {

        spawner.on = false;

        yield return new WaitForSeconds(2);

        introCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(true);
        gameOverScoreBoard.SetText("Score:  " + score);
        pauseCanvas.gameObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(false);
        audioSource.clip = gameOverClip;
        audioSource.loop = false;
        audioSource.volume = 1f;
        audioSource.Play();


    }

    public void ResetGame() {
        initializeGame();
        StartGame();
    }

    public void RegisterScore() {
        score++;
        inGameScoreBoard.SetText("Score: " + score);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TogglePause() {
        introCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(!pauseCanvas.gameObject.activeInHierarchy);
    }
}
