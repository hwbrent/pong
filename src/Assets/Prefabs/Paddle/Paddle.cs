using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public KeyCode upKey;
    public KeyCode downKey;

    private BoxCollider2D box;

    // Used when moving the paddle up/down
    private float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        box = this.GetComponent<BoxCollider2D>();
        movementFactor = 0.0625f;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.KeysAreDown()) this.ProcessKeyPress();
    }

    /// <summary>
    /// Indicates whether exactly one of the up/down keys are currently pressed.
    /// </summary>
    bool KeysAreDown() {
        return Input.GetKey(this.upKey) ^ Input.GetKey(this.downKey);
    }

    /// <summary>
    /// Modifies `this.transform` in order to move the paddle up/down.
    /// </summary>
    void ProcessKeyPress() {
        Vector3 maxPosInScreen = Camera.main.WorldToScreenPoint(box.bounds.max);
        Vector3 minPosInScreen = Camera.main.WorldToScreenPoint(box.bounds.min);

        // If the key for moving the paddle up is pressed, and the top of
        // the paddle is below the top of the screen.
        if (Input.GetKey(this.upKey) && maxPosInScreen.y < Camera.main.pixelHeight)
        {
            this.transform.Translate(Vector3.up * this.movementFactor);
        }

        // If the key for moving the paddle down is pressed, and the top of
        // the paddle is above the bottom of the screen.
        else if (Input.GetKey(this.downKey) && minPosInScreen.y > 0)
        {
            this.transform.Translate(Vector3.down * this.movementFactor);
        }
    }
}
