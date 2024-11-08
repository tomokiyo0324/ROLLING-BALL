using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public Slider timerSlider;
    public float sliderTimer;
    public bool stopTimer = false;

    void Start()
    {
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(StartTheTimerTicker());
    }

    IEnumerator StartTheTimerTicker()
    {
        while (stopTimer == false)
        {
            sliderTimer -= Time.deltaTime;
            yield return new WaitForSeconds(0.001f);

            if(sliderTimer <= 0)
            {
                stopTimer = true;
                SceneManager.LoadScene("Gameover");
                //Timer��0�ɂȂ�����Q�[���I�[�o�[��ʂɈڍs����
            }

            if (stopTimer == false)
            {
                timerSlider.value = sliderTimer;
            }
        }
    }
    
    public void StopTimer()
    {
        stopTimer = true;
    }
}
