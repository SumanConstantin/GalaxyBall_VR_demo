using UnityEngine;
using System.Collections;
using System.Timers;

public class CountDownTimer {
	// Count Update Event
	public delegate void CountUpdateEventDelegate (int loops);
	public event CountUpdateEventDelegate OnUpdated;
	// Count Finish Event
	public delegate void CountFinishEventDelegate ();
	public event CountFinishEventDelegate OnFinished;

	private Timer timer;
	private float timeSinceWaveInit = 0f;

	private int loopsInit = 1;
	private int loops = 1;
	private int inverval = 1000;	// Milliseconds
	private bool destroyOnElapsed = false;
	private bool _timerElapsed = false;
	public bool timerElapsed
	{
		get {return _timerElapsed;}
		set {_timerElapsed = value;}
	}

	public CountDownTimer(int loops = 0, int inverval = 1000, bool destroyOnElapsed = false)
	{
		this.loopsInit = loops;
		this.inverval = inverval;
		this.destroyOnElapsed = destroyOnElapsed;
	}

	public void Init()
	{
		Reset();
	}

	public void Reset()
	{
		loops = loopsInit;

		if(timer == null)
		{
			timer = new System.Timers.Timer();
		}

		timer.Interval = inverval;

		timer.Elapsed -= OnCountdownTimerElapsed;
		timer.Elapsed += OnCountdownTimerElapsed;
	}

	public void Start()
	{
		//timeSinceWaveInit = Time.timeSinceLevelLoad;

		timer.Start();

		if(OnUpdated != null)
		{
			OnUpdated(loops);
		}
	}

	public void Stop()
	{
		timer.Stop();
	}

	public void Restart()
	{
		Reset();
		Start();
	}

	private void OnCountdownTimerElapsed(object sender, ElapsedEventArgs args)
	{
		// Do not call Update directly on timer event,
		// because update may call get/set_enabled, set_text and other functions
		// which can only be called from the main tread
		// Update should be called on Monobehaviour Update or FixedUpdate
		timerElapsed = true;
	}

	// This update has to be called on main thread (e.g. with Monobehaviour Update() or FixedUpdate())
	public void Update () {
		if(!timerElapsed)
		{
			return;
		}

		timerElapsed = false;
		loops--;

		if(loops > 0)
		{
			OnUpdated(loops);
		}
		else
		if(loops == 0)
		{
			Stop();

			if(OnFinished != null)
			{
				OnFinished();
			}
		}
	}

	public void Dispose()
	{
		timer.Dispose();
	}
}
