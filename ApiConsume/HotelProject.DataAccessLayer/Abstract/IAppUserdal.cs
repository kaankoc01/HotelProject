﻿using HotelProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.DataAccessLayer.Abstract
{
    public interface IAppUserdal : IGenericDal<Appuser>
    {
        List<Appuser> UserListWithWorkLocation();
        List<Appuser> UsersListWithWorkLocations();

        int AppUserCount();

    }
}
