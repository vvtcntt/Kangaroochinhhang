using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Kangaroochinhhang.models
{
    public partial class KangaroochinhhangContext : DbContext
    {
        public virtual DbSet<TblAddress> TblAddress { get; set; }
        public virtual DbSet<TblAgency> TblAgency { get; set; }
        public virtual DbSet<TblBanks> TblBanks { get; set; }
        public virtual DbSet<TblConfig> TblConfig { get; set; }
        public virtual DbSet<TblConnectCriteria> TblConnectCriteria { get; set; }
        public virtual DbSet<TblConnectImages> TblConnectImages { get; set; }
        public virtual DbSet<TblConnectLoiloc> TblConnectLoiloc { get; set; }
        public virtual DbSet<TblConnectManuProduct> TblConnectManuProduct { get; set; }
        public virtual DbSet<TblConnectManuToAddress> TblConnectManuToAddress { get; set; }
        public virtual DbSet<TblConnectManuToImages> TblConnectManuToImages { get; set; }
        public virtual DbSet<TblConnectManuToNews> TblConnectManuToNews { get; set; }
        public virtual DbSet<TblConnectNews> TblConnectNews { get; set; }
        public virtual DbSet<TblConnectWebs> TblConnectWebs { get; set; }
        public virtual DbSet<TblContact> TblContact { get; set; }
        public virtual DbSet<TblCountOnline> TblCountOnline { get; set; }
        public virtual DbSet<TblCriteria> TblCriteria { get; set; }
        public virtual DbSet<TblGroupAddress> TblGroupAddress { get; set; }
        public virtual DbSet<TblGroupAgency> TblGroupAgency { get; set; }
        public virtual DbSet<TblGroupCriteria> TblGroupCriteria { get; set; }
        public virtual DbSet<TblGroupImage> TblGroupImage { get; set; }
        public virtual DbSet<TblGroupNews> TblGroupNews { get; set; }
        public virtual DbSet<TblGroupProduct> TblGroupProduct { get; set; }
        public virtual DbSet<TblHistoryLogin> TblHistoryLogin { get; set; }
        public virtual DbSet<TblHotline> TblHotline { get; set; }
        public virtual DbSet<TblImage> TblImage { get; set; }
        public virtual DbSet<TblImageProduct> TblImageProduct { get; set; }
        public virtual DbSet<TblInfoCriteria> TblInfoCriteria { get; set; }
        public virtual DbSet<TblLoiloc> TblLoiloc { get; set; }
        public virtual DbSet<SelectListItem> SelectListItem { get; set; }
        public virtual DbSet<TblMaps> TblMaps { get; set; }
        public virtual DbSet<TblModule> TblModule { get; set; }
        public virtual DbSet<TblNews> TblNews { get; set; }
        public virtual DbSet<TblNewsTag> TblNewsTag { get; set; }
        public virtual DbSet<TblOrder> TblOrder { get; set; }
        public virtual DbSet<TblOrderDetail> TblOrderDetail { get; set; }
        public virtual DbSet<TblPartner> TblPartner { get; set; }
        public virtual DbSet<TblProduct> TblProduct { get; set; }
        public virtual DbSet<TblProductTag> TblProductTag { get; set; }
        public virtual DbSet<TblRegister> TblRegister { get; set; }
        public virtual DbSet<TblRight> TblRight { get; set; }
        public virtual DbSet<TblSupport> TblSupport { get; set; }
        public virtual DbSet<TblTags> TblTags { get; set; }
        public virtual DbSet<TblUrl> TblUrl { get; set; }
        public virtual DbSet<TblUser> TblUser { get; set; }
        public virtual DbSet<TblVideo> TblVideo { get; set; }
        public virtual DbSet<TblWeb> TblWeb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                 optionsBuilder.UseSqlServer(@"Data Source=thiepvu\sqlexpress;Initial Catalog=Kangaroochinhhang;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAddress>(entity =>
            {
                entity.ToTable("TblAddress");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Hotline).HasMaxLength(50);

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.Maps).HasColumnType("ntext");

                entity.Property(e => e.Mobile).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TblAgency>(entity =>
            {
                entity.ToTable("tblAgency");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(300);

                entity.Property(e => e.Certificate).HasMaxLength(200);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.IdMenu).HasColumnName("idMenu");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.People).HasMaxLength(50);

                entity.Property(e => e.Tabs).HasMaxLength(500);
            });

            modelBuilder.Entity<TblBanks>(entity =>
            {
                entity.ToTable("tblBanks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.NameBank).HasMaxLength(200);

                entity.Property(e => e.NumberBank).HasMaxLength(100);
            });

            modelBuilder.Entity<TblConfig>(entity =>
            {
                entity.ToTable("TblConfig");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Analytics).HasMaxLength(300);

                entity.Property(e => e.AppFacebook).HasMaxLength(300);

                entity.Property(e => e.Authorship).HasMaxLength(500);

                entity.Property(e => e.Bct)
                    .HasColumnName("BCT")
                    .HasMaxLength(300);

                entity.Property(e => e.Bni)
                    .HasColumnName("BNI")
                    .HasMaxLength(300);

                entity.Property(e => e.CodeChat).HasMaxLength(500);

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.Dmca)
                    .HasColumnName("DMCA")
                    .HasMaxLength(300);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FanpageFacebook).HasMaxLength(500);

                entity.Property(e => e.FanpageGoogle).HasMaxLength(300);

                entity.Property(e => e.FanpageYoutube).HasMaxLength(300);

                entity.Property(e => e.Favicon).HasMaxLength(200);

                entity.Property(e => e.GeoMeta).HasMaxLength(500);

                entity.Property(e => e.GoogleMaster).HasMaxLength(300);

                entity.Property(e => e.Host).HasMaxLength(200);

                entity.Property(e => e.HotlineIn)
                    .HasColumnName("HotlineIN")
                    .HasMaxLength(50);

                entity.Property(e => e.HotlineOut)
                    .HasColumnName("HotlineOUT")
                    .HasMaxLength(50);

                entity.Property(e => e.Logo).HasMaxLength(200);

                entity.Property(e => e.MobileIn)
                    .HasColumnName("MobileIN")
                    .HasMaxLength(500);

                entity.Property(e => e.MobileOut)
                    .HasColumnName("MobileOUT")
                    .HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.PassEmail).HasMaxLength(200);

                entity.Property(e => e.Skh)
                    .HasColumnName("SKH")
                    .HasMaxLength(300);

                entity.Property(e => e.Slogan).HasMaxLength(200);

                entity.Property(e => e.Tags).HasColumnType("ntext");

                entity.Property(e => e.TimeWork).HasMaxLength(300);

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.UserEmail).HasMaxLength(50);
            });

            modelBuilder.Entity<TblConnectCriteria>(entity =>
            {
                entity.ToTable("tblConnectCriteria");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCre).HasColumnName("idCre");

                entity.Property(e => e.Idpd).HasColumnName("idpd");
            });

            modelBuilder.Entity<TblConnectImages>(entity =>
            {
                entity.ToTable("tblConnectImages");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.IdImg).HasColumnName("idImg");
            });

            modelBuilder.Entity<TblConnectLoiloc>(entity =>
            {
                entity.ToTable("tblConnectLoiloc");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Idkh).HasColumnName("idkh");

                entity.Property(e => e.Idll).HasColumnName("idll");
            });

            modelBuilder.Entity<TblConnectManuProduct>(entity =>
            {
                entity.ToTable("tblConnectManuProduct");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.IdManu).HasColumnName("idManu");
            });

            modelBuilder.Entity<TblConnectManuToAddress>(entity =>
            {
                entity.ToTable("tblConnectManuToAddress");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAdress).HasColumnName("idAdress");

                entity.Property(e => e.IdManu).HasColumnName("idManu");
            });

            modelBuilder.Entity<TblConnectManuToImages>(entity =>
            {
                entity.ToTable("tblConnectManuToImages");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdImage).HasColumnName("idImage");

                entity.Property(e => e.IdManu).HasColumnName("idManu");
            });

            modelBuilder.Entity<TblConnectManuToNews>(entity =>
            {
                entity.ToTable("tblConnectManuToNews");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdManu).HasColumnName("idManu");

                entity.Property(e => e.IdNews).HasColumnName("idNews");
            });

            modelBuilder.Entity<TblConnectNews>(entity =>
            {
                entity.ToTable("tblConnectNews");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.IdNew).HasColumnName("idNew");
            });

            modelBuilder.Entity<TblConnectWebs>(entity =>
            {
                entity.ToTable("tblConnectWebs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.IdWeb).HasColumnName("idWeb");
            });

            modelBuilder.Entity<TblContact>(entity =>
            {
                entity.ToTable("tblContact");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TblCountOnline>(entity =>
            {
                entity.ToTable("tblCountOnline");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblCriteria>(entity =>
            {
                entity.ToTable("tblCriteria");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<TblGroupAddress>(entity =>
            {
                entity.ToTable("tblGroupAddress");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Tag).HasMaxLength(100);
            });

            modelBuilder.Entity<TblGroupAgency>(entity =>
            {
                entity.ToTable("tblGroupAgency");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Level).HasMaxLength(50);
            });

            modelBuilder.Entity<TblGroupCriteria>(entity =>
            {
                entity.ToTable("tblGroupCriteria");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.IdCri).HasColumnName("idCri");
            });

            modelBuilder.Entity<TblGroupImage>(entity =>
            {
                entity.ToTable("tblGroupImage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<TblGroupNews>(entity =>
            {
                entity.ToTable("tblGroupNews");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Images).HasMaxLength(300);

                entity.Property(e => e.Keyword).HasMaxLength(300);

                entity.Property(e => e.Name).HasMaxLength(300);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Tag).HasMaxLength(300);

                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<TblGroupProduct>(entity =>
            {
                entity.ToTable("tblGroupProduct");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Background).HasMaxLength(200);

                entity.Property(e => e.Color).HasColumnType("nchar(50)");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Favicon).HasMaxLength(300);

                entity.Property(e => e.ICon)
                    .HasColumnName("iCon")
                    .HasMaxLength(255);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Images).HasMaxLength(300);

                entity.Property(e => e.Keyword).HasMaxLength(300);

                entity.Property(e => e.Name).HasMaxLength(120);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Tag).HasMaxLength(300);

                entity.Property(e => e.Title).HasMaxLength(120);

                entity.Property(e => e.Video).HasMaxLength(200);
            });

            modelBuilder.Entity<TblHistoryLogin>(entity =>
            {
                entity.ToTable("tblHistoryLogin");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Task).HasMaxLength(200);
            });

            modelBuilder.Entity<TblHotline>(entity =>
            {
                entity.ToTable("tblHotline");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.Hotline).HasMaxLength(100);

                entity.Property(e => e.Maps).HasColumnType("ntext");

                entity.Property(e => e.Mobile).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TblImage>(entity =>
            {
                entity.ToTable("tblImage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.IdManu).HasColumnName("idManu");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.ImagesMobile).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Url).HasMaxLength(500);
            });

            modelBuilder.Entity<TblImageProduct>(entity =>
            {
                entity.ToTable("tblImageProduct");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdProduct).HasColumnName("idProduct");

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TblInfoCriteria>(entity =>
            {
                entity.ToTable("tblInfoCriteria");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCri).HasColumnName("idCri");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Url).HasMaxLength(200);
            });

            modelBuilder.Entity<TblLoiloc>(entity =>
            {
                entity.ToTable("tblLoiloc");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Age).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<SelectListItem>(entity =>
            {
                entity.ToTable("SelectListItem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Analytics).HasMaxLength(500);

                entity.Property(e => e.AppFacebook).HasMaxLength(200);

                entity.Property(e => e.Color).HasColumnType("nchar(10)");

                entity.Property(e => e.Color1).HasColumnType("nchar(10)");

                entity.Property(e => e.Color2).HasColumnType("nchar(10)");

                entity.Property(e => e.Company).HasMaxLength(200);

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FanpageFacebook).HasMaxLength(200);

                entity.Property(e => e.FanpageGoogle).HasMaxLength(200);

                entity.Property(e => e.FanpageYoutube).HasMaxLength(200);

                entity.Property(e => e.FanpageZalo).HasMaxLength(500);

                entity.Property(e => e.Favicon).HasMaxLength(200);

                entity.Property(e => e.GeoMeta).HasMaxLength(500);

                entity.Property(e => e.GoogleMaster).HasMaxLength(500);

                entity.Property(e => e.Hotline1).HasMaxLength(100);

                entity.Property(e => e.Hotline2).HasMaxLength(100);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.ImageGenuine)
                    .HasColumnName("imageGenuine")
                    .HasMaxLength(200);

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.ImagesShowroom).HasMaxLength(200);

                entity.Property(e => e.Layout).HasMaxLength(200);

                entity.Property(e => e.Logo).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.Tag).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.Video).HasMaxLength(200);

                entity.Property(e => e.Website).HasMaxLength(120);
            });

            modelBuilder.Entity<TblMaps>(entity =>
            {
                entity.ToTable("tblMaps");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<TblModule>(entity =>
            {
                entity.ToTable("tblModule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TblNews>(entity =>
            {
                entity.ToTable("tblNews");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Images).HasMaxLength(200);

                entity.Property(e => e.Keyword).HasMaxLength(500);

                entity.Property(e => e.Meta).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Tabs).HasMaxLength(500);

                entity.Property(e => e.Tag).HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<TblNewsTag>(entity =>
            {
                entity.ToTable("tblNewsTag");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Idn).HasColumnName("idn");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Tag).HasMaxLength(200);
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.ToTable("tblOrder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Address1).HasMaxLength(200);

                entity.Property(e => e.AddressCp)
                    .HasColumnName("AddressCP")
                    .HasMaxLength(20);

                entity.Property(e => e.DateByy).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Email1).HasMaxLength(50);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Mobile).HasMaxLength(100);

                entity.Property(e => e.Mobile1).HasMaxLength(100);

                entity.Property(e => e.Mobile1s).HasMaxLength(100);

                entity.Property(e => e.Mobiles).HasMaxLength(100);

                entity.Property(e => e.Mst)
                    .HasColumnName("MST")
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Name1).HasMaxLength(50);

                entity.Property(e => e.NameCp)
                    .HasColumnName("NameCP")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<TblOrderDetail>(entity =>
            {
                entity.ToTable("tblOrderDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdOrder).HasColumnName("idOrder");

                entity.Property(e => e.IdProduct).HasColumnName("idProduct");

                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<TblPartner>(entity =>
            {
                entity.ToTable("tblPartner");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Hotline).HasMaxLength(50);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.ToTable("tblProduct");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Access).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.IdCate).HasColumnName("idCate");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdVideo).HasColumnName("idVideo");

                entity.Property(e => e.ImageLinkDetail).HasMaxLength(200);

                entity.Property(e => e.ImageLinkThumb).HasMaxLength(200);

                entity.Property(e => e.ImageSale).HasMaxLength(200);

                entity.Property(e => e.Info).HasColumnType("ntext");

                entity.Property(e => e.Keyword).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NoteInfo).HasMaxLength(500);

                entity.Property(e => e.Parameter).HasColumnType("ntext");

                entity.Property(e => e.PriceNote).HasMaxLength(100);

                entity.Property(e => e.Sale).HasColumnType("ntext");

                entity.Property(e => e.Size).HasMaxLength(150);

                entity.Property(e => e.Tabs).HasMaxLength(500);

                entity.Property(e => e.Tag).HasMaxLength(500);

                entity.Property(e => e.Time).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.Video).HasMaxLength(50);

                entity.Property(e => e.Warranty).HasMaxLength(50);
            });

            modelBuilder.Entity<TblProductTag>(entity =>
            {
                entity.ToTable("tblProductTag");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Idp).HasColumnName("idp");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Tag).HasMaxLength(200);
            });

            modelBuilder.Entity<TblRegister>(entity =>
            {
                entity.ToTable("tblRegister");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DateTimebyy).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameMachine).HasMaxLength(200);
            });

            modelBuilder.Entity<TblRight>(entity =>
            {
                entity.ToTable("tblRight");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdModule).HasColumnName("idModule");

                entity.Property(e => e.IdUser).HasColumnName("idUser");
            });

            modelBuilder.Entity<TblSupport>(entity =>
            {
                entity.ToTable("tblSupport");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Images).HasMaxLength(300);

                entity.Property(e => e.Mission).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Skyper).HasMaxLength(50);

                entity.Property(e => e.Yahoo).HasMaxLength(50);
            });

            modelBuilder.Entity<TblTags>(entity =>
            {
                entity.ToTable("tblTags");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Keyword).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Tag).HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<TblUrl>(entity =>
            {
                entity.ToTable("tblUrl");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Rel).HasColumnType("nchar(10)");

                entity.Property(e => e.Url).HasMaxLength(200);
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("tblUser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.IdModule).HasColumnName("idModule");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(200);
            });

            modelBuilder.Entity<TblVideo>(entity =>
            {
                entity.ToTable("tblVideo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code).HasColumnType("ntext");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TblWeb>(entity =>
            {
                entity.ToTable("tblWeb");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Url).HasMaxLength(200);
            });
        }
    }
}
