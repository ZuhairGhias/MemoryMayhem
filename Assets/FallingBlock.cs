using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FallingBlock : MonoBehaviour {

    [SerializeField] public TMP_Text text;
    [SerializeField] public int value = 0;
    public float targetHeight;
    [SerializeField] public LayerMask memSpaceLayer;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] Sprite lowSprite;
    [SerializeField] Sprite mediumSprite;
    [SerializeField] Sprite highSprite;
    [SerializeField] GameObject particles;

    // Use this for initialization
    void Start()
    {
        setValue(value);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetHeight > transform.position.y) {
            Collider2D memoryCollider = Physics2D.OverlapBox(new Vector2(transform.position.x, targetHeight), new Vector2(0.25f, 0.25f), 0f, memSpaceLayer);
            MemorySpace memSpace = memoryCollider.GetComponent<MemorySpace>();

            memSpace.Unlock();
            memSpace.OccupySpace(value);

            Collider2D playerCollider = Physics2D.OverlapBox(new Vector2(transform.position.x, targetHeight), new Vector2(0.25f, 0.25f), 0f, playerLayer);
            if (playerCollider != null) {
                GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
                gameManager.RegisterDeath();
                print("Player Killed");
            }

            Instantiate(particles, transform.position - (0.5f*Vector3.up), Quaternion.Euler(-10, 90, 0));
            Instantiate(particles, transform.position - (0.5f * Vector3.up), Quaternion.Euler(-10, -90, 0));
            Destroy(gameObject);
        }
    }

    public void setValue(int value)
    {
        this.value = value;
        text.SetText(value.ToString());
        if (value <= 4)
        {

            GetComponent<SpriteRenderer>().sprite = lowSprite;
        }
        else if (value <= 8)
        {

            GetComponent<SpriteRenderer>().sprite = mediumSprite;
        }
        else
        {

            GetComponent<SpriteRenderer>().sprite = highSprite;
        }
    }

    internal void setTargetHeight(float y)
    {
        targetHeight = y;
        transform.position = new Vector3(transform.position.x, transform.position.y, -transform.position.y);
    }
}
