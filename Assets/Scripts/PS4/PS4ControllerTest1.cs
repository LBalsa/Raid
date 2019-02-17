//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class Gesture
//{
//    public Vector2[] inputs;
//}

//public class PS4ControllerTest1 : MonoBehaviour
//{

//    [SerializeField]
//    public Gesture[] gestures;

//    public bool inGesture;
//    public int gestureIndex;
//    public bool[] activeGestures;

//    Use this for initialization

//   void Start () {
//        activeGestures = new bool[gestures.Length];
//    }

//    Update is called once per frame

//    void Update()
//    {

//        Vector2 inputVector = new Vector2(Input.GetAxis("RightJoystickHorizontal"), Input.GetAxis("RightJoystickVertical"));

//        if (!inGesture) //first direction (we're not YET doing a gesture)
//        {
//            for (int i = 0; i < gestures.Length; ++i)
//            {
//                if (inputVector.x == gestures[i].inputs[0].x && inputVector.y == gestures[i].inputs[0].y)
//                {
//                    activeGestures[i] = true;
//                    inGesture = true;
//                    gestureIndex = 1;
//                }
//                else
//                {
//                    activeGestures[i] = false;
//                }
//            }
//        }
//        else //All the next directional inputs...
//        {
//            bool noPossibleGestures = true;
//            for (int i = 0; i < gestures.Length; ++i)
//            {
//                if (activeGestures[i])
//                {
//                    if (gestures[i].inputs.Length > gestureIndex)
//                    {
//                        if (inputVector.x == gestures[i].inputs[gestureIndex].x && inputVector.y == gestures[i].inputs[gestureIndex].y)
//                        {
//                            activeGestures[i] = true;
//                            ++gestureIndex;
//                            noPossibleGestures = false;
//                        }
//                        else
//                        {
//                            activeGestures[i] = false;
//                        }
//                    }
//                    else
//                    {
//                        GestureComplete(i);
//                    }
//                }
//            }
//            if (noPossibleGestures)
//            {
//                inGesture = false;
//                for (int i = 0; i < activeGestures.Length; ++i)
//                {
//                    activeGestures[i] = false;
//                }
//            }
//        }
//    }

//    HANDLE COMPLETED GESTURES
//    private void GestureComplete(int index)
//    {
//        switch (index)
//        {
//            case 0:
//                DO GESTURE 0
//                Debug.Log("Gesture 0");
//                break;
//            case 1:
//                DO GESTURE 0
//                Debug.Log("Gesture 1");
//                break;
//            case 2:
//                DO GESTURE 0
//                Debug.Log("GESTURE 2");
//                break;
//            default:
//                break;
//        }
//    }
//}
