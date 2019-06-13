/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2017 (14.0.1000)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/
USE [bkicdb]
GO
INSERT [dbo].[BK_TravelCoverage] ([Code], [CoverageType]) VALUES (N'WORLDWIDE', N'WORLDWIDE')
INSERT [dbo].[BK_TravelCoverage] ([Code], [CoverageType]) VALUES (N'EXCLUDING USA&CANADA', N'EXCLUDING USA&CANADA')
SET IDENTITY_INSERT [dbo].[BK_TravelInsurancePackage] ON 

INSERT [dbo].[BK_TravelInsurancePackage] ([ID], [Code], [Name], [CreatedDate], [ModifiedDate]) VALUES (1, N'FM001', N'Family', CAST(N'2017-09-04T12:36:13.927' AS DateTime), NULL)
INSERT [dbo].[BK_TravelInsurancePackage] ([ID], [Code], [Name], [CreatedDate], [ModifiedDate]) VALUES (2, N'IN001', N'Individual', CAST(N'2017-09-04T12:36:13.927' AS DateTime), NULL)
INSERT [dbo].[BK_TravelInsurancePackage] ([ID], [Code], [Name], [CreatedDate], [ModifiedDate]) VALUES (3, N'SCHEN', N'Schengen', CAST(N'2018-02-18T06:57:58.937' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[BK_TravelInsurancePackage] OFF
SET IDENTITY_INSERT [dbo].[BK_TravelInsurancePeroid] ON 

INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (1, N'AN001', N'Annual', CAST(N'2017-09-04T12:36:13.957' AS DateTime), 365)
INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (2, N'TW001', N'Two Years', CAST(N'2017-09-04T12:36:13.957' AS DateTime), 730)
INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (3, N'7d', N'Up To 7days', CAST(N'2018-02-07T09:45:21.260' AS DateTime), 7)
INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (4, N'10d', N'Up to 10days', CAST(N'2018-02-07T09:45:21.260' AS DateTime), 10)
INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (5, N'15d', N'Up to 15days', CAST(N'2018-02-07T09:45:21.260' AS DateTime), 15)
INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (6, N'21d', N'Up to 21days', CAST(N'2018-02-07T09:45:21.260' AS DateTime), 21)
INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (7, N'30d', N'Up to 30days', CAST(N'2018-02-07T09:45:21.260' AS DateTime), 30)
INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (8, N'60d', N'Up to 60days', CAST(N'2018-02-07T09:45:21.260' AS DateTime), 60)
INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (9, N'90d', N'Up to 90days', CAST(N'2018-02-07T09:45:21.260' AS DateTime), 90)
INSERT [dbo].[BK_TravelInsurancePeroid] ([ID], [Code], [Name], [CreatedDate], [NoOfDays]) VALUES (10, N'180d', N'Up to 180days', CAST(N'2018-02-07T09:45:21.260' AS DateTime), 180)
SET IDENTITY_INSERT [dbo].[BK_TravelInsurancePeroid] OFF
SET IDENTITY_INSERT [dbo].[BK_TravelInsuranceRate] ON 

INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (1, N'Family', N'Annual', CAST(58.500 AS Decimal(25, 3)), CAST(N'2017-09-04T12:36:13.890' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (2, N'Individual', N'Annual', CAST(48.500 AS Decimal(25, 3)), CAST(N'2017-09-04T12:36:13.907' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (3, N'Family', N'Two Years', CAST(118.000 AS Decimal(25, 3)), CAST(N'2017-09-04T12:36:13.910' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (4, N'Individual', N'Two Years', CAST(98.000 AS Decimal(25, 3)), CAST(N'2017-09-04T12:36:13.910' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (5, N'Family', N'Up to 7days', CAST(11.500 AS Decimal(25, 3)), CAST(N'2018-02-07T07:19:18.180' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (6, N'Individual', N'Up to 7days', CAST(10.000 AS Decimal(25, 3)), CAST(N'2018-02-07T07:19:18.180' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (7, N'Family', N'Up to 10days', CAST(14.000 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (8, N'Individual', N'Up to 10days', CAST(11.000 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (9, N'Family', N'Up to 15days', CAST(16.500 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (10, N'Individual', N'Up to 15days', CAST(14.500 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (11, N'Family', N'Up to 21days', CAST(23.500 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (12, N'Individual', N'Up to 21days', CAST(16.000 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (13, N'Family', N'Up to 30days', CAST(26.000 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (14, N'Individual', N'Up to 30days', CAST(21.500 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (15, N'Family', N'Up to 60days', CAST(30.000 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (16, N'Individual', N'Up to 60days', CAST(29.000 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (17, N'Family', N'Up to 90days', CAST(43.500 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.900' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (18, N'Individual', N'Up to 90days', CAST(36.000 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.913' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (19, N'Family', N'Up to 180days', CAST(54.500 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.913' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (20, N'Individual', N'Up to 180days', CAST(44.000 AS Decimal(25, 3)), CAST(N'2018-02-07T07:45:26.913' AS DateTime), NULL, N'WORLDWIDE')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (29, N'Individual', N'Up to 7days', CAST(6.500 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.597' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (30, N'Individual', N'Up to 10days', CAST(7.000 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.597' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (31, N'Individual', N'Up to 15days', CAST(8.500 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.613' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (32, N'Individual', N'Up to 21days', CAST(12.000 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.613' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (33, N'Individual', N'Up to 30days', CAST(14.500 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.613' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (34, N'Individual', N'Up to 60days', CAST(19.000 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.613' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (35, N'Individual', N'Up to 90days', CAST(27.000 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.613' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (36, N'Individual', N'Up to 180days', CAST(32.000 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.613' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (37, N'Individual', N'Annual', CAST(45.000 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.613' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (38, N'Individual', N'Two Years', CAST(89.000 AS Decimal(25, 3)), CAST(N'2018-02-07T09:38:17.613' AS DateTime), NULL, N'EXCLUDING USA&CANADA')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (39, N'Schengen', N'Up to 7days', CAST(5.500 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.943' AS DateTime), NULL, N'SCHENGEN')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (40, N'Schengen', N'Up to 10days', CAST(6.500 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.943' AS DateTime), NULL, N'SCHENGEN')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (41, N'Schengen', N'Up to 15days', CAST(8.000 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.943' AS DateTime), NULL, N'SCHENGEN')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (42, N'Schengen', N'Up to 21days', CAST(11.000 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.960' AS DateTime), NULL, N'SCHENGEN')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (43, N'Schengen', N'Up to 30days', CAST(11.500 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.960' AS DateTime), NULL, N'SCHENGEN')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (44, N'Schengen', N'Up to 60days', CAST(18.500 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.960' AS DateTime), NULL, N'SCHENGEN')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (45, N'Schengen', N'Up to 90days', CAST(26.500 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.960' AS DateTime), NULL, N'SCHENGEN')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (46, N'Schengen', N'Up to 180days', CAST(30.500 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.960' AS DateTime), NULL, N'SCHENGEN')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (47, N'Schengen', N'Annual', CAST(44.000 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.960' AS DateTime), NULL, N'SCHENGEN')
INSERT [dbo].[BK_TravelInsuranceRate] ([ID], [Package], [Peroid], [Rate], [CreatedDate], [ModifiedDate], [Coverage]) VALUES (48, N'Schengen', N'Two Years', CAST(88.000 AS Decimal(25, 3)), CAST(N'2018-02-20T12:33:55.960' AS DateTime), NULL, N'SCHENGEN')
SET IDENTITY_INSERT [dbo].[BK_TravelInsuranceRate] OFF
