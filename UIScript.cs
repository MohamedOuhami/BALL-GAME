using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    // Canvas variables

    public GameObject GameUI,DeathUI;
    public AdsManager adscr;
    // Timer variables
    public TextMeshProUGUI timertext;
    public int minutes,seconds;
    public float timer = 60;
    // Referencing scripts
    
    // Start function
    private void Start() {
        adscr = GetComponent<AdsManager>();
        Time.timeScale = 1f;
    }
    
    // Update
    private void Update() {
        Timer();
    }
    
    // Show the gameUI
    public void ShowGameUI()
    {
        GameUI.SetActive(true);
        DeathUI.SetActive(false);
    }
    // Show the DeathUI
    public void ShowDeathUI()
    {
        GameUI.SetActive(false);
        DeathUI.SetActive(true);
    }
    // Calculate the time remaining
    void Timer()
    {
        timer -= Time.deltaTime;
        minutes = Mathf.FloorToInt(timer/60);
        seconds = Mathf.FloorToInt(timer%60);
        timertext.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if(timer <= 0)
        {
            ShowDeathUI();
            Time.timeScale = 0f;
        }
    }

    public void RetrythisLevel()
    {
        adscr.PlayRewardedAds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RetryFromBeginning()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
