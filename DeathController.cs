using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author:  Rebekah Bledsoe
 * Version: 03/30/2025
 * Summary: The purpose of this script is to control the instant death zones
 *          like the invisble out of bounds area of the first level. This
 *          mainly includes displaying the death UI menu and acting based on
 *          player input.
 **/
public class DeathZone : MonoBehaviour
{
    //These variables are shown in the inspector
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private string mainMenuName;
    [SerializeField] private string levelName;

    //This variable is not shown in the inspector
    private GameObject playerObject;

    //This method checks for if the player enters the trigger zone
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerObject = other.gameObject;
            playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            deathScreen.SetActive(true);
        }
    }

    //This method loads the player back to the main menu of the game
    public void LoadMainMenu() {
        SceneManager.LoadScene(mainMenuName);
    }

    //This method quits the game's application
    public void QuitGame() {
        Application.Quit();
    }

    //This method respawns the player at the start of the level
    public void RestartLevel() {
        SceneManager.LoadScene(levelName);
    }
}
