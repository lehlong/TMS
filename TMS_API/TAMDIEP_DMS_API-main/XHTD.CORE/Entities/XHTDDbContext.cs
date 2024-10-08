using Microsoft.EntityFrameworkCore;

namespace XHTD.CORE.Entities;

public partial class XHTDDbContext : DbContext
{
    public XHTDDbContext(DbContextOptions<XHTDDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemFormula> ItemFormulas { get; set; }

    public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }

    public virtual DbSet<TblAccount> TblAccounts { get; set; }

    public virtual DbSet<TblAccountGroup> TblAccountGroups { get; set; }

    public virtual DbSet<TblAccountGroupFunction> TblAccountGroupFunctions { get; set; }

    public virtual DbSet<TblCallToTrough> TblCallToTroughs { get; set; }

    public virtual DbSet<TblCategoriesDevice> TblCategoriesDevices { get; set; }

    public virtual DbSet<TblCategoriesDevicesLog> TblCategoriesDevicesLogs { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblCompany> TblCompanies { get; set; }

    public virtual DbSet<TblConfigApp> TblConfigApps { get; set; }

    public virtual DbSet<TblDevice> TblDevices { get; set; }

    public virtual DbSet<TblDeviceGroup> TblDeviceGroups { get; set; }

    public virtual DbSet<TblDeviceOperating> TblDeviceOperatings { get; set; }

    public virtual DbSet<TblDriver> TblDrivers { get; set; }

    public virtual DbSet<TblDriverVehicle> TblDriverVehicles { get; set; }

    public virtual DbSet<TblFunction> TblFunctions { get; set; }

    public virtual DbSet<TblLongVehicle> TblLongVehicles { get; set; }

    public virtual DbSet<TblMachine> TblMachines { get; set; }

    public virtual DbSet<TblMachineTypeProduct> TblMachineTypeProducts { get; set; }

    public virtual DbSet<TblNotification> TblNotifications { get; set; }

    public virtual DbSet<TblPrintConfig> TblPrintConfigs { get; set; }

    public virtual DbSet<TblRfid> TblRfids { get; set; }

    public virtual DbSet<TblRfidSign> TblRfidSigns { get; set; }

    public virtual DbSet<TblScaleOperating> TblScaleOperatings { get; set; }

    public virtual DbSet<TblStoreOrderLocation> TblStoreOrderLocations { get; set; }

    public virtual DbSet<TblStoreOrderOperating> TblStoreOrderOperatings { get; set; }

    public virtual DbSet<TblStoreOrderOperatingPriority> TblStoreOrderOperatingPriorities { get; set; }

    public virtual DbSet<TblStoreOrderOperatingVoice> TblStoreOrderOperatingVoices { get; set; }

    public virtual DbSet<TblSystemParameter> TblSystemParameters { get; set; }

    public virtual DbSet<TblTrough> TblTroughs { get; set; }

    public virtual DbSet<TblTroughTypeProduct> TblTroughTypeProducts { get; set; }

    public virtual DbSet<TblTypeProduct> TblTypeProducts { get; set; }

    public virtual DbSet<TblVehicle> TblVehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Items");

            entity.HasIndex(e => e.ItemFormulaId, "IX_ItemFormula_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ItemFormulaId).HasColumnName("ItemFormula_Id");
            entity.Property(e => e.TypeCode).HasColumnName("Type_Code");
            entity.Property(e => e.TypeCode1).HasColumnName("TypeCode");
            entity.Property(e => e.TypeName).HasColumnName("Type_Name");
            entity.Property(e => e.UnitCode).HasColumnName("Unit_Code");
            entity.Property(e => e.UnitCode1).HasColumnName("UnitCode");
            entity.Property(e => e.UnitName).HasColumnName("Unit_Name");

            entity.HasOne(d => d.ItemFormula).WithMany(p => p.Items)
                .HasForeignKey(d => d.ItemFormulaId)
                .HasConstraintName("FK_dbo.Items_dbo.ItemFormulas_ItemFormula_Id");
        });

        modelBuilder.Entity<ItemFormula>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ItemFormulas");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MigrationHistory>(entity =>
        {
            entity.HasKey(e => new { e.MigrationId, e.ContextKey }).HasName("PK_dbo.__MigrationHistory");

            entity.ToTable("__MigrationHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ContextKey).HasMaxLength(300);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<TblAccount>(entity =>
        {
            entity.HasKey(e => e.UserName);

            entity.ToTable("tblAccount");

            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeviceId).HasMaxLength(250);
            entity.Property(e => e.DeviceIdDayUpdate).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.GroupId).HasDefaultValue(0);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.PassWord).HasMaxLength(50);
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblAccountGroup>(entity =>
        {
            entity.ToTable("tblAccountGroup");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblAccountGroupFunction>(entity =>
        {
            entity.ToTable("tblAccountGroupFunction");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FAdd)
                .HasDefaultValue(false)
                .HasColumnName("F_Add");
            entity.Property(e => e.FDel)
                .HasDefaultValue(false)
                .HasColumnName("F_Del");
            entity.Property(e => e.FEdit)
                .HasDefaultValue(false)
                .HasColumnName("F_Edit");
            entity.Property(e => e.FOther)
                .HasDefaultValue(false)
                .HasColumnName("F_Other");
            entity.Property(e => e.FPrint)
                .HasDefaultValue(false)
                .HasColumnName("F_Print");
            entity.Property(e => e.FView)
                .HasDefaultValue(false)
                .HasColumnName("F_View");
            entity.Property(e => e.FunctionId).HasDefaultValue(0);
            entity.Property(e => e.GroupId).HasDefaultValue(0);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblCallToTrough>(entity =>
        {
            entity.ToTable("tblCallToTrough");

            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.DeliveryCode).HasMaxLength(50);
            entity.Property(e => e.Machine).HasMaxLength(50);
            entity.Property(e => e.SumNumber).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.Vehicle).HasMaxLength(50);
        });

        modelBuilder.Entity<TblCategoriesDevice>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("tblCategoriesDevices");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasComment("Mã thiết bị");
            entity.Property(e => e.CatCode)
                .HasMaxLength(50)
                .HasComment("Mã hạng mục");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Descriptioon).HasMaxLength(500);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.ManCode)
                .HasMaxLength(50)
                .HasComment("Mã thiết bị quản lý");
            entity.Property(e => e.Name).HasMaxLength(350);
            entity.Property(e => e.PortNumber).HasDefaultValue(-1);
            entity.Property(e => e.PortNumberDeviceIn).HasDefaultValue(-1);
            entity.Property(e => e.PortNumberDeviceIn1).HasDefaultValue(-1);
            entity.Property(e => e.PortNumberDeviceIn2).HasDefaultValue(-1);
            entity.Property(e => e.PortNumberDeviceOut).HasDefaultValue(-1);
            entity.Property(e => e.PortNumberDeviceOut1).HasDefaultValue(-1);
            entity.Property(e => e.PortNumberDeviceOut2).HasDefaultValue(-1);
            entity.Property(e => e.ShowIndex).HasDefaultValue(0);
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblCategoriesDevicesLog>(entity =>
        {
            entity.ToTable("tblCategoriesDevicesLog");

            entity.Property(e => e.ActionDate).HasColumnType("datetime");
            entity.Property(e => e.ActionInfo).HasMaxLength(350);
            entity.Property(e => e.ActionType)
                .HasDefaultValue(0)
                .HasComment("1: Mở; 2: Đóng; 3: Khác");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("tblCategories");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.ShowIndex).HasDefaultValue(0);
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblCompany>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblCompany");

            entity.Property(e => e.ComAddress).HasMaxLength(300);
            entity.Property(e => e.ComEmail).HasMaxLength(50);
            entity.Property(e => e.ComFax).HasMaxLength(50);
            entity.Property(e => e.ComId).HasMaxLength(50);
            entity.Property(e => e.ComManager).HasMaxLength(200);
            entity.Property(e => e.ComName).HasMaxLength(200);
            entity.Property(e => e.ComPhone).HasMaxLength(50);
            entity.Property(e => e.ComTax).HasMaxLength(100);
            entity.Property(e => e.ComWebsite).HasMaxLength(500);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblConfigApp>(entity =>
        {
            entity.HasKey(e => e.KeyItem);

            entity.ToTable("tblConfigApp");

            entity.Property(e => e.KeyItem).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.ValueItem).HasColumnType("ntext");
        });

        modelBuilder.Entity<TblDevice>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK_tblDeviceInfo_1");

            entity.ToTable("tblDevice");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CodeParent).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DoorOrAuxoutId).HasColumnName("DoorOrAuxoutID");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.InputPort).HasDefaultValue(-1);
            entity.Property(e => e.Ipaddress).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.OperId).HasColumnName("OperID");
            entity.Property(e => e.OutputPort).HasDefaultValue(-1);
            entity.Property(e => e.Port).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblDeviceGroup>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK_tblDevice");

            entity.ToTable("tblDeviceGroup");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.TypeId)
                .HasDefaultValue(0)
                .HasComment("0: Chưa xác định; 1: Thiết bị; 2: Server");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblDeviceOperating>(entity =>
        {
            entity.ToTable("tblDeviceOperating");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.DayCreate).HasColumnType("datetime");
            entity.Property(e => e.GroupDeviceCode).HasMaxLength(50);
            entity.Property(e => e.GroupDeviceName).HasMaxLength(250);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<TblDriver>(entity =>
        {
            entity.ToTable("tblDriver");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasDefaultValue("Nam");
            entity.Property(e => e.IdCard).HasMaxLength(50);
            entity.Property(e => e.IdCardImgBack).HasMaxLength(500);
            entity.Property(e => e.IdCardImgFront).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblDriverVehicle>(entity =>
        {
            entity.ToTable("tblDriverVehicle");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.Vehicle).HasMaxLength(50);
        });

        modelBuilder.Entity<TblFunction>(entity =>
        {
            entity.ToTable("tblFunction");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupCode).HasMaxLength(50);
            entity.Property(e => e.GroupIndex).HasDefaultValue(0);
            entity.Property(e => e.GroupName).HasMaxLength(250);
            entity.Property(e => e.ItemIndex).HasDefaultValue(0);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblLongVehicle>(entity =>
        {
            entity.ToTable("tblLongVehicle");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.Vehicle).HasMaxLength(50);
        });

        modelBuilder.Entity<TblMachine>(entity =>
        {
            entity.ToTable("tblMachine");

            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblMachineTypeProduct>(entity =>
        {
            entity.ToTable("tblMachineTypeProduct");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.MachineCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TypeProduct).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblNotification>(entity =>
        {
            entity.ToTable("tblNotification");

            entity.Property(e => e.ContentMessage).HasMaxLength(500);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.DayCreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsView).HasDefaultValue(false);
            entity.Property(e => e.TimeView).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.UserNameReceiver).HasMaxLength(50);
            entity.Property(e => e.UserNameSender).HasMaxLength(50);
        });

        modelBuilder.Entity<TblPrintConfig>(entity =>
        {
            entity.ToTable("tblPrintConfig");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.KeyName).HasMaxLength(50);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.ValueName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblRfid>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK_RFIDs");

            entity.ToTable("tblRfid");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DayExpired).HasColumnType("datetime");
            entity.Property(e => e.DayReleased).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LastEnter).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(250);
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Vehicle).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRfidSign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblRFIDSign");

            entity.ToTable("tblRfidSign");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RfidCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.Vehicle)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblScaleOperating>(entity =>
        {
            entity.HasKey(e => e.ScaleCode);

            entity.ToTable("tblScaleOperating");

            entity.Property(e => e.ScaleCode).HasMaxLength(50);
            entity.Property(e => e.CardNo).HasMaxLength(50);
            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.DeliveryCode).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ScaleName).HasMaxLength(50);
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.Vehicle).HasMaxLength(50);
        });

        modelBuilder.Entity<TblStoreOrderLocation>(entity =>
        {
            entity.ToTable("tblStoreOrderLocation");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeliveryCode).HasMaxLength(50);
            entity.Property(e => e.DriverName).HasMaxLength(250);
            entity.Property(e => e.DriverUserName).HasMaxLength(50);
            entity.Property(e => e.Latitude)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 7)");
            entity.Property(e => e.Longitude)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 7)");
            entity.Property(e => e.OrderId).HasDefaultValue(0);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.Vehicle).HasMaxLength(50);
        });

        modelBuilder.Entity<TblStoreOrderOperating>(entity =>
        {
            entity.ToTable("tblStoreOrderOperating");

            entity.HasIndex(e => e.OrderId, "OrderId_Unique").IsUnique();

            entity.Property(e => e.AreaId).HasDefaultValue(0);
            entity.Property(e => e.AreaName).HasMaxLength(250);
            entity.Property(e => e.CardNo).HasMaxLength(50);
            entity.Property(e => e.CatId).HasMaxLength(50);
            entity.Property(e => e.CodeStore).HasMaxLength(50);
            entity.Property(e => e.Confirm1).HasDefaultValue(0);
            entity.Property(e => e.Confirm2).HasDefaultValue(0);
            entity.Property(e => e.Confirm3).HasDefaultValue(0);
            entity.Property(e => e.Confirm4).HasDefaultValue(0);
            entity.Property(e => e.Confirm5).HasDefaultValue(0);
            entity.Property(e => e.Confirm6).HasDefaultValue(0);
            entity.Property(e => e.Confirm7).HasDefaultValue(0);
            entity.Property(e => e.Confirm8).HasDefaultValue(0);
            entity.Property(e => e.Confirm9Image).HasMaxLength(300);
            entity.Property(e => e.Confirm9Note).HasMaxLength(500);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DayCreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeliveryCode).HasMaxLength(50);
            entity.Property(e => e.DeliveryCodeParent).HasMaxLength(50);
            entity.Property(e => e.DriverAccept).HasColumnType("datetime");
            entity.Property(e => e.DriverName).HasMaxLength(250);
            entity.Property(e => e.DriverPrintNumber).HasDefaultValue(0);
            entity.Property(e => e.DriverPrintTime).HasColumnType("datetime");
            entity.Property(e => e.DriverUserName).HasMaxLength(50);
            entity.Property(e => e.IddistributorSyn).HasColumnName("IDDistributorSyn");
            entity.Property(e => e.IndexOrder).HasDefaultValue(0);
            entity.Property(e => e.IndexOrder1).HasDefaultValue(1000);
            entity.Property(e => e.IndexOrder2).HasDefaultValue(0);
            entity.Property(e => e.IsSyncedByNewWs).HasColumnName("IsSyncedByNewWS");
            entity.Property(e => e.ItemId).HasDefaultValue(0.0);
            entity.Property(e => e.Latitude).HasMaxLength(50);
            entity.Property(e => e.LocationCode).HasMaxLength(50);
            entity.Property(e => e.LockInDbet).HasDefaultValue(false);
            entity.Property(e => e.LogHistory).HasMaxLength(500);
            entity.Property(e => e.Longitude).HasMaxLength(50);
            entity.Property(e => e.MoocCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameDistributor).HasMaxLength(500);
            entity.Property(e => e.NameProduct).HasMaxLength(250);
            entity.Property(e => e.NameStore).HasMaxLength(500);
            entity.Property(e => e.NoteFinish).HasMaxLength(500);
            entity.Property(e => e.NumberVoice).HasDefaultValue(0);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasDefaultValue(0);
            entity.Property(e => e.PackageNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Prioritize).HasDefaultValue(0);
            entity.Property(e => e.Shifts).HasDefaultValue(0);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.Step)
                .HasDefaultValue(0)
                .HasComment("0: Chưa nhận đơn \r\n1: Đã nhận đơn \r\n2: Đã vào cổng \r\n3: Đã cân vào \r\n4: Đang gọi xe \r\n5: Đang lấy hàng \r\n6: Đã lấy hàng \r\n7: Đã cân ra\r\n8: Đã hoàn thành \r\n9: Đã giao hàng\r\n");
            entity.Property(e => e.SumNumber).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.TimeConfirm1).HasColumnType("datetime");
            entity.Property(e => e.TimeConfirm2).HasColumnType("datetime");
            entity.Property(e => e.TimeConfirm3).HasColumnType("datetime");
            entity.Property(e => e.TimeConfirm4).HasColumnType("datetime");
            entity.Property(e => e.TimeConfirm5).HasColumnType("datetime");
            entity.Property(e => e.TimeConfirm6).HasColumnType("datetime");
            entity.Property(e => e.TimeConfirm7).HasColumnType("datetime");
            entity.Property(e => e.TimeConfirm8).HasColumnType("datetime");
            entity.Property(e => e.TimeConfirm9).HasColumnType("datetime");
            entity.Property(e => e.TimeConfirmHistory).HasColumnType("datetime");
            entity.Property(e => e.TimeIn21).HasColumnType("datetime");
            entity.Property(e => e.TimeIn22).HasColumnType("datetime");
            entity.Property(e => e.TimeIn33).HasColumnType("datetime");
            entity.Property(e => e.TransportMethodName).HasMaxLength(50);
            entity.Property(e => e.Trough).HasDefaultValue(0);
            entity.Property(e => e.Trough1).HasDefaultValue(0);
            entity.Property(e => e.TroughLineCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypeProduct).HasMaxLength(50);
            entity.Property(e => e.TypeXk)
                .HasMaxLength(50)
                .HasColumnName("TypeXK");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.Vehicle).HasMaxLength(50);
            entity.Property(e => e.WeightInTime).HasColumnType("datetime");
            entity.Property(e => e.WeightInTimeAuto).HasColumnType("datetime");
            entity.Property(e => e.WeightOutTime).HasColumnType("datetime");
            entity.Property(e => e.WeightOutTimeAuto).HasColumnType("datetime");
            entity.Property(e => e.XiRoiAttatchmentFile).HasMaxLength(250);
        });

        modelBuilder.Entity<TblStoreOrderOperatingPriority>(entity =>
        {
            entity.ToTable("tblStoreOrderOperatingPriority");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TypeProduct).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblStoreOrderOperatingVoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblStoreOrderOperatingVoice");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeliveryCode).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IndexNumber).HasDefaultValue(0);
            entity.Property(e => e.OrderId).HasDefaultValue(0);
            entity.Property(e => e.Step).HasDefaultValue(0);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.VoiceText).HasMaxLength(3500);
        });

        modelBuilder.Entity<TblSystemParameter>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK_tblSystemParametes");

            entity.ToTable("tblSystemParameter");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(350);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.Value).HasMaxLength(250);
        });

        modelBuilder.Entity<TblTrough>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("tblTrough");

            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CheckInTimeCurrent).HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeliveryCodeCurrent)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LineCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Machine)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Problem).HasDefaultValue(false);
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.TransportNameCurrent)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.Working).HasDefaultValue(false);
        });

        modelBuilder.Entity<TblTroughTypeProduct>(entity =>
        {
            entity.ToTable("tblTroughTypeProduct");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TroughCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TypeProduct).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblTypeProduct>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("tblTypeProduct");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblVehicle>(entity =>
        {
            entity.HasKey(e => e.Idvehicle);

            entity.ToTable("tblVehicle");

            entity.Property(e => e.Idvehicle).HasColumnName("IDVehicle");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdCardNumber).HasMaxLength(50);
            entity.Property(e => e.NameDriver).HasMaxLength(250);
            entity.Property(e => e.UnladenWeight1LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.UnladenWeight2LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.UnladenWeight3LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDay).HasColumnType("datetime");
            entity.Property(e => e.Vehicle).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
