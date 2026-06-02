using Store.Domain.Entities.Carts;
using Store.Domain.Entities.Commons;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Finances
{
    public class RequestPay : BaseEntity
    {
        public Guid Guid { get; set; }
        public virtual User User { get; set; }
        public long UserId { get; set; }
        public virtual Cart Cart { get; set; }
        public long CartId { get; set; }
        public int Amount { get; set; }
        public bool IsPayed { get; set; }
        public DateTime? PayTime { get; set; }
        public string Athority { get; set; } = "";
        public string RefId { get; set; } = "";
        public virtual ICollection<Order> Orders { get; set; }
    }
}
