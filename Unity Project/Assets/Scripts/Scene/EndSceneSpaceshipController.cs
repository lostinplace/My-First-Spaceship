using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneSpaceshipController : MonoBehaviour
{
    [SerializeField] private float rotSpeedZ;

    [SerializeField] private float rotSpeedY;

    [SerializeField] private float parabolaScale; // affects how fast the rocket travels
    
    private float x = 0;
    private float y;

    private Vector3 initPosn;
    
    // Start is called before the first frame update
    void Start()
    {
        initPosn = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        x += Time.deltaTime;
        y = parabola(x);

        transform.position = new Vector3(initPosn.x + x*parabolaScale, initPosn.y + y*parabolaScale, initPosn.z);
        
        transform.Rotate(Vector3.up, rotSpeedZ * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotSpeedY * Time.deltaTime);
    }

    private float parabola(float x)
    {
        return Mathf.Sqrt(4f*(x+1f)) + 1f;
    }
}
