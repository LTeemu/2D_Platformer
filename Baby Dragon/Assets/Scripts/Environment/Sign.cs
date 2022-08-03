using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject textBubble;
    [SerializeField] private TextMeshPro textField;
    [SerializeField] private string signText;

    // Start is called before the first frame update
    void Start()
    {
        textField.text = signText;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        textBubble.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        textBubble.SetActive(false);
    }
}
