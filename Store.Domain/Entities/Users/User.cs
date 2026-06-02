using Store.Domain.Entities.Carts;
using Store.Domain.Entities.Commons;
using Store.Domain.Entities.Finances;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<UserInRole> UserInRoles { get; set; }
        public virtual ICollection<Cart>? Cart { get; set; }   
        public virtual ICollection<RequestPay>? RequestPay { get; set; }    
        public virtual ICollection<Order>? Orders { get; set; }    
    }
}
