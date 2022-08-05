using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight; // Top edge of a ground object
    public float groundRight; // Right side edge of a ground object
    public float screenRight; // Right border of the screen
    new BoxCollider2D collider;

    bool didGenerateGround = false;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        collider = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + (collider.size.y / 2); // Gets position of ground (top edge)
        screenRight = Camera.main.transform.position.x * 2; // Gets x position of right border of camera
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        groundRight = transform.position.x + (collider.size.x / 2);

        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }
        transform.position = pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float h1 = player.jumpVelocity * player.maxHoldJumpTime; // Max height player can get WHILE holding jump button (no gravity)
        float t = player.jumpVelocity / -player.gravity; // Ratio of how long it takes to reach velocity.y = 0 (zenith) based on gravity
                                                         // "-" since the value of gravity is negative but no direction is needed
                                                         // and only magnitude is needed (ie. its absolute value)
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        // Distance = Rate/Velocity * t + (with gravity so rate is constantly changes)
        // the (0.5f) in the equation is only the first half of the "parabolic" jump
        // Natural jump arc WITHOUT holding jump button AFTER h1
        // After letting go of jump button, gravity takes effect
        // Velocity decrementally gets smaller until velocity = 0, total height = max (zenith)
        // h2 starts at top of h1 to the peak/zenith
        float maxJumpHeight = h1 + h2; // Maximum possible height player can jump
        float maxY = maxJumpHeight * 0.6f; // Current max y height player can jump
                                            // Since player is not perfect, buffer zone with lower height
        maxY += groundHeight; // Based off go ground height
        float minY = 1; // So it doesn't start at bottom border
        float actualY = Random.Range(minY, maxY);

        pos.y = actualY - goCollider.size.y / 2; // Y positon of where to put go next
        if (pos.y > 1.7f)
        {
            pos.y = 1.7f; // limits height of go to below the distance ui
        }


        float t1 = t + player.maxMaxHoldJumpTime; // Natural jump arc time + jump time from button to reach zenith
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity); // Time taken to fall from maxY/zenith to actualY
        float totalTime = t1 + t2; // Time taken for the whole "parabolic" jump
        float maxX = totalTime * player.velocity.x; // Maximum jump distance
        maxX *= 0.75f; // Buffer zone
        maxX += groundRight;
        float minX = screenRight + 5; // Position of go being generated
        float actualX = Random.Range(minX, maxX);

        pos.x = actualX + goCollider.size.x / 2; // X position


        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);
    }
}
