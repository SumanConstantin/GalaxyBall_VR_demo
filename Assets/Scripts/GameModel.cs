using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModel
{
	private const string GAME_STATE_INIT = "gameStateInit";
	private const string GAME_STATE_BEGIN_COUNTDOWN = "gameStateBeginCountdown";
	private const string GAME_STATE_PLAYING = "gameStatePlaying";
	private const string GAME_STATE_STOPPED = "gameStateStopped";

	public GameObject brickPrefab;
	public GameObject ballPrefab;
	public int ballsAvailable;

	private int ballsCountInit = 5;

	//private Vector3 initBrickPos = new Vector3(-4,0,5);
	private Vector3 initBrickPos = new Vector3(-4,9,10);
	private float brickWidth = 1;
	private float brickHeight = .5f;

	private GameObject playerChar;
	private List<GameObject> balls;			// Active balls that currently are fixed on the char and also the ones which are flying
	private List<BallScript> ballScripts;	// Cashed scripts attached to "balls" to make calls on frame-updates less expensive
	private GameObject[] bricks;
	private CountDownTimer countDownTimer;
	private bool doAutoLaunchBalls = true;

	private static int _score = 0;
	public static int Score
	{
		get { return _score; }
		set { _score = value; }
	}

	private static string _gameState = GAME_STATE_INIT;
	public static string GameState
	{
		get { return _gameState; }
		set { _gameState = value; }
	}

	private int _bricksCount = 0;
	public int BricksCount
	{
		get{return _bricksCount;}
		set
		{
			_bricksCount = value;

			if(value == 0)
			{
				OnAllBricksDestroyed();
			}
		}
	}

	private int _ballsCount = 0;
	public int BallsCount
	{
		get{return _ballsCount;}
		set
		{
			_ballsCount = value;
			GameEvent.BallsCountUpdated(value);
		}
	}

	public void Destroy()
	{
		this.RemoveEventListeners();

		MonoBehaviour.Destroy(brickPrefab);
		MonoBehaviour.Destroy(ballPrefab);
		//MonoBehaviour.Destroy(initBrickPos);
		MonoBehaviour.Destroy(playerChar);
		//MonoBehaviour.Destroy(countDownTimer);
		MonoBehaviour.Destroy(brickPrefab);

		for(int i = 0; i < balls.Count; i++)
		{
			if(balls[i] != null)
			{
				MonoBehaviour.Destroy(balls[i]);
			}
		}

		for(int i = 0; i < ballScripts.Count; i++)
		{
			if(ballScripts[i] != null)
			{
				MonoBehaviour.Destroy(ballScripts[i]);
			}
		}

		for(int i = 0; i < bricks.Length; i++)
		{
			if(bricks[i] != null)
			{
				MonoBehaviour.Destroy(bricks[i]);
			}
		}
	}

	public GameModel()
	{
		Init();
	}

	private void Init()
	{
		GameState = GAME_STATE_INIT;
		StartGame();
	}

	public void StartGame ()
	{
		BallsCount = ballsCountInit;

		LevelModel.InitLevelData(1);
		LevelModel.InitWave(1);

		InitWave();

		AddBallToCharFromStack();
		InitEventListeners();

		StartWave();
	}

	public void Update()
	{
		switch( GameState)
		{
			case GAME_STATE_STOPPED:
				break;
			case GAME_STATE_BEGIN_COUNTDOWN:
				countDownTimer.Update();
				UpdateBalls();
				break;
			case GAME_STATE_PLAYING:
				UpdateBalls();
				break;
		}
	}

	private void LevelBegin()
	{
		if(GameState == GAME_STATE_BEGIN_COUNTDOWN)
		{
			GameState = GAME_STATE_PLAYING;

			// Auto-launch balls
			if(doAutoLaunchBalls)
			{
				LaunchBalls();
			}
		}
	}

	private void InitWave()
	{
		Debug.Log ("InitWave() " + LevelModel.WaveID);

		if(LevelModel.InitWave(LevelModel.WaveID))
		{
			InitPlayerChar();
			InitBricks();
			InitBalls();
			InitLaunchBallCounter();
		}
	}

	private void StartWave()
	{
		StartLaunchBallCounter();
	}

	private void StartLaunchBallCounter()
	{
		countDownTimer.Start();
		GameState = GAME_STATE_BEGIN_COUNTDOWN;
	}

	private void InitPlayerChar()
	{
		playerChar = Context.playerModel.playerChar;
	}

	private void InitBricks()
	{
		// TODO: add brickFactory

		if(bricks!=null)
		{
			for(int i = 0; i < bricks.Length; i++)
			{
				if(bricks[i] != null)
				{
					MonoBehaviour.Destroy(bricks[i]);
				}
			}
		}

		bricks = new GameObject[200];

		int rowsCount = 0;
		int collumnsCount = 0;
		int len = LevelModel.bricks.Length;
		for(int i = 0; i < len; i++)
		{
			if(LevelModel.bricks[i] == 1)
			{
				Vector3 pos = new Vector3();
				pos.x = initBrickPos.x + (brickWidth * collumnsCount);
				pos.y = initBrickPos.y - (brickHeight * rowsCount);
				pos.z = initBrickPos.z;

				BricksCount++;
				bricks[i] = MonoBehaviour.Instantiate( 
					PrefabsMap.brickPrefab,
					new Vector3(pos.x, pos.y, pos.z),
					new Quaternion( -90, -90, 0, 0 )) as GameObject;
			}
			else
				if(LevelModel.bricks[i] == -1)
				{
					rowsCount++;
					collumnsCount = -1;
				}

			collumnsCount++;
		}
	}

	private void InitBalls()
	{
		// TODO: add ballFactory
		balls = new List<GameObject>();
		ballScripts = new List<BallScript>();
	}

	private void InitLaunchBallCounter()
	{
		if(countDownTimer == null)
		{
			countDownTimer = new CountDownTimer(3);
			countDownTimer.Init();
		}

		countDownTimer.Reset();
	}

	private void AddBallToCharFromStack()
	{
		if(BallsCount > 0)
		{
			AddBallToChar();
			BallsCount--;
		}
		else
			OnNoBallsLeft();
	}

	private GameObject AddBallToChar()
	{
		GameObject ball = MonoBehaviour.Instantiate(PrefabsMap.ballPrefab, 
			new Vector3(
				playerChar.transform.position.x,
				playerChar.transform.position.y,
				playerChar.transform.position.z), 
			new Quaternion()) as GameObject;

		// Setting the playerChar in the ball to allow movement of ball when 
		//the ball is stuck on the playerChar
		ball.GetComponent<BallScript>().InitPlayerChar(playerChar);

		balls.Add( ball );
		ballScripts.Add( ball.GetComponent<BallScript>() );

		return ball;
	}

	private void InitEventListeners()
	{
		GameEvent.onPlayerFireOne += OnPlayerFireOne;
		GameEvent.onBallLost += OnBallLost;
		GameEvent.onLevelCountdownFinished += OnLevelCountdownFinish;
		GameEvent.onBricksUpdated += OnBricksUpdated;
		GameEvent.onScoreUpdated += OnScoreUpdated;

		countDownTimer.OnUpdated += GameEvent.LevelCountdownUpdated;
		countDownTimer.OnFinished += GameEvent.LevelCountdownFinished;
	}

	private void RemoveEventListeners()
	{
		GameEvent.onPlayerFireOne -= OnPlayerFireOne;
		GameEvent.onBallLost -= OnBallLost;
		GameEvent.onLevelCountdownFinished -= OnLevelCountdownFinish;
		GameEvent.onBricksUpdated -= OnBricksUpdated;
		GameEvent.onScoreUpdated -= OnScoreUpdated;

		countDownTimer.OnUpdated -= GameEvent.LevelCountdownUpdated;
		countDownTimer.OnFinished -= GameEvent.LevelCountdownFinished;
	}


	// Init
	//-----

	//-----
	// Runtime

	public void OnPlayerFireOne()
	{
		LaunchBalls();
	}

	public void OnBallLost(GameObject _ball)
	{
		RemoveBall(_ball);
		AddBallToCharFromStack();

		InitLaunchBallCounter();
		StartLaunchBallCounter();
	}

	public void OnLevelCountdownFinish()
	{
		LevelBegin();
	}

	public void OnBricksUpdated(int value)
	{
		BricksCount += value;
	}

	public void OnScoreUpdated(int value)
	{
		Score += value;
	}

	private void RemoveBall(GameObject _ball)
	{
		if(balls != null)
		{
			for(int i = 0; i < balls.Count; i++)
			{
				if(balls[i] == _ball)
				{
					MonoBehaviour.Destroy(balls[i]);
					MonoBehaviour.Destroy(ballScripts[i]);
					balls.Remove(balls[i]);
					ballScripts.Remove(ballScripts[i]);

					break;
				}
			}
		}
	}

	private void LaunchBalls()
	{
		int len = ballScripts.Count;
		for(int i = 0; i < len; i++)
		{
			ballScripts[i].Launch();
		}
	}

	public void UpdateBalls()
	{
		if(ballScripts == null)
		{
			return;
		}

		int len = ballScripts.Count;
		for(int i = 0; i < len; i++)
		{
			ballScripts[i].MoveWithChar();
		}
	}

	private void OnAllBricksDestroyed()
	{
		StartNextWave();
	}

	private void StartNextWave()
	{
		if(LevelModel.WaveID + 1 <= LevelModel.waveIDMax)
		{
			LevelModel.WaveID++;

			CleanUp();	// Remove flying balls
			InitWave();
			StartWave();
			AddBallToChar();
		}
		else
		{
			// Disable controls
			this.RemoveEventListeners();

			// Level finished: WIN
			InitGameWin();

			// TODO: Show "You Win" message

			// TODO: Show map, load next level, etc.


		}
	}

	private void OnNoBallsLeft()
	{
		InitGameLose();
	}

	private void InitGameWin()
	{
		GameEvent.LevelEnded("win");
		GameState = GAME_STATE_STOPPED;
		//Application.LoadLevel("MainMenu");
	}

	private void InitGameLose()
	{
		GameEvent.LevelEnded("lose");
		GameState = GAME_STATE_STOPPED;
		//Application.LoadLevel("MainMenu");
	}

	// Runtime
	//-----

	//-----
	// Cleanup

	private void CleanUp()
	{
		// PlayerChar

		//if(playerChar!=null)
		//MonoBehaviour.Destroy(playerChar);

		// Balls
		if(balls != null)
		{
			for(int i = 0; i < balls.Count; i++)
			{
				if(balls[i] != null)
				{
					MonoBehaviour.Destroy( balls[i] );
				}
			}
		}

		if(ballScripts != null)
		{
			ballScripts = null;
		}

		// Bricks

	}

	void OnDisable()
	{
		RemoveEventListeners();
	}

	// Cleanup
	//-----
}
