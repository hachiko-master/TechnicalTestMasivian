USE [master]
GO
/****** Object:  Database [TechnicalTestMasivian]    Script Date: 5/09/2021 8:47:26 a. m. ******/
CREATE DATABASE [TechnicalTestMasivian]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TechnicalTestMasivian', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\TechnicalTestMasivian.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TechnicalTestMasivian_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\TechnicalTestMasivian_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [TechnicalTestMasivian] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TechnicalTestMasivian].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TechnicalTestMasivian] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET ARITHABORT OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TechnicalTestMasivian] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TechnicalTestMasivian] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TechnicalTestMasivian] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TechnicalTestMasivian] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TechnicalTestMasivian] SET  MULTI_USER 
GO
ALTER DATABASE [TechnicalTestMasivian] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TechnicalTestMasivian] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TechnicalTestMasivian] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TechnicalTestMasivian] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [TechnicalTestMasivian] SET DELAYED_DURABILITY = DISABLED 
GO
USE [TechnicalTestMasivian]
GO
/****** Object:  Table [dbo].[Bets]    Script Date: 5/09/2021 8:47:26 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[BetRoulette] [int] NULL,
	[BetMoney] [decimal](10, 2) NULL,
	[BetOption] [int] NULL,
	[BetDate] [datetime] NULL CONSTRAINT [DF_Bets_BetDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Bets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roulettes]    Script Date: 5/09/2021 8:47:26 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roulettes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [bit] NULL,
	[OpenAt] [datetime] NULL,
	[ClosedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL CONSTRAINT [DF_Roulettes_CreateDateTime]  DEFAULT (getdate()),
 CONSTRAINT [PK_Roulettes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  StoredProcedure [dbo].[SP_CLOSE_ROULETTE]    Script Date: 5/09/2021 8:47:26 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CLOSE_ROULETTE] @rouletteId INT
AS BEGIN
	BEGIN TRY
		BEGIN TRANSACTION t_Transaction
			IF NOT EXISTS(SELECT [Status] FROM Roulettes WHERE Id=@rouletteId)
			BEGIN
				SELECT 0 AS 'Result';
			END
			ELSE IF((SELECT [Status] FROM Roulettes WHERE Id=@rouletteId)=0)
			BEGIN
				SELECT 2 AS 'Result';
			END
			ELSE
			BEGIN
				UPDATE Roulettes
				SET [ClosedAt] = GETDATE()
				WHERE Id = @rouletteId;

				SELECT B.UserId, B.BetRoulette, B.BetMoney, B.BetOption
				FROM Roulettes R, Bets B
				WHERE R.Id = B.BetRoulette AND B.BetRoulette = @rouletteId AND B.BetDate >= R.OpenAt AND B.BetDate <= R.ClosedAt;

				UPDATE Roulettes
				SET [Status] = 0,
				[ClosedAt] = NULL,
				[OpenAt] = NULL
				WHERE Id = @rouletteId;
			END
		COMMIT TRANSACTION t_Transaction
	END TRY 
	BEGIN CATCH
		ROLLBACK TRANSACTION t_Transaction
	END CATCH
END

-- EXEC [dbo].[SP_OPEN_ROULETTE] 11
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_BET_ROULETTE]    Script Date: 5/09/2021 8:47:26 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CREATE_BET_ROULETTE] @rouletteId INT, @userId INT, @betOption INT, @betMoney DECIMAL(10,2)
AS BEGIN
	BEGIN TRY
		BEGIN TRANSACTION t_Transaction
			IF NOT EXISTS(SELECT [Status] FROM Roulettes WHERE Id=@rouletteId)
			BEGIN
				SELECT 0 AS 'Result';
			END
			ELSE IF(@betMoney<1 OR @betMoney>10000)
			BEGIN
				SELECT 1 AS 'Result';
			END
			ELSE IF(@betOption<-2 OR @betOption>36)
			BEGIN
				SELECT 2 AS 'Result';
			END
			ELSE IF((SELECT [Status] FROM Roulettes WHERE Id=@rouletteId)=0)
			BEGIN
				SELECT 3 AS 'Result';
			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[Bets] ([UserId],[BetRoulette],[BetMoney],[BetOption])
				VALUES(@userId, @rouletteId, @betMoney, @betOption);
				SELECT 4 AS 'Result';
			END
		COMMIT TRANSACTION t_Transaction
	END TRY 
	BEGIN CATCH
		ROLLBACK TRANSACTION t_Transaction
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_ROULETTE]    Script Date: 5/09/2021 8:47:26 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CREATE_ROULETTE]
AS BEGIN
	BEGIN TRY
		BEGIN TRANSACTION t_Transaction
			INSERT INTO Roulettes([Status],[OpenAt],[ClosedAt]) VALUES(0,NULL,NULL);
			SELECT SCOPE_IDENTITY() AS 'RouleteId';
		COMMIT TRANSACTION t_Transaction
	END TRY 
	BEGIN CATCH
		ROLLBACK TRANSACTION t_Transaction
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_ALL_ROULLETES]    Script Date: 5/09/2021 8:47:26 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GET_ALL_ROULLETES]
AS BEGIN
	SELECT * FROM Roulettes;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_OPEN_ROULETTE]    Script Date: 5/09/2021 8:47:26 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_OPEN_ROULETTE] @rouletteId INT
AS BEGIN
	BEGIN TRY
		BEGIN TRANSACTION t_Transaction
			IF NOT EXISTS(SELECT [Status] FROM Roulettes WHERE Id=@rouletteId)
			BEGIN
				SELECT 0 AS 'Result';
			END
			ELSE IF((SELECT [Status] FROM Roulettes WHERE Id=@rouletteId)=1)
			BEGIN
				SELECT 2 AS 'Result';
			END
			ELSE
			BEGIN
				UPDATE Roulettes
				SET [Status] = 1,
				[OpenAt] = GETDATE()
				WHERE Id = @rouletteId;
				SELECT 1 AS 'Result';
			END
		COMMIT TRANSACTION t_Transaction
	END TRY 
	BEGIN CATCH
		ROLLBACK TRANSACTION t_Transaction
	END CATCH
END


GO
USE [master]
GO
ALTER DATABASE [TechnicalTestMasivian] SET  READ_WRITE 
GO
