using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Select a button from the menu if the event system does not do so automatically.
public class SelectMenuButton : MonoBehaviour
{
    public GameObject buttonToBeSelected;
    private bool buttonSelected = false;

    private void Awake()
    {
        OnEnable();
    }

    private void OnEnable()
    {
      // Find button to be selected if not specified.
        if (!buttonToBeSelected)
        {
            buttonToBeSelected = GetComponentInChildren<Button>().gameObject;
        }
        EventSystem.current.SetSelectedGameObject(buttonToBeSelected);
        buttonSelected = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        //if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(buttonToBeSelected);
            buttonSelected = true;
        }
    }
    private void OnDisable()
    {
        buttonSelected = false;
    }
}
