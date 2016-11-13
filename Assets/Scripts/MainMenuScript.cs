using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript: MonoBehaviour {
	
	[SerializeField]
	Toggle toggleVrMode;

	void Awake()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	public void StartGame()
	{
		if(toggleVrMode.isOn)
		{
			Application.LoadLevel("MainGameVR");
		}
		else
		{
			Application.LoadLevel("MainGame");
		}
	}
}
