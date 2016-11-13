using UnityEngine;
using System.Collections;

public class PlayerModel
{
	private float speed = 6;
	private Quaternion initRotation = new Quaternion( 0, 180, -90, 0);
	private float width;
	private float nextFire;
	private bool _enabled = false;
	private Rigidbody playerCharRigidBody;
	private Vector3 initPos = new Vector3( 0, 2, 10);

	public float tilt;
	public float xMin = -5, xMax = 5;
	public GameObject playerChar;
	public float moveHorizontalLimited;
	private bool isAccelerometerSupported = false;
	private bool isGyroSupported = false;

	public PlayerModel()
	{
		Init();
	}

	private void Init()
	{
		playerChar = (MonoBehaviour.Instantiate(PrefabsMap.playerCharPrefab) as GameObject);

		playerChar.name = "playerChar";
		playerChar.transform.position = initPos;
		width = playerChar.GetComponent<Collider>().bounds.size.x;
		playerCharRigidBody = playerChar.GetComponent<Rigidbody>();
		playerCharRigidBody.rotation = Quaternion.Euler
			(
				initRotation.x,
				initRotation.y,
				initRotation.z
				//+ playerCharRigidBody.velocity.x * tilt
			);

		Enabled = true;
		isAccelerometerSupported = SystemInfo.supportsAccelerometer;
		isGyroSupported = SystemInfo.supportsGyroscope;
	}

	private void Fire()
	{
		if (Input.GetButton("Fire1")) 
		{
			GameEvent.PlayerFireOne();

			//nextFire = Time.time + fireRate;

			//Instantiate(shot,shotSpawn.position,shotSpawn.rotation);
			//audio.Play();
		}
	}

	public void FixedUpdate()
	{
		if(playerChar==null)
		{
			return;
		}

		Move();
	}

	public void Update()
	{
		if(playerChar == null)
		{
			return;
		}

		Fire();
	}

	private void Move()
	{
		float moveHorizontal;

		if(isGyroSupported)
		{
			moveHorizontal = moveHorizontalLimited = Input.gyro.userAcceleration.x;
		}
		else
		if(isAccelerometerSupported)
		{
			moveHorizontal = moveHorizontalLimited = Input.acceleration.x;
		}
		else
		{
			moveHorizontal = moveHorizontalLimited = Input.GetAxis("Horizontal");
		}

		Vector3 movement = new Vector3( moveHorizontal, 0, 0);
		playerCharRigidBody.velocity = movement*speed;

		// Limit
		float targetX = playerCharRigidBody.position.x;
		if(playerCharRigidBody.position.x > xMax - width / 2)
		{
			targetX = xMax - width / 2;
			moveHorizontalLimited = 0;
		}
		if(playerCharRigidBody.position.x < xMin + width / 2)
		{
			targetX = xMin + width / 2;
			moveHorizontalLimited = 0;
		}

		playerCharRigidBody.position = new Vector3
			(
				targetX,
				initPos.y,
				initPos.z
				);

		playerCharRigidBody.rotation = Quaternion.Euler
			(
				initRotation.x,
				initRotation.y + playerCharRigidBody.velocity.x * tilt,
				initRotation.z
			);

	}

	public bool Enabled
	{
		get{
			return _enabled;
		}
		set{
			if(_enabled != value)
			{
				_enabled = value;

				if(playerChar != null)
				{
					playerChar.SetActive(value);
				}
			}
			else
			{
				Debug.Log("PlayerController set Enabled error: already enabled/disabled.");
			}
		}
	}

	void onUpdateEnabled(bool value)
	{
		playerChar.SetActive(value);
	}

	public void Enable()
	{
		Enabled = true;
	}

	public void Disable()
	{
		Enabled = false;
	}

	public void Destroy()
	{
		//MonoBehaviour.Destroy(initRotation);
		MonoBehaviour.Destroy(playerCharRigidBody);
		//MonoBehaviour.Destroy(initPos);
		MonoBehaviour.Destroy(playerChar);
	}
}