using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerSelectJoy : MonoBehaviour
{

    public List<GameObject> playersAvailable;
    public GameObject selectedPlayer;
    public int listSize;
    private MobileController mobileController;
    public float angleRatio = 30f;
    public float startingLaunchAngle = 50f;
    public float launchAngleMin = 10f;
    public float launchAngle;
    public float launchPlaceholder = 0f;
    private bool bustedShootMeter = false;

    public GameObject snowBank;
    public Transform enemyPos;
    private float downTime;
    public float lapActual;
    public Image powerBar;
    public Image buildBank;
    public float timeForBuildBank = 2f;
    int throwMask;
    public bool buildButtonDown = false;
    private float buttonDownTimer;
    private bool isThrowDown = false;
    public Image healthBar;

    void Start()
    {
        throwMask = LayerMask.GetMask("ThrowableArea");
        playersAvailable = new List<GameObject>();
        playersAvailable.RemoveAll(item => item == null);
        AddAllAvailable();
    }

    private void Update()
    {
        SetLaunchByMouse();
        SetLaunchByTouch();
        powerBar.fillAmount = launchPlaceholder;
        //   (StartingAngle - (Starting Angle - Lowest Angle)*METER) +Lowest Angle
        launchAngle = (startingLaunchAngle - (startingLaunchAngle - launchAngleMin) * lapActual);
        playersAvailable.RemoveAll(item => item == null);
        listSize = playersAvailable.Count;
        IsBuildButtonDown();
        SelectNewPlayer();
        healthBar.fillAmount = mobileController.playerCurrentHealth / mobileController.playerStartHealth;


    }
    public void AddAllAvailable()
    {
        GameObject[] gobj = GameObject.FindGameObjectsWithTag("Player").OrderBy(go => go.name).ToArray();
        foreach (GameObject goodGuy in gobj)
        {
            AddTarget(goodGuy.gameObject);
        }
    }
    public void AddTarget(GameObject goodGuy)
    {
        if (playersAvailable.IndexOf(goodGuy) < 0)
        {
            playersAvailable.Add(goodGuy);
        }
    }


    private void SelectNewPlayer()
    {
        if (listSize > 0)
        {
            selectedPlayer = playersAvailable[0];
            mobileController = selectedPlayer.GetComponent<MobileController>();
            mobileController.enabled = true;
            mobileController.isSelected = true;
        }
    }

    public void SnowBankButtonUp()
    {
        float heldTime = Time.time - downTime;
        if (heldTime > timeForBuildBank) // seconds
        {
            GameObject newSnowBank = Instantiate(snowBank, new Vector3(selectedPlayer.transform.position.x - 5f, selectedPlayer.transform.position.y - 1.5f, selectedPlayer.transform.position.z),
            Quaternion.LookRotation(enemyPos.position - selectedPlayer.transform.position)) as GameObject;
            newSnowBank.transform.Rotate(-90f, 180f, 0f);
        }
        buildButtonDown = false;
        buttonDownTimer = 0f;
    }
    public void SnowBankButtonDown()
    {
        downTime = Time.time;
        buildBank.fillAmount = timeForBuildBank - downTime;
        buildButtonDown = true;
    }

    public void IsBuildButtonDown()
    {
        buildBank.fillAmount = buttonDownTimer / timeForBuildBank;
        if (buildButtonDown)
        {
            buttonDownTimer += Time.deltaTime;
        }
        //       else

        //       buildButtonDown = false;
    }
    private void SetLaunchByMouse()
    {
        if (!IsPointerOverUIObject() && !PauseMenu.gameIsPaused)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, throwMask))
            {
                if (Input.GetMouseButton(0) && !bustedShootMeter)
                {
                    isThrowDown = true;
                    launchPlaceholder += Time.deltaTime * angleRatio;
                    lapActual = launchPlaceholder;

                    if (launchPlaceholder >= 1)
                    {
                        bustedShootMeter = true;
                        //launchPlaceholder = 0;
                        lapActual = 0;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    isThrowDown = false;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                isThrowDown = false;
            }
            if (!isThrowDown)
            {
                launchPlaceholder -= Time.deltaTime * 1.5f * angleRatio;
                if (launchPlaceholder <= 0)
                {
                    bustedShootMeter = false;
                    launchPlaceholder = 0;
                }
            }
        }

    }
    private void SetLaunchByTouch()
    {
        if (!IsPointerOverUIObject() && !PauseMenu.gameIsPaused)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Ray touchray = Camera.main.ScreenPointToRay(Input.touches[i].position);
                RaycastHit touchhit;

                if (Physics.Raycast(touchray, out touchhit, Mathf.Infinity, throwMask) && !bustedShootMeter)
                {
                    isThrowDown = true;
                    launchPlaceholder += Time.deltaTime * angleRatio;
                    lapActual = launchPlaceholder;

                    if (launchPlaceholder >= 1)
                    {
                        bustedShootMeter = true;
                        //launchPlaceholder = 0;
                        lapActual = 0;

                    }
                }
                if (Input.touches[i].phase == TouchPhase.Ended)
                {
                    isThrowDown = false;
                }
            }
        }
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
