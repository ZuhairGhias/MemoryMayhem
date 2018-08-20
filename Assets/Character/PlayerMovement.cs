using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] public float speed = 5f;
    [SerializeField] private string direction;
    [SerializeField] private Vector3 target;
    [SerializeField] private bool carryingBlock;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject block;
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] public float height = 1;
    [SerializeField] public float reach = 0.6f;
    [SerializeField] public float rearClearance = 0.5f;
    [SerializeField] public GameObject deathAnimation;
    private MemorySpace previousSpace = null;
    private MemorySpace currentSpace = null;
    [SerializeField] public GameObject dustParticle;
    [SerializeField] public float dustParticleRate = 1;
    public bool isMoving = false;

    // Use this for initialization
    void Start()
    {
        previousSpace = null;
        currentSpace = null;
        target = new Vector3();
        carryingBlock = false;
        direction = "down";
        block = null;

        StartCoroutine("EmitDust");
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (direction == "left" || direction == "right") moveX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        else moveY = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

        isMoving = false;
        if (Mathf.Abs(moveX) > 0 || Mathf.Abs(moveY) > 0) isMoving = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        if(carryingBlock && block != null) block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, transform.position.y);
        GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x + moveX, transform.position.y + moveY));


        
        if (Input.GetKey("d"))
        {
            direction = "right";
            target = new Vector3(reach, 0, 0);
            animator.SetInteger("Direction", 3);
            
            
        }
        else if (Input.GetKey("a"))
        {
            direction = "left";
            target = new Vector3(-reach, 0, 0);
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey("w"))
        {
            direction = "up";
            target = new Vector3(0, reach, 0);
            animator.SetInteger("Direction", 0);
        }
        else if (Input.GetKey("s"))
        {
            direction = "down";
            target = new Vector3(0, -reach, 0);
            animator.SetInteger("Direction", 1);
        }
        bool spriteFlip = false;
        if (direction == "right") spriteFlip = true;
        spriteRenderer.flipX = spriteFlip;


        if (Input.GetKeyDown("space"))
        {

            if (carryingBlock)
            {
                DropBlock();
            }
            else
            {
                PickupBlock();
            }



        }
        animator.SetBool("Holding", carryingBlock);
        animator.SetBool("Moving", isMoving);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + target, target, 0, layerMask);
        if (hit) {
            currentSpace = hit.transform.GetComponent<MemorySpace>();
        }
        if (previousSpace) previousSpace.UnHighlight();
        if(currentSpace)currentSpace.HighLight();
        previousSpace = currentSpace;

    }

    private void DropBlock()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + target, target, 0, layerMask);
        RaycastHit2D hitFromBehind = Physics2D.Raycast(transform.position - (rearClearance * target.normalized), transform.position, 0, obstructionMask);

        if (hit && !hitFromBehind)
        {
            MemorySpace memSpace = hit.transform.GetComponent<MemorySpace>();
            currentSpace = memSpace;
            if (memSpace && memSpace.OccupySpace(block.GetComponent<Block>().value))
            {
                Destroy(block);
                carryingBlock = false;
            }
        }
    }

    private void PickupBlock()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + target, target, 0, layerMask);
        if (hit)
        {
            MemorySpace memSpace = hit.transform.GetComponent<MemorySpace>();
            currentSpace = memSpace;
            if (memSpace)
            {
                int value = memSpace.UnOccupySpace();
                if (value > 0) {
                    block = Instantiate(blockPrefab, transform.position + (height * Vector3.up), Quaternion.identity, gameObject.transform);
                    block.GetComponent<Block>().setValue(value);
                    carryingBlock = true;
                }
                

            }
        }
    }

    public void Kill() {
        print("here");
        GameObject death = Instantiate(deathAnimation);
        Destroy(death, 4);
        Destroy(gameObject);
    }

    IEnumerator EmitDust() {
        for (; ; ) {
            if (isMoving) Instantiate(dustParticle, transform.position - (0.2f * Vector3.up), Quaternion.identity);
            yield return new WaitForSeconds(dustParticleRate/10);
        }
    }
}
