namespace Moist.Configuration.UI.ViewModels.Panel
{
    public enum MessageType
    {
        Nothing = -1,
        Welcome = 0
    }

    public static class MessageTypeExtensions
    {
        public static string ToMessage(this MessageType @this)
        {
            switch (@this)
            {
                case MessageType.Welcome:
                    return "Welcome, setup your shop";
                default:
                    return "";
            }
        }
    }
}