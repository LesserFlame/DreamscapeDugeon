using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TextureScrolling : MonoBehaviour
{
    public Vector2 speed;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.material = new Material(image.material);
    }

    void Update()
    {
        image.material.mainTextureOffset += speed * Time.deltaTime;
    }
}
