using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Calculate the ball's trajectory regarding the touch's position

    /*
        1- Get the touch's position
        2- Calculate the distance between the player and the touch
        3- Use the distance as the sending power
        4- Draw a line between the player and the touch
        5- Send the player in the opposite direction of the line
    */


    // Referencing scripts
    public UIScript uiscr;
    public AdsManager adscr;


    // DrawDragLine variables
    Vector3 LineStartPos,LineEndPos;
    [SerializeField] AnimationCurve ac;
    Vector3 cameraOffSet = new Vector3(0,0,10);
    Camera cam;
    public LineRenderer lr;

    // Ball Physics variables
    public Rigidbody2D rb;
    Vector3 ThrowDir;
  

    // Death particle system
    public ParticleSystem deathps;
    public GameObject Respawn;
    public int DeathCount = 0;
    // Loading scenes
    public Scene loadedscene;
    public GameObject UIcontroler;


    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        deathps.Stop();
        uiscr.ShowGameUI();
        UIcontroler = GameObject.FindGameObjectWithTag("UIctrl");
        lr = GetComponent<LineRenderer>();
    }


    private void Update() {
        // Draw the drag line
        DrawDragLine();
        playad();


    }

    // DrawLine method
    void DrawDragLine()
    {
        
        if(Input.touchCount > 0)
        {
            if(lr == null)
            {
                lr = gameObject.AddComponent<LineRenderer>();
            }
            lr.widthCurve = ac;
            lr.enabled = true;
            lr.positionCount = 2;
            lr.useWorldSpace = true;
            lr.numCapVertices = 10;
            LineStartPos = transform.position + cameraOffSet;
            lr.SetPosition(0,LineStartPos);
            LineEndPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) + cameraOffSet;
            lr.SetPosition(1,LineEndPos);
            // Slow-Motion
            Time.timeScale = 0.3f;

            if(Input.GetTouch(0).phase == TouchPhase.Ended)
        {
        // Throw the ball when letting go of the Mouse
            BallTrajectory();
            Time.timeScale = 1f;
            lr.enabled = false;
        }

        }
        
    
    }

    // Ball Trajectory method
    void BallTrajectory()
    {
        // I want to take the drag vector and invert It. This will be the throw direction
        ThrowDir = LineStartPos - LineEndPos;
        rb.AddForce(ThrowDir,ForceMode2D.Impulse);
    }


    // Destory the player
    void DestroyPlayer()
    {
        GameObject go = Instantiate(deathps.gameObject);
        go.transform.position = this.transform.position;
        Destroy(go,1f);
        rb.velocity = Vector3.zero;
        transform.position = Respawn.transform.position;
        DeathCount += 1;
    }

    // Destory the player when hitting a spike
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Spikes"))
        {
            DestroyPlayer();
        }
        if(other.gameObject.CompareTag("Flag"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    void playad()
    {
        if(DeathCount % 10 == 0)
        {
            DeathCount++;
            adscr.PlayAd();
        }
    }

}
