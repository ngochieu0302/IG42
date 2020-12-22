using FDI.Base;
using FDI.DA;

namespace FFDI.DA
{
    public class GalleryPictureDL : BaseDA
    {
        
        public int Add(Gallery_Picture questionPicture)
        {
            FDIDB.Gallery_Picture.Add(questionPicture);
            return FDIDB.SaveChanges();
        }
    }
}