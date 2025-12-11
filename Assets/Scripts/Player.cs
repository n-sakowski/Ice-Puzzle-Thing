using UnityEngine;


public class GridPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;          
    public LayerMask wallLayer;           

    public int moveCount = 0;

    private Vector2 moveDirection;
    private bool isSliding = false;

    void Update()
    {
        if (!isSliding)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                TryStartSlide(Vector2.up);

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                TryStartSlide(Vector2.down);

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                TryStartSlide(Vector2.left);

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                TryStartSlide(Vector2.right);
        }
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            Vector2 newPos = (Vector2)transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            transform.position = newPos;
            
            if (IsWallInDirection(moveDirection))
            {
                isSliding = false;
                
                transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
            }
        }
    }

    void TryStartSlide(Vector2 direction)
    {
        if (!IsWallInDirection(direction))
        {
            moveDirection = direction;
            isSliding = true;
            moveCount++;
            Debug.Log(moveCount);
        }
    }

    bool IsWallInDirection(Vector2 direction)
    {
        float distance = 1f; 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, wallLayer);
        return hit.collider != null;
    }

    /*
    public void ResetMoveCount()
    {
        moveCount = 0;
    }
    */

}
