using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Depth = 1 is same depth as player
    public float depth = 1;

    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        // The larger the depth, the slower the velocity relative to player velocity
        float realVelocity = player.velocity.x / depth;
        Vector2 pos = transform.position;

        // Since Player is moving "forward", background is left behind
        // going in negative velocity therefore going "backward"
        pos.x -= realVelocity * Time.fixedDeltaTime;

        // As soon as object leaves the camera on the left, it immediately gets put back beyond the right border
        // + added randomness with a higher scale for closer depth objects to the player
        if (pos.x <= 0 - transform.localScale.x / 2)
        {
            pos.x = (Camera.main.transform.localPosition.x * 2) + (transform.localScale.x / 2) + Random.Range(0, 1000 / depth);
        }

/*        if (pos.x <= -25)
            pos.x = 85;*/

        transform.position = pos;
    }
}
