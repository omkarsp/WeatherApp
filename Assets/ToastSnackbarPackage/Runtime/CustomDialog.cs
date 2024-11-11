using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomDialog : MonoBehaviour
{
    public static CustomDialog Instance { get; private set; }

    [SerializeField] private Text titleText;
    [SerializeField] private Text messageText;
    [SerializeField] private Button positiveButton;
    [SerializeField] private Button negativeButton;
    [SerializeField] private Text positiveButtonText;
    [SerializeField] private Text negativeButtonText;

    private Action onPositiveClick;
    private Action onNegativeClick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Show(
        string title,
        string message,
        string positiveText,
        string negativeText,
        Action onPositive,
        Action onNegative)
    {
        Instance.ShowDialog(title, message, positiveText, negativeText, onPositive, onNegative);
    }

    private void ShowDialog(
        string title,
        string message,
        string positiveText,
        string negativeText,
        Action onPositive,
        Action onNegative)
    {
        titleText.text = title;
        messageText.text = message;
        positiveButtonText.text = positiveText;
        negativeButtonText.text = negativeText;
        onPositiveClick = onPositive;
        onNegativeClick = onNegative;

        gameObject.SetActive(true);
    }

    public void OnPositiveButtonClick()
    {
        onPositiveClick?.Invoke();
        gameObject.SetActive(false);
    }

    public void OnNegativeButtonClick()
    {
        onNegativeClick?.Invoke();
        gameObject.SetActive(false);
    }
}