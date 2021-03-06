USE [master]
GO
/****** Object:  Database [dh_job_summary]    Script Date: 2017/9/25 15:39:07 ******/
CREATE DATABASE [dh_job_summary]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dh_job_summary', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\dh_job_summary.mdf' , SIZE = 4160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'dh_job_summary_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\dh_job_summary_log.ldf' , SIZE = 1040KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [dh_job_summary] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dh_job_summary].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [dh_job_summary] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [dh_job_summary] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [dh_job_summary] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [dh_job_summary] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [dh_job_summary] SET ARITHABORT OFF 
GO
ALTER DATABASE [dh_job_summary] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [dh_job_summary] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [dh_job_summary] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [dh_job_summary] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [dh_job_summary] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [dh_job_summary] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [dh_job_summary] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [dh_job_summary] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [dh_job_summary] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [dh_job_summary] SET  ENABLE_BROKER 
GO
ALTER DATABASE [dh_job_summary] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [dh_job_summary] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [dh_job_summary] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [dh_job_summary] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [dh_job_summary] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [dh_job_summary] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [dh_job_summary] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [dh_job_summary] SET RECOVERY FULL 
GO
ALTER DATABASE [dh_job_summary] SET  MULTI_USER 
GO
ALTER DATABASE [dh_job_summary] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [dh_job_summary] SET DB_CHAINING OFF 
GO
ALTER DATABASE [dh_job_summary] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [dh_job_summary] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'dh_job_summary', N'ON'
GO
USE [dh_job_summary]
GO
/****** Object:  Table [dbo].[identity_info]    Script Date: 2017/9/25 15:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[identity_info](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[mac] [varchar](50) NOT NULL,
	[add_time] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[job_info]    Script Date: 2017/9/25 15:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[job_info](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[identity_id] [int] NOT NULL,
	[add_time] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[job_info_item]    Script Date: 2017/9/25 15:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[job_info_item](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[job_id] [int] NOT NULL,
	[item_type] [nvarchar](50) NOT NULL,
	[item_content] [nvarchar](500) NOT NULL,
	[item_project] [nvarchar](50) NOT NULL,
	[item_process] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[v_default_project]    Script Date: 2017/9/25 15:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[v_default_project]

as 

select 'GM工具' as project
union select 'OSS系统'
union select 'CC系统'
union select '口袋梦三国'
union select '梦塔防相关'
union select 'ECS系统'
union select '客服系统'
union select 'ECS系统'
union select '接口配置系统'
union select '大数据接口平台'
union select '星盟冲突'
union select 'ECS系统'
union select '系统稳定性监控工具'
union select '泛微OA'
union select '闪电玩广告系统'
union select '闪电玩游戏管理平台'

GO
/****** Object:  View [dbo].[v_project]    Script Date: 2017/9/25 15:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE view [dbo].[v_project] 
as 

select distinct(item_project)as project from job_info_item
union  select project from v_default_project;






GO
/****** Object:  UserDefinedFunction [dbo].[f_project]    Script Date: 2017/9/25 15:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE function [dbo].[f_project](@identity_id int)
returns table
as 

return select distinct(item_project)as project from job_info_item where job_id in (select id from job_info where identity_id = @identity_id)
union select project from v_default_project;





GO
/****** Object:  UserDefinedFunction [dbo].[f_get_result]    Script Date: 2017/9/25 15:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[f_get_result](@type nvarchar(50), @idString varchar(1000))
returns table
as

return select stuff(
                    (select '；'+ item_content + ' ' + (case item_process when '不选也没事' then '' else item_process end ) 
                        from job_info_item b 
                        where  item_type = @type and charindex(',' + CAST( job_id as varchar) + ',', ',' + @idString + ',') >  0 and a.item_project=b.item_project for xml path(''))
                        ,1
                        ,1
                        ,'') as item_content
                        ,item_project
                        from job_info_item a
                        where item_type = @type and charindex(',' + CAST( job_id as varchar) + ',', ',' + @idString + ',') >  0  group by item_project




GO
ALTER TABLE [dbo].[identity_info] ADD  DEFAULT (getdate()) FOR [add_time]
GO
ALTER TABLE [dbo].[job_info] ADD  DEFAULT (getdate()) FOR [add_time]
GO
USE [master]
GO
ALTER DATABASE [dh_job_summary] SET  READ_WRITE 
GO
