﻿using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.IRepositories
{
    public interface IShiftServiceDapper
    {
        Task<List<Shift>> GetAll();
    }
}
