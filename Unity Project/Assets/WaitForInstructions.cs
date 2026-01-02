using UnityEngine;

public class WaitForInstructions : MonoBehaviour
{
    public float waitTimeInSeconds = 30.0f;
    public GameObject swap;
    private float timeWaitedInSeconds = 0.0f;
    private bool isVisible;
    void Start()
    {
        isVisible = gameObject.GetComponent<MeshRenderer>().enabled;
    }

    public void ToggleVisible()
    {
        Debug.Log("Toggle Text!");
        isVisible = !isVisible;
        gameObject.GetComponent<MeshRenderer>().enabled = isVisible;
        foreach(var childRenderer in gameObject.GetComponentsInChildren<MeshRenderer>())
            childRenderer.enabled = isVisible;
    }

    void Update()
    {
        if(isVisible == true) timeWaitedInSeconds += Time.deltaTime;
        if (swap != null && timeWaitedInSeconds >= waitTimeInSeconds)
        {
            timeWaitedInSeconds = 0.0f;
            ToggleVisible();
            swap.GetComponent<WaitForInstructions>().ToggleVisible();
        }
    }
}
