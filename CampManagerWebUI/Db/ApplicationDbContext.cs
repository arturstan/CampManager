using System.Data.Entity;
using CampManager.Domain.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

using CampManagerWebUI.Models;

namespace CampManagerWebUI.Db
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Organization> Organization { get; set; }

        public DbSet<MeasureOrganization> MeasureOrganization { get; set; }
        public DbSet<SeasonOrganization> SeasonOrganization { get; set; }
        public DbSet<Camp> Camp { get; set; }
        public DbSet<CampMeal> CampMeal { get; set; }
        public DbSet<BaseOrganization> BaseOrganization { get; set; }
        public DbSet<Place> Place { get; set; }
        public DbSet<SupplierOrganization> SupplierOrganizations { get; set; }
        public DbSet<ProductOrganization> ProductOrganization { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoicePosition> InvoicePosition { get; set; }
        public DbSet<ProductOut> ProductOut { get; set; }
        public DbSet<ProductOutPosition> ProductOutPosition { get; set; }

        public DbSet<ProductAmount> ProductAmount { get; set; }

        public DbSet<ProductExpend> ProductExpend { get; set; }

        public DbSet<MealBid> MealBid { get; set; }

        public DbSet<Menu> Menu { get; set; }

        public DbSet<UserEmailAllow> UserEmailAllow { get; set; }
        public DbSet<UserOrganization> UserOrganization { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }


        public DbSet<SepticTankKindOrganization> SepticTankKindOrganization { get; set; }
        public DbSet<SepticTank> SepticTank { get; set; }
    }
}