using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Author:  Rebekah Bledsoe
 * Version: 03/31/2025
 * Summary: The purpose of this script is to control the functions of a
 *          damaging object. Based on input knockback and damage amounts,
 *          the player will be damaged at random amounts and launched when
 *          colliding with an object with this script. A short move wait 
 *          function is used to remove the ability for input player movement, 
 *          which helps the knockback function smoother.
 **/
public class Damager : MonoBehaviour
{
    //These variables are shown in the inspector
    [SerializeField] private float minimumDamageAmount = 5.0f;
    [SerializeField] private float maximumDamageAmount = 25.0f;
    [SerializeField] private float knockbackAmount = 8.0f;
    [SerializeField] private float knockbackWaitTime = 0.2f;

    //This variable is not shown in the inspector
    private bool isWaiting = false;

    //This method checks for if the player enters the damager collision area
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            if (collision.gameObject.GetComponent<PlayerHealth>() != null) {
                if (collision.gameObject.GetComponent<PlayerHealth>().GetCurrentHealth() > 0) {
                    float damageAmount = Random.Range(minimumDamageAmount, maximumDamageAmount);
                    collision.gameObject.GetComponent<PlayerHealth>().ChangeCurrentHealth(-damageAmount);
                    if (collision.gameObject.GetComponent<Rigidbody>() != null) {
                        if (!isWaiting) {
                            Vector3 knockBack = new Vector3(knockbackAmount * Mathf.Sign(collision.gameObject.GetComponent<Rigidbody>().velocity.x),
                                knockbackAmount * Mathf.Sign(collision.gameObject.GetComponent<Rigidbody>().velocity.y),
                                knockbackAmount * Mathf.Sign(collision.gameObject.GetComponent<Rigidbody>().velocity.z));
                            Variables.Application.Set("canMove", false);
                            collision.gameObject.GetComponent<Rigidbody>().velocity = knockBack;
                            StartCoroutine(MoveWaiter(knockbackWaitTime));
                        }
                    }
                }
            }
        }
    }

    //This method handles waiting for the knockback
    private IEnumerator MoveWaiter(float waitTime) {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        Variables.Application.Set("canMove", true);
    }
}
