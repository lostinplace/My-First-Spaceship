using UnityEngine;

public class WaitForInstructions : MonoBehaviour
{
    public float waitTimeInSeconds = 30.0f;
    public GameObject swap;
    public KeyCode swapKey = KeyCode.I;
    public float swapKeyWaitTimeInSeconds = 0.3f;
    private float timeWaitedInSeconds = 0.0f;
    private bool isVisible;
    private bool toggled = false;
    public float toggleDelay = 1.0f;
    private float toggleTimer = 0.0f;

    void Start() {
        isVisible = gameObject.GetComponent<MeshRenderer>().enabled;
    }


    public void ToggleVisible()
    {
        Debug.Log("Toggle Text!");
        isVisible = !isVisible;
        gameObject.GetComponent<MeshRenderer>().enabled = isVisible;
        foreach (var childRenderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
            childRenderer.enabled = isVisible;
        }
        toggleTimer = toggleDelay;
    }

    void Update()
    {
        bool doToggle = false;
        if (isVisible == true)
        {
            float deltaTime = Time.deltaTime;
            timeWaitedInSeconds += deltaTime;
            if (Input.GetKeyDown(swapKey) == true && toggleTimer <= 0.0f)
            {
                  toggled = true;
                  doToggle = true;
            }
            else if(toggleTimer >= 0.0f) toggleTimer -= deltaTime; 
        }

        bool doTimeBasedSwap = (timeWaitedInSeconds >= waitTimeInSeconds) && (toggled == false);
        if (swap != null && (doTimeBasedSwap == true || doToggle == true))
        {
            timeWaitedInSeconds = 0.0f;
            ToggleVisible();
            swap.GetComponent<WaitForInstructions>().ToggleVisible();
        }
    }
}
