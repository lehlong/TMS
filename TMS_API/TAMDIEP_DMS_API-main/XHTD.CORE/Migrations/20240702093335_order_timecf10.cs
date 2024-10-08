using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XHTD.CORE.Migrations
{
    /// <inheritdoc />
    public partial class order_timecf10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "__MigrationHistory",
            //    columns: table => new
            //    {
            //        MigrationId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            //        ContextKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
            //        Model = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
            //        ProductVersion = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_dbo.__MigrationHistory", x => new { x.MigrationId, x.ContextKey });
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ItemFormulas",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Cement = table.Column<double>(type: "float", nullable: true),
            //        Stone = table.Column<double>(type: "float", nullable: true),
            //        Sand = table.Column<double>(type: "float", nullable: true),
            //        Admixture = table.Column<double>(type: "float", nullable: true),
            //        Water = table.Column<double>(type: "float", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_dbo.ItemFormulas", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblAccount",
            //    columns: table => new
            //    {
            //        UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        PassWord = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        GroupId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        State = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
            //        DeviceId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        DeviceIdDayUpdate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblAccount", x => x.UserName);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblAccountGroup",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        State = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblAccountGroup", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblAccountGroupFunction",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        GroupId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        FunctionId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        F_Add = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        F_Edit = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        F_Del = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        F_View = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        F_Print = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        F_Other = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblAccountGroupFunction", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblCallToTrough",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        OrderId = table.Column<int>(type: "int", nullable: false),
            //        DeliveryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Vehicle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        SumNumber = table.Column<decimal>(type: "decimal(18,1)", nullable: true),
            //        Machine = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        IndexTrough = table.Column<int>(type: "int", nullable: true),
            //        CountTry = table.Column<int>(type: "int", nullable: false),
            //        CountReindex = table.Column<int>(type: "int", nullable: false),
            //        IsDone = table.Column<bool>(type: "bit", nullable: true),
            //        CallLog = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblCallToTrough", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblCategories",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        State = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
            //        ShowIndex = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblCategories", x => x.Code);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblCategoriesDevices",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Mã thiết bị"),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CatCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Mã hạng mục"),
            //        ManCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Mã thiết bị quản lý"),
            //        Name = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
            //        IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        PortNumber = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
            //        PortNumberDeviceIn = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
            //        PortNumberDeviceOut = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
            //        PortNumberDeviceIn1 = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
            //        PortNumberDeviceOut1 = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
            //        PortNumberDeviceIn2 = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
            //        PortNumberDeviceOut2 = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
            //        Descriptioon = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        State = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
            //        ShowIndex = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblCategoriesDevices", x => x.Code);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblCategoriesDevicesLog",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        ActionType = table.Column<int>(type: "int", nullable: true, defaultValue: 0, comment: "1: Mở; 2: Đóng; 3: Khác"),
            //        ActionInfo = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
            //        ActionDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblCategoriesDevicesLog", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblCompany",
            //    columns: table => new
            //    {
            //        ComId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        ComManager = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
            //        ComName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
            //        ComAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
            //        ComPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        ComFax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        ComEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        ComTax = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        ComWebsite = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblConfigApp",
            //    columns: table => new
            //    {
            //        KeyItem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ValueItem = table.Column<string>(type: "ntext", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblConfigApp", x => x.KeyItem);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblDevice",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CodeParent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        OperID = table.Column<int>(type: "int", nullable: true),
            //        DoorOrAuxoutID = table.Column<int>(type: "int", nullable: true),
            //        OutputAddrType = table.Column<int>(type: "int", nullable: true),
            //        DoorAction = table.Column<int>(type: "int", nullable: true),
            //        InputPort = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
            //        OutputPort = table.Column<int>(type: "int", nullable: true, defaultValue: -1),
            //        Ipaddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
            //        Port = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblDeviceInfo_1", x => x.Code);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblDeviceGroup",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TypeId = table.Column<int>(type: "int", nullable: true, defaultValue: 0, comment: "0: Chưa xác định; 1: Thiết bị; 2: Server"),
            //        Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        PortNumber = table.Column<int>(type: "int", nullable: true),
            //        State = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblDevice", x => x.Code);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblDeviceOperating",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        GroupDevice = table.Column<int>(type: "int", nullable: true),
            //        GroupDeviceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        GroupDeviceName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        PortNumber = table.Column<int>(type: "int", nullable: true),
            //        State = table.Column<bool>(type: "bit", nullable: true),
            //        DayCreate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        LogHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Flag = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblDeviceOperating", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblDriver",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Birthday = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Nam"),
            //        IdCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        IdCardImgFront = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        IdCardImgBack = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        State = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblDriver", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblDriverVehicle",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Vehicle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblDriverVehicle", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblFunction",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        GroupName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        GroupIndex = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        ItemIndex = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblFunction", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblLongVehicle",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Vehicle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblLongVehicle", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblMachine",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Code = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        State = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblMachine", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblMachineTypeProduct",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        MachineCode = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
            //        TypeProduct = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblMachineTypeProduct", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblNotification",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserNameSender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UserNameReceiver = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        ContentMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        DayCreate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        IsView = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        TimeView = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblNotification", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblPrintConfig",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false),
            //        KeyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        ValueName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblPrintConfig", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblRfid",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Vehicle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        DayReleased = table.Column<DateTime>(type: "datetime", nullable: true),
            //        DayExpired = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        State = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
            //        LastEnter = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RFIDs", x => x.Code);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblRfidSign",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Vehicle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        RfidCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        Image = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblRFIDSign", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblScaleOperating",
            //    columns: table => new
            //    {
            //        ScaleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ScaleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        IsScaling = table.Column<bool>(type: "bit", nullable: true),
            //        Vehicle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        CardNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        DeliveryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        ScaleIn = table.Column<bool>(type: "bit", nullable: true),
            //        ScaleOut = table.Column<bool>(type: "bit", nullable: true),
            //        TimeIn = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblScaleOperating", x => x.ScaleCode);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblStoreOrderLocation",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        OrderId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        DeliveryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Vehicle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        DriverUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        DriverName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true, defaultValue: 0m),
            //        Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true, defaultValue: 0m),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblStoreOrderLocation", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblStoreOrderOperating",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Vehicle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        DriverName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        NameDistributor = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        ItemId = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
            //        NameProduct = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        CatId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        SumNumber = table.Column<decimal>(type: "decimal(18,1)", nullable: true),
            //        TimeIn33 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CardNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        OrderId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        DeliveryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        DeliveryCodeParent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        TypeProduct = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        TypeXK = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        TimeIn21 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        TimeIn22 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm1 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        TimeConfirm1 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm2 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        TimeConfirm2 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm3 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        TimeConfirm3 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm4 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        TimeConfirm4 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm5 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        TimeConfirm5 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm6 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        TimeConfirm6 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm7 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        TimeConfirm7 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm8 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        TimeConfirm8 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm9 = table.Column<int>(type: "int", nullable: true),
            //        TimeConfirm9 = table.Column<DateTime>(type: "datetime", nullable: true),
            //        Confirm9Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Confirm9Image = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
            //        Step = table.Column<int>(type: "int", nullable: true, defaultValue: 0, comment: "0: Chưa nhận đơn \r\n1: Đã nhận đơn \r\n2: Đã vào cổng \r\n3: Đã cân vào \r\n4: Đang gọi xe \r\n5: Đang lấy hàng \r\n6: Đã lấy hàng \r\n7: Đã cân ra\r\n8: Đã hoàn thành \r\n9: Đã giao hàng\r\n"),
            //        IndexOrder = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        IndexOrder1 = table.Column<int>(type: "int", nullable: true, defaultValue: 1000),
            //        IndexOrder2 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        Trough = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        Trough1 = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        NumberVoice = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Prioritize = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        DayCreate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        IDDistributorSyn = table.Column<int>(type: "int", nullable: true),
            //        AreaId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        AreaName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        CodeStore = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        NameStore = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        DriverUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        DriverAccept = table.Column<DateTime>(type: "datetime", nullable: true),
            //        IndexOrderTemp = table.Column<int>(type: "int", nullable: true),
            //        WeightIn = table.Column<int>(type: "int", nullable: true),
            //        WeightInTime = table.Column<DateTime>(type: "datetime", nullable: true),
            //        WeightOut = table.Column<int>(type: "int", nullable: true),
            //        WeightOutTime = table.Column<DateTime>(type: "datetime", nullable: true),
            //        WeightInAuto = table.Column<int>(type: "int", nullable: true),
            //        WeightInTimeAuto = table.Column<DateTime>(type: "datetime", nullable: true),
            //        WeightOutAuto = table.Column<int>(type: "int", nullable: true),
            //        WeightOutTimeAuto = table.Column<DateTime>(type: "datetime", nullable: true),
            //        NoteFinish = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Longitude = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Latitude = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        CountReindex = table.Column<int>(type: "int", nullable: true),
            //        IsVoiced = table.Column<bool>(type: "bit", nullable: true),
            //        LocationCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        TransportMethodId = table.Column<int>(type: "int", nullable: true),
            //        TransportMethodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        LockInDbet = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        LogJobAttach = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsSyncedByNewWS = table.Column<bool>(type: "bit", nullable: true),
            //        TroughLineCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        IsScaleAuto = table.Column<bool>(type: "bit", nullable: true),
            //        TimeConfirmHistory = table.Column<DateTime>(type: "datetime", nullable: true),
            //        LogHistory = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        MoocCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        LogProcessOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DriverPrintNumber = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        DriverPrintTime = table.Column<DateTime>(type: "datetime", nullable: true),
            //        WarningNotCall = table.Column<bool>(type: "bit", nullable: true),
            //        XiRoiAttatchmentFile = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        PackageNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        Shifts = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        AutoScaleOut = table.Column<bool>(type: "bit", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblStoreOrderOperating", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblStoreOrderOperatingPriority",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false),
            //        TypeProduct = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Priority = table.Column<int>(type: "int", nullable: false),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblStoreOrderOperatingPriority", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblStoreOrderOperatingVoice",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        OrderId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        DeliveryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Step = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        VoiceText = table.Column<string>(type: "nvarchar(3500)", maxLength: 3500, nullable: true),
            //        IndexNumber = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblSystemParameter",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
            //        Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblSystemParametes", x => x.Code);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblTrough",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        Machine = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
            //        Height = table.Column<double>(type: "float", nullable: true),
            //        Width = table.Column<double>(type: "float", nullable: true),
            //        Long = table.Column<double>(type: "float", nullable: true),
            //        Working = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        Problem = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
            //        State = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
            //        DeliveryCodeCurrent = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        PlanQuantityCurrent = table.Column<double>(type: "float", nullable: true),
            //        CountQuantityCurrent = table.Column<double>(type: "float", nullable: true),
            //        IsPicking = table.Column<bool>(type: "bit", nullable: true),
            //        TransportNameCurrent = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        CheckInTimeCurrent = table.Column<DateTime>(type: "datetime", nullable: true),
            //        IsInviting = table.Column<bool>(type: "bit", nullable: true),
            //        LineCode = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblTrough", x => x.Code);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblTroughTypeProduct",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TroughCode = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
            //        TypeProduct = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblTroughTypeProduct", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblTypeProduct",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        State = table.Column<bool>(type: "bit", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblTypeProduct", x => x.Code);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblVehicle",
            //    columns: table => new
            //    {
            //        IDVehicle = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Vehicle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Tonnage = table.Column<double>(type: "float", nullable: true),
            //        TonnageDefault = table.Column<double>(type: "float", nullable: true),
            //        NameDriver = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        IdCardNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        HeightVehicle = table.Column<double>(type: "float", nullable: true),
            //        WidthVehicle = table.Column<double>(type: "float", nullable: true),
            //        LongVehicle = table.Column<double>(type: "float", nullable: true),
            //        UnladenWeight1 = table.Column<int>(type: "int", nullable: true),
            //        UnladenWeight1LastUpdate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UnladenWeight2 = table.Column<int>(type: "int", nullable: true),
            //        UnladenWeight2LastUpdate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UnladenWeight3 = table.Column<int>(type: "int", nullable: true),
            //        UnladenWeight3LastUpdate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        IsSetMediumUnladenWeight = table.Column<bool>(type: "bit", nullable: true),
            //        CreateDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
            //        CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        UpdateDay = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblVehicle", x => x.IDVehicle);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Items",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Unit_Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Unit_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Type_Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Type_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UnitCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TypeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CostPrice = table.Column<double>(type: "float", nullable: true),
            //        SellPrice = table.Column<double>(type: "float", nullable: true),
            //        Proportion = table.Column<double>(type: "float", nullable: true),
            //        PercentageOfImpurities = table.Column<double>(type: "float", nullable: true),
            //        IsMainObject = table.Column<bool>(type: "bit", nullable: true),
            //        IsQuantitative = table.Column<bool>(type: "bit", nullable: true),
            //        Cement = table.Column<double>(type: "float", nullable: true),
            //        Stone = table.Column<double>(type: "float", nullable: true),
            //        Sand = table.Column<double>(type: "float", nullable: true),
            //        Admixture = table.Column<double>(type: "float", nullable: true),
            //        Water = table.Column<double>(type: "float", nullable: true),
            //        ItemFormula_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_dbo.Items", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_dbo.Items_dbo.ItemFormulas_ItemFormula_Id",
            //            column: x => x.ItemFormula_Id,
            //            principalTable: "ItemFormulas",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItemFormula_Id",
            //    table: "Items",
            //    column: "ItemFormula_Id");

            //migrationBuilder.CreateIndex(
            //    name: "OrderId_Unique",
            //    table: "tblStoreOrderOperating",
            //    column: "OrderId",
            //    unique: true,
            //    filter: "[OrderId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "__MigrationHistory");

            //migrationBuilder.DropTable(
            //    name: "Items");

            //migrationBuilder.DropTable(
            //    name: "tblAccount");

            //migrationBuilder.DropTable(
            //    name: "tblAccountGroup");

            //migrationBuilder.DropTable(
            //    name: "tblAccountGroupFunction");

            //migrationBuilder.DropTable(
            //    name: "tblCallToTrough");

            //migrationBuilder.DropTable(
            //    name: "tblCategories");

            //migrationBuilder.DropTable(
            //    name: "tblCategoriesDevices");

            //migrationBuilder.DropTable(
            //    name: "tblCategoriesDevicesLog");

            //migrationBuilder.DropTable(
            //    name: "tblCompany");

            //migrationBuilder.DropTable(
            //    name: "tblConfigApp");

            //migrationBuilder.DropTable(
            //    name: "tblDevice");

            //migrationBuilder.DropTable(
            //    name: "tblDeviceGroup");

            //migrationBuilder.DropTable(
            //    name: "tblDeviceOperating");

            //migrationBuilder.DropTable(
            //    name: "tblDriver");

            //migrationBuilder.DropTable(
            //    name: "tblDriverVehicle");

            //migrationBuilder.DropTable(
            //    name: "tblFunction");

            //migrationBuilder.DropTable(
            //    name: "tblLongVehicle");

            //migrationBuilder.DropTable(
            //    name: "tblMachine");

            //migrationBuilder.DropTable(
            //    name: "tblMachineTypeProduct");

            //migrationBuilder.DropTable(
            //    name: "tblNotification");

            //migrationBuilder.DropTable(
            //    name: "tblPrintConfig");

            //migrationBuilder.DropTable(
            //    name: "tblRfid");

            //migrationBuilder.DropTable(
            //    name: "tblRfidSign");

            //migrationBuilder.DropTable(
            //    name: "tblScaleOperating");

            //migrationBuilder.DropTable(
            //    name: "tblStoreOrderLocation");

            //migrationBuilder.DropTable(
            //    name: "tblStoreOrderOperating");

            //migrationBuilder.DropTable(
            //    name: "tblStoreOrderOperatingPriority");

            //migrationBuilder.DropTable(
            //    name: "tblStoreOrderOperatingVoice");

            //migrationBuilder.DropTable(
            //    name: "tblSystemParameter");

            //migrationBuilder.DropTable(
            //    name: "tblTrough");

            //migrationBuilder.DropTable(
            //    name: "tblTroughTypeProduct");

            //migrationBuilder.DropTable(
            //    name: "tblTypeProduct");

            //migrationBuilder.DropTable(
            //    name: "tblVehicle");

            //migrationBuilder.DropTable(
            //    name: "ItemFormulas");
        }
    }
}
