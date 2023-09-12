using System.Threading.Tasks;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Service
{
    public interface IImageService
    {
        Task<ImageModal> Save(ImageModal image);
    }
}