using UnityEngine;
using System.Collections;

public class BackGroundCollaidControler : MonoBehaviour
{
    private int numberOfBackground;
    private float distanceBetweenBackgrounds;

    private int numberOfGround;
    private float distanceBetweenGrounds;

    private int numberOfPipes;
    private float distanceBetweenPipes;


    public void Start()
    {
        var backGrounds = GameObject.FindGameObjectsWithTag("BackGround");
        var grounds = GameObject.FindGameObjectsWithTag("Ground");
        var pipes = GameObject.FindGameObjectsWithTag("Pipe");

        RandomaizPipes(pipes);

        this.numberOfBackground = backGrounds.Length;
        this.numberOfGround = grounds.Length;
        this.numberOfPipes = pipes.Length;

        this.distanceBetweenBackgrounds = Mathf.Abs(backGrounds[2].transform.position.x - backGrounds[1].transform.position.x);
        this.distanceBetweenGrounds = Mathf.Abs(grounds[2].transform.position.x - grounds[1].transform.position.x);
        this.distanceBetweenPipes = Mathf.Abs(pipes[3].transform.position.x - pipes[1].transform.position.x);// DistanceBetweenObject(pipes[0], pipes[1]);// Mathf.Abs(pipes[0].transform.position.x - pipes[1].transform.position.x / 2);
    }

    private void RandomaizPipes(GameObject[] pipes)
    {
        int count = 0;
        for (int i = 1; i < pipes.Length; i++)
        {
            count++;
            var curentPipe = pipes[i];
            float randomY;
            if (count % 2 == 0)
            {
                randomY = Random.Range(1.5f, 3);
            }
            else
            {
                randomY = Random.Range(-1, 0.5f);

            }

            var pipePosition = curentPipe.transform.position;
            pipePosition.y = randomY;
            curentPipe.transform.position = pipePosition;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("BackGround"))
        {
            Debug.Log(distanceBetweenBackgrounds);
        }


        if (collider.CompareTag("BackGround") || collider.CompareTag("Ground") || collider.CompareTag("Pipe"))
        {
            var go = collider.gameObject;
            var originalPosition = go.transform.position;
            if (collider.CompareTag("BackGround"))
            {
                originalPosition.x += this.numberOfBackground * this.distanceBetweenBackgrounds;
            }
            else if (collider.CompareTag("Ground"))
            {
                originalPosition.x += this.numberOfGround * this.distanceBetweenGrounds;
            }
            else
            {
                originalPosition.x += this.numberOfPipes * this.distanceBetweenPipes;

            }
            go.transform.position = originalPosition;
        }


    }
    private float DistanceBetweenObject(GameObject[] gameObject)
    {
        var minDistance = float.MinValue;

        for (int i = 2; i < gameObject.Length; i++)
        {
            var currentDistance = Mathf.Abs(gameObject[i - 1].transform.position.x - gameObject[i].transform.position.x);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
            }
        }
        return minDistance;
    }
}
