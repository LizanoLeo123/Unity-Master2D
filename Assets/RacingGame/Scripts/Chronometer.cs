using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Chronometer : MonoBehaviour
{
    public Text txt_time;
    public Text txt_distance;
    public Text txt_FinalDistance;

    private RoadMovement _roadMovement;

    public float myTime;
    public float myDistance;

    // Start is called before the first frame update
    void Start()
    {
        _roadMovement = GameObject.Find("RoadManager").GetComponent<RoadMovement>();

        txt_time.text = "1:40";
        txt_distance.text = "0";

        myTime = 100f;
        myDistance = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_roadMovement.gameStart && !_roadMovement.gameFinished) //game started but not finished
        {
            calculateTimeDistance();
        }
        if (myTime <= 0 && _roadMovement.gameFinished == false)
        {
            txt_time.text = "0:00";
            _roadMovement.gameFinished = true;
            _roadMovement.gameStart = false;
            _roadMovement.finishGame();
            txt_FinalDistance.text = ((int)myDistance).ToString() + " m";
        }
    }

    void calculateTimeDistance()
    {
        myDistance += Time.deltaTime * _roadMovement.speed;
        txt_distance.text = ((int)myDistance).ToString();

        myTime -= Time.deltaTime;
        int minutes = (int)myTime / 60;
        int seconds = (int)myTime % 60;
        txt_time.text = minutes.ToString() + ":" + seconds.ToString().PadLeft(2, '0');
    }
}
