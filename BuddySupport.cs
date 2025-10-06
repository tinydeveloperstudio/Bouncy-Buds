using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Author:  Rebekah Bledsoe
 * Version: 03/30/2025
 * Summary: The purpose of this script is to control the support functionality
 *          of the buddy for the first level. This includes its various states
 *          of waiting and supporting the player as well as the different support
 *          functions. The buddy uses application variables from the player's movement
 *          visual script, and it enables/disables the healing pulse object with its own
 *          health boost functionality.
 **/
public class BuddySupport : MonoBehaviour
{
    //These variables are shown in the inspector
    [SerializeField] private float resetWaitTime = 8.0f;
    [SerializeField] private float healWaitTime = 4.0f;
    [SerializeField] private float speedWaitTime = 4.0f;
    [SerializeField] private float healAmount = 15.0f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject healPulse;
    [SerializeField] private GameObject speedPulse;

    //These variables are hidden from the inspector
    private bool isSupporting = false;
    private bool isHealing = true;
    private bool isSpeed = false;
    private bool resetWaiting = false;
    private bool healWaiting = false;
    private bool speedWaiting = false;

    //Update is called once per frame
    void Update() {
        //Checks for inputs to change the type of support
        if (Input.GetKeyUp(KeyCode.Space)) {
            if (!isSupporting) {
                if (isHealing) {
                    isHealing = false;
                    isSpeed = true;
                }
                else if (isSpeed) {
                    isSpeed = false;
                    isHealing = true;
                }
            }
        }

        //Checks for if the buddy is supporting and the type of support
        if (isSupporting) {
            if (isHealing) {
                if (!healWaiting) {
                    StartCoroutine(HealWait(healWaitTime));
                }
            }
            else if (isSpeed) {
                if (!speedWaiting) {
                    StartCoroutine(SpeedWait(speedWaitTime));
                }
            }

        }
        else {
            if (!resetWaiting) {
                StartCoroutine(WaitTimer(resetWaitTime));
            }
        }
    }

    //This method waits before resetting the support ability
    private IEnumerator WaitTimer(float waitTime) {
        resetWaiting = true;
        //Debug.Log("Reset waiting");
        isSupporting = false;
        healPulse.SetActive(false);
        speedPulse.SetActive(false);
        yield return new WaitForSecondsRealtime(waitTime);
        resetWaiting = false;
        isSupporting = true;
    }

    //This method waits while healing the player
    private IEnumerator HealWait(float waitTime) {
        healWaiting = true;
        //Debug.Log("Heal waiting");
        HealPlayer();
        yield return new WaitForSecondsRealtime(waitTime);
        healWaiting = false;
        if (!resetWaiting) {
            StartCoroutine(WaitTimer(resetWaitTime));
        }
    }

    //This method waits while boosting the player's speed
    private IEnumerator SpeedWait(float waitTime) {
        speedWaiting = true;
        Debug.Log("Speed boost waiting");
        SpeedPlayer();
        yield return new WaitForSecondsRealtime(waitTime);
        speedWaiting = false;
        Variables.Application.Set("boostingSpeed", false);
        Variables.Application.Set("positiveSpeed", 10.0f);
        Variables.Application.Set("negativeSpeed", -10.0f);
        if (!resetWaiting) {
            StartCoroutine(WaitTimer(resetWaitTime));
        }
    }

    //This method heals the player
    private void HealPlayer() {
        isSupporting = true;
        //Debug.Log("Is healing");
        healPulse.SetActive(true);
        if (player.GetComponent<PlayerHealth>().GetCurrentHealth() < player.GetComponent<PlayerHealth>().GetMaxHealth()) {
            player.GetComponent<PlayerHealth>().ChangeCurrentHealth(healAmount);
        }
    }

    //This method gives the player a speed boost
    private void SpeedPlayer() {
        isSupporting = true;
        Debug.Log("Is boosting speed");
        speedPulse.SetActive(true);
        float speedAmount;
        if (((float)Variables.Application.Get("positiveSpeed")) * 2.5f <= 25.0f) {
            speedAmount = ((float)Variables.Application.Get("positiveSpeed")) * 2.5f;
        }
        else {
            speedAmount = 25.0f;
        }
        Variables.Application.Set("boostingSpeed", true);
        Variables.Application.Set("positiveSpeed", speedAmount);
        Variables.Application.Set("negativeSpeed", -speedAmount);
    }

    //This public method sets the buddy type
    public void SetSupportType(string supportType) {
        if (supportType == "Healing") {
            isHealing = true;
            isSpeed = false;
        }

        if (supportType == "Speed") {
            isSpeed = true;
            isHealing = false;
        }
    }
}
