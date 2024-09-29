using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenIdle : MonoBehaviour
{
    public Button[] buttons; 
    public float idleTimeLimit = 15f; 
    private float idleTimer;

    void Start()
    {
        idleTimer = 0f; 
    }

    void Update()
    {
        if (Input.anyKey)
        {
            ResetIdleTimer();
        }
        else
        {
            idleTimer += Time.deltaTime;
            
            if (idleTimer >= 11f) {
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Where";
            }

            if (idleTimer >= 13f) {
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "are";
            }

            if (idleTimer >= 15f) {
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "you?";
            }
        }
    }

    private void ResetIdleTimer()
    {
        idleTimer = 0f;

        ResetButtonText();
    }


    private void ResetButtonText()
    {
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Start";
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Credit";
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Quit";
    }
}
