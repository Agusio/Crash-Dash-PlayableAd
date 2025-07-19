using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform bobTarget;
    [SerializeField] private float bobAmount = 10f, bobSpeed = 2f;
    
    void Start()
    {
        StartCoroutine(Bob());
    }

    private IEnumerator Bob()
    {
        Vector3 startPos = bobTarget.anchoredPosition;
        var count = 0;

        while (count < 5)
        {
            float t = 0f;
            
            //This ensures a full loop that ends in the same position as it started
            while (t < Mathf.PI * 2f)
            {
                float yOffset = Mathf.Sin(t) * bobAmount;
                bobTarget.anchoredPosition = startPos + new Vector3(0, yOffset, 0);
                t += bobSpeed * Time.deltaTime;
                yield return null;
            }

            count++;
        }
    }
}
