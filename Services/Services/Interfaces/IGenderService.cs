using Models.Models;

namespace Services.Services.Interfaces
{
    public interface IGenderService
    {
        List<Gender> GetAll();
        string GetGenderName(int id);
    }

}
