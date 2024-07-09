namespace JazzyLucas.Core.Utils
{
    [System.Serializable]
    public class RaycastInfo
    {
        public readonly RaycastClickType RaycastClickType;
        public readonly RaycastHoldType RaycastHoldType;
        public RaycastInfo(RaycastClickType raycastClickType, RaycastHoldType raycastHoldType)
        {
            RaycastClickType = raycastClickType;
            RaycastHoldType = raycastHoldType;
        }
    }
    
    public enum RaycastClickType
    {
        NONE,
        PRIMARY, // Left click
        SECONDARY, // Right click
    }
    public enum RaycastHoldType
    {
        NONE,
        // TODO: not sure if we should put holdings in the mods
        HOLDING_CLICK, // Holding the click
        HOLDING_SHIFT, // Holding shift
        HOLDING_CTRL, // Holding ctrl
    }
}