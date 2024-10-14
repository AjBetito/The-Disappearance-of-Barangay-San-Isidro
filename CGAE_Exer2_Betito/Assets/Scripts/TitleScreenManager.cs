using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{   
    public float idleTime = 15f;
    public float timer = 0f;


    public void StartGame(){
        SceneManager.LoadScene("Entrance");
    }

    public void QuitGame(){
        Application.Quit();
    }

    void Update(){
        if (Input.anyKey){
            timer = 0f;
        } else {
            timer += Time.deltaTime;
        }
    }

}
