using UnityEngine;

public class PlayerState : MonoBehaviour
{

  protected UnityEngine.UI.RawImage playerHurtUI;
  protected UnityEngine.UI.Image fadeToBlack;
  protected Canvas uiCanvas;
  protected Color playerHurtOriginalColor, originalFadeToBlackColor;
  [SerializeField] public float playerHurtThrobSpeed;
  [SerializeField] public float fadeToBlackSpeed;
  [SerializeField] public float colorFadeTolerence = 0.003f;
  protected bool throbbing = true;
  protected bool throbDown = false;
  protected bool gameOver = false;
  protected bool lost = false;
  protected string gameOverMessage;
  // -------------------- GENERAL -------------------
  [SerializeField]
  private float totalTripTime = 300f;
  private float curTripTime;

  public DeviceBase air;
  public DeviceBase engine;

  // -------------------- HUNGER --------------------
  [SerializeField]
  public float unburntHungerMax = 90f;

  private float hungerBurnDown;

  private float curHungerBurnDown;

  public short PipesHeld = 0;

  public short BatteriesHeld = 0;

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
  
  //TODO: Suggest not using this for this.
  private void Air_DeviceDeactivated(object sender, System.EventArgs e) {
    isAirOn = false;
    curAirlessTime = 0f;
  }

    //TODO: Suggest not using this for this.
    private void Air_DeviceActivated(object sender, System.EventArgs e) {
    isAirOn = true;
    curAirlessTime = 0f;
  }

  // todo: subscribe to some player controller event
  public void ReceiveFood()
  {
    curHungerBurnDown = hungerBurnDown;
    curHungerTime = 0;
  }

  void ThrobHurt()
  {
        if ((throbDown == true || throbbing == false ) && GameOverText.ColorsAreClose(playerHurtUI.color, Color.clear, .003f) != true)
            playerHurtUI.color = Color.Lerp(playerHurtUI.color, Color.clear, playerHurtThrobSpeed * Time.deltaTime);
        else if (throbDown == false && throbbing == true && GameOverText.ColorsAreClose(playerHurtUI.color, playerHurtOriginalColor, colorFadeTolerence) != true)
            playerHurtUI.color = Color.Lerp(playerHurtUI.color, playerHurtOriginalColor, playerHurtThrobSpeed * Time.deltaTime);
        else if (GameOverText.ColorsAreClose(playerHurtUI.color, playerHurtOriginalColor, colorFadeTolerence))
            throbDown = true;
        else if (GameOverText.ColorsAreClose(playerHurtUI.color, Color.clear, .003f))
            throbDown = false;
  }
    private void OnDisable()
  {
    if (!air) return;
     air.DeviceActivated -= Air_DeviceActivated;
     air.DeviceDeactivated -= Air_DeviceDeactivated;
  }

  void Awake() {
    curHungerBurnDown = unburntHungerMax;
  }

  // Start is called before the first frame update
  void Start()
  {
    playerHurtUI = GetComponentInChildren<UnityEngine.UI.RawImage>();
    fadeToBlack = GetComponentInChildren<UnityEngine.UI.Image>();
    originalFadeToBlackColor = fadeToBlack.color;
    fadeToBlack.color = new Color(0f, 0f, 0f, 0f);
    uiCanvas = GetComponentInChildren<Canvas>();
    playerHurtOriginalColor = playerHurtUI.color;
    playerHurtUI.color = Color.clear;
    DontDestroyOnLoad(this.gameObject);
    if (air)
    {
        isAirOn = air.isActive;
        air.DeviceActivated += Air_DeviceActivated;
        air.DeviceDeactivated += Air_DeviceDeactivated;
    }

    curHungerBurnDown = maxHungerTime;

    curHungerTime = 0f;
    curAirlessTime = 0f;
    curAirFill = 0f;

    curTripTime = 0f;
  }

  // Update is called once per frame
  void Update()
  {
    isAirOn = ( air.plug.currentBattery != null ) && air.isActive;
    ThrobHurt();
    if (curHungerBurnDown > 0)
      curHungerBurnDown -= Time.deltaTime;
    else
    {
      if (curHungerTime < maxHungerTime)
        curHungerTime += Time.deltaTime;
      else
        TriggerEndgame(false, "You passed out from being hungry for too long.");
    }

    if (isAirOn)
    {
      curAirFill = Mathf.Min(maxAirlessTime, curAirFill + Time.deltaTime * airRefillRate);
    }
    else
    {
      if (curAirlessTime < maxAirlessTime)
        curAirlessTime += Time.deltaTime;
      else
        TriggerEndgame(false, "You went without air for too long and passed out.");
    }
    throbbing = !(curHungerBurnDown > 0 && isAirOn);
    /*TODO: (From Chris G.) This needs to be fixed so that the player doesent find out they loose because
     the engine was not online for enough time at the end. Or so that when the engine 
     has been off for too long it doesent keep going.*/
    curTripTime += Time.deltaTime;
    if (curTripTime >= totalTripTime)
    {
      if (totalTripTime - engine.timeActiveInSeconds < maxEngineOffTime) {
        TriggerEndgame(true, "Congrats! You've successfully kept your crappy used " +
          "spaceship together long enough to make it home!");
      }
      else
        TriggerEndgame(false, "Your engine stopped running for too long.");
    }
    UpdateFade();
  }

  void TriggerEndgame(bool won, string message)
  {
    gameOver = true;
    gameOverMessage = message;
    lost = !won;
  }
  void UpdateFade()
  {
    if( gameOver == true )
    {
      if (1f - fadeToBlack.color.a > colorFadeTolerence)
        fadeToBlack.color = new Color( 0f, 0f, 0f, fadeToBlack.color.a + ( fadeToBlackSpeed * Time.deltaTime ) );
      else if (lost == true)
        SceneChanger.GameOver(gameOverMessage);
    }
  }
  
  //<properties>

  public float TotalTripTime => totalTripTime;

  public float CurTripTime => curTripTime;

  public DeviceBase Air => air;

  public DeviceBase Engine => engine;

  public float HungerBurnDown => hungerBurnDown;

  public float CurHungerBurnDown => curHungerBurnDown;

  public float MaxHungerTime => maxHungerTime;

  public float CurHungerTime => curHungerTime;

  public float MaxAirlessTime => maxAirlessTime;

  public float CurAirlessTime => curAirlessTime;

  public float AirRefillRate => airRefillRate;

  public float CurAirFill => curAirFill;

  public bool IsAirOn => isAirOn;

  public float MaxEngineOffTime => maxEngineOffTime;

  //</properties>
  
}
