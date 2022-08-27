using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StockManager : MonoBehaviour
{
    [SerializeField] private TMP_Text stockText;
    [SerializeField] private GameObject insufficientQtyWindow;
    private int productQty = 100;

    [SerializeField] private Image status;
    [SerializeField] private Sprite waitingProductSprite, lowProductSprite, noProductSprite;

    [SerializeField] private int standardProdQty;


    private Animator anim;

    [SerializeField] TMP_InputField sendInput;
    private void OnEnable()
    {
        insufficientQtyWindow.SetActive(false);
        status.gameObject.SetActive(false);
        UpdateStockUI();
        anim = GetComponent<Animator>();

    }

    public void ShowBoxUI()
    {
        anim.SetBool("show", !anim.GetBool("show"));
    }

    public void SendProduct(TMP_InputField input)
    {
        int sendQty = int.Parse(input.text);

        input.text = string.Empty;
        if (sendQty < productQty)
        {
            productQty -= sendQty;
            UpdateStockUI();
        }
        else
        {
            productQty = 0;
            insufficientQtyWindow.SetActive(true);
        }
    }

    public void BuyProducts()
    {
        UpdateStockUI(true);
        StartCoroutine(WaitForProducts());
        insufficientQtyWindow.SetActive(false);
    }

    public void DontBuyProducts()
    {
        UpdateStockUI();
        insufficientQtyWindow.SetActive(false);
    }

    IEnumerator WaitForProducts()
    {
        sendInput.interactable = false;

        yield return new WaitForSeconds(5);

        sendInput.interactable = true;

        productQty += standardProdQty;

        UpdateStockUI();
    }

    private void UpdateStockUI(bool isWaiting = false)
    {
        stockText.text = "Quantidade em estoque: " + productQty;

        if (productQty == 0)
        {
            status.gameObject.SetActive(true);

            if (isWaiting)
                status.sprite = waitingProductSprite;
            else
                status.sprite = noProductSprite;
        }
        else if (productQty > 0 && productQty < 10)
        {
            status.gameObject.SetActive(true);
            status.sprite = lowProductSprite;
        }
        else
        {
            status.gameObject.SetActive(false);
        }
    }
}
