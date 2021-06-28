using DynaxInvoice.BO;
using DynaxInvoice.DL;
using DynaxInvoice.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class UserBL
    {        
        public IEnumerable<DynaxUser> GetAllUser()
        {
            try
            {
                var _objDb = new DbUser();
                var userList = _objDb.GetUserList();
                return userList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetAllUser() - " + ex.Message);
            }
        }
        public DynaxUser GetUserById(int id)
        {
            try
            {
                var _objDb = new DbUser();
                var oUser = _objDb.GetUserDetails(id);
                return oUser;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetUserById() - " + ex.Message);
            }
        }
        public int AddUser(DynaxUser user)
        {
            int id;
            try
            {
                var _objDb = new DbUser();
              id=  _objDb.AddUser(user);               
            }
            catch (Exception ex)
            {               
                throw new Exception("Dynax:AddUser() - " + ex.Message);
            }
            return id;
        }
        public bool UpdateUser(DynaxUser user)
        {
            bool flag = true;
            try
            {
                var _objDb = new DbUser();
                _objDb.UpdateUser(user);
            }
            catch (Exception ex)
            {
                flag = false;
                throw new Exception("Dynax:userLogin() - " + ex.Message);
            }
            return flag;
        }

        public DynaxUser UserLogin(LoginViewModel objLogin)
        {
            try
            {
                var objUser = new DynaxUser();
                var objUser1 = new DynaxUser();
                var _objDb = new DbUser();
                var utility = new Utilities();

                objLogin.Password = utility.Encrypt(objLogin.Password);
                objUser1 = _objDb.Login(objLogin);

                if (objUser1 != null)
                {
                    objUser.Email = objUser1.Email;
                    objUser.UserName = objUser1.UserName;
                    objUser.FullName = objUser1.FullName;
                    objUser.UserType = objUser1.UserType;
                    objUser.DealerId = objUser1.DealerId;
                    objUser.Id = objUser1.Id;
                    objUser.Status = objUser1.Status;
                }
                else
                {
                    objUser = null;
                }
                return objUser;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:UserLogin() - " + ex.Message);
            }
        }

        public bool ChangePassword(int id, string pass)
        {
            bool flag;
            try
            {
                var _objDb = new DbUser();
                var utility = new Utilities();
                flag =_objDb.ChangePassword(id, utility.Encrypt(pass));
            }
            catch (Exception ex)
            {             
                throw new Exception("Dynax:userLogin() - " + ex.Message);
            }
            return flag;
        }
    }
}
