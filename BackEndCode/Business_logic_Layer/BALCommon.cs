using Data_Access_Layer;
using Data_Access_Layer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_Layer
{
    public class BALCommon
    {
        private readonly DALCommon _dalcommon;
        public BALCommon(DALCommon dALCommon)
        {
            _dalcommon = dALCommon;
        }

        public async Task<List<Country>> CountryList()
        {
            return await _dalcommon.CountryList();
        }

        public async Task<List<City>> CityList(int id)
        {
            return await _dalcommon.CityList(id);
        }
    }
}
