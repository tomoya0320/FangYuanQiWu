using UnityEngine;

public enum ButtonType {
  Jump,
  Dash,
  Attack,
}

public enum Direction {
  Horizontal,
  Vertical
}

public class InputManager {
  public static float GetAxisRaw(Direction direction) {
    switch (Application.platform) {
      case RuntimePlatform.Android:
      case RuntimePlatform.IPhonePlayer: {
          return MoveController.GetAxis(Mathf.Clamp((int)direction, 0, 1));
        }
      case RuntimePlatform.WindowsEditor:
      case RuntimePlatform.WindowsPlayer: {
          return Input.GetAxisRaw(direction.ToString());
        }
      default: {
          return 0;
        }
    }
  }
  public static float GetAxis(Direction direction) {
    switch (Application.platform) {
      case RuntimePlatform.Android:
      case RuntimePlatform.IPhonePlayer: {
          return MoveController.GetAxis(Mathf.Clamp((int)direction, 0, 1));
        }
      case RuntimePlatform.WindowsEditor:
      case RuntimePlatform.WindowsPlayer: {
          return Input.GetAxis(direction.ToString());
        }
      default: {
          return 0;
        }
    }
  }
  public static bool GetButton(ButtonType buttonType) {
    switch (Application.platform) {
      case RuntimePlatform.Android:
      case RuntimePlatform.IPhonePlayer: {
          return ButtonController.GetClick(buttonType);
        }
      case RuntimePlatform.WindowsEditor:
      case RuntimePlatform.WindowsPlayer: {
          return Input.GetButton(buttonType.ToString());
        }
      default: {
          return false;
        }
    }
  }
}