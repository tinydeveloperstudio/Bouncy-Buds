using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Author:  Rebekah Bledsoe
 * Version: 05/05/2025
 * Summary: The purpose of this script is to control the boosts
 *          for the buddy in the second level. This mainly includes
 *          the movement boost.
 **/
public class MovementAdjustor : MonoBehaviour
{
    //These variables are shown in the inspector
    [SerializeField] private string roomType = "Random";
    [SerializeField] private float healAmount = 1f;
    [SerializeField] private float adjustedSpeed = 7.0f;
    [SerializeField] private float waitTime = 1.0f;
    [SerializeField] private GameObject buddy;

    //Thssi variable is not shown in the inspector
    private bool inSupportRoom = false;

    // Update is called once per frame
    void Update()
    {
        if (inSupportRoom) {
            if (roomType == "Movement") {
                buddy.GetComponent<BuddyNavigation>().BoostSpeed(adjustedSpeed, waitTime);
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    //This method checks for if the player enters a support room
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Buddy")) {
            inSupportRoom = true;
        }
    }

    //This method checks for if the player leaves the support room
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Buddy")) {
            inSupportRoom = false;
        }
    }
}
