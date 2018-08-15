using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour {

    [SerializeField] public TMP_Text text;
    [SerializeField] public int value = 0;
    [SerializeField] Sprite lowSprite;
    [SerializeField] Sprite mediumSprite;
    [SerializeField] Sprite highSprite;

    // Use this for initialization
    void Start () {
        setValue(value);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setValue(int value) {
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
        else {

            GetComponent<SpriteRenderer>().sprite = highSprite;
        }
    }
}
