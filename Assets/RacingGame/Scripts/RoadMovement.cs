using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    GameObject RoadContainer;
    public GameObject[] roadsContainerArray;

    public float speed;
    public bool gameStart;
    public bool gameFinished;

    private int _roadCounter = 0;
    private int _selectedRoad;

    private GameObject _lastRoad;
    private GameObject _newRoad;
    private float _roadHeight = 0;

    private Vector3 _ScreenLimit;
    private bool _outOfScreen;

    private GameObject _car;
    private AudioFX _audioFX;
    public GameObject FinishBG;


    // Start is called before the first frame update
    void Start()
    {
        startGame();
    }

    void startGame()
    {
        RoadContainer = GameObject.Find("RoadContainer");
        _car = GameObject.FindObjectOfType<Car>().gameObject;
        _audioFX = GameObject.Find("SoundFX").GetComponent<AudioFX>();
        FinishBG.SetActive(false);

        findRoads();
        placeRoad();
        MoveRoad();
        meazureScreen();
    }

    public void finishGame()
    {
        _car.GetComponent<AudioSource>().Stop();
        _audioFX.playGameMusic();
        FinishBG.SetActive(true);
    }

    void MoveRoad()
    {
        speed = 18f;
    }

    void findRoads()
    {
        roadsContainerArray = GameObject.FindGameObjectsWithTag("Road");
        for (int i = 0; i < roadsContainerArray.Length; i++)
        {
            roadsContainerArray[i].gameObject.transform.parent = RoadContainer.transform;
            roadsContainerArray[i].gameObject.SetActive(false);
            roadsContainerArray[i].gameObject.name = "RoadOFF_" + i.ToString();
        }
    }

    void placeRoad()
    {
        _roadCounter++;
        _selectedRoad = Random.Range(0, roadsContainerArray.Length);
        GameObject road = Instantiate(roadsContainerArray[_selectedRoad]);
        road.SetActive(true);
        road.name = "Road_" + _roadCounter.ToString();
        road.transform.parent = gameObject.transform;

        _lastRoad = GameObject.Find("Road_" + (_roadCounter-1).ToString());
        _newRoad = GameObject.Find("Road_" + _roadCounter.ToString());
        meazureRoad();
        _newRoad.transform.position = new Vector3(_lastRoad.transform.position.x,
            _lastRoad.transform.position.y + _roadHeight - 0.02f, 0);
    }

    void meazureRoad()
    {
        for (int i = 0; i < _lastRoad.transform.childCount; i++)
        {
            if(_lastRoad.transform.GetChild(i).gameObject.GetComponent<RoadPiece>() != null)
            {
                float pieceHeight = _lastRoad.transform.GetChild(i).gameObject
                .GetComponent<SpriteRenderer>().bounds.size.y;
                _roadHeight += pieceHeight;
            }
            
        }
    }

    void meazureScreen()
    {
        _ScreenLimit = new Vector3(0, Camera.main.ScreenToWorldPoint(Vector3.zero).y - 0.5f, 0);
    }

    void destroyLastRoad()
    {
        Destroy(_lastRoad);
        _roadHeight = 0;
        _lastRoad = null;
        placeRoad();
        _outOfScreen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStart == true && gameFinished == false)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (_lastRoad.transform.position.y + _roadHeight < _ScreenLimit.y && _outOfScreen == false)
            {
                _outOfScreen = true;
                destroyLastRoad();
            }
        }
    } 
}
