using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
	private const string MOVE_DIRECTION_UP = "moveDirectionUp";
	private const string MOVE_DIRECTION_DOWN = "moveDirectionDown";
	private const string MOVE_DIRECTION_LEFT = "moveDirectionLeft";
	private const string MOVE_DIRECTION_RIGHT = "moveDirectionRight";

	private const string MOVE_STATE_FLY = "moveStateFly";
	private const string MOVE_STATE_STUCK = "moveStateStuck";

	private float xMin = -5, xMax = 5, zMin = -5.5f, zMax = 11;
	private float width;
	private float height;
	private Rigidbody ballRigidbody;

	public float speed;
	public float deltaAngleRad;
	public Transform playerCharTransform;
	public Collider playerCollider;

	private string moveState = MOVE_STATE_STUCK;

	void Start()
	{
		this.name = "ball";

		ballRigidbody = GetComponent<Rigidbody>();

		Collider collider = this.GetComponent<Collider>();
		width = collider.bounds.size.x;
		height = collider.bounds.size.z;
	}

	public void InitPlayerChar(GameObject playerChar)
	{
		playerCharTransform = playerChar.transform;
		playerCollider = playerChar.GetComponent<Collider>();
	}

	void OnTriggerEnter(Collider collider) 
	{
		if(moveState != MOVE_STATE_FLY) 
		{
			return;
		}

		switch(collider.name)
		{
			case "playerChar":
				OnHitPlayerChar(collider);
				break;

			case "brick":		
				OnHitBrick(collider);
				break;
		}
	}

	public void OnHitPlayerChar(Collider collider)
	{
		float x = ballRigidbody.velocity.x;
		float y = ballRigidbody.velocity.y;
		float z = ballRigidbody.velocity.z;
		
		// Get position of ball relative to center of player
		float diff = this.transform.position.x - collider.transform.position.x;

		// Check diff against max/min values
		float diffMax = 1, diffMin = -1;
		diff = diff > diffMax ? diffMax : diff < diffMin ? diffMin : diff;
		
		float rad = deltaAngleRad * diff * Mathf.Deg2Rad;
		
		x = Mathf.Atan(rad) * speed;
		y = Mathf.Cos(rad) * speed;
		
		ballRigidbody.velocity = new Vector3(x, y, z);
	}

	public void OnHitBrick(Collider collider)
	{
		// Check side of colission with brick
		float deltaX = this.transform.position.x - collider.transform.position.x;
		float deltaY = this.transform.position.y - collider.transform.position.y;

		if(Mathf.Abs(deltaX) + speed / 30 < (width + collider.bounds.size.x) / 2)
		{
			// Hit from up-down
			if(deltaY > 0)
			{
				hitObject(MOVE_DIRECTION_UP);
			}
			else
			{
				hitObject(MOVE_DIRECTION_DOWN);
			}
		}
		else
		{
			// Hit from left-right
			if(deltaX > 0)
			{
				hitObject(MOVE_DIRECTION_LEFT);
			}
			else
			{
				hitObject(MOVE_DIRECTION_RIGHT);
			}
		}
	}

	void FixedUpdate()
	{
		Move();
		CheckBallLost();
	}

	public void Launch()
	{
		if(moveState != MOVE_STATE_STUCK) 
		{
			return;
		}

		moveState = MOVE_STATE_FLY;
		ballRigidbody.velocity = new Vector3( 0, -speed, 0);
		OnHitPlayerChar(playerCollider);
	}

	private void Move()
	{
		if(moveState == MOVE_STATE_FLY)
		{
			hitBorder();
		}
	}

	private void CheckBallLost()
	{
		if(moveState == MOVE_STATE_FLY && transform.position.y < -.8)
		{
			GameEvent.BallLost(gameObject);
		}
	}

	public void MoveWithChar()
	{
		if(moveState == MOVE_STATE_STUCK)
		{
 			Vector3 newPosition = new Vector3( 	playerCharTransform.position.x, 
												playerCharTransform.position.y + height,
												playerCharTransform.position.z);
			transform.position = newPosition;
		}
	}

	public void hitBorder()
	{
		if(ballRigidbody.position.y >= zMax)
		{
			hitObject(MOVE_DIRECTION_DOWN);
		}
		else
		if(ballRigidbody.position.y <= zMin)
		{
			hitObject(MOVE_DIRECTION_UP);
		}
		else
		if(ballRigidbody.position.x >= xMax)
		{
			hitObject(MOVE_DIRECTION_LEFT);
		}
		else
		if(ballRigidbody.position.x <= xMin)
		{
			hitObject(MOVE_DIRECTION_RIGHT);
		}
	}
	
	private void hitObject(string _direction)
	{
		float x = ballRigidbody.velocity.x;
		float y = ballRigidbody.velocity.y;
		float z = ballRigidbody.velocity.z;
		
		if(_direction == MOVE_DIRECTION_UP || _direction == MOVE_DIRECTION_DOWN)
		{
			y = -y;
		}
		else
		if(_direction == MOVE_DIRECTION_LEFT || _direction == MOVE_DIRECTION_RIGHT)
		{
			x = -x;
		}
		
		ballRigidbody.velocity = new Vector3( x, y, z);
	}

	void OnDisable()
	{

	}
}
