using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject particle;

    [SerializeField]
    private float speed;
    bool started;
    Rigidbody rb;
    bool gameOver;

    void Awake()
    {
        rb = GetComponent<Rigidbody> ();
    }
    // Start is called before the first frame update
    void Start()
    {
        started = false;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            // Get the mouse position
            Vector2 mousePos = Input.mousePosition;

            // Check if the mouse is within the left half of the screen
            if (mousePos.y <= Screen.height * 0.8)
            {
                // Check if the left mouse button is down
                if (Input.GetMouseButtonDown(0))
                {
                    rb.velocity = new Vector3(speed, 0, 0);
                    started = true;

                    GameManager.instance.StartGame();
                }
            }
        }

        Debug.DrawRay(transform.position, Vector3.down, Color.red);

        if(! Physics.Raycast(transform.position, Vector3.down, 1f))
        {
            gameOver = true;
            rb.velocity = new Vector3(0, -25f, 0);

            Camera.main.GetComponent<CameraFollow> ().gameOver = true;

            GameManager.instance.GameOver();
        }

        if(Input.GetMouseButtonDown(0) && !gameOver)
        {
            SwitchDirection();
        }
    }
    void SwitchDirection()
    {
        if(rb.velocity.z > 0)
        {
            rb.velocity = new Vector3 (speed, 0, 0);
        }
        else if(rb.velocity.x > 0)
        {
            rb.velocity = new Vector3(0, 0, speed);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Stars")
        {
            GameObject part = Instantiate(particle, col.gameObject.transform.position, Quaternion.identity) as GameObject;
            
            Destroy(col.gameObject);
            Destroy(part, 1f); 
        }
    }
}
