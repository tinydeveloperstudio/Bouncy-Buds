using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author:  Rebekah Bledsoe
 * Version: 04/15/2025
 * Summary: The purpose of this script is to handle the main functions
 *          of scene control like transitions called publicly, transitions
 *          triggered by collisions, and quitting the game entirely.
 **/
public class SceneController : MonoBehaviour {
    //This variable is shown in the inspector
    [SerializeField] private string newSceneName;

    //This public method transitions the scene with a string input
    public void TransitionScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    //This method checks for collisions with the game object and transitions the scene if it is the player
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Buddy")) {
            TransitionScene(newSceneName);
        }
    }

    //This method quits the game
    public void QuitGame() {
        Application.Quit();
    }
}
