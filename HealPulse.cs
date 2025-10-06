using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author:  Rebekah Bledsoe
 * Version: 03/31/2025
 * Summary: The purpose of this script is to control healing pulse
 *          ability that the buddy has in the first level. When the pulse's
 *          trigger zone enters the player's collider, it will heal the player.
 *          This healing will stop after the player reaches full health.
 **/
public class HealPulse : MonoBehaviour
{
    //This variable is shown in the inspector
    [SerializeField] private float healAmount = 10.0f;

    //This method checks for when the player object enters the heal pulse trigger zone
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (other.GetComponent<PlayerHealth>() != null) {
                if (other.GetComponent<PlayerHealth>().GetCurrentHealth() < other.GetComponent<PlayerHealth>().GetMaxHealth()) {
                    other.GetComponent<PlayerHealth>().ChangeCurrentHealth(healAmount);
                    GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
}
