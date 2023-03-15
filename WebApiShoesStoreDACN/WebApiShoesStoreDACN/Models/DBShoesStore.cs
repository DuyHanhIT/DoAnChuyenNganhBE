using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApiShoesStoreDACN.Models
{
    public partial class DBShoesStore : DbContext
    {
        public DBShoesStore()
            : base("name=DBShoesStore")
        {
        }

        public virtual DbSet<account> accounts { get; set; }
        public virtual DbSet<brand> brands { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<employee> employees { get; set; }
        public virtual DbSet<monthlyrevenue> monthlyrevenues { get; set; }
        public virtual DbSet<monthlyrevenuedetail> monthlyrevenuedetails { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<rating> ratings { get; set; }
        public virtual DbSet<reply> replies { get; set; }
        public virtual DbSet<role> roles { get; set; }
        public virtual DbSet<sale> sales { get; set; }
        public virtual DbSet<searchhistory> searchhistories { get; set; }
        public virtual DbSet<shoes> shoes { get; set; }
        public virtual DbSet<sizetable> sizetables { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<favorite> favorites { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<orderdetail> orderdetails { get; set; }
        public virtual DbSet<saleDetail> saleDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<account>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.comments)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.comments1)
                .WithRequired(e => e.account1)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.employees)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.employees1)
                .WithRequired(e => e.account1)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.favorites)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.favorites1)
                .WithRequired(e => e.account1)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.orders)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.orders1)
                .WithRequired(e => e.account1)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.products)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.products1)
                .WithRequired(e => e.account1)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.ratings)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.ratings1)
                .WithRequired(e => e.account1)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.replies)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.replies1)
                .WithRequired(e => e.account1)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.searchhistories)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.searchhistories1)
                .WithRequired(e => e.account1)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.users)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.users1)
                .WithRequired(e => e.account1)
                .HasForeignKey(e => e.accountid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<brand>()
                .Property(e => e.brandname)
                .IsUnicode(false);

            modelBuilder.Entity<brand>()
                .Property(e => e.logo)
                .IsUnicode(false);

            modelBuilder.Entity<brand>()
                .HasMany(e => e.shoes)
                .WithRequired(e => e.brand)
                .HasForeignKey(e => e.brandid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<brand>()
                .HasMany(e => e.shoes1)
                .WithRequired(e => e.brand1)
                .HasForeignKey(e => e.brandid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.shoes)
                .WithRequired(e => e.category)
                .HasForeignKey(e => e.categoryid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.shoes1)
                .WithRequired(e => e.category1)
                .HasForeignKey(e => e.categoryid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<comment>()
                .Property(e => e.image)
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .HasMany(e => e.replies)
                .WithRequired(e => e.comment)
                .HasForeignKey(e => e.cmtid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<comment>()
                .HasMany(e => e.replies1)
                .WithRequired(e => e.comment1)
                .HasForeignKey(e => e.cmtid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<employee>()
                .Property(e => e.phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<monthlyrevenue>()
                .Property(e => e.month)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<monthlyrevenue>()
                .Property(e => e.year)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<monthlyrevenue>()
                .HasMany(e => e.monthlyrevenuedetails)
                .WithRequired(e => e.monthlyrevenue)
                .HasForeignKey(e => new { e.month, e.year })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<monthlyrevenue>()
                .HasMany(e => e.monthlyrevenuedetails1)
                .WithRequired(e => e.monthlyrevenue1)
                .HasForeignKey(e => new { e.month, e.year })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<monthlyrevenuedetail>()
                .Property(e => e.month)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<monthlyrevenuedetail>()
                .Property(e => e.year)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<rating>()
                .Property(e => e.rate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<reply>()
                .Property(e => e.content)
                .IsFixedLength();

            modelBuilder.Entity<reply>()
                .Property(e => e.image)
                .IsUnicode(false);

            modelBuilder.Entity<role>()
                .HasMany(e => e.accounts)
                .WithRequired(e => e.role)
                .HasForeignKey(e => e.roleid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<role>()
                .HasMany(e => e.accounts1)
                .WithRequired(e => e.role1)
                .HasForeignKey(e => e.roleid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sale>()
                .Property(e => e.salename)
                .IsUnicode(false);

            modelBuilder.Entity<sale>()
                .HasMany(e => e.saleDetails)
                .WithRequired(e => e.sale)
                .HasForeignKey(e => e.saleid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sale>()
                .HasMany(e => e.saleDetails1)
                .WithRequired(e => e.sale1)
                .HasForeignKey(e => e.saleid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<searchhistory>()
                .Property(e => e.keyword)
                .IsUnicode(false);

            modelBuilder.Entity<shoes>()
                .Property(e => e.shoename)
                .IsUnicode(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.comments)
                .WithRequired(e => e.sho)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.comments1)
                .WithRequired(e => e.sho1)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.monthlyrevenuedetails)
                .WithRequired(e => e.sho)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.monthlyrevenuedetails1)
                .WithRequired(e => e.sho1)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.ratings)
                .WithRequired(e => e.sho)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.ratings1)
                .WithRequired(e => e.sho1)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.favorites)
                .WithRequired(e => e.sho)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.favorites1)
                .WithRequired(e => e.sho1)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.orderdetails)
                .WithRequired(e => e.sho)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.orderdetails1)
                .WithRequired(e => e.sho1)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.saleDetails)
                .WithRequired(e => e.sho)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.saleDetails1)
                .WithRequired(e => e.sho1)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.sizetables)
                .WithRequired(e => e.sho)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<shoes>()
                .HasMany(e => e.sizetables1)
                .WithRequired(e => e.sho1)
                .HasForeignKey(e => e.shoeid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<status>()
                .HasMany(e => e.products)
                .WithRequired(e => e.status)
                .HasForeignKey(e => e.statusid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<status>()
                .HasMany(e => e.products1)
                .WithRequired(e => e.status1)
                .HasForeignKey(e => e.statusid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<status>()
                .HasMany(e => e.orders)
                .WithRequired(e => e.status)
                .HasForeignKey(e => e.statusid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<status>()
                .HasMany(e => e.orders1)
                .WithRequired(e => e.status1)
                .HasForeignKey(e => e.statusid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .Property(e => e.phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<order>()
                .Property(e => e.phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<order>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<order>()
                .Property(e => e.momo)
                .IsUnicode(false);
        }
    }
}
