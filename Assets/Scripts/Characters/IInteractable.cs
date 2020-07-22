using UnityEngine;

namespace Characters
{
    public delegate void EndInteraction();
    public interface IInteractable
    {
        event EndInteraction OnEndInteraction;
        void StartInteraction(GameObject other);
        void EndInteraction();
    }
}
