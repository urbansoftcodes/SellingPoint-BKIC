USE [SellingPoint]
GO

/****** Object:  Table [dbo].[InsuranceProductMaster]    Script Date: 5/10/2018 2:59:23 AM ******/
SET ANSI_NULLS ON
GO
CREATE TABLE [dbo].[InsuranceProductMaster]( 
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AGENCY] [varchar](50) NULL,
	[AGENTCODE] [varchar](50) NULL,
	[MAINCLASS] [varchar](50) NULL,
	[SUBCLASS] [varchar](50) NULL,
	[EFFECTIVEDATEFROM] [datetime] NULL,
	[EFFECTIVEDATETO] [datetime] NULL,
	[ISACTIVE] [bit] NULL,
	[CREATEDDATE] [datetime] NULL,
	[UPDATEDDATE] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



