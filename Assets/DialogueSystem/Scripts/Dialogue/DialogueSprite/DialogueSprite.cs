using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls the animation of a dialogue sprite, which position themselves on the left or right of the dialogue display
/// </summary>
public class DialogueSprite : MonoBehaviour
{
    #region Class Variables
    [Header("Dialogue Sprite Information")]
    [Tooltip("The m_SpriteAnimator which controls animations to move the sprite on and off the screen")]
    public Animator m_SpriteAnimator;
    [Tooltip("The m_SpriteImage of the dialogue sprite that gets changed to match the m_Speaker's sprite")]
    public Image m_SpriteImage;
    [Tooltip("The name of the character currently using this dialogue sprite")]
    public string m_CurrentCharacterName;
    [Tooltip("The position of this dialogue sprite on the screen, either left or right")]
    public Speaker.Position m_SpritePosition = Speaker.Position.Left;
    #endregion

    #region Methods
    #region Custom Methods
    /// <summary>
    /// Makes this dialogue sprite active in the display and makes its m_SpriteImage use the sprite from the passed in dialogue.
    /// </summary>
    /// <param name="dialogue">The dialogue controlling this dialogue sprite</param>
    public void MakeActiveSprite(Dialogue dialogue)
    {
        if (dialogue.m_Speaker.m_SpeakerSprite != null)
        {
            m_SpriteImage.sprite = dialogue.m_Speaker.m_SpeakerSprite;
            m_SpriteImage.SetNativeSize();

            // Flip the sprite to make sure it faces towards the dialogue box (towards the other m_Speaker)
            RectTransform imagesRectTransform = m_SpriteImage.GetComponent<RectTransform>();
            // also apply any scaling for the spearker's sprite to the m_SpriteImage
            imagesRectTransform.localScale = (m_SpritePosition == dialogue.m_Speaker.m_BaseSpriteFacesDirection) ? new Vector3(-1 * dialogue.m_Speaker.m_ScaleMultiplier, 1 * dialogue.m_Speaker.m_ScaleMultiplier, 1) : new Vector3(1 * dialogue.m_Speaker.m_ScaleMultiplier, 1 * dialogue.m_Speaker.m_ScaleMultiplier, 1);

            // Apply any offset information to position the m_SpriteImage
            RectTransform canvasImageIsOnRectTransform = imagesRectTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            float newXPosition = (m_SpritePosition == Speaker.Position.Right) ? -dialogue.m_Speaker.m_FractionalPositionOffsetX * canvasImageIsOnRectTransform.rect.width : dialogue.m_Speaker.m_FractionalPositionOffsetX * canvasImageIsOnRectTransform.rect.width;
            imagesRectTransform.anchoredPosition = new Vector2(newXPosition, dialogue.m_Speaker.m_FractionalPositionOffsetY * canvasImageIsOnRectTransform.rect.height);

            m_SpriteAnimator.SetBool("active", true);
            m_SpriteAnimator.SetBool("enter", true);
        }
        
    }

    /// <summary>
    /// Makes this dialogue sprite inactive, thus making it not the current m_Speaker using the dialogue box
    /// </summary>
    public void MakeInactiveSprite()
    {
        m_SpriteAnimator.SetBool("active", false);
    }

    /// <summary>
    /// Resets the state of the dialogue sprite and removes it from being on the display
    /// </summary>
    public void ResetSprite()
    {
        m_SpriteAnimator.SetTrigger("reset");
        m_SpriteAnimator.SetBool("active", false);
        m_SpriteAnimator.SetBool("enter", false);
        m_CurrentCharacterName = "";
    }

    /// <summary>
    /// Returns whether or not the dialogue sprite is in the active position.
    /// </summary>
    /// <returns>Whether or not the dialogue sprite is in the active position</returns>
    public bool CheckIfActive()
    {
        return m_SpriteAnimator.GetBool("active");
    }
    #endregion
    #endregion
}
