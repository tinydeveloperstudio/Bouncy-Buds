using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author:  Rebekah Bledsoe
 * Version: 05/05/2025
 * Summary: The purpose of this script is to control the health and
 *          health UI for the buddy. While this script is active on the
 *          buddy, it's health will gradually diminish. If it runs out of
 *          health and dies, the death menu screen will activate.
 **/
public class BuddyHealth : MonoBehaviour
{
    //These variables are shown in the inspector
    [SerializeField] private float timeDamageAmount = 0.05f;
    [SerializeField] private float damageInterval = 1.0f;
    [SerializeField] private Image buddyHealthBar;
    [SerializeField] private GameObject buddyDeathMenu;

    //These variables are hidden from the inspector
    private float maxHealth = 1.0f;
    private float currentHealth = 1.0f;
    private bool isAlive = true;
    private bool isWaiting = false;

    //Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            isAlive = false;
        }

        buddyHealthBar.fillAmount = currentHealth;

        if (isAlive) {
            if (!isWaiting) {
                StartCoroutine(DamageWaiter(damageInterval));
            }
        }
        else {
            StopAllCoroutines();
            GetComponent<BuddyNavigation>().SetNavigation(false);
            buddyDeathMenu.SetActive(true);
        }
    }

    //This method controls the damage at intervals
    private IEnumerator DamageWaiter(float waitInterval) {
        isWaiting = true;
        currentHealth = currentHealth - timeDamageAmount;
        yield return new WaitForSeconds(waitInterval);
        isWaiting = false;
    }
}
