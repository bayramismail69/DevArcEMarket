using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
   public class Order: IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int AddressId { get; set; }
        public int OrderStatusId { get; set; }
        public int PaymentMethodId { get; set; }
        public int Count { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }

    }
}
