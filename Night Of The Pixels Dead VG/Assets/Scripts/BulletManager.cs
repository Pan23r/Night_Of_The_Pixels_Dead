using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static float distanceArea = 7f;
    Collider2D myCollider;
    Rigidbody2D myRigidbody;
    Transform playerTransform;
    Rigidbody2D playerRigidbody;
    LineRenderer myLineRenderer;
    SpriteRenderer sprite;
    public bool takePlayerDirection = true;
    string saveDirection = "";
    float velocityBullet = 50;
    float lineWidth = 0.2f;
    Color32 myColorStart = new Color32(200, 200, 200, 0);
    Color32 myColorEnd = new Color32(109, 109, 109, 90);
    Vector2 savePlayerPos = Vector2.zero; 

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        myLineRenderer = GetComponent<LineRenderer>();
        sprite = GetComponent<SpriteRenderer>();

        myLineRenderer.startWidth = lineWidth;
        myLineRenderer.endWidth = lineWidth;
        myLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        myLineRenderer.startColor = myColorStart;
        myLineRenderer.endColor = myColorEnd;

        //CREAZIONE DEL PROITETTILE A SCHERMO
        sprite.material = new Material(Shader.Find("Sprites/Default"));
        sprite.sprite = Resources.Load<Sprite>("Sprites/Bullet");
        sprite.color = Color.yellow;
        sprite.transform.localScale = new Vector2(2,2);
    }

    // Update is called once per frame
    void Update()
    {
        if (takePlayerDirection)
        {
            //Prendi direzione player
            takePlayerDirection = false;
            saveDirection = PlayerMovement.currentIdle;
            savePlayerPos = playerRigidbody.transform.position;
        }

        myLineRenderer.SetPosition(0, savePlayerPos);
        myLineRenderer.SetPosition(1, myRigidbody.transform.position);
        myRigidbody.velocity = BulletDirection();

    }
    
    Vector2 BulletDirection()
    {
        switch (saveDirection)
        {
            case "Front":
                sprite.transform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);
                return new Vector2(0, -velocityBullet);

            case "Back":
                sprite.transform.localRotation = Quaternion.AngleAxis(-90, Vector3.forward);
                return new Vector2(0, velocityBullet);

            case "Right":
                sprite.transform.localRotation = Quaternion.AngleAxis(180, Vector3.forward);
                return new Vector2(velocityBullet, 0);

            case "Left":
                sprite.transform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);
                return new Vector2(-velocityBullet, 0);
        }

        return Vector2.zero;
    }
    
    private void LateUpdate()
    {
        //DISATTIVA I PROIETTILI CHE COLLIDONO CON UNO ZOMBIE O SE ESCE DALLA MAPPA
        if(Vector2.Distance(transform.position, playerTransform.position) > distanceArea)
        {
            this.gameObject.SetActive(false);
            takePlayerDirection = true;
        }
    }
}
