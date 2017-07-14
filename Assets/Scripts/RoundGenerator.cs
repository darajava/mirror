using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundGenerator : MonoBehaviour {

    public GameObject[] sprites;
    public GameObject[] spawnsP1;
    public GameObject[] spawnsP2;
    public List<GameObject> createdSpritesObj;
    public List<Vector3> createdSprites;
    public int spriteAmount;
    public int maxSky;
    public int maxLand;

    public int p1Score = 0;
    public int p2Score = 0;

    public string correct = "";

    public bool[] seenSpawnsP1;
    public bool[] seenSpawnsP2;

    // Use this for initialization
    void Start () {
        StartCoroutine(GenerateRound());
    }
    
    // Don't use this for initialization you fucking idiot
    void Update() {
        if (Input.GetMouseButtonDown (0)) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null) {
                if (hit.collider.name.Contains(correct)) {
                    if (hit.collider.gameObject.name.Contains("P1")) {
                        p1Score++;
                    } else if (hit.collider.gameObject.name.Contains("P2")) {
                        p2Score++;
                    }
                    GameObject.Find("Win").GetComponent<AudioSource>().Play();

                    StartCoroutine(GenerateRound());
                } else {
                    if (hit.collider.gameObject.name.Contains("P1")) {
                        p1Score--;
                    } else if (hit.collider.gameObject.name.Contains("P2")) {
                        p2Score--;
                    }
                    Debug.Log(hit.collider.gameObject.name.Contains("P1"));
                    Debug.Log(hit.collider.gameObject.name);
                }

                GameObject.Find("Score1").GetComponent<TextMesh>().text = "" + p1Score;
                GameObject.Find("Score2").GetComponent<TextMesh>().text = "" + p2Score;
            } else {
                Debug.Log("Touch enter on " );
            }
        }
    }

    IEnumerator GenerateRound() {

        foreach (GameObject obj in createdSpritesObj){
           GameObject.Destroy(obj);
        }

        RandomiseArray(spawnsP1);
        RandomiseArray(spawnsP2);

        seenSpawnsP1 = new bool[spawnsP1.Length];
        seenSpawnsP2 = new bool[spawnsP2.Length];
        for (int i=0; i<seenSpawnsP1.Length; i++) { seenSpawnsP1[i] = false; }
        for (int i=0; i<seenSpawnsP2.Length; i++) { seenSpawnsP2[i] = false; }

        int skySprites = 0;
        int landSprites = 0;

        int loopcount = 0;
        do {
            loopcount++;
            if (loopcount > 1000) {
                Debug.Log("infiinite");
                break;
            }
            RandomiseArray(sprites);
            
            // Check that we don't have more sprites than we can fit
            skySprites = 0;
            landSprites = 0;
            for (int i = 0; i < spriteAmount; i++) {
                if (sprites[i].name.Contains("Sky")) {
                    skySprites++;
                } else {
                    landSprites++;
                }
            }

            if (skySprites > maxSky || landSprites > maxLand) {
                continue;
            }

            skySprites = 0;
            landSprites = 0;
            for (int i = spriteAmount - 1; i < spriteAmount * 2 - 1; i++) {
                if (sprites[i].name.Contains("Sky")) {
                    skySprites++;
                } else {
                    landSprites++;
                }
            }
    
        } while(skySprites > maxSky || landSprites > maxLand);

        correct = sprites[spriteAmount - 1].name;

        for (int i = 0; i < spriteAmount; i++) {
            createSprite(sprites[i], spawnsP1, seenSpawnsP1, 1);
        }

        for (int i = spriteAmount - 1; i < spriteAmount * 2 - 1; i++) {
            createSprite(sprites[i], spawnsP2, seenSpawnsP2, 2);
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

    void createSprite(GameObject sprite, GameObject[] spawns, bool[] seenSpawns, int player) {
        GameObject spriteObject = null;
       
        transform.position = new Vector3(0, 0, 0);

        for (int i = 0; i < spawns.Length; i++) {
            if (
                ((sprite.name.Contains("Sky") && spawns[i].name.Contains("Sky")) ||
                (!sprite.name.Contains("Sky") && !spawns[i].name.Contains("Sky"))) &&
                !seenSpawns[i]) {
                spriteObject = Instantiate(sprite, spawns[i].transform.position, spawns[i].transform.rotation) as GameObject;
                spriteObject.transform.localScale = new Vector3(
                        spriteObject.transform.localScale.x * spawns[i].transform.localScale.x,
                        spriteObject.transform.localScale.y * spawns[i].transform.localScale.y,
                        spriteObject.transform.localScale.z * spawns[i].transform.localScale.z
                );

                spriteObject.name = spriteObject.name + 'P' + player;

                seenSpawns[i] = true;
                break;
            }
        }

        createdSpritesObj.Add(spriteObject);

    }
}
