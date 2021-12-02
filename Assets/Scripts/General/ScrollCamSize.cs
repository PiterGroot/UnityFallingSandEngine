using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScrollCamSize : MonoBehaviour
{
    private Camera cam;
    private float scrollListener;
    public bool CanScroll = true;
    [SerializeField]private bool InvertedControls = false; 
    [Space, SerializeField]private int MaxCamSize = 5;
    [SerializeField]private int MinCamSize = 5;
    [Space, SerializeField]private int StartCamSize = 5;
    [SerializeField]private float ScrollSpeed = 1;

    private void Awake() {
        cam = gameObject.GetComponent<Camera>();
        scrollListener = StartCamSize;
    }
    void Update()
    {
        if(CanScroll){
            if(!InvertedControls){
                if(Input.mouseScrollDelta.y > 0){
                    scrollListener-=ScrollSpeed;
                    PlaySound();
                
                }
                if(Input.mouseScrollDelta.y < 0){
                    scrollListener+=ScrollSpeed;
                    PlaySound();
                }
            }
            if(InvertedControls){
                if(Input.mouseScrollDelta.y > 0){
                    scrollListener+=ScrollSpeed;
                    PlaySound();
            }
                if(Input.mouseScrollDelta.y < 0){
                    scrollListener-=ScrollSpeed;
                    PlaySound();
                }
            }
            if(scrollListener < MinCamSize){
                scrollListener = MinCamSize;
            }
            if(scrollListener > MaxCamSize){
                scrollListener = MaxCamSize;
            }
            //vcam.m_Lens.OrthographicSize = scrollListener;
            cam.orthographicSize = scrollListener;
        }   
    }
    private void PlaySound(){
        //FindObjectOfType<AudioManager>().Play("Scroll");
    }
}
