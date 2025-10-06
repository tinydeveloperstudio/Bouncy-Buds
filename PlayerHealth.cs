using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author:  Rebekah Bledsoe
 * Version: 03/31/2025
 * Summary: The purpose of this script is to control the health and health UI
 *          for the player in the first level. This includes various variables, 
 *          using cinemachine when taking damage, methods for changing health, 
 *          dying, and getter methods for health.
 **/
public class PlayerHealth : MonoBehaviour
{
    //These variables are shown in the inspector
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth = 100;
    [SerializeField] private float waiterTime = 0.2f;
    [SerializeField] private float screenShakeAmplitude = 1.0f;
    [SerializeField] private float screenShakeFrequency = 2.0f;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private CinemachineVirtualCamera mainCamera;

    //These variables are not shown in the inspector
    private bool isAlive = true;
    private bool isWaiting = false;

    //Update is called once per frame
    void Update()
    {
        if (!isWaiting) {
            mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.0f;
            mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.0f;
        }

        if (currentHealth <= 0) {
            isAlive = false;
        }
        else {
            isAlive = true;
        }


        if (isAlive) {
            healthBar.fillAmount = (currentHealth / maxHealth);
        }
        else {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            deathScreen.SetActive(true);
        }
    }

    //This method handles changing the current health amount (for healing and damage)
    public void ChangeCurrentHealth(float deltaHealth) {
        currentHealth += deltaHealth;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        if (currentHealth < 0) {
            currentHealth = 0;
        }

        //Screenshake
        if (deltaHealth < 0) {
            mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = screenShakeAmplitude;
            mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = screenShakeFrequency;
            if (!isWaiting) {
                StartCoroutine(ShakeWaiter(waiterTime));
            }
        }
    }

    //This method handles returning the current health amount
    public float GetCurrentHealth() {
        return currentHealth;
    }

    //This method handles returning the max health amount
    public float GetMaxHealth() {
        return maxHealth;
    }

    //This method handles changing the max health amount (for upgrades)
    public void ChangeMaxHealth(float deltaMaxHealth) {
        maxHealth += deltaMaxHealth;
    }

    //This waiter method waits a set time before setting the screenshake to false
    private IEnumerator ShakeWaiter(float waitTime) {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
