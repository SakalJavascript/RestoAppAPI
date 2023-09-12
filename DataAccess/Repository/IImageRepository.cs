using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public interface IImageRepository
    {
        ImageModal Save(ImageModal image);
    }
}