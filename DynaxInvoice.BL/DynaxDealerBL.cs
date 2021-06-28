using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class DynaxDealerBL
    {
        public int AddDealer(DynaxDealer state)
        {
            try
            {
                var _objDb = new DbDealer();
                var id = _objDb.AddDealer(state);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddDealer() - " + ex.Message);
            }
        }

        public DynaxDealer GetDealerDetails(int id)
        {
            try
            {
                var _objDb = new DbDealer();
                var Details = _objDb.GetDealerDetails(id);
                return Details;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetDealerDetails() - " + ex.Message);
            }
        }

        public IEnumerable<DynaxDealer> GetDealerList()
        {
            try
            {
                var _objDb = new DbDealer();
                var DetailsList = _objDb.GetDealerList();
                return DetailsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetDealerList() - " + ex.Message);
            }
        }

        public bool UpdateDealer(DynaxDealer state)
        {
            try
            {
                var _objDb = new DbDealer();
                var flag = _objDb.UpdateDealer(state);
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:UpdateDealer() - " + ex.Message);
            }
        }
    }

}
