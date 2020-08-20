using System;
using UnityEngine;

public class ButtonController : MonoBehaviour {
  private static bool[] buttonStates;

  private void Awake() {
    buttonStates = new bool[Enum.GetValues(typeof(ButtonType)).Length];
  }
  public void OnClickDown(int buttonType) {
    if (buttonType < buttonStates.Length) {
      buttonStates[buttonType] = true;
    }
  }
  public void OnClickUp(int buttonType) {
    if (buttonType < buttonStates.Length) {
      buttonStates[buttonType] = false;
    }
  }
  public static bool GetClick(ButtonType buttonType) {
    if ((int)buttonType < buttonStates.Length) {
      return buttonStates[(int)buttonType];
    } else {
      return false;
    }
  }
}