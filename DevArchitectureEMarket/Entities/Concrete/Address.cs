using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
   public class Address:IEntity
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int UserId { get; set; }
        public int PostalCode { get; set; }
        public string NameSurname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AddressTitle { get; set; }
        public string OpenAddress { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }
    }
}
