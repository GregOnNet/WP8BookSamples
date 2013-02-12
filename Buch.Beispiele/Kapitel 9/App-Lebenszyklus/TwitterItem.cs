namespace Twitter
{
    public class TwitterItem
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string ImageUrl { get; set; }

        public string Preview
        {
            get
            {
                if (this.Message.Length < 50)
                {
                    return this.Message;
                }

                return this.Message.Substring(0, 50);
            }
        }
    }
}