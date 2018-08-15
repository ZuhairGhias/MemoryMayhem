using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySpace : MonoBehaviour {

    [SerializeField]
    public bool occupied = false;
    [SerializeField]
    public GameObject blockPrefab;
    public GameObject block;
    public bool locked = false;
    public bool doNotSpawn = false;
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite lockedSprite;
    [SerializeField] public Sprite highlightedSprite;
    public bool highLighted = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public bool OccupySpace(int value) {
        if (!occupied && !locked) {
            block = Instantiate(blockPrefab, transform);
            block.GetComponent<Block>().setValue(value);
            occupied = true;
            return true;
        }

        return false;

    }

    public int UnOccupySpace()
    {
        if (occupied && !locked)
        {
            int value = block.GetComponent<Block>().value;
            Destroy(block);
            block = null;
            occupied = false;
            return value;
        }
        return -1;

    }

    public void Lock(){
        locked = true;
        GetComponent<SpriteRenderer>().sprite = lockedSprite;
    }

    public void Unlock()
    {
        locked = false;
        if (highLighted)
        {
            GetComponent<SpriteRenderer>().sprite = highlightedSprite;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
    }

    public void HighLight() {
        highLighted = true;
        if(!locked) GetComponent<SpriteRenderer>().sprite = highlightedSprite;
    }

    public void UnHighlight() {
        highLighted = false;
        if (!locked) GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
