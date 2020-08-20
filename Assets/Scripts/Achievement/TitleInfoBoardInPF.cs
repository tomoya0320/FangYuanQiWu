using UnityEngine;
using UnityEngine.UI;

public class TitleInfoBoardInPF : MonoBehaviour {
  public Sprite[] buttonImgs;
  public Text description;
  public Button selectTitleButton;
  private Title selectedTitle;

  public void Select(Title title) {
    if (selectedTitle) {
      selectedTitle.GetComponent<Image>().sprite = buttonImgs[0];
    }
    selectTitleButton.gameObject.SetActive(true);
    if (title.index == Database.CurrentTitleId) {
      selectTitleButton.transform.GetChild(0).GetComponent<Text>().text = StringMap.UNSELECT_TITLE;
    } else {
      selectTitleButton.transform.GetChild(0).GetComponent<Text>().text = StringMap.SELECT_TITLE;
    }
    title.GetComponent<Image>().sprite = buttonImgs[1];
    description.text = Database.GetTitleInfoByIndex(title.index)
    [StringMap.DESCRIPTION].str.Replace("\\n", "\n");
    selectedTitle = title;
  }
  public void WearOrNotTitle() {
    if (selectedTitle.index == Database.CurrentTitleId) {
      Database.SetTitleById(-1);
      selectTitleButton.transform.GetChild(0).GetComponent<Text>().text = StringMap.SELECT_TITLE;
    } else {
      Database.SetTitleById(selectedTitle.index);
      selectTitleButton.transform.GetChild(0).GetComponent<Text>().text = StringMap.UNSELECT_TITLE;
    }
  }
}