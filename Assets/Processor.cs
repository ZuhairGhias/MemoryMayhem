using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Processor : MonoBehaviour {

    [SerializeField] public MemorySpace memSpace1;
    [SerializeField] public MemorySpace memSpace2;
    [SerializeField] public MemorySpace ResultMemSpace;
    [SerializeField] public Animator animator;
    [SerializeField] public GameManager gameManager;
    [SerializeField] public TMP_Text Display;
    [SerializeField] public GameObject poofParticles;
    public int answer;
    public int lowAnswer = 4;
    public int highAnswer = 16;
    public int maxBlockSize = 8;

    // Use this for initialization
    void Start () {
        GenerateNewAnswer();
	}

    private void GenerateNewAnswer()
    {
        answer = UnityEngine.Random.Range(lowAnswer, highAnswer);
        Display.SetText(answer.ToString());
    }

    // Update is called once per frame
    void Update () {
        bool correctAnswer = false;
        if (memSpace1.occupied && memSpace2.occupied && !ResultMemSpace.occupied && !ResultMemSpace.locked) {
            if (memSpace1.block.GetComponent<Block>().value <= maxBlockSize && memSpace2.block.GetComponent<Block>().value <= maxBlockSize) {
                int result = memSpace1.UnOccupySpace() + memSpace2.UnOccupySpace();
                ResultMemSpace.OccupySpace(result);
                Instantiate(poofParticles, new Vector3(1.5f, 1.5f, -1.5f), Quaternion.identity);
            }
            
        }

        if (ResultMemSpace.occupied && ResultMemSpace.block.GetComponent<Block>().value == answer) {
            correctAnswer = true;
            gameManager.RegisterScore();
            GenerateNewAnswer();
            ResultMemSpace.UnOccupySpace();
            GetComponent<AudioSource>().Play();
            // play poof animation
        }
        animator.SetBool("correctAnswer", correctAnswer);
	}
}
