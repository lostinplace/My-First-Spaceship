using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // -------------------- GENERAL -------------------
    [SerializeField]
    private float totalTripTime = 300f;
    private float curTripTime;

    public DeviceBase air;
    public DeviceBase engine;

    // -------------------- HUNGER --------------------
    [SerializeField]
    private float hungerBurnDown = 90f;

    private float curHungerBurnDown;

    [SerializeField]
    private float maxHungerTime = 30f;

    private float curHungerTime;

    // ---------------------- AIR ---------------------
    [SerializeField]
    private float maxAirlessTime = 60f;

    private float curAirlessTime;

    [SerializeField]
    private float airRefillRate = 1f;

    private float curAirFill;

    private bool isAirOn;

    // -------------------- ENGINE --------------------
    [SerializeField]
    private float maxEngineOffTime = 120f;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        air.DeviceActivated += Air_DeviceActivated;
        air.DeviceDeactivated += Air_DeviceDeactivated;

        curHungerBurnDown = maxHungerTime;

        curHungerTime = 0f;
        curAirlessTime = 0f;
        curAirFill = 0f;

        curTripTime = 0f;
    }

    private void Air_DeviceDeactivated(object sender, System.EventArgs e)
    {
        isAirOn = false;
    }

    private void Air_DeviceActivated(object sender, System.EventArgs e)
    {
        isAirOn = true;
        curAirlessTime = 0f;
    }

    // todo: subscribe to some player controller event
    public void ReceiveFood()
    {
        curHungerBurnDown = hungerBurnDown;
        curHungerTime = 0;
    }

    private void OnDisable()
    {
       air.DeviceActivated -= Air_DeviceActivated;
       air.DeviceDeactivated -= Air_DeviceDeactivated;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHungerBurnDown > 0)
        {
            curHungerBurnDown -= Time.deltaTime;
        }
        else
        {
            if (curHungerTime < maxHungerTime)
            {
                curHungerTime += Time.deltaTime;
            }
            else
            {
                TriggerEndgame(false, "If you ignore the munchies for too long, it gets deadly.");
            }
        }

        if (isAirOn)
        {
            curAirFill = Mathf.Min(maxAirlessTime, curAirFill + Time.deltaTime * airRefillRate);
        }
        else
        {
            if (curAirlessTime < maxAirlessTime)
            {
                curAirlessTime += Time.deltaTime;
            }
            else
            {
                TriggerEndgame(false, "So it turns out you need air to not die.");
            }
        }

        curTripTime += Time.deltaTime;
        if (curTripTime >= totalTripTime)
        {
            if (totalTripTime - engine.timeActiveInSeconds < maxEngineOffTime)
            {
                TriggerEndgame(true, "Congrats! You've successfully kept your crappy used " +
                    "spaceship together long enough to make it home!");
            }
            else
            {
                TriggerEndgame(false, "Looks like you didn't keep the engine running for long " +
                    "enough and ran out of fuel before you got home.");
            }
        }
    }

    void TriggerEndgame(bool won, string message)
    {
        // TODO: DO THIS
        Debug.Log("Game Over:\n" + message);
    }
}
