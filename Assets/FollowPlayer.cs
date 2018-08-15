using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    [SerializeField] public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
	}
}
