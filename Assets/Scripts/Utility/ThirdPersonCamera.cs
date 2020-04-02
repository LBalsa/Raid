// Source: https://forum.unity.com/threads/complete-camera-collision-detection-third-person-games.347233/

using UnityEngine;
namespace Utility
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public static ThirdPersonCamera playerCamera;
        [Header("Camera Properties")]
        private float DistanceAway;                     //how far the camera is from the player.

        public float minDistance = 1;                //min camera distance
        public float maxDistance = 2;                //max camera distance

        public float DistanceUp = 2;                    //how high the camera is above the player
        private float smooth = 4.0f;                    //how smooth the camera moves into place
        public float smoothBase = 4.0f;
        public float rotateAround = 70f;            //the angle at which you will rotate the camera (on an axis)

        [Header("Player to follow")]
        public Transform target;                    //the target the camera follows

        [Header("Layer(s) to include")]
        public LayerMask CamOcclusion;                //the layers that will be affected by collision

        [Header("Map coordinate script")]
        private//    public worldVectorMap wvm;
    RaycastHit hit;
        private float cameraHeight = 55f;
        private float cameraPan = 0f;
        private float camRotateSpeed = 180f;
        private Vector3 camPosition;
        private Vector3 camMask;
        private Vector3 followMask;

        private float HorizontalAxis;
        private float VerticalAxis;

        // Use this for initialization
        private void Start()
        {
            playerCamera = this;
            //the statement below automatically positions the camera behind the target.
            rotateAround = target.eulerAngles.y - 45f;


        }

        private void LateUpdate()
        {
            // Let camera roam if player is stationary and not in combat.
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0 && !PlayerController.inst.InCombat)
            {
                HorizontalAxis = Input.GetAxis("Mouse X");
                VerticalAxis = Input.GetAxis("Mouse Y");
            }
            // Else keep the camera behind the player.
            else if (PlayerController.inst.CanMove)
            {
                rotateAround = target.eulerAngles.y - 45f;
                HorizontalAxis = 0;
                VerticalAxis = 0;
            }

            //Offset of the targets transform (Since the pivot point is usually at the feet).
            Vector3 targetOffset = new Vector3(target.position.x, target.position.y, target.position.z);
            Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, cameraPan);
            Vector3 vectorMask = Vector3.one;
            Vector3 rotateVector = rotation * vectorMask;
            //this determines where both the camera and it's mask will be.
            //the camMask is for forcing the camera to push away from walls.
            camPosition = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;
            camMask = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;

            //occludeRay(ref targetOffset);
            // Raycasting to target position wrongly aims the camera at player's feet.
            occludeRay(new Vector3(targetOffset.x, targetOffset.y + 1f, targetOffset.z));

            smoothCamMethod();

            transform.LookAt(target);

            #region wrap the cam orbit rotation
            if (rotateAround > 360)
            {
                rotateAround = 0f;
            }
            else if (rotateAround < 0f)
            {
                rotateAround = (rotateAround + 360f);
            }
            #endregion

            rotateAround += HorizontalAxis * camRotateSpeed * Time.deltaTime;

            //Optional
            DistanceUp = Mathf.Clamp(DistanceUp += VerticalAxis, 0.5f, 2.3f);
            DistanceAway = Mathf.Clamp(DistanceAway += VerticalAxis, minDistance, maxDistance);

        }

        private void smoothCamMethod()
        {

            smooth = smoothBase;
            transform.position = Vector3.Lerp(transform.position, camPosition, Time.deltaTime * smooth);
        }

        private void occludeRay(ref Vector3 targetFollow)
        {
            #region prevent wall clipping
            //declare a new raycast hit.
            RaycastHit wallHit = new RaycastHit();
            //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
            if (Physics.Linecast(targetFollow, camMask, out wallHit, CamOcclusion))
            {
                //the smooth is increased so you detect geometry collisions faster.
                smooth = 10f;
                //the x and z coordinates are pushed away from the wall by hit.normal.
                //the y coordinate stays the same.
                camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
            }
            #endregion
        }

        private void occludeRay(Vector3 targetFollow)
        {
            #region prevent wall clipping
            //declare a new raycast hit.
            RaycastHit wallHit = new RaycastHit();
            //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
            if (Physics.Linecast(targetFollow, camMask, out wallHit, CamOcclusion))
            {
                //the smooth is increased so you detect geometry collisions faster.
                smooth = 10f;
                //the x and z coordinates are pushed away from the wall by hit.normal.
                //the y coordinate stays the same.
                camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
            }
            #endregion
        }
    }
}