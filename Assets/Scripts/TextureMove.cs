using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMove : MonoBehaviour
{
    private Material _mat;
    //Tracking the value of the offset is required to avoid the texture to reset itself;
    private float _offsetY;
    [SerializeField] private float speed = -0.6f;
    private void Start()
    {
        _mat = GetComponent<Renderer>().material;
        GameManager.Instance.OnGameRestart += () =>
        {
            _offsetY = 0;
            _mat.SetFloat("_OffsetY", 0);
        };
    }

    private void Update()
    {
        if (GameManager.IsPaused) return;
        _offsetY += Time.deltaTime * speed;
        _mat.SetFloat("_OffsetY", _offsetY);
    }
}
