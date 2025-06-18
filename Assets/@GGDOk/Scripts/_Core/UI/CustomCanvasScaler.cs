using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CustomCanvasScaler : MonoBehaviour
{
    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] private int widthRatio = 16;
    [SerializeField] private int heightRatio = 9;
    public bool InvertRatio;

    private void Awake()
    {
        var width = Screen.width;
        var height = Screen.height;
        var ratio = (float)width / height;
        if (ratio > (float)widthRatio / heightRatio != InvertRatio)
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
    }
}