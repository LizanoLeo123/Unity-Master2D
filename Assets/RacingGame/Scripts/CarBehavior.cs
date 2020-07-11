using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : MonoBehaviour
{
    private CarController _carController;
    private RoadMovement _roadMovement;
    public ParticleSystem _grassParticles;

    // Start is called before the first frame update
    void Start()
    {
        _carController = GameObject.Find("CarController").GetComponent<CarController>();
        _roadMovement = GameObject.Find("RoadManager").GetComponent<RoadMovement>();
        _grassParticles = _carController.grassParticles.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Road")
        {
            Debug.Log("Enter: " + collision.tag);
            //Logica para alentar la velocidad y activar las particulas
            _roadMovement.speed = _roadMovement.speed / 2;
            _carController.speed = _carController.speedGrass;
            //_carController.grassParticles.Play();
            //if (_grassParticles.isStopped)
                _grassParticles.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Road")
        {
            Debug.Log("Exit: " + collision.tag);
            //Logica para acelerar la velocidad y desactivar las particulas
            _roadMovement.speed = _roadMovement.speed * 2;
            _carController.speed = _carController.speedGrass * 2; //Normal Speed
            //_carController.grassParticles.Stop();
            //if (!_grassParticles.isStopped)
                _grassParticles.Stop();
        }
    }
}
