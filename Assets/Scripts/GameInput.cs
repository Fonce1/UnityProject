using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }
    private PlayerInputAction PlayerInputAction;

    private void Awake()
    {
        Instance = this;
        PlayerInputAction = new PlayerInputAction();
        PlayerInputAction.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = PlayerInputAction.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

}
