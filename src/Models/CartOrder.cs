namespace SessionTest.Models
{
    public class CartOrder
    {
        public string Id { get; set; }

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

    }
}
