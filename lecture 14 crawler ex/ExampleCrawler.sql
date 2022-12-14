USE [master]
GO
/****** Object:  Database [ExampleCrawler]    Script Date: 1/6/2023 2:43:05 PM ******/
CREATE DATABASE [ExampleCrawler]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ExampleCrawler', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\ExampleCrawler.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ExampleCrawler_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\ExampleCrawler_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ExampleCrawler] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ExampleCrawler].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ExampleCrawler] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ExampleCrawler] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ExampleCrawler] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ExampleCrawler] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ExampleCrawler] SET ARITHABORT OFF 
GO
ALTER DATABASE [ExampleCrawler] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ExampleCrawler] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ExampleCrawler] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ExampleCrawler] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ExampleCrawler] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ExampleCrawler] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ExampleCrawler] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ExampleCrawler] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ExampleCrawler] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ExampleCrawler] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ExampleCrawler] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ExampleCrawler] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ExampleCrawler] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ExampleCrawler] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ExampleCrawler] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ExampleCrawler] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ExampleCrawler] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ExampleCrawler] SET RECOVERY FULL 
GO
ALTER DATABASE [ExampleCrawler] SET  MULTI_USER 
GO
ALTER DATABASE [ExampleCrawler] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ExampleCrawler] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ExampleCrawler] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ExampleCrawler] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ExampleCrawler] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ExampleCrawler] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ExampleCrawler', N'ON'
GO
ALTER DATABASE [ExampleCrawler] SET QUERY_STORE = OFF
GO
USE [ExampleCrawler]
GO
/****** Object:  Table [dbo].[RootDomains]    Script Date: 1/6/2023 2:43:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RootDomains](
	[RootDomainId] [int] IDENTITY(1,1) NOT NULL,
	[RootDomainUrlHash] [char](64) NOT NULL,
 CONSTRAINT [PK_RootDomains] PRIMARY KEY CLUSTERED 
(
	[RootDomainId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Urls]    Script Date: 1/6/2023 2:43:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Urls](
	[UrlHash] [char](64) NOT NULL,
	[Url] [varchar](2000) NOT NULL,
	[DepthLevel] [tinyint] NOT NULL,
	[ParentUrlHash] [char](64) NOT NULL,
	[Crawled] [bit] NOT NULL,
	[LastCrawlDate] [datetime] NOT NULL,
	[CrawlTryCount] [tinyint] NOT NULL,
	[Title] [nvarchar](2000) NULL,
	[Description] [nvarchar](2000) NULL,
	[InnerText] [nvarchar](max) NULL,
	[LinkText] [nvarchar](2000) NULL,
	[DiscoveryDate] [datetime] NOT NULL,
	[RootDomainId] [int] NOT NULL,
 CONSTRAINT [PK_Urls] PRIMARY KEY CLUSTERED 
(
	[UrlHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [domain_hash]    Script Date: 1/6/2023 2:43:05 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [domain_hash] ON [dbo].[RootDomains]
(
	[RootDomainUrlHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [crawled_discoverydate]    Script Date: 1/6/2023 2:43:05 PM ******/
CREATE NONCLUSTERED INDEX [crawled_discoverydate] ON [dbo].[Urls]
(
	[RootDomainId] ASC,
	[Crawled] ASC,
	[CrawlTryCount] ASC,
	[DiscoveryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Urls] ADD  CONSTRAINT [DF_Urls_DepthLevel]  DEFAULT ((0)) FOR [DepthLevel]
GO
ALTER TABLE [dbo].[Urls] ADD  CONSTRAINT [DF_Urls_Crawled]  DEFAULT ((0)) FOR [Crawled]
GO
ALTER TABLE [dbo].[Urls] ADD  CONSTRAINT [DF_Urls_LastCrawlDate]  DEFAULT (((1900)-(1))-(1)) FOR [LastCrawlDate]
GO
ALTER TABLE [dbo].[Urls] ADD  CONSTRAINT [DF_Urls_CrawlTryCount]  DEFAULT ((0)) FOR [CrawlTryCount]
GO
ALTER TABLE [dbo].[Urls] ADD  CONSTRAINT [DF_Urls_DiscoveryDate]  DEFAULT (sysutcdatetime()) FOR [DiscoveryDate]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Urls', @level2type=N'COLUMN',@level2name=N'DepthLevel'
GO
USE [master]
GO
ALTER DATABASE [ExampleCrawler] SET  READ_WRITE 
GO
