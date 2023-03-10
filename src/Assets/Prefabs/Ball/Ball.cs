using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // (The below components allow for collision detection with the paddles).
    private CircleCollider2D circleCollider;
    private Rigidbody2D rigidBody;

    // The rates of movement of the ball in the x and y directions.
    private Vector2 velocity;

    // The overall rate of movement of the ball.
    private float magnitude = 0.0625f;

    // Start is called before the first frame update.
    void Start()
    {
        this.circleCollider = this.GetComponent<CircleCollider2D>();
        this.rigidBody = this.GetComponent<Rigidbody2D>();

        this.velocity = this.GenerateRandomVelocity();
    }

    // Update is called once per frame.
    void Update()
    {
        this.Move();
    }

    /// <summary>
    /// Randomly generates the initial 'x' and 'y' components of 'this.velocity'.
    /// </summary>
    private Vector2 GenerateRandomVelocity()
    {
        float y = UnityEngine.Random.Range(-0.03125f, 0.03125f);
        float x = Mathf.Sqrt(
            Mathf.Pow(this.magnitude, 2) - Mathf.Pow(y, 2)
        );

        // Randomly flip the x direction.
        // 'https://www.loekvandenouweland.com/content/random-boolean-in-csharp.html'
        System.Random random = new System.Random();
        bool randomBool = random.Next(2) == 1; // 0 = false, 1 = true;
        if (randomBool)
        {
            x *= -1;
        }

        return new Vector2(x,y);
    }

    /// <summary>
    /// Moves the ball in the x and y direction every frame.
    /// </summary>
    private void Move()
    {
        // If the ball is exiting the screen view, we need to bounce
        // it back down.
        if (!this.IsInBounds())
        {
            this.BounceOffScreenBoundary();
        }
        this.rigidBody.position += this.velocity;
    }

    /// <summary>
    /// Indicates whether the ball is fully inside the boundaries of the screen.
    /// </summary>
    public bool IsInBounds()
    {
        Vector3 maxPos = Camera.main.WorldToScreenPoint(this.circleCollider.bounds.max);
        Vector3 minPos = Camera.main.WorldToScreenPoint(this.circleCollider.bounds.min);
        return 0 <= minPos.y && maxPos.y <= Camera.main.pixelHeight;
    }

    /// <summary>
    /// Bounces the ball off the top/bottom boundaries of the screen.
    /// </summary>
    private void BounceOffScreenBoundary()
    {
        this.velocity.y *= -1;
    }

    /// <summary>
    /// Bounces the ball off the paddle.
    /// </summary>
    private void BounceOffPaddle(GameObject paddle)
    {
        /*
        - Basically we want the overall velocity of the ball to be the same (this.velocity).
        - The difference in the y values of the ball and paddle at the point of contact dictates
          the y component of the new velocity.
        - And we can work out the new x velocity based off these facts.
        */
        Vector2 paddleCentre = paddle.GetComponent<BoxCollider2D>().bounds.center; // this is a Vector3 but you can convert it implicitly
        Vector2 circleCentre = this.circleCollider.bounds.center;

        float diffX = circleCentre.x - paddleCentre.x;
        float diffY = circleCentre.y - paddleCentre.y;
        float hypotenuse = Mathf.Sqrt(
            Mathf.Pow(diffX,2) + Mathf.Pow(diffY,2)
        );

        // The number we divide by to scale everything down to the scale of 'this.velocity'.
        float scale = (1/0.0625f) * hypotenuse;

        float adjustedDiffY = diffY / scale;
        float newXMovement = Mathf.Sqrt(
            Mathf.Pow(this.magnitude,2) - Mathf.Pow(adjustedDiffY,2)
        );

        // Add correct polarity depending on whether the ball should be going left or right.
        if (this.velocity.x > 0)
        {
            newXMovement *= -1;
        }

        this.velocity.x = newXMovement;
        this.velocity.y = adjustedDiffY;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject paddle = collision.gameObject;
        this.BounceOffPaddle(paddle);
    }
}
