namespace WatchListMVC.Models
{
    public class AddMovieViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime WatchDate { get; set; }
        public string Platform { get; set; }
        public byte[] ImageData { get; set; }
    }
}
