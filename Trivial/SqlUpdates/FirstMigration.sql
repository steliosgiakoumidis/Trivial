CREATE TABLE [dbo].[Trivial](
	[id] [int] NULL,
	[category] [varchar](max) NOT NULL,
	[correct_answer] [varchar](max) NOT NULL,
	[difficulty] [varchar](max) NOT NULL,
	[incorrect_answers] [varchar](max) NOT NULL,
	[question] [varchar](max) NOT NULL,
	[type] [varchar](20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


