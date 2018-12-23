using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

    
[RequireComponent(typeof(Rigidbody))]
public class DragRigidbody : MonoBehaviour
{

    public float force = 9000;
    public float damping = 500;
    Transform jointTrans;

    public ParticleSystem playerDeathParticle;
    [Header("Healthbar Stuff")]
    public float playerStartHealth;
    public float playerCurrentHealth;
    public Image healthBar;
    private int totalEnemiesLeft;
    public LayerMask mask;

    [Header("Shooting Stuff")]
    public GameObject throwBallSpot;
    public GameObject snowBallGO;
    public GameObject shootGuide;

    [Header("Animation Stuff")]
    private Animator animationComp;

    private void Awake()
    {
        playerCurrentHealth = playerStartHealth;
        animationComp = this.gameObject.GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        totalEnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (totalEnemiesLeft < 1)
        {
            //WIN
            StartCoroutine(WaitForIt(3.0F));
        }
    }
    void OnMouseDown()
    {
        HandleInputBegin(Input.mousePosition);
    }

    void OnMouseUp()
    {
        HandleInputEnd(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        HandleInput(Input.mousePosition);
    }

    public void HandleInputBegin(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (jointTrans != null)
        {
            Destroy(jointTrans.gameObject);
        }
        else
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactive"))
                {
                    jointTrans = AttachJoint(hit.rigidbody, hit.point);
                }
            }
        }
    }
// joint is attached -- We are then moving the joint below along a ray that hits the terrain
//the ray has to be set with an offset on the y (heightOfGround(changes when mouse is moved) + attachmentofJointfromGround(static when joint is created


  public void HandleInput(Vector3 screenPosition)
    {
        if (jointTrans == null)
            return;
        RaycastHit hitGround;
        Ray mouseray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(mouseray, out hitGround, 100, mask))
        {
            jointTrans.position = new Vector3(hitGround.point.x, hitGround.point.y + .25f, -1.32408f + hitGround.point.z);
        }
        animationComp.Play("WindUp");
        shootGuide.gameObject.SetActive(true);
    }

    public void HandleInputEnd(Vector3 screenPosition)
    {
        NewShootMethod();
        animationComp.Play("Throw");
        shootGuide.gameObject.SetActive(false);
        Destroy(jointTrans.gameObject);
    }

    Transform AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        GameObject go = new GameObject("Attachment Point");
        go.transform.position = attachmentPosition;
        var newRb = go.AddComponent<Rigidbody>();
        newRb.isKinematic = true;
        var joint = go.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rb;
        joint.configuredInWorldSpace = true;
        joint.xDrive = NewJointDrive(force, damping);
        joint.yDrive = NewJointDrive(force, damping);
        joint.zDrive = NewJointDrive(force, damping);
        joint.slerpDrive = NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;
        return go.transform;
    }

    private JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive
        {
            //drive.mode = JointDriveMode.Position;
            positionSpring = force,
            positionDamper = damping,
            maximumForce = Mathf.Infinity
        };
        return drive;
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "EnemySnowball")
        {
            playerCurrentHealth -= 1f;
            healthBar.fillAmount = playerCurrentHealth / playerStartHealth;
            if (playerCurrentHealth <= 0)
            {
                Destroy(gameObject);
                ParticleSystem newDeathParticle = Instantiate(playerDeathParticle, transform.position, transform.rotation) as ParticleSystem;
            }
        }
    }
    IEnumerator WaitForIt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void NewShootMethod()
    {
        GameObject newProjectile = Instantiate(snowBallGO, throwBallSpot.transform.position, throwBallSpot.transform.rotation);
    }
}