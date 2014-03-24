using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {
	public float fadeSpeed = 1.5f; 
	bool fadeIn;
	bool fadeOut;
	
	void Start () {
		guiTexture.pixelInset = new Rect(Screen.width/2, Screen.height/2, 0, 0);
		fadeIn = true;
        guiTexture.color = Color.clear;
	}
	
    void Update ()
    {
        if(fadeIn)
            StartScene();
		else if (fadeOut)
			EndScene();
    }
	
	void OnGUI()
	{
		if (!fadeIn && !fadeOut) {
			GUILayout.BeginArea(new Rect(Screen.width/2 - 50, Screen.height/2 - 50, 100, 100));
			
			if (GUILayout.Button("Новая игра")) 
				StartGame();
			
			if (GUILayout.Button("Выход"))
				Application.Quit();		
			
			GUILayout.EndArea();
		}
	}
	
	void StartGame()
	{
		fadeOut = true;
	}
	
    void FadeToClear ()
    {
        guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
        audio.volume = Mathf.Lerp(audio.volume, 0, fadeSpeed * Time.deltaTime);
    }
    
    void FadeToNormal ()
    {
        guiTexture.color = Color.Lerp(guiTexture.color, Color.gray, fadeSpeed * Time.deltaTime);
        audio.volume = Mathf.Lerp(audio.volume, 1, fadeSpeed * Time.deltaTime);
    }
	
    void EndScene ()
    {
        FadeToClear();
        
        if(guiTexture.color.a <= 0.05f)
        {
        	guiTexture.enabled = false;
			Application.LoadLevel(1);
        }
    }
	
    public void StartScene ()
    {
        guiTexture.enabled = true;
        
        FadeToNormal();
        
        if(guiTexture.color.a >= 0.95f) {
			guiTexture.color = Color.gray;
 			fadeIn = false;
		}
    }	
}
