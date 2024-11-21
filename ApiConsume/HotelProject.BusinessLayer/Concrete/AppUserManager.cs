using HotelProject.BusinessLayer.Abstract;
using HotelProject.DataAccessLayer.Abstract;
using HotelProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.BusinessLayer.Concrete
{
    public class AppUserManager : IAppUserService
    {
        private readonly IAppUserdal _appUserDal;

        public AppUserManager(IAppUserdal appUserDal)
        {
            _appUserDal = appUserDal;
        }

        public void TDelete(Appuser t)
        {
            _appUserDal.Delete(t);
        }

        public Appuser TGetById(int id)
        {
            return _appUserDal.GetById(id);
        }

        public List<Appuser> TGetList()
        {
            return _appUserDal.GetList();
        }

        public void TInsert(Appuser t)
        {
            _appUserDal.Insert(t);
        }

        public void TUpdate(Appuser t)
        {
            _appUserDal.Update(t);
        }

        public List<Appuser> tUserListWithWorkLocation()
        {
            return _appUserDal.UserListWithWorkLocation();
        }

        public List<Appuser> TUsersListWithWorkLocations()
        {
            return _appUserDal.UsersListWithWorkLocations();
        }
    }
}
