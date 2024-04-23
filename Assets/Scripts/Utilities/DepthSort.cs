using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSort : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _spriteRenderer.sortingOrder= (int)(transform.position.y * -100);
    }
}
