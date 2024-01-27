using UnityEngine;

/// <summary>
/// This class controls setting up a m_Speaker to be used by the dialogue system
/// A m_Speaker controls the name of the character speaking, the sprite that represents them in dialogue, the position they prefer to have on the screen,
/// and the scaing of their m_SpriteImage to make it fit on the screen.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/Speaker")]
public class Speaker : ScriptableObject
{
    #region Class Variables
    /// <summary>
    /// Name of the m_Speaker as a string
    /// </summary>
    [Header("General Settings")]
    [Tooltip("The name of the m_Speaker")]
    public string m_SpeakerName;

    /// <summary>
    /// Sprite of m_Speaker 
    /// </summary>
    [Tooltip("The sprite to use for this m_Speaker")]
    public Sprite m_SpeakerSprite;
    /// <summary>
    /// Enum class of sprite positions
    /// </summary>
    public enum Position { Left, Right };

    /// <summary>
    /// Preferred position of the m_Speaker on the screen
    /// </summary>
    [Header("Positioning Settings")]
    [Tooltip("The side of the screen this m_Speaker would prefer to be on")]
    public Position m_PreferredPosition = Position.Left;

    /// <summary>
    /// The side that the original sprite faces with no modification
    /// </summary>
    [InspectorName("Base Sprite Faces")]
    [Tooltip("The direction the m_Speaker's sprite faces in its import")]
    public Position m_BaseSpriteFacesDirection = Position.Right;

    /// <summary>
    /// The Y offset of the sprite in relation to the height of the screen
    /// </summary>
    [Tooltip("How should the m_Speaker's sprite be offset in relation to the screen's height? 0.5 would put the bottom of the sprite at half the screens height, -0.5 would put it below the edge of the screen.")]
    public float m_FractionalPositionOffsetY = 0;

    /// <summary>
    /// The X offset of the sprite in relation to the height of the screen
    /// </summary>
    [Tooltip("How should the m_Speaker's sprite be offset in relation to the screen's width? 0.5 would move it to the right (or left when the m_Speaker enters from screen right) by half the screen width.")]
    public float m_FractionalPositionOffsetX = 0;

    /// <summary>
    /// Multiplier of the scale of the sprite
    /// </summary>
    [Tooltip("How much should the sprite be scaled down or up? 1 is native aspect ratio, 0.5 would be half the regular aspect ratio for example")]
    public float m_ScaleMultiplier = 1;
    #endregion
}
