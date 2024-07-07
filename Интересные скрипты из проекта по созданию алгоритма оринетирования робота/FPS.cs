using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public Text text;

    private float deltaTime = 0.0f;
    private float updateRate = 5f;  // Количество обновлений в секунду
    private float timer = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        timer += Time.unscaledDeltaTime;

        if (timer > 1.0f / updateRate)
        {
            float fps = 1.0f / deltaTime;
            text.text = $"FPS = {Math.Round(fps, 2)}";
            timer = 0.0f;
        }
    }










    //private float deltaTime = 0.0f;

    //void Update()
    //{
    //    deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    //    float fps = 1.0f / deltaTime;
    //    text.text = $"FPS = {Math.Round(fps, 2)}";
    //}








    //public float avgFrameRate;

    //private void Start()
    //{

    //}

    //void Update()
    //{
    //    avgFrameRate = (float)Math.Round((Time.frameCount / Time.time), 2);
    //    text.text = $"FPS = {avgFrameRate}";
    //}
}