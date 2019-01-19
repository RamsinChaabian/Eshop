using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmDetailsProduct
    {
        public int ProductId { get; set; }
        public string UserId_FK { get; set; }
        public string ProductCode { get; set; }
        public int CategoryId_FK { get; set; }
        public int FirstSubCat_FK { get; set; }
        public int SecondSubCat_FK { get; set; }
        public int LastSubCat_FK { get; set; }
        public string ProductNameFA { get; set; }
        public string ProductNameEN { get; set; }
        public string DefaultPic { get; set; }
        public int SeeProduct { get; set; }
        public decimal Price { get; set; }
        public int OffProduct { get; set; }
        public int CountProduct { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public string CategoryName { get; set; }
        public string SecondCatName { get; set; }
        public string LastCatName { get; set; }
        public string LinkProduct { get; set; }
        public string Username { get; set; }
        
        public List<ProductGallery> ListImagesProduct { get; set; }
        public List<ProductComments> ListCommentsProduct { get; set; }

        public class ProductGallery
        {
            public int PictureId { get; set; }
            public int ProductId_FK { get; set; }
            public string TitlePicture { get; set; } //For Seo
            public string PictureName { get; set; }
            public bool DefaultPicProduct { get; set; }
        }
        public class ProductComments
        {
            public int CommentId { get; set; }
            public int ProductId_FK { get; set; }
            public string UserId_FK { get; set; }
            public string FulllName { get; set; }
            public string Email { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public DateTime DateComment { get; set; }
        }
    }
}
