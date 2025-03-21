USE [ToDoListDB]
GO
/****** Object:  Table [dbo].[State]    Script Date: 29/1/2025 01:03:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[States](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NULL,
 CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 29/1/2025 01:03:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NULL,
	[state_id] [int] NULL,
	[initial_date] [datetime] NULL,
	[finish_date] [datetime] NULL,
	[done] [bit] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskHistory]    Script Date: 29/1/2025 01:03:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TasksHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[task_id] [int] NOT NULL,
	[state_id] [int] NOT NULL,
	[changed_date] [datetime] NOT NULL,
 CONSTRAINT [PK_TaskHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_State]  DEFAULT ((1)) FOR [state_id]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_State] FOREIGN KEY([state_id])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_State]
GO
ALTER TABLE [dbo].[TaskHistory]  WITH CHECK ADD  CONSTRAINT [FK_TaskHistory_State] FOREIGN KEY([state_id])
REFERENCES [dbo].[State] ([Id])
GO
ALTER TABLE [dbo].[TaskHistory] CHECK CONSTRAINT [FK_TaskHistory_State]
GO
ALTER TABLE [dbo].[TaskHistory]  WITH CHECK ADD  CONSTRAINT [FK_TaskHistory_Task] FOREIGN KEY([task_id])
REFERENCES [dbo].[Task] ([Id])
GO
ALTER TABLE [dbo].[TaskHistory] CHECK CONSTRAINT [FK_TaskHistory_Task]
GO
