using SrvIDO.DATA.Entities;

namespace SrvIDO.DATA.Interfaces
{
    public interface IIDOService
    {
        Task<bool> EnviaIDO();
        void UpdateIDO();
    }
}
