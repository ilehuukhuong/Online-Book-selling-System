using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPT_LIBRARY.Models.EF
{
    [Table("tb_Order")]
    public class Order:CommonAbstract
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required(ErrorMessage = "Full Name cannot be left blank")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Phone number cannot be left blank")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address cannot be left blank")]
        public string Address { get; set; }
        public string Email { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public int TypePayment { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}