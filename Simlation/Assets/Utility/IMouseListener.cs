namespace Game.Utility 
{
    /// <summary>
    /// Interface for general mouse functions 
    /// </summary>
    public interface IMouseListener 
    {
        /// <summary>
        /// Single mouse click on object (collider) from screenspace to worldspace
        /// </summary>
        void MouseClick();
        /// <summary>
        /// Mouse over object (collider) from screenspace to worldspace
        /// </summary>
        void MouseOver();
        /// <summary>
        /// Mouse exits object (collider) from screenspace to worldspace
        /// </summary>
        void MouseExit();
    }
}