using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Author:  Rebekah Bledsoe
 * Version: 05/04/2025
 * Summary: The purpose of this script is to control the movement and
 *          navigation of the buddy for the second level using navMesh
 *          agent functionality. Public methods for speed boosts can be
 *          called from other scripts to start the function. These then
 *          trigger a coroutine which applies the boost, waits, and returns
 *          the buddy back to its default speed.
 **/
public class BuddyNavigation : MonoBehaviour {

    //These variables are shown in the inspector
    [SerializeField] private Transform playerGoal;
    [SerializeField] private float maximumSpeed = 10.0f;
    [SerializeField] private float defaultSpeed = 3.5f;

    //These variables are not shown in the inspector
    private bool canMove = true;
    private bool isWaiting = false;
    private bool isBoosting = false;
    private float boostedSpeed;


    //Start is called before the first frame update
    private void Start() {
        isWaiting = false;
        canMove = true;
        GetComponent<NavMeshAgent>().speed = defaultSpeed;
    }

    //Update is called once per frame
    private void Update() {
        GetComponent<NavMeshAgent>().destination = playerGoal.position;
        if (canMove) {
            GetComponent<NavMeshAgent>().isStopped = false;
            if (isBoosting) {
                GetComponent<NavMeshAgent>().speed = boostedSpeed;
                if (GetComponent<NavMeshAgent>().speed > maximumSpeed) {
                    GetComponent<NavMeshAgent>().speed = maximumSpeed;
                }
            }
        }
        else {
            GetComponent<NavMeshAgent>().isStopped = true;
        }
    }

    //This method boosts the buddy's speed
    private IEnumerator SpeedWaiter(float multiplier, float waitTime) {
        isWaiting = true;
        boostedSpeed = multiplier;
        isBoosting = true;
        yield return new WaitForSeconds(waitTime);
        isBoosting = false;
        GetComponent<NavMeshAgent>().speed = defaultSpeed;
        isWaiting = false;
    }

    //This public method starts the buddy's speed waiter if it hasn't been yet
    public void BoostSpeed(float multiplier, float waitTime) {
        if (!isWaiting) {
            StartCoroutine(SpeedWaiter(multiplier, waitTime));
        }
    }

    //This methdo sets the canMove status of the agent and controls its movement
    public void SetNavigation(bool movement) {
        canMove = movement;
        GetComponent<NavMeshAgent>().isStopped = movement;
    }
}
