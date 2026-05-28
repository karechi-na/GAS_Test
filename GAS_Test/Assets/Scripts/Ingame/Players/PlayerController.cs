using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの移動を制御するクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの移動方向を表す列挙型
    /// </summary>
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
        playerInput.actions["Move"].performed += OnMovePerformed;
        playerInput.actions["Move"].canceled += OnMoveCanceled;
    }
    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMovePerformed;
        playerInput.actions["Move"].canceled -= OnMoveCanceled;
    }
    #endregion

    /// <summary>
    /// Moveアクションがperformedされたときの処理
    /// </summary>
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // 入力されたベクトルから移動方向を判別
        Vector2 input = context.ReadValue<Vector2>();
        Direction direction = GetDirection(input);

        // Neutralの場合は移動しない
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
    /// <summary>
    /// Moveアクションがcanceledされたときの処理
    /// </summary>
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        Rigidbody.linearVelocity = Vector3.zero;
    }

    /// <summary>
    /// 4方向を判別するメソッド
    /// </summary>
    /// <param name="input">入力ベクトル</param>
    /// <returns>判別された方向</returns>
    private Direction GetDirection(Vector2 input)
    {
        if (input.x > 0.5f) return Direction.Right;
        if (input.x < -0.5f) return Direction.Left;
        return Direction.Neutral;
    }
}
