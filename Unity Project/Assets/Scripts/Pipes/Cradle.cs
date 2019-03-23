using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/*Stationary part of the ship that other pipes plug into.*/
public partial class Cradle : MonoBehaviour
{
  public bool startWithPipe = true;

  public float overrideStartingPipeIntegrity = 0;
  public float overrideStartingPipeHeat = 0;
  
  public CradleNetwork network { get; set; }

  public DeviceBase device => network.device;

  private MeshRenderer placeholderRenderer;

  private static PlayerState playerState => SceneChanger.playerState;

  public UnityEvent pipeInAudio, pipeOutAudio;

  bool isLoading = true;

  public bool isUICradle = false;

  void Start()
  {

    if (startWithPipe)
    {
      var prefab = Resources.Load("RuntimePipe");
      var obj = (GameObject)Instantiate(prefab);
      ((GameObject) obj).GetComponent<Pipe>().IsUIPipe = isUICradle;
      var myPipe = obj.GetComponent<Pipe>();
      
      if (overrideStartingPipeIntegrity != 0) myPipe.currentIntegrity = overrideStartingPipeIntegrity;
      if (overrideStartingPipeHeat != 0) myPipe.currentHeat = overrideStartingPipeHeat;
      
      AttachPipe(myPipe);
    }

    var tmp = transform.Find("PipeArea/Placeholder");
    placeholderRenderer = tmp.GetComponent<MeshRenderer>();
    
    isLoading = false;
  }

  public void AttachPipe(Pipe aPipe)
  {
    if(!connectPipe(aPipe)) return;

    aPipe.currentCradle = this;
    aPipe.Lock();

    var pipeAreaTransform = this.gameObject.GetComponentInChildren<Transform>().Find("PipeArea");
    var capsuleCollider = this.gameObject.GetComponent<CapsuleCollider>();

    aPipe.transform.SetPositionAndRotation(pipeAreaTransform.position, pipeAreaTransform.rotation);

    if (!isLoading)
    {
      pipeInAudio.Invoke();
    }
  }

  public void DetachPipe()
  {
    connectedPipe = null;
    pipeOutAudio.Invoke();
  }

  private void OnTriggerStay(Collider other)
  {
    Pipe tmpPipe = other.gameObject.GetComponent<Pipe>();
    if (!tmpPipe || connectedPipe || tmpPipe.currentCradle || tmpPipe.IsBeingHeld) return;
    this.AttachPipe(tmpPipe);
  }

  void Update() {
    placeholderRenderer.enabled = ( playerState ? playerState.PipesHeld > 0 : false ) && !this.connectedPipe ;
  }

}
