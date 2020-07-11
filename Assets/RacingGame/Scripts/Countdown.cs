using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public Sprite[] numbers;
    
    private RoadMovement _roadMovement;

    public GameObject numberCounter;
    private SpriteRenderer _numberCounterComp;

    public GameObject carController;
    public GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        initializeComponents();
        startCountdown();
    }

    void initializeComponents()
    {
        _roadMovement = GameObject.Find("RoadManager").GetComponent<RoadMovement>();
        _numberCounterComp = numberCounter.GetComponent<SpriteRenderer>();
    }

    void startCountdown()
    {
        StartCoroutine(counting());
    }

    IEnumerator counting()
    {
        carController.GetComponent<AudioSource>().Play();
        this.gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2);

        _numberCounterComp.sprite = numbers[1];
        this.gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);

        _numberCounterComp.sprite = numbers[2];
        this.gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);

        _numberCounterComp.sprite = numbers[3];
        _roadMovement.gameStart = true;
        numberCounter.GetComponent<AudioSource>().Play();
        car.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2);

        numberCounter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
