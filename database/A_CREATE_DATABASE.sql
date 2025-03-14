USE [master]
GO

/****** Object:  Database [ToDoListDB]    Script Date: 29/1/2025 01:03:46 ******/
CREATE DATABASE [ToDoListDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ToDoListDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ToDoListDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ToDoListDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ToDoListDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ToDoListDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [ToDoListDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [ToDoListDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [ToDoListDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [ToDoListDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [ToDoListDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [ToDoListDB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [ToDoListDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [ToDoListDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [ToDoListDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [ToDoListDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [ToDoListDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [ToDoListDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [ToDoListDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [ToDoListDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [ToDoListDB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [ToDoListDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [ToDoListDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [ToDoListDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [ToDoListDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [ToDoListDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [ToDoListDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [ToDoListDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [ToDoListDB] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [ToDoListDB] SET  MULTI_USER 
GO

ALTER DATABASE [ToDoListDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [ToDoListDB] SET DB_CHAINING OFF 
GO

ALTER DATABASE [ToDoListDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [ToDoListDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [ToDoListDB] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [ToDoListDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [ToDoListDB] SET QUERY_STORE = OFF
GO

ALTER DATABASE [ToDoListDB] SET  READ_WRITE 
GO

