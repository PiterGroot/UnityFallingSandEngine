using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private Camera cam;
    private bool DebugMenuState = false;
    public static float fps;
    private GUIStyle Shadow = new GUIStyle();
    private GUIStyle FPSText = new GUIStyle();

    private void Awake() {
        cam = Camera.main;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.F3)){
            DebugMenuState = !DebugMenuState;
        }
    }
    private void FixedUpdate() {
        if(DebugMenuState){
            //GUI LABEL STUFF
            FPSText.fontSize = 20;
            FPSText.fontStyle = FontStyle.Bold;
            FPSText.normal.textColor = Color.black;
            Shadow.fontSize = 20;
            Shadow.fontStyle = FontStyle.Bold;
            Shadow.normal.textColor = Color.white;
        }
        else{
            FPSText.normal.textColor = new Color(0, 0 , 0 , 0);
            Shadow.normal.textColor = new Color(0, 0 , 0 , 0);
        }
    }
    private void OnGUI(){ 
          float newFps = (int)(1f / Time.unscaledDeltaTime);
        fps = Mathf.Lerp(fps, newFps, 0.0005f);
        GUI.Label(new Rect(25, 22, 100, 100), "FPS: " + ((int)fps).ToString(), FPSText);
        GUI.Label(new Rect(25, 20, 100, 100), "FPS: " + ((int)fps).ToString(), Shadow);
        GUI.Label(new Rect(25, 47, 100, 100), $"XYZ: {cam.transform.position}", FPSText);
        GUI.Label(new Rect(25, 45, 100, 100), $"XYZ: {cam.transform.position}", Shadow);
    }
}
