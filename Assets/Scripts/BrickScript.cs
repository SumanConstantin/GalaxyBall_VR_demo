using UnityEngine;
using System.Collections;

public class BrickScript: MonoBehaviour
{
	public int life = 3;
	public int score = 300;

	void Start()
	{
		this.name = "brick";
	}

	void OnTriggerEnter(Collider triggerCollider) 
	{
		// TODO: add explosion
		//Instantiate(explosion,transform.position,transform.rotation);

		if(triggerCollider.name == "ball")
		{
			Destroy(gameObject);
		}

		GameEvent.ScoreUpdated(score);
		GameEvent.BricksUpdated(-1);
	}

}
