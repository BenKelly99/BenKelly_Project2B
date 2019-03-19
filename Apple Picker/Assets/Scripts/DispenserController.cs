using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserController : MonoBehaviour
{
    [SerializeField]
    float changeChance = 0.025f, minSpeed = 2.0f, maxSpeed = 6.0f;
    [SerializeField]
    float throwInterval = 2.5f, throwVelocity = 0.5f;
    bool isInvoking;
    float lastThrownTime;
    float speed;
    int sign;
    [SerializeField]
    GameObject thrown;
    [SerializeField]
    Transform throwableParent;
    [SerializeField]
    GameFlowController gfc;
    Vector3 minStageDimensions, maxStageDimensions;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
        minStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        minStageDimensions.x += GetComponent<SpriteRenderer>().sprite.bounds.extents.x * transform.localScale.x;
        maxStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        maxStageDimensions.x -= GetComponent<SpriteRenderer>().sprite.bounds.extents.x * transform.localScale.x;
        isInvoking = false;
    }


    public void Reset()
    {
        Change();
        ResetThrownTime();
    }

    public void ResetThrownTime()
    {
        lastThrownTime = Time.time - 2*throwInterval;
    }

    void Change()
    {
        sign = 2 * Random.Range(0, 2) - 1;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gfc.GetRunning())
        {
            isInvoking = false;
            CancelInvoke();
            return;
        }
        if (!isInvoking)
        {
            isInvoking = true;
            InvokeRepeating("SpawnThrowable", 0, throwInterval);
        }

        transform.Translate(sign * speed * Time.deltaTime * Vector3.right);
        if(transform.position.x < minStageDimensions.x || transform.position.x > maxStageDimensions.x)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minStageDimensions.x, maxStageDimensions.x), transform.position.y, 0f);
            sign *= -1;
        }
        
    }

    void SpawnThrowable()
    {
        GameObject newThrown = Instantiate(thrown,throwableParent);
        newThrown.transform.position = transform.position;
        newThrown.GetComponent<Rigidbody2D>().velocity = Vector3.up * throwVelocity + sign * speed * Vector3.right;
        lastThrownTime = Time.time;
    }

    void FixedUpdate()
    {
        if (!gfc.GetRunning())
            return;

        if (Random.value < changeChance)
        {
            Change();
        }
    }
}
