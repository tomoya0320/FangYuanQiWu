using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInit : MonoBehaviour {
  private GameObject player;
  public PlayerDead playerDead;
  public Image dashCoolImg;
  public RectTransform shortBuffInfoContent;
  public RectTransform detailBuffInfoContent;
  public CinemachineFreeLook freeLook;
  public static GameObject playerPrefab;

  private void Awake() {
    Init();
    playerDead.Init();
  }
  public void Restart() {
    playerDead.Hide();
    Destroy(player);
    Init();
  }
  private void Init() {
    player = Instantiate(playerPrefab);
    player.GetComponent<PlayerStateManager>().playerDead = playerDead;
    player.GetComponent<PlayerController>().dashCoolImg = dashCoolImg;
    player.GetComponent<BuffManager>().shortBuffInfoContent = shortBuffInfoContent;
    player.GetComponent<BuffManager>().detailBuffInfoContent = detailBuffInfoContent;
    freeLook.LookAt = freeLook.Follow = player.GetComponent<PlayerController>().lookAtAndFollow;
    freeLook.m_XAxis.Value = 0;
    freeLook.m_YAxis.Value = 0.2f;
  }
}