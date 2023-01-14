using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // The below components allow for collision detection with the paddles.
    private CircleCollider2D circleCollider;
    private Rigidbody2D rigidBody;

    private Vector2 movementFactor;

    // Start is called before the first frame update.
    void Start()
    {
        this.circleCollider = this.GetComponent<CircleCollider2D>();
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        // this.movementFactor = new Vector2(0.0625f, 0.03125f);
        this.movementFactor = new Vector2(0.0625f, 0f); 
    }

    // Update is called once per frame.
    void Update()
    {
        this.Move();
    }

    /// <summary>
    /// Moves the ball in the x and y direction every frame.
    /// </summary>
    void Move()
    {
        // If the ball is exiting the screen view, we need to bounce
        // it back down.
        if (!this.IsInBounds())
        {
            this.BounceOffScreenBoundary();
        }
        this.rigidBody.position += this.movementFactor;
    }

    /// <summary>
    /// Bounces the ball off the top/bottom boundaries of the screen.
    /// </summary>
    void BounceOffScreenBoundary()
    {
        this.movementFactor.y *= -1;
    }

    /// <summary>
    /// Indicates whether the ball is fully inside the boundaries of the screen.
    /// </summary>
    bool IsInBounds()
    {
        Vector3 maxPos = Camera.main.WorldToScreenPoint(this.circleCollider.bounds.max);
        Vector3 minPos = Camera.main.WorldToScreenPoint(this.circleCollider.bounds.min);
        return 0 <= minPos.y && maxPos.y <= Camera.main.pixelHeight;
    }

    /// <summary>
    /// Bounces the ball off the paddle.
    /// </summary>
    void BounceOffPaddle()
    {
        this.movementFactor.x *= -1;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        this.BounceOffPaddle();
    }
}
