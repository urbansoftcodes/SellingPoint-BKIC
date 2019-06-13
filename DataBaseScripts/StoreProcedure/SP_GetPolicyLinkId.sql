USE SellingPoint
GO
/****** Object:  StoredProcedure [dbo].[GetPolicyLinkId]    Script Date: 6/16/2018 2:10:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[SP_GetPolicyLinkId](
@LinkId as nvarchar(250) out
)AS
BEGIN
		DECLARE @SeriesType nvarchar(50), @LastSeries int ,@SeriesFormatLength int, @CurrentYear nvarchar(4),@Month nvarchar(2)
		SELECT  @SeriesType = SeriesType, @LastSeries = LastSeries, @SeriesFormatLength = SeriesFormatLength From CodeSeries WITH(nolock)
				where SeriesForApplication = 'PolicyLinkedId' and  IsActive = 1

		SET @LastSeries = @LastSeries + 1
		SET @CurrentYear =(select CONVERT(nvarchar(4),(select YEAR(CURRENT_TIMESTAMP))))
		Set @Month=FORMAT(getdate(),'MM') 

		IF (@SeriesFormatLength > 0)
		BEGIN
			SET @LinkId =(SELECT @SeriesType + @CurrentYear + @Month + REPLICATE('0',@SeriesFormatLength-LEN(RTRIM(@LastSeries))) + RTRIM(@LastSeries))
		END
		ELSE
		BEGIN
			SET @LinkId =(SELECT @SeriesType + @CurrentYear + @Month + CONVERT(nvarchar(max), @LastSeries))
		END

		Update BK_CodeSeries set LastSeries = @LastSeries, modifieddate = GETDATE() where SeriesForApplication = 'PolicyLinkedId' and IsActive = 1

END
