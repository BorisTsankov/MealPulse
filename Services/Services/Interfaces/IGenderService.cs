using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IGenderService
    {
            List<Gender> GetAll();
            string GetGenderName(int id);
        }
    
}
