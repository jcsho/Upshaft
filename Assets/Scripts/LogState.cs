using System.Collections;
using UnityEngine;

public class LogState : MonoBehaviour
{
  void Update()
  {
    Debug.Log("Current Game Mode: " + GameState.GameMode);
  }
}