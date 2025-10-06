using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Author:  Rebekah Bledsoe
 * Version: 05/05/2025
 * Summary: The purpose of this script is to control the room functions
 *          for the second level. This includes triggering the door selection
 *          UI, removing the selected doors and the set of room's trigger collider, 
 *          and influencing the buddy. All of the functions are based on player input 
 *          from the door selection menu.
 **/
public class RoomController : MonoBehaviour
{
    //These variables are shown in the inspector
    [SerializeField] private GameObject[] frontDoors;
    [SerializeField] private GameObject[] backDoors;
    [SerializeField] private GameObject[] roomPanels;
    [SerializeField] private MeshRenderer[] roomOneIndicators;
    [SerializeField] private MeshRenderer[] roomTwoIndicators;
    [SerializeField] private MeshRenderer[] roomThreeIndicators;
    [SerializeField] private GameObject doorMenu;
    [SerializeField] private BoxCollider roomOneCollider;
    [SerializeField] private SphereCollider roomTwoCollider;
    [SerializeField] private CapsuleCollider roomThreeCollider;

    //These variables are not shown in the inspector
    private bool atDoors = false;
    private int playerInput = -1;
    private int roomIndex = 0;
    private GameObject buddy;

    //Start is called before the first frame update
    private void Start() {
        for (int i = 0; i < roomOneIndicators.Length; i++) {
            roomOneIndicators[i].enabled = false;
        }
    }

    //Update is called once per frame
    void Update()
    {
        //Main functionality for entering the doors of each room
        if (atDoors) {                                                                          //While the buddy is at the doors, the UI menu activates
            doorMenu.SetActive(true);
            for (int i = 0; i < roomPanels.Length; i++) {                                       //Based on which set of rooms the buddy is at (tracked by the roomIndex),
                if (i == roomIndex) {                                                           //the method cycles through the room array and indicates the different
                    roomPanels[i].SetActive(true);                                              //speed boosts for each room in the set
                    if (i == 0) {
                        for (int j = 0; j < roomOneIndicators.Length; j++) {
                            roomOneIndicators[j].enabled = true;
                        }
                    }
                    else if (i == 1) {
                        for (int j = 0; j < roomTwoIndicators.Length; j++) {
                            roomTwoIndicators[j].enabled = true;
                        }
                    }
                    else if (i == 2) {
                        for (int j = 0; j < roomThreeIndicators.Length; j++) {
                            roomThreeIndicators[j].enabled = true;
                        }
                    }
                }
                else {
                    roomPanels[i].SetActive(false);
                }
            }
            if (playerInput >= 0) {                                                             //Based on player input, the set of room's collider is disabled and the specific
                if (roomIndex == 0) {                                                           //front door and back door are disabled (this includes being disabled as a nav mesh obstacle)
                    roomOneCollider.enabled = false;
                    roomIndex++;
                }
                else if (roomIndex == 1) {
                    roomTwoCollider.enabled = false;
                    roomIndex++;
                }
                else {
                    roomThreeCollider.enabled = false;
                    roomIndex++;
                }
                frontDoors[playerInput].GetComponent<NavMeshObstacle>().enabled = false;
                frontDoors[playerInput].GetComponent<Collider>().enabled = false;
                backDoors[playerInput].GetComponent<NavMeshObstacle>().enabled = false;
                backDoors[playerInput].GetComponent<Collider>().enabled = false;
                atDoors = false;
                buddy.GetComponent<BuddyNavigation>().SetNavigation(true);
            }
        }
        else {
            doorMenu.SetActive(false);                                                          //If the buddy is not at any set of doors, the door UI deactivates and the player input is reset
            playerInput = -1;
        }
    }

    //This method checks for if the buddy has collided with the front door area of the room
    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Buddy")) {
            buddy = collider.gameObject;
            atDoors = true;
            collider.gameObject.GetComponent<BuddyNavigation>().SetNavigation(false);
        }
    }

    //This public method allows UI buttons to set the player input
    public void SetPlayerInput(int input) {
        playerInput = input;
    }
}
