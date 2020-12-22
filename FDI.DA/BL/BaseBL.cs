using FDI.Memcached;

namespace FDI.DA
{
    public class BaseBL
    {
        protected readonly CacheController Cache = CacheController.GetInstance();
    }
}
