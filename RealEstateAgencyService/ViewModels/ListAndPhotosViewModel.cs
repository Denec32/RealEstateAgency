namespace RealEstateAgencyService.Models
{
    public class ListAndPhotosViewModel
    {
        public Listing? Listing { get; set; }

        public List<IFormFile> Photos { get; set; }

        public ListAndPhotosViewModel()
        {
            Photos = new List<IFormFile>();
        }
    }
}
