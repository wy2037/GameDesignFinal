using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private Collider2D _col;
    private bool isTriggered;
    [SerializeField] int tutorialID;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && !isTriggered){
            isTriggered = true;
            UIManager.Instance.activateUI(tutorialID);
        }
    }
}
