INSERT INTO dbo.Sample
    (CreatedBy,CreateTime,UpdatedBy,UpdateTime,UpdateCount,Code,Name,Description,SampleType) 
VALUES
	 (N'system',GETDATE(),NULL,NULL,0,N'Code1',N'Name1',N'Description1',1),
  (N'system',GETDATE(),NULL,NULL,0,N'Code2',N'Name2',N'Description2',1)