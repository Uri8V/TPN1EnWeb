using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IShoeSizeService
    {
        public int GetId();
        public ShoeSizes GetIdShoeSize(int size, int shoe);
    }
}
