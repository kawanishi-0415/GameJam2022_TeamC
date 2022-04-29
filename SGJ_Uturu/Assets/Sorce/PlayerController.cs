using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uturu
{
    public class PlayerController : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.Translate(-3, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.Translate(3, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
        }
    }
}
