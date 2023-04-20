using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private TextAsset creditsFile;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private RectTransform creditsRectTransform;
    private Coroutine _scrollCreditsCoroutine;
    private float _inactivityTimer;
    
    /// <summary>
    /// This method is called when the game object is enabled. It sets up the credits text and starts scrolling it. It also resets the inactivity timer.
    /// </summary>
    private void OnEnable()
    {
        // Set the credits text to the text from the credits file
        creditsText.text = creditsFile.text;
        // Adjust the height of the credits rect transform to fit the text
        creditsRectTransform.sizeDelta = new Vector2(creditsRectTransform.sizeDelta.x, creditsText.preferredHeight);
        // Start the coroutine that scrolls the credits
        _scrollCreditsCoroutine = StartCoroutine(ScrollCredits());
        // Reset the inactivity timer to zero
        _inactivityTimer = 0;
    }

    /// <summary>
    /// This method is called when the game object is disabled. It stops the coroutine that scrolls the credits if it is not null.
    /// </summary>
    private void OnDisable()
    {
        // If the scroll credits coroutine is null, return
        if (_scrollCreditsCoroutine == null) 
            return;
        // Stop the scroll credits coroutine
        StopCoroutine(_scrollCreditsCoroutine);
        // Set the scroll credits coroutine to null
        _scrollCreditsCoroutine = null;
    }

    /// <summary>
    /// This method is called once per frame. It checks the mouse scroll input and adjusts the credits position accordingly. It also manages the inactivity timer and the scroll credits coroutine.
    /// </summary>
    private void Update()
    {
        // If the mouse scroll is positive, move the credits up and stop the scroll credits coroutine if it is not null
        if(Mouse.current.scroll.y.ReadValue() > 0)
        {
            if (_scrollCreditsCoroutine != null)
            {
                StopCoroutine(_scrollCreditsCoroutine);
                _scrollCreditsCoroutine = null;
            }
            // Set the inactivity timer to 3 seconds
            _inactivityTimer = 3;
            // Get the current anchored position of the credits rect transform
            var anchoredPosition = creditsRectTransform.anchoredPosition;
            // Increase the y value by 25
            anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + 25);
            // Set the new anchored position of the credits rect transform
            creditsRectTransform.anchoredPosition = anchoredPosition;
        }
        // If the mouse scroll is negative, move the credits down and stop the scroll credits coroutine if it is not null
        else if(Mouse.current.scroll.y.ReadValue() < 0)
        {
            if (_scrollCreditsCoroutine != null)
            {
                StopCoroutine(_scrollCreditsCoroutine);
                _scrollCreditsCoroutine = null;
            }
            // Set the inactivity timer to 3 seconds
            _inactivityTimer = 3;
            // Get the current anchored position of the credits rect transform
            var anchoredPosition = creditsRectTransform.anchoredPosition;
            // Decrease the y value by 25
            anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y - 25);
            // Set the new anchored position of the credits rect transform
            creditsRectTransform.anchoredPosition = anchoredPosition;
        }
        // If the mouse scroll is zero and the inactivity timer is greater than or equal to zero and the scroll credits coroutine is null
        else if(Mouse.current.scroll.y.ReadValue() == 0 && _inactivityTimer >= 0 && _scrollCreditsCoroutine == null)
        {
            // Decrease the inactivity timer by the unscaled time since the last frame
            _inactivityTimer -= Time.unscaledDeltaTime;
            // If the inactivity timer is less than or equal to zero and the scroll credits coroutine is null
            if(_inactivityTimer <= 0)
                // Start the scroll credits coroutine if it is not already started
                _scrollCreditsCoroutine ??= StartCoroutine(ScrollCredits());
        }
    }

    /// <summary>
    /// This method is a coroutine that scrolls the credits up until they reach the end. It sets the scroll credits coroutine to null when it is done.
    /// </summary>
    /// <returns>
    /// IEnumerator - a sequence of instructions that can be paused and resumed.
    /// </returns>
    private IEnumerator ScrollCredits()
    {
        // While the y value of the credits anchored position is less than the height of the credits plus 100
        while (creditsRectTransform.anchoredPosition.y < creditsRectTransform.sizeDelta.y + 100)
        {
            // Get the current anchored position of the credits rect transform
            var anchoredPosition = creditsRectTransform.anchoredPosition;
            // Increase the y value by 1
            anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + 1);
            // Set the new anchored position of the credits rect transform
            creditsRectTransform.anchoredPosition = anchoredPosition;
            // Wait for 0.01 seconds in real time
            yield return new WaitForSecondsRealtime(0.01f);
        }
        // Set the scroll credits coroutine to null
        _scrollCreditsCoroutine = null;
    }
}
