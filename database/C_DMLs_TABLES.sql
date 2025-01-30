USE [ToDoListDB]
GO

--DMLS Tabla State:

INSERT INTO [dbo].[State]([Id],[name]) VALUES('1','Pending')
INSERT INTO [dbo].[State]([Id],[name]) VALUES('2','Completed')
INSERT INTO [dbo].[State]([Id],[name]) VALUES('3','Archived')


--DMLS Tabla Task:

INSERT INTO [dbo].[Task]([name],[state_id],[initial_date],[finish_date]) VALUES('Llamar primos',1,'2025-01-26 01:46:00.000','2025-02-02 01:46:00.000', NULL)
INSERT INTO [dbo].[Task]([name],[state_id],[initial_date],[finish_date]) VALUES('Salir a correr #1',1,'2025-01-26 01:46:00.000','2025-02-02 01:47:00.000', NULL)
INSERT INTO [dbo].[Task]([name],[state_id],[initial_date],[finish_date]) VALUES('Salir a correr #2',1,'2025-01-26 01:47:00.000','2025-02-02 01:47:00.000', NULL)
INSERT INTO [dbo].[Task]([name],[state_id],[initial_date],[finish_date]) VALUES('Corte pelo',1,'2025-01-26 01:47:00.000','2025-02-02 01:47:00.000', NULL)

--DMLS Tabla TaskHistory:

INSERT INTO [dbo].[TaskHistory]([task_id],[state_id],[changed_date]) VALUES('1','3','2025-01-28 17:02:20.140')
INSERT INTO [dbo].[TaskHistory]([task_id],[state_id],[changed_date]) VALUES('2','3','2025-01-28 17:02:20.140')
INSERT INTO [dbo].[TaskHistory]([task_id],[state_id],[changed_date]) VALUES('3','3','2025-01-28 17:02:20.140')

