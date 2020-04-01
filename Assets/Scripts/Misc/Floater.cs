using UnityEngine;
using System.Collections;

public class Floater : MonoBehaviour
{
    public float waterLevel, floatHeight;
    public Vector3 buoyancyCentreOffset;
    public float bounceDamp;

    public Vector2 xMinMax = new Vector2(0, 0);
    public Vector2 zMinMax = new Vector2(0, 0);
    private void Start()
    {
        InvokeRepeating("Change", 1, 1);
    }

    void FixedUpdate()
    {
        Vector3 actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
        float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

        if (forceFactor > 0f)
        {
            Vector3 uplift = -Physics.gravity * (forceFactor - GetComponent<Rigidbody>().velocity.y * bounceDamp);
            GetComponent<Rigidbody>().AddForceAtPosition(uplift, actionPoint);
        }


    }

    private void Change()
    {
        if (buoyancyCentreOffset.x > xMinMax.x || buoyancyCentreOffset.z > zMinMax.x)
        {
            buoyancyCentreOffset.x = xMinMax.x;
            buoyancyCentreOffset.z = zMinMax.x;

        }
        else if (buoyancyCentreOffset.x < xMinMax.y || buoyancyCentreOffset.z > zMinMax.y)
        {
            buoyancyCentreOffset.x = xMinMax.y;
            buoyancyCentreOffset.z = zMinMax.y;
        }
    }
}
