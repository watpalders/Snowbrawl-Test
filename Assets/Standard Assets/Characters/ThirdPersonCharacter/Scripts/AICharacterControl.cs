using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        public GameObject[] moveSpots;
        public float waitTime;
        public float startWaitTime = 2f;
        private int randomSpot;
        public Transform moveToGO;
        public bool isJoyActive;

        private void Start()
        {
            waitTime = startWaitTime + UnityEngine.Random.Range(-1, 1);
            moveSpots = GameObject.FindGameObjectsWithTag("GameSpot");
            randomSpot = UnityEngine.Random.Range(0, moveSpots.Length); // sets random spot

            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
	        agent.updateRotation = false;
	        agent.updatePosition = true;
            moveToGO = GameObject.FindWithTag("MoveToGameOBJ").transform;
            if (gameObject.tag == "ActivePlayer") 
            {
                target = moveToGO.transform;
            }
            if(gameObject.tag == "Enemy")
            {
                target = moveSpots[randomSpot].transform;
            }

        }


        private void Update()
        {
            if (gameObject.tag == "Enemy")
            {
                GetRandomSpot();
            }
            if (!isJoyActive)
            {
                if (target != null)
                    agent.SetDestination(target.position);

                if (agent.remainingDistance > agent.stoppingDistance)
                    character.Move(agent.desiredVelocity, false, false);
                else
                    character.Move(Vector3.zero, false, false);
            }

        }


        public void SetTarget(Transform target)
        {
            
            this.target = target;
        }

        private void GetRandomSpot()
        {
            float xdiff;
            float zdiff;
            xdiff = transform.position.x - target.transform.position.x;
            zdiff = transform.position.z - target.transform.position.z;
            //if (Vector3.Distance(transform.position, target.transform.position) < 2.5f)
            if(xdiff < 2.5f && zdiff <2.5f)
            {
                if (waitTime <= 0)
                {
                    randomSpot = UnityEngine.Random.Range(0, moveSpots.Length);
                    target = moveSpots[randomSpot].transform; 
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }
}
