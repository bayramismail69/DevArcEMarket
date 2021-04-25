﻿
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class OrderStatusRepository : EfEntityRepositoryBase<OrderStatus, ProjectDbContext>, IOrderStatusRepository
    {
        public OrderStatusRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
