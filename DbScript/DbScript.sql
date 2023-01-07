USE [DbShopBridge]
GO
/****** Object:  Table [dbo].[InventoryAPIErrorLog]    Script Date: 20-Jun-21 12:19:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryAPIErrorLog](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ErrorMessage] [nvarchar](800) NULL,
	[ActionName] [nvarchar](100) NULL,
	[ReqJsonData] [nvarchar](800) NULL,
	[ReqDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventoryAPILog]    Script Date: 20-Jun-21 12:19:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryAPILog](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [nvarchar](100) NULL,
	[ReqJsonData] [nvarchar](800) NULL,
	[ReqDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventoryMaster]    Script Date: 20-Jun-21 12:19:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryName] [nvarchar](100) NULL,
	[Description] [nvarchar](300) NULL,
	[Price] [decimal](18, 2) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[DeletedDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Prc_Insert_Error_InventoryAPI]    Script Date: 20-Jun-21 12:19:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
Create proc [dbo].[Prc_Insert_Error_InventoryAPI]
(
@Message nvarchar(800),
@ActionName nvarchar(20),
@RequestData nvarchar(800)
)
as
begin
insert into InventoryAPIErrorLog(ErrorMessage,ActionName,ReqJsonData,ReqDate)
values(@Message,@ActionName,@RequestData,GETDATE())
end




GO
/****** Object:  StoredProcedure [dbo].[Prc_Insert_Log_InventoryAPI]    Script Date: 20-Jun-21 12:19:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
Create proc [dbo].[Prc_Insert_Log_InventoryAPI]
(
@ActionName nvarchar(20),
@RequestData nvarchar(800)
)
as
begin
insert into InventoryAPILog(ActionName,ReqJsonData,ReqDate)
values(@ActionName,@RequestData,GETDATE())
end




GO
/****** Object:  StoredProcedure [dbo].[USP_Inventory_Crud_Operation]    Script Date: 20-Jun-21 12:19:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_Inventory_Crud_Operation]
(
@flag nvarchar(10),
@ID int=0,
@InvName nvarchar(100)='NA',
@Description nvarchar(300)='NA',
@Price decimal(18,2) =0.0
)
as
begin

if(@flag='List')
 begin
	if(@ID>0)
	begin
	if not exists(select 1 from InventoryMaster where IsActive=1  and ID=@ID)
	begin 
	select 'No Data found' Description,0.0 as Price
	end
	else
	begin
	select ID,InventoryName,Description,cast(Price as decimal(18,2)) as Price 
	 from InventoryMaster where IsActive=1  and ID=@ID
	 end
	return;
	end
	select ID,InventoryName,Description,cast(Price as decimal(18,2)) as Price 
	 from InventoryMaster where IsActive=1 
	return;
 end

if(@flag='Add')
 begin
	insert into InventoryMaster(InventoryName,Description,Price,IsActive,CreatedDate)
	values(@InvName,@Description,@Price,1,getdate())
	return;
 end
 if(@flag='Update')
 begin 
	update InventoryMaster set InventoryName=@InvName,Description=@Description,
	Price=@Price,UpdatedDate=getdate() where ID=@ID
	return;
 end
 if(@flag='Delete')
 begin 
	update InventoryMaster set  IsActive=0,DeletedDate=getdate() where ID=@ID
	return;
 end
end

GO
