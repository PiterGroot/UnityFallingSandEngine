using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    [SerializeField]private Font debugFont;
    [SerializeField]private string gameVersion;
    private Camera cam;
    private bool DebugMenuState = false;
    public static float fps;
    private GUIStyle Shadow = new GUIStyle();
    private GUIStyle FPSText = new GUIStyle();
    private GameManager gameManager;
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        cam = Camera.main;
        //FPSText.font = debugFont;
        //Shadow.font = debugFont;
        if(PlayerPrefs.GetInt("DebugMenu") == 0){
            //menu is off 
            FPSText.normal.textColor = new Color(0, 0 , 0 , 0);
            Shadow.normal.textColor = new Color(0, 0 , 0 , 0);
        }
        else{
            FPSText.fontSize = 20;
            FPSText.fontStyle = FontStyle.Bold;
            FPSText.normal.textColor = Color.black;
            Shadow.fontSize = 20;
            Shadow.fontStyle = FontStyle.Bold;
            Shadow.normal.textColor = Color.white;
            DebugMenuState = true;
            PlayerPrefs.SetInt("DebugMenu", 1);
        }
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.F3)){
            DebugMenuState = !DebugMenuState;
            if(DebugMenuState){
                //GUI LABEL STUFF
                FPSText.fontSize = 20;
                FPSText.fontStyle = FontStyle.Bold;
                FPSText.normal.textColor = Color.black;
                Shadow.fontSize = 20;
                Shadow.fontStyle = FontStyle.Bold;
                Shadow.normal.textColor = Color.white;
                PlayerPrefs.SetInt("DebugMenu", 1);
            }
            else{
                FPSText.normal.textColor = new Color(0, 0 , 0 , 0);
                Shadow.normal.textColor = new Color(0, 0 , 0 , 0);
                PlayerPrefs.SetInt("DebugMenu", 0);
            }
        }
    }
    private void OnGUI(){ 
        float newFps = (int)(1f / Time.unscaledDeltaTime);
        fps = Mathf.Lerp(fps, newFps, 0.001f);
        GUI.Label(new Rect(25, 22, 100, 100), "FPS: " + ((int)fps).ToString(), FPSText);
        GUI.Label(new Rect(25, 20, 100, 100), "FPS: " + ((int)fps).ToString(), Shadow);
        GUI.Label(new Rect(25, 47, 100, 100), $"XYZ: {cam.transform.position}", FPSText);
        GUI.Label(new Rect(25, 45, 100, 100), $"XYZ: {cam.transform.position}", Shadow);
        GUI.Label(new Rect(25, 72, 100, 100), $"± simulated entities: {gameManager.simulatedCells.ToString()}", FPSText);
        GUI.Label(new Rect(25, 70, 100, 100), $"± simulated entities: {gameManager.simulatedCells.ToString()}", Shadow);
        GUI.Label(new Rect(25, 97, 100, 100), $"Falling Sand Engine {gameVersion.ToString()} @PiterGroot", FPSText);
        GUI.Label(new Rect(25, 95, 100, 100), $"Falling Sand Engine {gameVersion.ToString()} @PiterGroot", Shadow);
    }
    [ContextMenu("ResetDebugMenuSave")]
    private void ResetDebugMenuSave(){
        PlayerPrefs.DeleteKey("DebugMenu");
        Debug.LogWarning("Deleted debug playerpref");
    }
    private void OnApplicationQuit() {
        PlayerPrefs.DeleteKey("DebugMenu");
    }
}
