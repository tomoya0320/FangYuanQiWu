using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MailShow : MonoBehaviour
{
    public Sprite selectedMail;
    public Sprite unselectedMail;
    public GameObject noMail;
    public GameObject show;
    public RectTransform mailListContent;
    public RectTransform attachmentListContent;
    public Text text;
    public Text signature;
    public Text date;
    protected MailType mailType;
    protected List<Mail> mailList;
    protected MailInList lastClickedMail;
    protected GameObject mailInListPrefab;
    protected GridLayoutGroup mailGLG;
    protected GridLayoutGroup attachmentGLG;
    protected GameObject attachmentPrefab;
    protected GameObject settlement;
    // Start is called before the first frame update
    protected virtual async void Start()
    {
        mailInListPrefab = await Addressables.LoadAssetAsync<GameObject>
        ("mailinlist").Task;

        attachmentPrefab = await Addressables.LoadAssetAsync<GameObject>
        ("propinsettlement").Task;

        settlement = await Addressables.LoadAssetAsync<GameObject>
        ("settlement").Task;

        mailList = DataBase.GetMailListByType(mailType);
        noMail.SetActive(mailList.Count == 0);

        foreach (Mail mail in mailList)
        {
            GameObject temp = Instantiate(mailInListPrefab, mailListContent);
            temp.GetComponent<MailInList>().Init(mail, this);
        }
        mailGLG = mailListContent.GetComponent<GridLayoutGroup>();
        attachmentGLG = attachmentListContent.GetComponent<GridLayoutGroup>();
        AdjustMailListContentSize();

        CheckGetAllButton();
        CheckReadAllButton();
    }
    protected void AdjustMailListContentSize()
    {
        float height = mailList.Count * (mailGLG.spacing.y + mailGLG.cellSize.y);
        mailListContent.sizeDelta = new Vector2(mailListContent.sizeDelta.x, height);
    }
    public virtual void Switch(MailInList mailInList)
    {
        if (lastClickedMail != null)
        {
            lastClickedMail.GetComponent<Image>().sprite = unselectedMail;
        }
        show.SetActive(true);
        mailInList.GetComponent<Image>().sprite = selectedMail;
        text.text = string.Format(mailInList.mail.text, DataBase.Nickname);
        signature.text = mailInList.mail.signature;
        date.text = Convert.ToDateTime(mailInList.mail.dateTime).ToShortDateString();

        for (int i = 0; i < attachmentListContent.childCount; i++)
        {
            Destroy(attachmentListContent.GetChild(i).gameObject);
        }
        float width = mailInList.mail.attachments.Count * (attachmentGLG.spacing.x + attachmentGLG.cellSize.x);
        attachmentListContent.sizeDelta = new Vector2(width, attachmentListContent.sizeDelta.y);
        foreach (Prop ownedPropData in mailInList.mail.attachments)
        {
            GameObject temp = Instantiate(attachmentPrefab, attachmentListContent);
            temp.GetComponent<PropInSettlement>().Init(ownedPropData);
            temp.GetComponent<PropInSettlement>().icon.color = Color.white * (mailInList.mail.isReceived ? 0.8f : 1.0f);
        }
        lastClickedMail = mailInList;
    }
    public virtual void CheckReadAllButton() { }
    public virtual void CheckGetAllButton() { }
}
