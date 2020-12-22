namespace FDI.DA
{
    public class ShopProductPictureDA : BaseDA
    {
        #region Constructer
        public ShopProductPictureDA()
        {
        }

        public ShopProductPictureDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ShopProductPictureDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
    }
}
