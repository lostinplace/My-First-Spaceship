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
    public Canvas canvas;
    public string stateName;

    // Start is called before the first frame update
    void Start()
    {
        //maximumHeight = this.GetComponent<RectTransform>().rect.height / canvas.scaleFactor;
        maximumWidth = this.GetComponent<RectTransform>().rect.width / canvas.scaleFactor;
        maximumHeight = this.GetComponent<RectTransform>().rect.height / canvas.scaleFactor;
        currentWidth = maximumWidth;
        currentHeight = maximumHeight;
        currentTime = Time.time;
    }

    public void StartChange(Status status, float changeRatePerSecond = 0f)
    {
        this.status = status;
        this.changeRatePerSecond = changeRatePerSecond;
        currentTime = Time.time;
    }

    public void AddToStatus(float increaseBy)
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
            Debug.Log(this.name);
            float changeBy = 0;
            if (this.name == "Battery")
            {
                changeBy = (currentTime - prevTime) * (changeRatePerSecond * maximumHeight);
                currentHeight += changeBy;
                CapHeight();
                RectTransform rectTransform = this.GetComponent<RectTransform>();
                float posBefore = rectTransform.position.y;
                Vector3[] fourCornersArrayBefore = new Vector3[4];
                rectTransform.GetWorldCorners(fourCornersArrayBefore);
                rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, currentHeight);
                Vector3[] fourCornersArrayAfter = new Vector3[4];
                rectTransform.GetWorldCorners(fourCornersArrayAfter);
                float calcY = posBefore - (fourCornersArrayAfter[0].y - fourCornersArrayBefore[0].y);
                Debug.Log((fourCornersArrayAfter[0].y - fourCornersArrayBefore[0].y));
                rectTransform.position = new Vector3(rectTransform.position.x, calcY, rectTransform.position.z);
            }
            else
            {
                changeBy = (currentTime - prevTime) * (changeRatePerSecond * maximumWidth);
                currentWidth += changeBy;
                CapWidth();
                RectTransform rectTransform = this.GetComponent<RectTransform>();
                float posBefore = rectTransform.position.x;
                Vector3[] fourCornersArrayBefore = new Vector3[4];
                rectTransform.GetWorldCorners(fourCornersArrayBefore);
                rectTransform.sizeDelta = new Vector2(currentWidth, rectTransform.rect.height);
                Vector3[] fourCornersArrayAfter = new Vector3[4];
                rectTransform.GetWorldCorners(fourCornersArrayAfter);
                float calcX = posBefore - (fourCornersArrayAfter[0].x - fourCornersArrayBefore[0].x);
                Debug.Log((fourCornersArrayAfter[0].x - fourCornersArrayBefore[0].x));
                rectTransform.position = new Vector3(calcX, rectTransform.position.y, rectTransform.position.z);
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
