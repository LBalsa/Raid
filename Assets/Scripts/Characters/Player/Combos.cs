using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combos : MonoBehaviour {

    public enum inputs { Square, Triangle };
    Combos futureState;
    float comboStartTime, comboEndTime;
    public Animator attachedAnimation;
    string comboName;

    public abstract void GiveInput(inputs anInput);
}
