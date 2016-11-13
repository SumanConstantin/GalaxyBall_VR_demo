using UnityEngine;
using System.Collections;

public class Context : MonoBehaviour
{
	// Controllers
	public static GameModel gameModel;
	public static PlayerModel playerModel;
	public static UIModel uiModel;
	public static PrefabsMap prefabsMap;
	public static CameraModel cameraModel;

	// Use this for initialization
	void Start ()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Input.gyro.enabled = false;

		SetupModels();
		InitEventListeners();
	}

	private void SetupModels()
	{		
		prefabsMap = FindObjectOfType<PrefabsMap>();
		prefabsMap.Init();

		playerModel = new PlayerModel();
		uiModel = new UIModel();
		gameModel = new GameModel();
		cameraModel = new CameraModel();
	}

	void Update()
	{
		playerModel.Update();
		gameModel.Update();
	}

	void FixedUpdate()
	{
		playerModel.FixedUpdate();
		cameraModel.UpdateRotation(playerModel.moveHorizontalLimited);
	}

	private void InitEventListeners()
	{
		GameEvent.onLevelEnd += OnLevelEnd;
	}

	private void RemoveEventListeners()
	{
		GameEvent.onLevelEnd -= OnLevelEnd;
	}

	private void Destroy()
	{
		RemoveEventListeners();

		playerModel.Destroy();
		uiModel.Destroy();
		gameModel.Destroy();
		cameraModel.Destroy();
	}

	private void OnLevelEnd(string reason)
	{
		Destroy();

		Application.LoadLevel("MainMenu");
	}
}
