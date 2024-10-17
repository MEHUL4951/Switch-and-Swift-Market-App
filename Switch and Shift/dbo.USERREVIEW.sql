CREATE TABLE [dbo].[USERREVIEW] (
    [review_id]          INT           IDENTITY (1, 1) NOT NULL,
    [reviewee_name]      VARCHAR (50)  NOT NULL,
    [reviewee_email]     VARCHAR (50)  NOT NULL,
	[reviewer_name]      VARCHAR (50)  NOT NULL,
    [reviewer_email]     VARCHAR (50)  NOT NULL,
    [review_description] VARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([review_id] ASC)
);

