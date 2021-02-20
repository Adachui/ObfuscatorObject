using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public interface IModel
{

}

public class UIScrollRect : MonoBehaviour
{
    public GameObject itemCell;
    public ScrollRect scrollRect;

    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> visualItems = new List<GameObject>();

    public float totalHeight;
    public float totalWidth;

    public float itemsHeight;
    public float minHeight = 100;
    public float spacing = 10;
    public float topSpaceing = 30;
    /**显示数量 */
    public int visualNums = 0;

    public int startIndex = 0;
    public int lastIndex = 0;

    public List<IModel> dataList;

    private bool isData = false;

    private RectTransform content;
    private void OnValidate()
    {
        scrollRect = this.GetComponent<ScrollRect>();
    }


    private void Awake()
    {
        totalWidth = this.GetComponent<RectTransform>().sizeDelta.x;
        totalHeight = this.GetComponent<RectTransform>().sizeDelta.y;

        content = scrollRect.content;
    }

    void Start()
    {

    }

    private float LineHeight
    {
        get
        {
            return minHeight + spacing;
        }
    }

    private void Init()
    {
        visualNums = Mathf.CeilToInt(totalHeight / LineHeight) + 1;
        Debug.Log("visualNums = " + visualNums);
        for (int i = 0; i < visualNums; i++)
        {
            this.AddItem(dataList[i]);
        }

        startIndex = 0;
        lastIndex = 0;
    }

    public void SetData()
    {
        this.Init();
        startIndex = 0;
        if (DataCount <= visualNums)
        {
            lastIndex = DataCount;
        }
        else
        {
            lastIndex = visualNums - 1;
        }

        for (int i = startIndex; i < lastIndex; i++)
        {
            GameObject go = this.PopItem();
            go.name = i.ToString();
            go.SetActive(true);
            go.transform.localPosition = new Vector3(0, -i * LineHeight - topSpaceing);
            visualItems.Add(go);

            UpdateItem(i, go);
        }

        itemsHeight = DataCount * LineHeight - spacing;
        SetSize();

        isData = true;
    }

    private GameObject PopItem()
    {
        GameObject go = null;
        if (items.Count > 0)
        {
            go = items[0];
            go.SetActive(true);
            items.RemoveAt(0);
        }
        else
        {
            print("没有元素了");
        }
        return go;
    }

    private void PushItem(GameObject go)
    {
        items.Add(go);
        go.SetActive(false);
    }

    private int DataCount
    {
        get { return dataList.Count; }
    }

    private void SetSize()
    {
        scrollRect.content.sizeDelta = new Vector2(totalWidth, itemsHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (isData)
        {
            Validate();
        }
    }

    private void Validate()
    {
        float vy = content.anchoredPosition.y;
        //上边界
        float rollUpperTopY = (startIndex + 1) * LineHeight;
        float rollUnderTopY = startIndex * LineHeight;

        if (vy > rollUpperTopY && lastIndex < DataCount)
        {
            print("上边界 移除");
            if (visualItems.Count > 0)
            {
                GameObject go = visualItems[0];
                visualItems.RemoveAt(0);
                PushItem(go);
            }
            startIndex++;
        }


        float rollUnderBottomY = (lastIndex - 1) * LineHeight - (spacing);

        if (vy < rollUnderBottomY - totalHeight && startIndex > 0)
        {
            print("下边界 移除");
            lastIndex--;

            if (visualItems.Count > 0)
            {
                GameObject go = visualItems[visualItems.Count - 1];
                visualItems.RemoveAt(visualItems.Count - 1);
                PushItem(go);
            }
        }

        if (vy < rollUnderTopY && startIndex > 0)
        {
            print("上边界 增加 " + startIndex);
            startIndex--;
            GameObject go = PopItem();
            visualItems.Insert(0, go);
            UpdateItem(startIndex, go);
            go.transform.localPosition = new Vector3(0, -startIndex * LineHeight - topSpaceing);
        }


        float rollUpperBottomY = lastIndex * LineHeight - (spacing);


        if (vy > rollUpperBottomY - totalHeight && lastIndex < DataCount)
        {
            print("下边界 增加");
            GameObject go = PopItem();
            visualItems.Add(go);
            go.transform.localPosition = new Vector3(0, -lastIndex * LineHeight - topSpaceing);
            UpdateItem(lastIndex, go);
            lastIndex++;
        }


    }

    private void UpdateItem(int index, GameObject go)
    {
        IModel _model = dataList[index];
        go.name = index.ToString();
        go.GetComponent<AppAdvisoryHelper>().InitModelData(_model);
    }

    public void CleanAllItem()
    {
        foreach(GameObject obj in items){
            Destroy(obj);
        }
        foreach (GameObject obj in visualItems)
        {
            Destroy(obj);
        }

        items.Clear();
        visualItems.Clear();
    }


    #region 
    public void AddItem(IModel _model)
    {
        GameObject go = Instantiate(itemCell);
        go.transform.SetParent(content);
        go.transform.localScale = new Vector3(1, 1, 1);
        Vector2 v = new Vector2(0.5f, 1);
        RectTransform rectTrans = go.GetComponent<RectTransform>();
        rectTrans.anchorMax = v;
        rectTrans.anchorMin = v;
        rectTrans.pivot = v;
        go.SetActive(false);
        go.GetComponent<AppAdvisoryHelper>().InitModelData(_model);
        items.Add(go);
    }

    #endregion

    public void DeleteItem()
    {

    }
}
