using System;
using System.Collections.Generic;

namespace eagles_food_backend.Domains.Models
{
    public partial class Organization
    {
        public Organization()
        {
            Lunches = new HashSet<Lunch>();
            OrganizationInvites = new HashSet<OrganizationInvite>();
            OrganizationLunchWallets = new HashSet<OrganizationLunchWallet>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal LunchPrice { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Lunch> Lunches { get; set; }
        public virtual ICollection<OrganizationInvite> OrganizationInvites { get; set; }
        public virtual ICollection<OrganizationLunchWallet> OrganizationLunchWallets { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
