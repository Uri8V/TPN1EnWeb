using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Servicios.Interfaces;

namespace TPN1EnWeb.Servicios.Servicios
{
    public class ShoeSizeService : IShoeSizeService
    {
        private readonly IShoeSizeRepository shoeSizeRepo;
        public ShoeSizeService(IShoeSizeRepository _shoeSizeRepo)
        {
            shoeSizeRepo = _shoeSizeRepo;
        }


        public int GetId()
        {
            return shoeSizeRepo.GetId();
        }
    }
}
