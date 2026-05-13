using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Neutral
    }

    [Header("Player Input")]
    [SerializeField] private PlayerInput playerInput = null;

    [Header("--- Player Settings ---")]
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody Rigidbody = null;
    [SerializeField] private float moveSpeed = 5.0f;

    #region イベント登録、解除
    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
    }
    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
    }
    #endregion

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Direction direction = GetDirection(input);
        if (direction == Direction.Neutral) return;

        // Directionに応じた移動処理をlinearVelocityで実装
        switch(direction)
        {
            case Direction.Up:
                Rigidbody.linearVelocity = Vector3.forward * moveSpeed;
                break;
            case Direction.Down:
                Rigidbody.linearVelocity = Vector3.back * moveSpeed;
                break;
            case Direction.Left:
                Rigidbody.linearVelocity = Vector3.left * moveSpeed;
                break;
            case Direction.Right:
                Rigidbody.linearVelocity = Vector3.right * moveSpeed;
                break;
        }

    }

    // 4方向を判別するメソッド
    private Direction GetDirection(Vector2 input)
    {
        if (input.x > 0.5f) return Direction.Right;
        if (input.x < -0.5f) return Direction.Left;
        return Direction.Neutral;
    }
}
