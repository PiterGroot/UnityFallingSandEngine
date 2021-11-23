using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
     private Vector3 movement;
     private Rigidbody2D rb;
     [SerializeField]private float accelerationSpeed = 1.5f;
     [SerializeField]private float moveSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Mathf.Lerp(movement.x, Input.GetAxisRaw("Horizontal"), Time.deltaTime * accelerationSpeed);
        movement.y = Mathf.Lerp(movement.y, Input.GetAxisRaw("Vertical"), Time.deltaTime * accelerationSpeed);
    }
    private void FixedUpdate()
    {
        rb.MovePosition(Camera.main.transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
