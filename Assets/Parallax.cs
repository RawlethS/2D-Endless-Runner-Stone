using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Depth = 1 is same depth as player
    public float depth = 1;

    Collider m_Collider;
    Vector3 m_Center;
    Vector3 m_Size, m_Min, m_Max;

    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<Collider>();
        //Fetch the center of the Collider volume
        m_Center = m_Collider.bounds.center;
        //Fetch the size of the Collider volume
        m_Size = m_Collider.bounds.size;
        //Fetch the minimum and maximum bounds of the Collider volume
        m_Min = m_Collider.bounds.min;
        m_Max = m_Collider.bounds.max;
        //Output this data into the console
        OutputData();
    }

    void OutputData()
    {
        //Output to the console the center and size of the Collider volume
        Debug.Log("Collider Center : " + m_Center);
        Debug.Log("Collider Size : " + m_Size);
        Debug.Log("Collider bound Minimum : " + m_Min);
        Debug.Log("Collider bound Maximum : " + m_Max);
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

        /*
        if (pos.x <= 0 - size.x)
            pos.x = 80;
        */

        /*
        if (pos.x <= -25)
            pos.x = 80;
        */

        transform.position = pos;
    }
}
