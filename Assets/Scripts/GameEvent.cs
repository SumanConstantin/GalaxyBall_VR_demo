using UnityEngine;
using System.Collections;

public class GameEvent : MonoBehaviour {

	// Ball Event
	public delegate void BallEventDelegate (GameObject ball);
	public static event BallEventDelegate onBallLost;

	public static void BallLost(GameObject ball)
	{
		if(onBallLost != null)
		{
			onBallLost( ball );
		}
	}

	// Player Event
	public delegate void PlayerEventDelegate ();
	public static event PlayerEventDelegate onPlayerFireOne;

	public static void PlayerFireOne()
	{
		if(onPlayerFireOne != null)
		{
			onPlayerFireOne();
		}
	}

	// UI Count Event
	public delegate void UICountEventDelegate (int value = 0);
	public static event UICountEventDelegate onBallsCountUpdate;
	public static event UICountEventDelegate onWaveIdUpdate;

	public static void BallsCountUpdated(int value = 0)
	{
		if(onBallsCountUpdate != null)
		{
			onBallsCountUpdate(value);
		}
	}

	public static void WaveIdUpdated(int value = 0)
	{
		if(onWaveIdUpdate != null)
		{
			onWaveIdUpdate(value);
		}
	}

	// Level End Event
	public delegate void LevelEndEventDelegate (string _reason);
	public static event LevelEndEventDelegate onLevelEnd;

	public static void LevelEnded(string _reason)
	{
		if(onLevelEnd != null)
		{
			onLevelEnd(_reason);
		}
	}

	// Level Countdown Update Event
	public delegate void LevelCountdownUpdateEventDelegate (int loops);
	public static event LevelCountdownUpdateEventDelegate onLevelCountdownUpdated;

	public static void LevelCountdownUpdated(int loops)
	{
		if(onLevelCountdownUpdated != null)
		{
			onLevelCountdownUpdated(loops);
		}
	}

	// Level Countdown Finish Event
	public delegate void LevelCountdownFinishEventDelegate ();
	public static event LevelCountdownFinishEventDelegate onLevelCountdownFinished;

	public static void LevelCountdownFinished()
	{
		if(onLevelCountdownFinished != null)
		{
			onLevelCountdownFinished();
		}
	}

	// Score Update Event
	public delegate void ScoreUpdateEventDelegate(int value);
	public static event ScoreUpdateEventDelegate onScoreUpdated;

	public static void ScoreUpdated(int value)
	{
		if(onScoreUpdated != null)
		{
			onScoreUpdated(value);
		}
	}

	// Brick Break Event
	public delegate void BricksUpdateEventDelegate(int value);
	public static event BricksUpdateEventDelegate onBricksUpdated;

	public static void BricksUpdated(int value)
	{
		if(onBricksUpdated != null)
		{
			onBricksUpdated(value);
		}
	}
}
