namespace Voxo.Models
{
    public class UserWishlistItem
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }

        public int ProductId { get; set; }

        public AppUser AppUser { get; set; }

        public Product Product { get; set; }


    }
}