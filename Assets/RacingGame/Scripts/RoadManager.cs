using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public float speed;
    public bool gameStart;
    public bool gameFinished;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void startGame()
    {
        MoveRoad();
    }

    void MoveRoad()
    {
        speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
