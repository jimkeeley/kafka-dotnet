create PROCEDURE [dbo].[RankingUpsert] (
  @data [dbo].[RankingType] READONLY
)
AS
BEGIN
	DECLARE @T TABLE([id] int, [_rownumber] int)

	MERGE INTO [dbo].Ranking AS t
	USING (SELECT *, [_rownumber] = ROW_NUMBER() OVER (ORDER BY (SELECT 1)) FROM @data) AS s
	ON
	(
	  t.[PlayerId] = s.[PlayerId]
	)
	WHEN MATCHED THEN UPDATE SET
	  t.FirstName = s.FirstName,
	  t.LastName = s.LastName,
	  t.Age = s.Age,
	  t.CountryName = s.CountryName,
	  t.CountryCode = s.CountryCode,
	  t.State = s.State,
	  t.City = s.City,
	  t.WpprPoints = s.WpprPoints,
	  t.CurrentWpprRank = s.CurrentWpprRank,
	  t.LastMonthRank = s.LastMonthRank,
	  t.RatingValue = s.RatingValue,
	  t.EfficiencyPercent = s.EfficiencyPercent,
	  t.EventCount = s.EventCount,
	  t.BestFinish = s.BestFinish,
	  t.BestFinishPosition = s.BestFinishPosition,
	  t.BestTournamentId = s.BestTournamentId

	WHEN NOT MATCHED BY TARGET THEN INSERT
	(
				[PlayerId]
			   ,[FirstName]
			   ,[LastName]
			   ,[Age]
			   ,[CountryName]
			   ,[CountryCode]
			   ,[State]
			   ,[City]
			   ,[WpprPoints]
			   ,[CurrentWpprRank]
			   ,[LastMonthRank]
			   ,[RatingValue]
			   ,[EfficiencyPercent]
			   ,[EventCount]
			   ,[BestFinish]
			   ,[BestFinishPosition]
			   ,[BestTournamentId]
	)
	VALUES
	(
				s.[PlayerId]
			   ,s.[FirstName]
			   ,s.[LastName]
			   ,s.[Age]
			   ,s.[CountryName]
			   ,s.[CountryCode]
			   ,s.[State]
			   ,s.[City]
			   ,s.[WpprPoints]
			   ,s.[CurrentWpprRank]
			   ,s.[LastMonthRank]
			   ,s.[RatingValue]
			   ,s.[EfficiencyPercent]
			   ,s.[EventCount]
			   ,s.[BestFinish]
			   ,s.[BestFinishPosition]
			   ,s.[BestTournamentId]
	)
	OUTPUT Inserted.[PlayerId], s.[_rownumber] INTO @T ;
	SELECT * FROM @T ORDER BY [_rownumber]

END