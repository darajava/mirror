using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundGenerator : MonoBehaviour {

	public GameObject[] sprites;
	public List<Vector3> createdSprites;
	public int spriteAmount;



	// Use this for initialization
	void Start () {
		StartCoroutine(GenerateRound());
	}
	
	IEnumerator GenerateRound() {
		Random rnd = new Random();
        RandomiseArray(sprites);

        for (int i = 0; i < spriteAmount; i++) {
        	createSprite(sprites[i]);
        }
		yield return 0;
	}

	void RandomiseArray(GameObject[] arr) {
	    for (var i = arr.Length - 1; i > 0; i--) {
	        var r = Random.Range(0, i);
	        var tmp = arr[i];
	        arr[i] = arr[r];
	        arr[r] = tmp;
	    }
	}

	void createSprite(GameObject sprite) {
		GameObject spriteObject;
		

		float RandX = Random.Range(-5f, 5f);
		float RandY = Random.Range(-5f, 5f);

		transform.position = new Vector3(transform.position.x + RandX, transform.position.y + RandY, 0);
		spriteObject = Instantiate(sprite, transform.position, transform.rotation) as GameObject;

		createdSprites.Add(spriteObject.transform.position);
	}
}
