using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Acceleration in x-axis
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float maxXVelocity = 100;
    public float distance = 0;

    // Acceleration in y-axis
    public float gravity;
    public Vector2 velocity;
    public float jumpVelocity = 40;
    public float groundHeight = 10;
    public bool isGrounded;

    // Jumping
    public bool isHoldingJump = false;
    // In seconds
    public float maxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;
    public float jumpGroundThreshold = 2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // Only used for inputs
    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }

    }

    // Updates in between every frame (between 2 fixed updates)
    private void FixedUpdate()
    {
        // Grabs the current position at current update
        Vector2 pos = transform.position;

        // When in the air
        if (!isGrounded)
        {
            // Allows Player to hold jump (to go higher) until
            // a max time (therefore max height)
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }

            pos.y += velocity.y * Time.fixedDeltaTime;
            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }
            
            // Ground collider for player whilst jumping/in the air
            // 0.7f is a buffer zone (shifting it forward from model) for the player
            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;

            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider != null)
            {
                // Player is colliding with ground
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    groundHeight = ground.groundHeight;
                    pos.y = groundHeight;
                    velocity.y = 0;
                    isGrounded = true;
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);
        }

        // Updates distance
        distance += velocity.x * Time.fixedDeltaTime;

        // When on the ground, velocity increases to a plateau
        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

            // Checks whether player is grounded for when the player is grounded but not jumping/in the air
            // 0.7f is a buffer zone (now in the opposite direction) for player to jump off at the edge without immediately falling off
            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;

            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            // If player is not colliding with anything, then player is NOT grounded
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
        }

        // Updates position with changes
        transform.position = pos;
    }

}
