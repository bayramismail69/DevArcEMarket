﻿using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
   public class EfProductDal:EfEntityRepositoryBase<Product,ProjectDbContext>,IProductDal
    {
        public EfProductDal(ProjectDbContext context) : base(context)
        {
        }
    }
}
