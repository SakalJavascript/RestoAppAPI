USE [master]
GO
/****** Object:  Database [RestoManager]    Script Date: 12-Dec-23 7:58:01 PM ******/
CREATE DATABASE [RestoManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RestoManager', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\RestoManager.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RestoManager_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\RestoManager_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RestoManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RestoManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RestoManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RestoManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RestoManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RestoManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [RestoManager] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [RestoManager] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [RestoManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RestoManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RestoManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RestoManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RestoManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RestoManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RestoManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RestoManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RestoManager] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RestoManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RestoManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RestoManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RestoManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RestoManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RestoManager] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RestoManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RestoManager] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RestoManager] SET  MULTI_USER 
GO
ALTER DATABASE [RestoManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RestoManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RestoManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RestoManager] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
USE [RestoManager]
GO
/****** Object:  StoredProcedure [dbo].[DELETE_MenuCategory]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[DELETE_MenuCategory]
/**
EXEC [dbo].[DELETE_MenuCategory]
@ID =0
**/
@ID as	int=0
AS
BEGIN
	DECLARE @TranDate AS DATETIME=GetDate()
	BEGIN TRY
		
		BEGIN TRAN
		BEGIN
			UPDATE MenuCategory 
				SET		IsDeleted=1,						
						UpdatedDate=@TranDate	
				WHERE ID=@ID
		END					
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Get_ALL_Forms]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_ALL_Forms]

AS
BEGIN

		SELECT	FormID,	ISNULL(ParentFormID,0) ParentFormID,	FormName,	FormURL	,Description,	CreatedDate,	CreatedBy,	UpdatedDate,	UpdatedBy,	IsActive
 
		FROM	FormMaster	
		WHERE	IsActive=1

		ORDER BY Sequence

END 


GO
/****** Object:  StoredProcedure [dbo].[Get_ALL_Menu]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_ALL_Menu]
@PageNumber int=0,
@pageSize int=0
AS
BEGIN

	DECLARE @StartPage INT = ((@PageNumber - 1) * @pageSize);

	WITH MenuWithPagination 
	AS
	(
		SELECT  ROW_NUMBER() Over( Order BY Name, MenuName) RowNum,M.ID,MenuName,M.Description,Price ,Name MenuCatName 
		FROM	Menu M		 
				INNER JOIN MenuCategory MC on M.CategoryID=MC.ID
		WHERE MC.IsDeleted=0 
			  AND M.IsDeleted=0
        
			
	)	
	SELECT * FROM MenuWithPagination WHERE RowNum > @StartPage AND RowNum <=  (@StartPage+@pageSize) 

END 


GO
/****** Object:  StoredProcedure [dbo].[Get_ALL_Orders]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_ALL_Orders]
@PageNumber int=0,
@pageSize int=0
AS
BEGIN

	DECLARE @StartPage INT = ((@PageNumber - 1) * @pageSize);

	WITH OrderWithPagination 
	AS
	(
		SELECT  ROW_NUMBER() Over( Order BY ID Desc) RowNum, ID,OrderDate,TotalAmount,IsPaid, OrderType 
		FROM	FinalOrders O		 
		WHERE IsDeleted=0
			
	)	
	SELECT * FROM OrderWithPagination WHERE RowNum > @StartPage AND RowNum <=  (@StartPage+@pageSize)

END 



GO
/****** Object:  StoredProcedure [dbo].[Get_ALL_Orders_Count]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_ALL_Orders_Count]
AS
BEGIN	
		SELECT  Count(ID) Total
		FROM	FinalOrders O		 
		WHERE IsDeleted=0			
	
END 


GO
/****** Object:  StoredProcedure [dbo].[Get_ALL_TABLES]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_ALL_TABLES]
AS
BEGIN
	SELECT T.ID,TableNumber,Description,Capacity, IsOccupied,ISNULL(TotalAmount,0) TotalAmount , IsOccupied , ISNULL(CurrentOrderId,0) CurrentOrderId
	FROM	RestaurantTable T		 
			LEFT JOIN Orders O ON O.ID=T.CurrentOrderId 
	WHERE T.IsDeleted=0
	ORDER BY T.Id
END 




GO
/****** Object:  StoredProcedure [dbo].[Get_Dash_Menu_Cat_wise_total]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[Get_Dash_Menu_Cat_wise_total]
@StartDate varchar(10)='',
@EndDate varchar(10)=''
AS
BEGIN

	
	SELECT  MC.Name, SUM( (KOD.Price * KOD.Quantity)) Total 
	FROM	FinalOrders O
			INNER JOIN KitchenOrders KO ON O.ID=KO.FinalOrderId
			INNER JOIN KitchenOrdersDetails KOD ON KOD.KitchenOrdersId=KO.ID
			INNER JOIN Menu M ON KOD.MenuID=M.ID
			INNER JOIN MenuCategory MC ON M.CategoryID=MC.ID
	WHERE	ISNULL(O.IsDeleted,0)=0
			AND ISNULL(KO.IsDeleted,0)=0
			AND ISNULL(KOD.IsDeleted,0)=0
			AND ISNULL(M.ISDeleted,0)=0
			AND CONVERT(date,O.OrderDate,103) >=CONVERT(date,@StartDate,103)
			AND CONVERT(date,O.OrderDate,103) <=CONVERT(date,@EndDate,103)
	GROUP BY MC.Name
	UNION
	SELECT 'Total', SUM(TotalAmount) Total
	FROM  FinalOrders
	WHERE ISNULL(IsDeleted,0)=0
		  AND CONVERT(date,OrderDate,103) >=CONVERT(date,@StartDate,103)
		  AND CONVERT(date,OrderDate,103) <=CONVERT(date,@EndDate,103)

END







 

 





GO
/****** Object:  StoredProcedure [dbo].[Get_Dash_Ordar_type_wise_count]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Get_Dash_Ordar_type_wise_count]
@StartDate varchar(10)='',
@EndDate varchar(10)=''
AS
BEGIN
	SELECT IIF(OrderType=1,'Table Billing','Fast Billing') OrderType, Count(Id) Count
	FROM	FinalOrders 
	WHERE   ISNULL(IsDeleted,0)=0
			AND CONVERT(date,OrderDate,103) >=CONVERT(date,@StartDate,103)
			 AND CONVERT(date,OrderDate,103) <=CONVERT(date,@EndDate,103)
	GROUP BY OrderType
	UNION
	SELECT  'Total' OrderType, Count(Id) Count
	FROM	FinalOrders 
	WHERE   ISNULL(IsDeleted,0)=0
			AND CONVERT(date,OrderDate,103) >=CONVERT(date,@StartDate,103)
			 AND CONVERT(date,OrderDate,103) <=CONVERT(date,@EndDate,103)
	
END
 
GO
/****** Object:  StoredProcedure [dbo].[Get_KitchenOrders_ById]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Get_KitchenOrders_ById]
@OrderId int=0
AS
BEGIN
	SELECT	 M.MenuName, KOD.Quantity 
	FROM	 KitchenOrders KO
			 INNER JOIN KitchenOrdersDetails KOD ON KOD.KitchenOrdersId=KO.ID
			 INNER JOIN Menu M ON KOD.MenuID=M.ID
	WHERE	KO.ID=@OrderId 
			AND ISNULL(KO.IsDeleted,0)=0
			AND ISNULL(KOD.IsDeleted,0)=0
			AND ISNULL(M.IsDeleted,0)=0
	ORDER  BY KO.ID 			

END 

GO
/****** Object:  StoredProcedure [dbo].[Get_Menu_By_Id]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[Get_Menu_By_Id] 
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ID], [MenuName], [Description], [Price],[CategoryID]
    FROM [dbo].[Menu]
    WHERE [Id] = @Id
        AND [IsDeleted] = 0
	ORDER BY MenuName
    
END

GO
/****** Object:  StoredProcedure [dbo].[Get_MenuCategories]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_MenuCategories]
@PageSize int=0,
@PageNumber int=0
AS
BEGIN
	
	
    SET NOCOUNT ON;
    SELECT * From (
		SELECT ROW_NUMBER() over(Order by Name) as RowNum , [ID], [Name], IIF([Description]='','Not Mantianed',[Description]) [Description]
		FROM [dbo].[MenuCategory]
		WHERE [IsDeleted] = 0
		
	) temp where RowNum between(@PageSize*@PageNumber)-@PageSize+1 and (@PageSize*@PageNumber) ORDER BY Name

END 

--exec [dbo].[Get_MenuCategories] 2, 3


--10
--drop proc [Get_MenuCategories]



GO
/****** Object:  StoredProcedure [dbo].[Get_MenuCategories_search]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Get_MenuCategories_search]
@SearchText nvarchar(40)=''

AS
BEGIN
	
	SELECT [ID], [Name], IIF([Description]='','Not Mantianed',[Description]) [Description]
	FROM [dbo].[MenuCategory]
	WHERE [IsDeleted] = 0 AND Name LIKE '%'+@SearchText+'%'
	ORDER BY Name
END 






GO
/****** Object:  StoredProcedure [dbo].[Get_Orders_ById]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Get_Orders_ById]
@OrderId int=0

AS
BEGIN
	SELECT	Id OrderId,TotalAmount,OrderType,OrderDate,IsPaid 
	FROM	FinalOrders where Id=@OrderId 
			AND ISNULL(IsDeleted,0)=0	
	
	SELECT	 KitchenOrdersId, M.MenuName, M.Price, KOD.Quantity,(KOD.Quantity * M.Price) Price  
	FROM	 FinalOrders O
			 INNER JOIN KitchenOrders KO ON KO.FinalOrderId=O.ID 
			 INNER JOIN KitchenOrdersDetails KOD ON KOD.KitchenOrdersId=KO.ID
			 INNER JOIN Menu M ON KOD.MenuID=M.ID
	WHERE	O.ID=@OrderId 
			AND ISNULL(O.IsDeleted,0)=0
	ORDER  BY KOD.KitchenOrdersId 			

END 

GO
/****** Object:  StoredProcedure [dbo].[Get_Table_bills]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_Table_bills]
@TableIds xml

AS
BEGIN
	SELECT KitchenOrdersId, M.MenuName, M.Price, KOD.Quantity,(KOD.Quantity * M.Price) Price  
	FROM  Orders O
		  INNER JOIN KitchenOrders KO ON O.Id =KO.OrderID
		  INNER JOIN KitchenOrdersDetails KOD ON KOD.KitchenOrdersId=KO.ID
		  INNER JOIN RestaurantTable T ON O.ID=T.CurrentOrderId	
		  INNER JOIN Menu M on M.ID=KOD.MenuID
	WHERE ISNULL(O.IsDeleted,0)=0
		  AND ISNULL(O.IsDeleted,0)=0
		  AND ISNULL(KO.IsDeleted,0)=0
		  AND ISNULL(KOD.IsDeleted,0)=0
		  AND ISNULL(T.IsOccupied,0)=1
		  AND ISNULL(T.IsDeleted,0)=0
		  AND ISNULL(M.IsDeleted,0)=0 
		  AND T.ID In( SELECT
						x.value('(.)[1]', 'INT') AS Id
						FROM
						 @TableIds.nodes('/DetailsRow/Id') AS T(x)
					  )

END 







GO
/****** Object:  StoredProcedure [dbo].[GetMenuByCategory]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[GetMenuByCategory]
    @CategoryID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ID], [MenuName], [Description], [Price]
    FROM [dbo].[Menu]
    WHERE [CategoryID] = @CategoryID
        AND [IsDeleted] = 0
	ORDER BY MenuName
    
END

GO
/****** Object:  StoredProcedure [dbo].[Insert_Exception_Log]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Insert_Exception_Log]
    @LineNumber INT,
    @MethodName NVARCHAR(255),
    @ClassName NVARCHAR(255),
    @StackTrace NVARCHAR(MAX),
    @ErrorMessage NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO ExceptionLog (LineNumber, MethodName, ClassName, StackTrace, ErrorMessage)
    VALUES (@LineNumber, @MethodName, @ClassName, @StackTrace, @ErrorMessage);
END;

GO
/****** Object:  StoredProcedure [dbo].[Insert_KitchenOrderWithDetails_XML]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Insert_KitchenOrderWithDetails_XML]
@TableID int=0, 
@OrderType  tinyint, 
@CreatedBy int,
@TotalAmount  decimal(10,2)=0,
@DetailsXML xml,
@OrderId int=0,
@OrderIDOut int Output
AS
BEGIN
   	DECLARE @KitchenOrdersId int;
	DECLARE @OrderDate DATETIME=GetDate();
	DECLARE @NewIdOrders INT=0
	DECLARE @NewIdFintalOrders INT=0

	BEGIN TRY	
		BEGIN TRAN
			IF @OrderID=0	
			BEGIN		

					INSERT INTO [dbo].[Orders]( [OrderType] ,[OrderDate],[TotalAmount] ,[IsPaid] ,[CreatedDate] ,[CreatedBy]  ,[UpdatedDate],[UpdatedBy])
					VALUES				      (  @OrderType ,@OrderDate   ,@TotalAmount ,0	,@OrderDate	  ,@CreatedBy	,@OrderDate   , @CreatedBy)
					   SET @NewIdOrders =  SCOPE_IDENTITY()
				IF(@OrderType=2)
					INSERT INTO [dbo].[FinalOrders]( [OrderType] ,[OrderDate],[TotalAmount] ,[IsPaid] ,[CreatedDate] ,[CreatedBy]  ,[UpdatedDate],[UpdatedBy])
					VALUES						    (  @OrderType,@OrderDate,0				,	1	  ,@OrderDate	  ,@CreatedBy	,@OrderDate   , @CreatedBy)			 
					SET @NewIdFintalOrders =  SCOPE_IDENTITY()
			
			END	
    
			IF @TableID>0
				UPDATE RestaurantTable 
				SET	   UpdatedDate=@OrderDate,
						UpdatedBy=99,
						IsOccupied=1,
						CurrentOrderId=IIF(@OrderID=0,@NewIdOrders,@OrderID)
				WHERE ID=@TableID
	

			-- Insert into KitchenOrders

			INSERT INTO dbo.KitchenOrders (TableID, OrderID    , OrderDate , CreatedBy , UpdatedBy,FinalOrderID)
								   VALUES (@TableID, IIF( @OrderID=0, @NewIdOrders,@OrderID), @OrderDate, @CreatedBy, @CreatedBy,@NewIdFintalOrders);

			SET @KitchenOrdersId = SCOPE_IDENTITY();
			-- Insert into KitchenOrdersDetails using XML input
			INSERT INTO dbo.KitchenOrdersDetails (KitchenOrdersId, MenuID, Quantity, Price, CreatedBy, UpdatedBy)
			SELECT @KitchenOrdersId, 
				   Details.value('MenuID[1]', 'int'),
				   Details.value('Quantity[1]', 'int'),
				   Details.value('Price[1]', 'decimal(10, 2)'),
				   @CreatedBy,
				   @CreatedBy
			FROM @DetailsXML.nodes('/DetailsRow') AS T(Details);

	
	
			IF(@OrderType=1)

				SELECT  @TotalAmount= SUM((Price*Quantity))
				FROM	KitchenOrdersDetails 
				WHERE	KitchenOrdersId in(select Id from KitchenOrders where OrderID=IIF(@OrderID=0,@NewIdOrders,@OrderID)	)

				UPDATE [dbo].[Orders]
					   SET 	[TotalAmount]=@TotalAmount
						  ,[UpdatedDate] = @OrderDate
						  ,[UpdatedBy] = 99			  
				WHERE ID=IIF(@OrderID=0,@NewIdOrders,@OrderID)	

			IF(@OrderType=2)

				SELECT  @TotalAmount= SUM((Price*Quantity))
				FROM	KitchenOrdersDetails 
				WHERE	KitchenOrdersId in(select Id from KitchenOrders where FinalOrderId=@NewIdFintalOrders)

				UPDATE [dbo].[FinalOrders]
					   SET 	[TotalAmount]=@TotalAmount
						  ,[UpdatedDate] = @OrderDate
						  ,[UpdatedBy] = 99			  
				WHERE ID=@NewIdFintalOrders

			set   @OrderIDOut=IIF(@OrderType=2,@NewIdFintalOrders,@KitchenOrdersId)
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
	END CATCH
END;

GO
/****** Object:  StoredProcedure [dbo].[Insert_Table_Billing]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Insert_Table_Billing]
@TableIds xml,
@OrderIDOut int Output
AS
BEGIN
   	DECLARE @KitchenOrdersId int;
	DECLARE @OrderDate DATETIME=GetDate();

	IF OBJECT_ID('tempdb..#KitchenOrdersIds') IS NULL
	BEGIN	
		CREATE TABLE #KitchenOrdersIds (ID INT)
	END
	
	INSERT INTO #KitchenOrdersIds
	SELECT		KO.ID 
				FROM	RestaurantTable T
						INNER JOIN Orders O on T.CurrentOrderId=O.ID
						INNER JOIN KitchenOrders KO on KO.OrderID=O.ID
				WHERE	ISNULL(T.IsDeleted,0)=0
						AND ISNULL(O.IsDeleted,0)=0
						AND ISNULL(KO.IsDeleted,0)=0
						 AND T.ID In( SELECT
									x.value('(.)[1]', 'INT') AS Id
									FROM
									 @TableIds.nodes('/DetailsRow/Id') AS T(x)
								  )

	INSERT INTO [dbo].[FinalOrders]( [OrderType] ,[OrderDate],[TotalAmount] ,[IsPaid] ,[CreatedDate] ,[CreatedBy]  ,[UpdatedDate],[UpdatedBy])
	VALUES						   (  1         ,@OrderDate  ,0          ,1		   ,@OrderDate	  ,99	,@OrderDate   , 99)

    SET @OrderIDOut =  SCOPE_IDENTITY()

	UPDATE	KitchenOrders 
	SET		FinalOrderId=@OrderIDOut,
			[UpdatedDate]=@OrderDate,
			UpdatedBy=99
	WHERE	ID IN( select ID FROM #KitchenOrdersIds)
	
	UPDATE RestaurantTable 
	SET	   UpdatedDate=@OrderDate,
			UpdatedBy=99,
			IsOccupied=0,
			CurrentOrderId=Null
	WHERE ID In( SELECT
				x.value('(.)[1]', 'INT') AS Id
				FROM
					@TableIds.nodes('/DetailsRow/Id') AS T(x)
				)
	

    
   DECLARE @TotalAmount decimal=0
	
	SELECT  @TotalAmount= SUM((Price*Quantity))
	FROM	KitchenOrdersDetails 
	WHERE	KitchenOrdersId in(select ID FROM #KitchenOrdersIds)

	UPDATE [dbo].[FinalOrders]
		   SET 	[TotalAmount]=@TotalAmount
			  ,[UpdatedDate] = @OrderDate
			  ,[UpdatedBy] = 99			  
	WHERE ID=@OrderIDOut	

	select  @OrderIDOut

END;


GO
/****** Object:  StoredProcedure [dbo].[Insert_Update_Image]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Insert_Update_Image]
    @Id INT = NULL,
    @FileName NVARCHAR(255),
    @FileDescription NVARCHAR(MAX),
    @FilePath NVARCHAR(MAX),
    @CreatedBy INT,
    @UpdatedBy INT,
    @ErrorMessage NVARCHAR(255) OUTPUT,
	@InsertedId Int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ErrorMessage = NULL;
	SET @InsertedId=0;
    -- Check for duplication if Id is NULL (new record)

        IF @Id IS NULL AND EXISTS (SELECT 1 FROM Images WHERE FilePath = @FilePath)
        BEGIN
            SET @ErrorMessage = 'A record with the same FilePath already exists.';
            RETURN;
        END
		IF @Id IS NOT NULL AND EXISTS (SELECT 1 FROM Images WHERE FilePath = @FilePath AND Id!=@Id)
        BEGIN
            SET @ErrorMessage = 'A record with the same FilePath already exists.';
            RETURN;
        END
   

    -- Update existing record if Id is provided
    IF @Id IS NOT NULL
    BEGIN
        UPDATE Images
        SET FileName = @FileName,
            FileDescription = @FileDescription,
            FilePath = @FilePath,
            UpdatedDate = GETDATE(),
            UpdatedBy = @UpdatedBy
        WHERE Id = @Id;
    END
    ELSE
    BEGIN
        -- Insert new record if Id is NULL
        INSERT INTO Images (FileName, FileDescription, FilePath, CreatedBy, UpdatedBy)
        VALUES (@FileName, @FileDescription, @FilePath, @CreatedBy, @UpdatedBy);
    END
	
	select @InsertedId=@@IDENTITY
END

GO
/****** Object:  StoredProcedure [dbo].[Insert_Update_Menu]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Insert_Update_Menu]
    @ID INT = NULL,
    @Name NVARCHAR(50),
    @Description NVARCHAR(MAX),   
    @ErrorMessage NVARCHAR(100) OUTPUT,
	@CategoryId Int =NULL,
	@UserId INT=0,
	@IsDeleted bit=0,
	@Price decimal(10, 2)= 0
AS
BEGIN
	
			DECLARE @TranDate DATETIME=GetDate();
			-- Check for duplicate name during insert
			IF @ID IS NULL AND EXISTS(SELECT 1 FROM [dbo].[Menu] WHERE [MenuName] = @Name)
			BEGIN				
				SET @ErrorMessage = 'Menu name already exists.';
				RETURN;
			END

			-- Check for duplicate name during update
			IF @ID IS NOT NULL AND EXISTS(SELECT 1 FROM [dbo].[Menu] WHERE [MenuName] = @Name AND [ID] <> @ID)
			BEGIN
				SET @ErrorMessage = 'Menu name already exists.';
				RETURN;
			END
			BEGIN TRY
				BEGIN TRAN

				IF @ID IS NULL
				BEGIN			
				-- Insert new record
					INSERT INTO [dbo].[Menu] ([MenuName], [Description],[CategoryID], [CreatedDate],[Price],  [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted])
					VALUES					  (@Name	, @Description ,@CategoryId ,@TranDate	   ,@Price,	   @UserId	, @UserId	   , @UserId	, 0);
				END
				ELSE
				BEGIN
					-- Update existing record
					UPDATE [dbo].[Menu]
					SET [MenuName] = @Name,
						[Description] = @Description,
						[CategoryID]=@CategoryId,
						[UpdatedBy] = @UserId,
						[UpdatedDate] =@TranDate,
						[Price]=@Price,
						[IsDeleted]=@IsDeleted
					WHERE [ID] = @ID;
				END

				SET @ErrorMessage = '';
			
				COMMIT TRAN 
		END TRY
	BEGIN CATCH 
		SET	@ErrorMessage='Somthing Wents Wrong! Contact To Administration'
		ROLLBACK TRAN
	END CATCH
    SET NOCOUNT ON;
	
END




GO
/****** Object:  StoredProcedure [dbo].[Insert_Update_MenuCategory]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Insert_Update_MenuCategory]
    @ID INT = NULL,
    @Name NVARCHAR(50),
    @Description NVARCHAR(MAX),   
    @ErrorMessage NVARCHAR(100) OUTPUT,
	@ImageId Int =NULL,
	@UserId INT=0,
	@IsDeleted bit=0
AS
BEGIN

	BEGIN TRY
		BEGIN TRAN
			DECLARE @TranDate DATETIME=GetDate();
			-- Check for duplicate name during insert
			IF @ID IS NULL AND EXISTS(SELECT 1 FROM [dbo].[MenuCategory] WHERE [Name] = @Name)
			BEGIN				
				SET @ErrorMessage = 'Category name already exists.';
				RETURN;
			END

			-- Check for duplicate name during update
			IF @ID IS NOT NULL AND EXISTS(SELECT 1 FROM [dbo].[MenuCategory] WHERE [Name] = @Name AND [ID] <> @ID)
			BEGIN
				SET @ErrorMessage = 'Category name already exists.';
				RETURN;
			END

			IF @ID IS NULL
			BEGIN
				-- Insert new record
				INSERT INTO [dbo].[MenuCategory] ([Name], [Description],ImagesId, [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted])
				VALUES (@Name, @Description,NULL, @TranDate, @UserId, @UserId, @UserId, 0);
			END
			ELSE
			BEGIN
				-- Update existing record
				UPDATE [dbo].[MenuCategory]
				SET [Name] = @Name,
					[Description] = @Description,
					[UpdatedBy] = @UserId,
					[UpdatedDate] =@TranDate,
					[IsDeleted]=@IsDeleted
				WHERE [ID] = @ID;
			END

			SET @ErrorMessage = '';
			
		COMMIT TRAN 
	END TRY
	BEGIN CATCH 
		SET	@ErrorMessage='Somthing Wents Wrong! Contact To Administration'
		ROLLBACK TRAN
	END CATCH
    SET NOCOUNT ON;
	
END


GO
/****** Object:  StoredProcedure [dbo].[Update_ChangeTable]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Update_ChangeTable]
@FromTableID int=0, 
@ToTableID int=0, 
@CurrentOrderId int=0
AS
BEGIN
	DECLARE @TrDate DATETIME=GetDate();
	
		UPDATE RestaurantTable 
		SET	   UpdatedDate=@TrDate,
				UpdatedBy=99,
				IsOccupied=1,
				CurrentOrderId=@CurrentOrderId
		WHERE ID=@ToTableID

		UPDATE RestaurantTable 
		SET	   UpdatedDate=@TrDate,
				UpdatedBy=99,
				IsOccupied=0,
				CurrentOrderId=null
		WHERE ID=@FromTableID


END;

GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExceptionLog]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExceptionLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LineNumber] [int] NULL,
	[MethodName] [nvarchar](255) NULL,
	[ClassName] [nvarchar](255) NULL,
	[StackTrace] [nvarchar](max) NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FinalOrders]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinalOrders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderType] [tinyint] NOT NULL,
	[OrderDate] [datetime] NULL,
	[TotalAmount] [decimal](10, 2) NOT NULL,
	[IsPaid] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FormMaster]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FormMaster](
	[FormID] [int] NOT NULL,
	[ParentFormID] [int] NULL,
	[FormName] [nvarchar](255) NOT NULL,
	[FormURL] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Sequence] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FormID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Images]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[FileDescription] [nvarchar](max) NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KitchenOrders]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KitchenOrders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TableID] [int] NULL,
	[OrderID] [int] NULL,
	[OrderDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
	[FinalOrderId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KitchenOrdersDetails]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KitchenOrdersDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KitchenOrdersId] [int] NOT NULL,
	[MenuID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Menu]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MenuName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MenuCategory]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
	[ImagesId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Orders]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderType] [tinyint] NOT NULL,
	[OrderDate] [datetime] NULL,
	[TotalAmount] [decimal](10, 2) NOT NULL,
	[IsPaid] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RestaurantTable]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestaurantTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TableNumber] [int] NOT NULL,
	[Capacity] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsOccupied] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
	[CurrentOrderId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 12-Dec-23 7:58:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[MobileNumber] [nvarchar](20) NOT NULL,
	[Salt] [nvarchar](64) NOT NULL,
	[Hash] [nvarchar](64) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 12-Dec-23 7:58:01 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [RoleNameIndex]    Script Date: 12-Dec-23 7:58:01 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 12-Dec-23 7:58:01 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 12-Dec-23 7:58:01 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 12-Dec-23 7:58:01 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmailIndex]    Script Date: 12-Dec-23 7:58:01 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UserNameIndex]    Script Date: 12-Dec-23 7:58:01 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ExceptionLog] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ExceptionLog] ADD  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[FinalOrders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[FinalOrders] ADD  DEFAULT ((0)) FOR [IsPaid]
GO
ALTER TABLE [dbo].[FinalOrders] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[FinalOrders] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[FinalOrders] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[FormMaster] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[FormMaster] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[FormMaster] ADD  DEFAULT ((0)) FOR [Sequence]
GO
ALTER TABLE [dbo].[Images] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Images] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Images] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[KitchenOrders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[KitchenOrders] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[KitchenOrders] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[KitchenOrders] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[KitchenOrdersDetails] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[KitchenOrdersDetails] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[KitchenOrdersDetails] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[MenuCategory] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[MenuCategory] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[MenuCategory] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((0)) FOR [IsPaid]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RestaurantTable] ADD  DEFAULT ((0)) FOR [IsOccupied]
GO
ALTER TABLE [dbo].[RestaurantTable] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RestaurantTable] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[RestaurantTable] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[KitchenOrdersDetails]  WITH CHECK ADD FOREIGN KEY([KitchenOrdersId])
REFERENCES [dbo].[KitchenOrders] ([ID])
GO
ALTER TABLE [dbo].[KitchenOrdersDetails]  WITH CHECK ADD FOREIGN KEY([MenuID])
REFERENCES [dbo].[Menu] ([ID])
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[MenuCategory] ([ID])
GO
ALTER TABLE [dbo].[MenuCategory]  WITH CHECK ADD  CONSTRAINT [FK_MenuCategory_ImagesId_Images_Id] FOREIGN KEY([ImagesId])
REFERENCES [dbo].[Images] ([Id])
GO
ALTER TABLE [dbo].[MenuCategory] CHECK CONSTRAINT [FK_MenuCategory_ImagesId_Images_Id]
GO
USE [master]
GO
ALTER DATABASE [RestoManager] SET  READ_WRITE 
GO
