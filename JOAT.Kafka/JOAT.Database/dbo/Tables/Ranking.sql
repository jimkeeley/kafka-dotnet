CREATE TABLE [dbo].[Ranking] (
    [PlayerId]           NVARCHAR (100) NULL,
    [FirstName]          NVARCHAR (200) NULL,
    [LastName]           NVARCHAR (200) NULL,
    [Age]                INT            NULL,
    [CountryName]        NVARCHAR (100) NULL,
    [CountryCode]        NVARCHAR (100) NULL,
    [State]              NVARCHAR (10)  NULL,
    [City]               NVARCHAR (100) NULL,
    [WpprPoints]         FLOAT (53)     NULL,
    [CurrentWpprRank]    INT            NULL,
    [LastMonthRank]      INT            NULL,
    [RatingValue]        FLOAT (53)     NULL,
    [EfficiencyPercent]  FLOAT (53)     NULL,
    [EventCount]         INT            NULL,
    [BestFinish]         NVARCHAR (200) NULL,
    [BestFinishPosition] INT            NULL,
    [BestTournamentId]   INT            NULL
);

