using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DepthSort : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public int _offset = 0;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _spriteRenderer.sortingOrder= (int)((transform.position.y + _offset) * -100);
    }
}
