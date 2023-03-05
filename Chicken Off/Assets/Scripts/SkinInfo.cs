using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinInfo : MonoBehaviour
{
    [SerializeField] private Sprite headshot;
    [SerializeField] private Color color = Color.white;
    

    public Color GetColor() { return color; }

    public Sprite GetSprite() { return headshot; }
}
