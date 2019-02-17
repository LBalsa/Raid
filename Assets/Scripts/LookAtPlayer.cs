using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject target;
    public bool canvas = false;

    private void Update()
    {
        // transform.position - PlayerController.inst.transform.position
        var lookPos = canvas ? transform.position - Camera.main.transform.position :PlayerController.inst.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

    }
}
