using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    STOPPED, INCREASING, DECREASING
}

public class StatusBar : MonoBehaviour
{
    public float statusPercentage = 1f;
    public float maximumWidth;
    public float currentWidth;
    public float maximumHeight;
    public float currentHeight;
    public float changeRatePerSecond = 0; //Let's go by percentage for this
    public Status status = Status.STOPPED;
    private float prevTime, currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentWidth = maximumWidth;
        currentHeight = maximumHeight;
    }

    public void StartChange(Status status, float changeRatePerSecond = 0f)
    {
        this.status = status;
        this.changeRatePerSecond = changeRatePerSecond;
        currentTime = Time.deltaTime;
    }

    public void AteFood(float increaseBy)
    {
        this.currentWidth += increaseBy / maximumWidth;
        CapWidth();
    }

    // Update is called once per frame
    void Update()
    {
        if (status != Status.STOPPED)
        {
            prevTime = currentTime;
            currentTime = Time.time;

            float changeBy = (currentTime - prevTime) * changeRatePerSecond;
            if (maximumHeight != 0f)
            {
                currentHeight += changeBy / maximumHeight;
                CapHeight();
            } else
            {
                currentWidth += changeBy / maximumWidth;
                CapWidth();
            }
        }
    }

    void CapWidth()
    {
        if (currentWidth > maximumWidth)
        {
            currentWidth = maximumWidth;
            status = Status.STOPPED;
        }
    }

    void CapHeight()
    {
        if (currentWidth > maximumWidth)
        {
            currentWidth = maximumWidth;
            status = Status.STOPPED;
        }
    }
}
