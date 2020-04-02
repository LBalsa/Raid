using UnityEngine;

namespace Utility
{
public class AirFloater : MonoBehaviour
{
    public bool _float,_rotate=true;
    public float degreesPerSecond = -75.0f;
    public float amplitude = 0.1f;
    public float frequency = 1f;
    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();

    // Use this for initialization
    private void Start()
    {
        posOffset = this.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_rotate)
        {
            this.transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
        }
        if (_float)
        {
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            this.transform.position = tempPos;
        }
    }
}
    }
