USE [master]
GO
/****** Object:  Database [EmployeeTracker]    Script Date: 07/06/2024 11:32:36 ******/
CREATE DATABASE [EmployeeTracker]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EmployeeTracker', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EmployeeTracker.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EmployeeTracker_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EmployeeTracker_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [EmployeeTracker] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EmployeeTracker].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EmployeeTracker] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EmployeeTracker] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EmployeeTracker] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EmployeeTracker] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EmployeeTracker] SET ARITHABORT OFF 
GO
ALTER DATABASE [EmployeeTracker] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EmployeeTracker] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EmployeeTracker] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EmployeeTracker] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EmployeeTracker] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EmployeeTracker] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EmployeeTracker] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EmployeeTracker] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EmployeeTracker] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EmployeeTracker] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EmployeeTracker] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EmployeeTracker] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EmployeeTracker] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EmployeeTracker] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EmployeeTracker] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EmployeeTracker] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EmployeeTracker] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EmployeeTracker] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EmployeeTracker] SET  MULTI_USER 
GO
ALTER DATABASE [EmployeeTracker] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EmployeeTracker] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EmployeeTracker] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EmployeeTracker] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EmployeeTracker] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EmployeeTracker] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [EmployeeTracker] SET QUERY_STORE = ON
GO
ALTER DATABASE [EmployeeTracker] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [EmployeeTracker]
GO
/****** Object:  User [ELTP-LAP-0185\Coditas-Admin]    Script Date: 07/06/2024 11:32:37 ******/
CREATE USER [ELTP-LAP-0185\Coditas-Admin] FOR LOGIN [ELTP-LAP-0185\Coditas-Admin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [ELTP-LAP-0185\Coditas-Admin]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ELTP-LAP-0185\Coditas-Admin]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ELTP-LAP-0185\Coditas-Admin]
GO
/****** Object:  Table [dbo].[ApplicationMessages]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationMessages](
	[MessageID] [int] NOT NULL,
	[SenderID] [int] NULL,
	[ReceiverID] [int] NULL,
	[MessageContent] [nvarchar](max) NULL,
	[SentTimeDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attendance_Records]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance_Records](
	[RecordID] [int] NOT NULL,
	[EmployeeID] [int] NULL,
	[CheckInTime] [datetime] NULL,
	[CheckOutTime] [datetime] NULL,
	[Date] [date] NULL,
	[Total_Hours] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](255) NOT NULL,
	[DateOfEstablishment] [date] NULL,
	[Industry] [nvarchar](100) NULL,
	[NumberOfEmployees] [int] NULL,
	[Location] [nvarchar](255) NULL,
	[Country] [nvarchar](100) NULL,
	[Website] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[Phone] [nvarchar](20) NULL,
	[Address] [nvarchar](max) NULL,
	[DomainName] [nvarchar](100) NULL,
	[PasswordSalt] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyLeaves]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyLeaves](
	[CompanyID] [int] NULL,
	[LeaveID] [int] NULL,
	[LeaveQuota] [int] NULL,
	[LeaveDescription] [nvarchar](255) NULL,
	[LeaveName] [nvarchar](255) NULL,
	[CompanyLeavesID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyLeavesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CompanyID] ASC,
	[LeaveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailNotification]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailNotification](
	[NotificationID] [int] NOT NULL,
	[LeaveType] [nvarchar](50) NULL,
	[ReferenceID] [int] NULL,
	[Status] [varchar](50) NULL,
	[SentTime] [datetime] NULL,
	[Subject] [nvarchar](255) NULL,
	[Body] [nvarchar](max) NULL,
	[RecipientEmail] [nvarchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeDetails]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeDetails](
	[EmployeeID] [int] NOT NULL,
	[Department] [nvarchar](max) NULL,
	[ManagerID] [int] NOT NULL,
	[ManagerName] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Leave_Application]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leave_Application](
	[LeaveID] [int] NOT NULL,
	[EmployeeID] [int] NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[LeaveTypeID] [int] NULL,
	[ReasonDescription] [nvarchar](max) NOT NULL,
	[ApplicationStatus] [nvarchar](50) NULL,
	[AppliedOn] [datetime] NULL,
	[ManagerID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LeaveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Leave_Balance]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leave_Balance](
	[EmployeeID] [int] NULL,
	[TotalEntitled] [int] NULL,
	[UsedLeave] [int] NULL,
	[RemainingLeaves] [int] NULL,
	[LeaveTypeID] [int] NULL,
	[BalanceID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BalanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Public_Holidays]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Public_Holidays](
	[Date] [date] NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](255) NULL,
	[HolidayID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[HolidayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[RoleDescription] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Types_of_Leaves]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Types_of_Leaves](
	[LeaveTypeID] [int] IDENTITY(1,1) NOT NULL,
	[LeaveName] [nvarchar](125) NOT NULL,
	[isGlobal] [bit] NOT NULL,
	[DefaultDays] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LeaveTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserDetails]    Script Date: 07/06/2024 11:32:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDetails](
	[UserID] [int] IDENTITY(1000,1) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PasswordSalt] [nvarchar](max) NULL,
	[RoleID] [int] NULL,
	[PhoneNumber] [nvarchar](15) NULL,
	[Address] [nvarchar](max) NULL,
	[DateOfBirth] [date] NOT NULL,
	[Gender] [nvarchar](10) NULL,
	[ProfilePicture] [nvarchar](max) NULL,
	[SocialMedias] [nvarchar](255) NULL,
	[LanguagePreference] [nvarchar](50) NULL,
	[LastLoginDate] [datetime2](7) NULL,
	[UserRole] [nvarchar](50) NULL,
	[SecurityQuestion] [nvarchar](max) NULL,
	[SecurityAnswer] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[CompanyName] [nvarchar](max) NULL,
	[isDeleted] [bit] NOT NULL,
	[CompanyID] [int] NULL,
	[EmployeeID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Types_of_Leaves] ADD  CONSTRAINT [DF__Types_of___isGlo__43D61337]  DEFAULT ((1)) FOR [isGlobal]
GO
ALTER TABLE [dbo].[UserDetails] ADD  DEFAULT ((0)) FOR [isDeleted]
GO
ALTER TABLE [dbo].[ApplicationMessages]  WITH CHECK ADD FOREIGN KEY([ReceiverID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[ApplicationMessages]  WITH CHECK ADD FOREIGN KEY([SenderID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[Attendance_Records]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[Attendance_Records]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[CompanyLeaves]  WITH CHECK ADD FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Company] ([CompanyID])
GO
ALTER TABLE [dbo].[EmployeeDetails]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeDetails_Manager] FOREIGN KEY([ManagerID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[EmployeeDetails] CHECK CONSTRAINT [FK_EmployeeDetails_Manager]
GO
ALTER TABLE [dbo].[EmployeeDetails]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeDetails_UserDetails] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[EmployeeDetails] CHECK CONSTRAINT [FK_EmployeeDetails_UserDetails]
GO
ALTER TABLE [dbo].[Leave_Application]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[Leave_Application]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[Leave_Application]  WITH CHECK ADD FOREIGN KEY([ManagerID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[Leave_Application]  WITH CHECK ADD FOREIGN KEY([ManagerID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[Leave_Balance]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[Leave_Balance]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[Leave_Balance]  WITH CHECK ADD FOREIGN KEY([LeaveTypeID])
REFERENCES [dbo].[Types_of_Leaves] ([LeaveTypeID])
GO
ALTER TABLE [dbo].[Leave_Balance]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeID4] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[UserDetails] ([UserID])
GO
ALTER TABLE [dbo].[Leave_Balance] CHECK CONSTRAINT [FK_EmployeeID4]
GO
ALTER TABLE [dbo].[UserDetails]  WITH CHECK ADD FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Company] ([CompanyID])
GO
ALTER TABLE [dbo].[UserDetails]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
USE [master]
GO
ALTER DATABASE [EmployeeTracker] SET  READ_WRITE 
GO
