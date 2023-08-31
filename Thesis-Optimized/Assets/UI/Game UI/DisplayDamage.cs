using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDamage : MonoBehaviour
{
    [SerializeField] private Canvas impactCanvas;
    [SerializeField] private float impactTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        impactCanvas.enabled = false;
    }

    public void OnDamageTaken()
    {
        StartCoroutine(ShowImpactEffect());
    }

    private IEnumerator ShowImpactEffect()
    {
        impactCanvas.enabled = true;
        yield return new WaitForSeconds(impactTime);
        impactCanvas.enabled = false;
    }
}
