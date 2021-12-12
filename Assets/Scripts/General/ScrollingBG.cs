using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBG : MonoBehaviour
{
    [SerializeField]private float scrollSpeed;
    private Renderer rend;
    private void Awake() {
        rend = gameObject.GetComponent<Renderer>();
    }
    private void Update() {
        rend.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, -scrollSpeed * Time.deltaTime);
    }
}
