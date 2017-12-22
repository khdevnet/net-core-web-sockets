namespace NightChat.Web.Extensibility.Authentication.Facebook.Models
{
    public class UserInfoModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Picture Picture { get; set; }
    }

    public class Picture
    {
        public PictureData Data { get; set; }
    }

    public class PictureData
    {
        public string Url { get; set; }
    }
}