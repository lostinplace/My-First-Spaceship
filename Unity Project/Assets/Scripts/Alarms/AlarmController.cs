using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmController : MonoBehaviour
{
    public DeviceBase deviceBase;
    
    [SerializeField]
    private float messageGapTime = 5f;
    private float curGapTime;

    [SerializeField]
    private float flashTime = 0.5f;
    private float curFlashTime = 0.0f;
    private bool isFlashOn;

    private Renderer renderer;
    private Material mat;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        curGapTime = Random.Range(0, messageGapTime);

        renderer = GetComponent<Renderer>();
        // mat = rend.material;
        // mat.EnableKeyword("_EMISSION");
        baseColor = mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!deviceBase.isActive)
        {
            if (curFlashTime < flashTime)
            {
                curFlashTime += Time.deltaTime;
            }
            else
            {
                isFlashOn = !isFlashOn;
                curFlashTime = 0;
            }
            
            // todo: fix emissive
            if (isFlashOn)
            {
                Debug.Log("ONNNN");
                
                // set emmisive intensity +2
                //mat.SetColor("_EmissionColor", baseColor*2);
                renderer.material.SetColor("_EmissionColor", new Color(1f, 0f, 0f));

            }
            else
            {
                Debug.Log("OFFFFFF");
                //mat.SetColor("_EmissionColor", baseColor);
                renderer.material.SetColor("_EmissionColor", new Color(0f, 0f, 0f));

            }
        }
    }
}
