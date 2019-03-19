using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownController : MonoBehaviour
{
    PlayerController pCon;
    Vector3 minStageDimensions, maxStageDimensions;
    Rigidbody2D rb;
    [SerializeField]
    int damage = 1, scoreIncr = 10;
    void Start()
    {
        minStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        minStageDimensions.x += GetComponent<SpriteRenderer>().sprite.bounds.extents.x * transform.localScale.x;
        maxStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        maxStageDimensions.x -= GetComponent<SpriteRenderer>().sprite.bounds.extents.x * transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        pCon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (transform.position.x < minStageDimensions.x || transform.position.x > maxStageDimensions.x)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minStageDimensions.x, maxStageDimensions.x), transform.position.y, 0f);
            rb.velocity = new Vector3(rb.velocity.x * -1, rb.velocity.y, 0f);
        }
        if (transform.position.y < minStageDimensions.y)
        {
            pCon.ChangeHp(-damage);
            Destroy(gameObject);
        }
    }

    public int GetScoreIncr() { return scoreIncr; }
}
