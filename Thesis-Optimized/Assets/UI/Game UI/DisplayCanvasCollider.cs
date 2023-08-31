using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCanvasCollider : MonoBehaviour
{
   [SerializeField] private UndergroundBehavior undergroundBehavior;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!undergroundBehavior.WasCanvasDisplayed)
            {
                undergroundBehavior.DisplayUndergroundCanvas();
                gameObject.SetActive(false);
            }
        }
    }
}
