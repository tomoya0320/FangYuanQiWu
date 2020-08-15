using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleInfoBoardInPF : MonoBehaviour
{
    public Sprite[] buttonImgs;
    public Text description;
    public Button selectTitleButton;
    private Title selectedTitle;

    public void Select(Title title)
    {
        if (selectedTitle)
        {
            selectedTitle.GetComponent<Image>().sprite = buttonImgs[0];
        }
        selectTitleButton.gameObject.SetActive(true);
        if(title.index == DataBase.GetCurrentTitleId())
        {
            selectTitleButton.transform.GetChild(0).GetComponent<Text>().text = "摘下称号";
        }
        else
        {
            selectTitleButton.transform.GetChild(0).GetComponent<Text>().text = "选中称号";
        }
        title.GetComponent<Image>().sprite = buttonImgs[1];
        description.text = DataBase.GetTitleInfoByIndex(title.index)
            ["description"].str.Replace("\\n","\n");
        selectedTitle = title;
    }
    public void WearOrNotTitle()
    {
        if (selectedTitle.index == DataBase.GetCurrentTitleId())
        {
            DataBase.SetTitleById(-1);
            selectTitleButton.transform.GetChild(0).GetComponent<Text>().text = "选中称号";
        }
        else
        {
            DataBase.SetTitleById(selectedTitle.index);
            selectTitleButton.transform.GetChild(0).GetComponent<Text>().text = "摘下称号";
        }
    }
}
