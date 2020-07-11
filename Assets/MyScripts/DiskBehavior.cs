using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskBehavior : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;

    public Vector2 minPower;
    public Vector2 maxPower;

    [SerializeField]
    private CircleCollider2D solidCollider;

    public GameObject player;

    private Vector2 _force;
    private Vector3 _startPoint;
    private Vector3 _endPoint;

    private TrayectoryLine _tl;

    private bool _followPlayer;

    private int _bounces;

    private void Start()
    {
        _followPlayer = false;
        _bounces = 0;
        _tl = GetComponent<TrayectoryLine>();
    }

    private void Update()
    {
        //Follow Player logic
        if (_followPlayer)
        {
            transform.position = player.transform.position + new Vector3(0.5f, 0.5f, 0);
        }
        
        //Prevent Disk from going out of bounds
        //if (transform.position.y > 7 || transform.position.y < -7 || transform.position.x < -10 || transform.position.x > 10)
        //    transform.position = Vector3.zero;

        //Shooting logic
        if(_bounces == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _startPoint.z = 15;
                //Debug.Log("Inicio" + _startPoint.ToString());
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 currentPoint = currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentPoint.z = 15;
                //currentPoint.x *= -1;
                //currentPoint.y *= -1;
                _tl.RenderLine(_startPoint, currentPoint); //transform + startpoint - currentpoint
            }

            if (Input.GetMouseButtonUp(0))
            {
                //Vector3 startPoint = transform.position;
                _followPlayer = false;
                _endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _endPoint.z = 15;
                //Debug.Log("Fin" + _endPoint.ToString());

                float directionX = _startPoint.x - _endPoint.x;
                float directionY = _startPoint.y - _endPoint.y;
                _force = new Vector2(Mathf.Clamp(directionX, minPower.x, maxPower.x) * -1, Mathf.Clamp(directionY, minPower.y, maxPower.y) * -1);
                rb.AddForce(_force * speed, ForceMode2D.Impulse);
                solidCollider.enabled = true;
                _tl.EndLine();
            }
        }        
    }

    //Collision logic
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Golpe enemigo");
            rb.velocity = rb.velocity * new Vector3(-1, 1, 1);
            return;
        }
            
        if(collision.tag == "Player")
        {
            solidCollider.enabled = false;
            //_followPlayer = true;
            //Stop disk
            rb.velocity = Vector3.zero;
            //Can shoot again
            _bounces = 0;
            return;
        }
        _bounces++;
        if (_bounces >= 5)
        {
            //Stop disk
            rb.velocity = Vector3.zero;

            //Return disk to player
            _endPoint = GameObject.Find("Player").transform.position;
            float directionX = transform.position.x - _endPoint.x;
            float directionY = transform.position.y - _endPoint.y;
            _force = new Vector2(Mathf.Clamp(directionX, minPower.x, maxPower.x) * -1, Mathf.Clamp(directionY, minPower.y, maxPower.y) * -1);
            rb.AddForce(_force * (speed/3), ForceMode2D.Impulse);
            //Can shoot again
            //_bounces = 0;
        }
    }
}
