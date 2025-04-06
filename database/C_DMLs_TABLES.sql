USE [ToDoListDB]
GO

--DMLS Table State:

INSERT INTO [dbo].[States]([Id],[name]) VALUES('1','Pending')
INSERT INTO [dbo].[States]([Id],[name]) VALUES('2','Completed')
INSERT INTO [dbo].[States]([Id],[name]) VALUES('3','Archived')


--DMLS Table Task:

INSERT INTO [dbo].[Tasks]([name],[state_id],[initial_date],[finish_date]) VALUES('Llamar primos',1,'2025-01-26 01:46:00.000','2025-02-02 01:46:00.000', NULL)
INSERT INTO [dbo].[Tasks]([name],[state_id],[initial_date],[finish_date]) VALUES('Salir a correr #1',1,'2025-01-26 01:46:00.000','2025-02-02 01:47:00.000', NULL)
INSERT INTO [dbo].[Tasks]([name],[state_id],[initial_date],[finish_date]) VALUES('Salir a correr #2',1,'2025-01-26 01:47:00.000','2025-02-02 01:47:00.000', NULL)
INSERT INTO [dbo].[Tasks]([name],[state_id],[initial_date],[finish_date]) VALUES('Corte pelo',1,'2025-01-26 01:47:00.000','2025-02-02 01:47:00.000', NULL)

--DMLS Table TaskHistory:

INSERT INTO [dbo].[TasksHistory]([task_id],[state_id],[changed_date]) VALUES('1','3','2025-01-28 17:02:20.140')
INSERT INTO [dbo].[TasksHistory]([task_id],[state_id],[changed_date]) VALUES('2','3','2025-01-28 17:02:20.140')
INSERT INTO [dbo].[TasksHistory]([task_id],[state_id],[changed_date]) VALUES('3','3','2025-01-28 17:02:20.140')

--DMLS Table Users:

INSERT INTO [dbo].[Users]([name],[password])VALUES('MWXA2582', 'XXGPTMRUtEbQL+dv2gwZsOk7HU+thZ3Gyx6lQ5VqW3Q=')
INSERT INTO [dbo].[Users]([name],[password])VALUES('REXA2577', 'XXGPTMRUtEbQL+dv2gwZsOk7HU+thZ3Gyx6lQ5VqW3Q=')
