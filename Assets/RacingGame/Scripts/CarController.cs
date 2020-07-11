using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float rotationAngle;
    public float speed;
    public float speedGrass;

    public GameObject grassParticles;

    private GameObject _car;

    // Start is called before the first frame update
    void Start()
    {
        _car = GameObject.FindObjectOfType<Car>().gameObject;
        speedGrass = speed / 2;
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxis("Horizontal");
        transform.Translate(new Vector2(direction,0) * speed * Time.deltaTime);
        float zRotation = 0;
        zRotation = Input.GetAxis("Horizontal") * -rotationAngle;

        _car.transform.rotation = Quaternion.Euler(0,0,zRotation);
    }
}
