using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Vector3 destinationPoint;
    private Vector3 moveDirection;
    public string aimTrigger;
    public float arrowSpeed;
    public float arrowDamage;
    public bool collAny;
    // Start is called before the first frame update
    void Start()
    {

        //set target point
        moveDirection = destinationPoint - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // move arrow to target point
        transform.Translate(moveDirection * arrowSpeed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.tag == aimTrigger || other.transform.root.gameObject.tag == "Obstacle")
        {        
            Destroy(gameObject);
        }
    }
}
