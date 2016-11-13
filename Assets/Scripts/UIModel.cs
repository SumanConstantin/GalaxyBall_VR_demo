using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIModel
{
	private GameObject[] hudBalls;
	private Text wavesText;
	private Text levelBeginText;

	public UIModel()
	{
		Init();
	}

	private void Init ()
	{
		InitGameHud();
		InitEventListeners();
	}

	private void InitGameHud()
	{
		// Init Ball images
		hudBalls = new GameObject[5];
		for( int i=0; i<hudBalls.Length; i++)
		{
			hudBalls[i] = GameObject.Find("BallImg" + i);
		}

		wavesText = GameObject.Find("WaveCount_Number").GetComponent<Text>();
		levelBeginText = GameObject.Find("LevelBegin_Number").GetComponent<Text>();
	}

	private void InitEventListeners()
	{
		GameEvent.onBallsCountUpdate += OnUpdateBallsCount;
		GameEvent.onWaveIdUpdate += OnUpdateWaveId;
		GameEvent.onLevelCountdownUpdated += OnLevelCountdownUpdate;
		GameEvent.onLevelCountdownFinished += OnLevelCountdownFinish;
	}

	private void RemoveEventListeners()
	{
		GameEvent.onBallsCountUpdate -= OnUpdateBallsCount;
		GameEvent.onWaveIdUpdate -= OnUpdateWaveId;
		GameEvent.onLevelCountdownUpdated -= OnLevelCountdownUpdate;
		GameEvent.onLevelCountdownFinished -= OnLevelCountdownFinish;
	}

	private void OnUpdateBallsCount(int value)
	{
		// HudBalls
		for( int i = 0; i < hudBalls.Length; i++)
		{
			hudBalls[i].SetActive( i + 1 <= value);
		}
	}

	private void OnUpdateWaveId(int value)
	{
		// Waves
		wavesText.text = value.ToString();
	}

	private void OnLevelCountdownUpdate(int count)
	{
		levelBeginText.enabled = true;
		levelBeginText.text = count.ToString();
	}

	public void OnLevelCountdownFinish()
	{
		levelBeginText.enabled = false;
	}
	
	void OnDisable()
	{
		RemoveEventListeners();
	}

	public void Destroy()
	{
		RemoveEventListeners();

		// HudBalls
		for( int i = 0; i < hudBalls.Length; i++)
		{
			MonoBehaviour.Destroy(hudBalls[i]);
		}
	}
}
