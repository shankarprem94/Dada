using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public  class DynaxStateBL
    {
        public int AddState(DynaxState st)
        {
           int id;
            try
            {
                var _db = new DbState();
                id = _db.AddState(st);
                return id;
            }
            catch(Exception ex)
            {
                throw new Exception("DynaxInvoice.BL:AddState() -" + ex.ToString());
            }
           
        }
        public DynaxState GetStateDetails(int id)
        {
            try
            {
                var _db = new DbState();
                var objSt = _db.GetStateDetails(id);  
                return objSt;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.BL:GetStateDetails() -" + ex.ToString());
            }
        }

        public bool UpdateState(DynaxState st)
        {
            bool flag;
            try
            {
                var _db = new DbState();
                flag = _db.UpdateState(st);
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.BL:UpdateState() -" + ex.ToString());
            }

        }

        public IEnumerable<DynaxState> StateList()
        {
            var lstState = new List<DynaxState>();
            try
            {               
                var _db = new DbState();
                lstState = _db.GetStateList().ToList();
               
            }
            catch(Exception ex)
            {
                throw new Exception("DynaxInvoice.BL:StateList() -" + ex.ToString());
            }
            return lstState;
        }

    }
}
