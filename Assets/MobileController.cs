using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.ThirdPerson;

public class MobileController : MonoBehaviour
{
    public bool isSelected;
    public PlayerSelect playerSelectobj;
    Vector3 velocity;
    Rigidbody myRigidbody;
    Camera viewCamera;


    public ParticleSystem playerDeathParticle;
    [Header("Healthbar Stuff")]
    public float playerStartHealth;
    public float playerCurrentHealth;

    public LayerMask mask;


    [Header("Shooting Stuff")]
    public GameObject throwBallSpot;
    public GameObject snowBallGO;

    [Header("Animation Stuff")]
    private Animator animationComp;
    [Header("Movement Stuff")]

    public float moveSpeed = 7;
    public Vector3 moveToHere;
    private int terrainMask;
    private int throwMask;
    public Transform moveToObj;
    private AICharacterControl aiActivate;
    public GameObject bankParticle;
    private Player3DExample player3DExample;


    private void Awake()
    {
        moveToObj = GameObject.FindWithTag("MoveToGameOBJ").transform;
        moveToHere = transform.position;
        playerCurrentHealth = playerStartHealth;
        animationComp = this.gameObject.GetComponent<Animator>();
        playerSelectobj = gameObject.GetComponentInParent<PlayerSelect>();
        aiActivate = gameObject.GetComponent<AICharacterControl>();
        player3DExample = gameObject.GetComponent<Player3DExample>();


    }
    private void Update()
    {
        ShootingBalls();
        BuildingSnowBank();
    }

    private void FixedUpdate()
    {
        MouseMove();
        MovingAround();
    }

    void Start()
    {
        Input.simulateMouseWithTouches = false;
        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        viewCamera = Camera.main;
        terrainMask = LayerMask.GetMask("Terrain");
        throwMask = LayerMask.GetMask("ThrowableArea");
        if (isSelected)
        {
            animationComp.SetBool("isSelectedDontDance", true);
        }

        if (aiActivate.isJoyActive == true)
        {
            player3DExample.enabled = true;
        }
        aiActivate.enabled = true;
        moveToObj.transform.position = new Vector3(243.5f, -12.4f, 90.8f);
        gameObject.tag = "ActivePlayer";
        //buildBank.onClick.AddListener(BuildSnowBank);
    }
    //double Click Script
    //        public virtual void OnPointerClick(PointerEventData eventData)
    //{
    //    if (eventData.clickCount == 2)
    //    {
    //        Debug.Log("double click");
    //    }
    //}


    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "EnemySnowball")
        {
            animationComp.Play("GreenHit");
            playerCurrentHealth -= 1f;
            if (playerCurrentHealth <= 0)
            {
                AudioManager.instance.Play("PlayerDeath");
                Destroy(gameObject);
                ParticleSystem newDeathParticle = Instantiate(playerDeathParticle, transform.position, transform.rotation) as ParticleSystem;
            }
        }
    }


    private void NewShootMethod()
    {
        if (!PauseMenu.gameIsPaused)
        {
            GameObject newProjectile = Instantiate(snowBallGO, throwBallSpot.transform.position, throwBallSpot.transform.rotation);
            newProjectile.GetComponent<Projectile>().shooter = transform;
        }

    }

    private void ShootingBalls()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mouseHitThrow;
        if (Physics.Raycast(mouseRay, out mouseHitThrow, Mathf.Infinity, throwMask) && !IsPointerOverUIObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                transform.LookAt(mouseHitThrow.point);
                animationComp.SetBool("isMouseDown", true);
                animationComp.Play("GreenWindUp");
            }
            if (Input.GetMouseButton(0))
            {
                transform.LookAt(mouseHitThrow.point);
            }
            if (Input.GetMouseButtonUp(0))
            {
                transform.LookAt(mouseHitThrow.point);
                animationComp.SetBool("isMouseDown", false);
                animationComp.Play("GreenThrow");
                NewShootMethod();
            }
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[i].position);
            RaycastHit hitThrow;
            if (Physics.Raycast(ray, out hitThrow, Mathf.Infinity, throwMask) && !IsPointerOverUIObject())
            {
                if (Input.touches[i].phase == TouchPhase.Began)
                {
                    transform.LookAt(hitThrow.point);
                    animationComp.SetBool("isMouseDown", true);
                    animationComp.Play("GreenWindUp");
                }
                if (Input.touches[i].phase == TouchPhase.Ended)
                {
                    transform.LookAt(hitThrow.point);
                    animationComp.SetBool("isMouseDown", false);
                    animationComp.Play("GreenThrow");
                    NewShootMethod();
                }
                transform.LookAt(mouseHitThrow.point);
            }
        }
    }

    private void MouseMove()
    {
        if (!IsPointerOverUIObject() && isSelected)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(mouseRay, out mouseHit, Mathf.Infinity, terrainMask))
                {
                    moveToObj.transform.position = mouseHit.point;
                   // moveToHere = new Vector3(mouseHit.point.x, mouseHit.point.y, mouseHit.point.z);
                }
            }
            //Vector3 direction = (moveToHere - transform.position) * moveSpeed * Time.deltaTime;
            //float stopMoving = 5f;
            //if (Mathf.Abs(direction.x) > stopMoving | Mathf.Abs(direction.z) > stopMoving)
            //{
            //    myRigidbody.velocity = direction;
            //}
            //else
            //{

            //    moveToHere = transform.position;
            //}
        }
    }

    private void MovingAround()
    {
        if (!IsPointerOverUIObject() && isSelected)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Ray touchray = Camera.main.ScreenPointToRay(Input.touches[i].position);
                RaycastHit touchhit;
                if (Physics.Raycast(touchray, out touchhit, Mathf.Infinity, terrainMask))
                {
                    moveToObj.transform.position = touchhit.point;
                    //moveToHere = new Vector3(hit.point.x, -12.71f, hit.point.z);
                }

            }
            //Vector3 direction = (moveToHere - transform.position) * moveSpeed * Time.deltaTime;
            //float stopMoving = 5f;
            //if (Mathf.Abs(direction.x) > stopMoving | Mathf.Abs(direction.z) > stopMoving)
            //{
            //    myRigidbody.velocity = direction;
            //}
            //else
            //{
            //    moveToHere = transform.position;
            //}
        }

    }

    //CHECKS IF TOUCH IS OVER UI AND IGNORES IT!
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    public void BuildingSnowBank()
    {
        if (playerSelectobj.buildButtonDown == true)
        {
            bankParticle.SetActive(true);
        }
        if (playerSelectobj.buildButtonDown == false)
        {
            bankParticle.SetActive(false);
        }

    }
}
