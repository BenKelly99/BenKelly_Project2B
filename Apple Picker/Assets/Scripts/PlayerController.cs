using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float startSpeed = 5.0f;
    float currSpeed;
    [SerializeField]
    int maxHp = 3;
    int hp;
    int score;
    [SerializeField]
    GameFlowController gfc;
    Vector3 minStageDimensions, maxStageDimensions;
    [SerializeField]
    Text scoreText, livesText;
    [SerializeField]
    AudioClip collectClip, missClip;
    [SerializeField]
    AudioSource sfxSource;


    void Start()
    {
        minStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        minStageDimensions.x += GetComponent<SpriteRenderer>().sprite.bounds.extents.x * transform.localScale.x;
        maxStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        maxStageDimensions.x -= GetComponent<SpriteRenderer>().sprite.bounds.extents.x * transform.localScale.x;
        Reset();
    }

    public void Reset()
    {
        currSpeed = startSpeed;
        hp = maxHp;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score + "/" + gfc.GetScoreThresh() + " "; 
        livesText.text = " Lives: " + hp;
        if (!gfc.GetRunning())
            return;
        transform.Translate(Input.GetAxisRaw("Horizontal") * currSpeed * Time.deltaTime * Vector3.right);
        if (transform.position.x < minStageDimensions.x || transform.position.x > maxStageDimensions.x)
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minStageDimensions.x, maxStageDimensions.x), transform.position.y, 0f);
    }

    public void ChangeHp(int deltaHp) {
        hp += deltaHp;
        if (hp <= 0)
            gfc.NewGame();
        hp = Mathf.Min(maxHp, hp);
        if(deltaHp < 0)
            sfxSource.PlayOneShot(missClip);
    }

    public void ChangeScore(int deltaScore) {
        score += deltaScore;
        if (score >= gfc.GetScoreThresh())
        {
            gfc.ResetStage();
            ChangeHp(1);
        }
    }

    public void ChangeSpeed(float deltaSpeed) { currSpeed += deltaSpeed; }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Apples"))
        {
            ChangeScore(collision.gameObject.GetComponent<ThrownController>().GetScoreIncr());
            sfxSource.PlayOneShot(collectClip);
            Destroy(collision.gameObject);
        }
    }
}
