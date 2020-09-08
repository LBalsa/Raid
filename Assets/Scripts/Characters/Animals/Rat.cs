using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Animals
{
    public class Rat : Animal
    {
        [SerializeField]
        public GameObject effect = null;
        bool on = false;

        public override void Aproach(GameObject other)
        {
            if (!on)
            {
                on = true;
                // Calculate vector to run away;
                Vector3 dir = transform.position - other.transform.position;
                dir = dir.normalized;
                transform.rotation = Quaternion.LookRotation(dir);
                // transform.eulerAngles = dir;
                // Run
                GetComponent<Animator>().SetBool("Walk", true);
                StartCoroutine(MoveObject(transform.position, dir, 2));
            }
        }

        IEnumerator MoveObject(Vector3 startPos, Vector3 endPos, float time)
        {
            var i = 0.0f;
            var rate = 1.0f / time;
            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                transform.position += endPos * 1.75f * Time.deltaTime;


                //transform.position = Vector3.Lerp(startPos, endPos, i);
                yield return null;
            }

            Instantiate(effect, transform.position, Quaternion.identity);
            yield return 5;
            Destroy(this.gameObject);
        }
    }
}