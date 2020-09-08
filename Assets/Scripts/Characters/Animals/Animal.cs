using UnityEngine;

namespace Characters.Animals
{
    public abstract class Animal : MonoBehaviour, IAnimal
    {
        public abstract void Aproach(GameObject other);

        protected void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ICharacter>() != null)
            {
                Aproach(other.gameObject);
            }
        }
    }
}
