using FDI.Base;
using FFDI.DA;

namespace FDI.DA
{
    public class GalleryPictureBL
    {
        readonly GalleryPictureDL _dl = new GalleryPictureDL();

        public int Add(Gallery_Picture galleryPicture)
        {
            return _dl.Add(galleryPicture);
        }
    }
}