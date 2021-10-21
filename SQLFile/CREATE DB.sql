--Open this file in Microsoft SQL Server Management Studio
--And Press Excute button above
    
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'DEMO_DB')
  BEGIN
    CREATE DATABASE DEMO_DB


    END
    GO
       USE DEMO_DB
    GO
------------------------
---some common proc we used
CREATE  PROC [dbo].[sp_generate_class] @tableName varchar(200)
AS
BEGIN

declare @Result varchar(max) = 'public class ' + @TableName + '
{'

select @Result = @Result + '
    public ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }
'
from
(
    select 
        replace(col.name, ' ', '_') ColumnName,
        column_id ColumnId,
        case typ.name 
            when 'bigint' then 'long'
            when 'binary' then 'byte[]'
            when 'bit' then 'bool'
            when 'char' then 'string'
            when 'date' then 'DateTime'
            when 'datetime' then 'DateTime'
            when 'datetime2' then 'DateTime'
            when 'datetimeoffset' then 'DateTimeOffset'
            when 'decimal' then 'decimal'
            when 'float' then 'double'
            when 'image' then 'byte[]'
            when 'int' then 'int'
            when 'money' then 'decimal'
            when 'nchar' then 'string'
            when 'ntext' then 'string'
            when 'numeric' then 'decimal'
            when 'nvarchar' then 'string'
            when 'real' then 'double'
            when 'smalldatetime' then 'DateTime'
            when 'smallint' then 'short'
            when 'smallmoney' then 'decimal'
            when 'text' then 'string'
            when 'time' then 'TimeSpan'
            when 'timestamp' then 'DateTime'
            when 'tinyint' then 'byte'
            when 'uniqueidentifier' then 'Guid'
            when 'varbinary' then 'byte[]'
            when 'varchar' then 'string'
            else 'UNKNOWN_' + typ.name
        end ColumnType,
        case 
            when col.is_nullable = 1 and typ.name in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifier') 
            then '?' 
            else '' 
        end NullableSign
    from sys.columns col
        join sys.types typ on
            col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id
    where object_id = object_id(@TableName)
) t
order by ColumnId

set @Result = @Result  + '
}'

PRINT  @Result
END
GO 
---------
CREATE  PROC [dbo].[USP_Lazy_GenerateParamester4Proc] @tableName VARCHAR(200)
AS
BEGIN
SELECT 

CASE 
	WHEN t.Name LIKE '%char' THEN 
	 '@' + c.name + ' ' + t.name + '(' +CAST(c.max_length AS VARCHAR(max))+ '),' 
	 ELSE
      '@' + c.name + ' ' + t.name + ','
END AS 'fullDatatype',

    c.name  as 'Column Name' ,

	'@' + c.name + ',' as N'Biến' ,

	  c.name + '=@' + c.name + ',' as N'Gán' ,

    t.Name 'Data type',
    c.max_length 'Max Length',
    c.precision ,
    c.scale ,
    c.is_nullable,
    ISNULL(i.is_primary_key, 0) 'Primary Key'
FROM    
    sys.columns c
INNER JOIN 
    sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN 
    sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN 
    sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
WHERE
    c.object_id = OBJECT_ID(@tableName)
END
GO 
------------------end-----
CREATE  proc [dbo].[USP_MakeResponse](@status varchar(50), @description nvarchar(1000), @jsonData nvarchar(max)='')
as
begin
	select @status Status, @description as Description,  @jsonData AS Data
END




--You need to check if the table exists
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tbl_users' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[tbl_users](
		[id] [INT] IDENTITY(1,1) NOT NULL,
		[f_name] [NVARCHAR](50) NOT NULL,
		[l_name] [NVARCHAR](50) NOT NULL,
		[dob] [DATE] NULL,
		[email] [NVARCHAR](200) NULL,
		[phone] [VARCHAR](20) NULL,
		[created_date] [DATETIME] NULL,
		[created_user] [VARCHAR](50) NULL,
		[updated_date] [DATETIME] NULL,
		[updated_user] [VARCHAR](50) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[tbl_users] ADD  DEFAULT (GETDATE()) FOR [created_date]

	INSERT INTO dbo.tbl_users
	(
		f_name,
		l_name,
		dob,
		email,
		phone,
		created_user
	)
	VALUES
	( 'TONA','DINH', '2021-01-01','dinhtona@gmail.com','84908697365', 'ROOT' ),
	( 'MEO','THAO', '2021-01-01','dinhtona@gmail.com','84908697365', 'ROOT' ),
	( 'KHANG','QUOC', '2021-01-01','dinhtona@gmail.com','84908697365', 'ROOT' ),
	( 'THAO','NGO', '2021-01-01','dinhtona@gmail.com','84908697365', 'ROOT' ),
	( 'MESSI','LEONEL', '2021-01-01','dinhtona@gmail.com','84908697365', 'ROOT' ),
	( 'RONALDO','CRITIANO', '2021-01-01','dinhtona@gmail.com','84908697365', 'ROOT' ),
	( 'NEYMAR','JR', '2021-01-01','dinhtona@gmail.com','84908697365', 'ROOT' )
END



SELECT * FROM dbo.tbl_users

GO
--------------------end common------------------------


CREATE PROC USP_User_Get @id VARCHAR(MAX)=''--@id=2 || '1,5,6,4,8, ...' || ''
AS
BEGIN
	SELECT * FROM tbl_users WHERE exists 
	(SELECT  value FROM STRING_SPLIT(IIF(ISNULL(@id,'')='', CONCAT(id,''), @id), ',')
	WHERE ltrim(rtrim(value)) = id)
END 

GO

--EXEC USP_User_Get '2,9'

CREATE  PROC USP_User_Save 
(
	@id int,
	@f_name varchar(100),
	@l_name varchar(100),
	@dob date,
	@username VARCHAR(50),
	@email varchar(200),
	@phone varchar(20)
)
AS
BEGIN
	DECLARE @isExists INT=0, @des NVARCHAR(1000)
	SELECT @isExists=COUNT(*) FROM tbl_users WHERE id=@id;
	IF(@isExists>0)
		BEGIN
			UPDATE dbo.tbl_users
			SET 
			f_name=@f_name,
			l_name=@l_name,
			dob=@dob,
			updated_user=@username,
			updated_date=GETDATE(),
			email=@email,
			phone=@phone
			WHERE id=@id;

			SET @des=CONCAT(N'Updated Successfully user: ', @f_name )
			EXEC dbo.USP_MakeResponse @status = 'OK',      -- varchar(50)
			                          @description = @des -- nvarchar(1000)
			
		END
	ELSE
		BEGIN
			INSERT INTO dbo.tbl_users
			(
			    f_name,
			    l_name,
			    dob,			    
			    created_user,
			    created_date,
			    email,
			    phone
			)
			VALUES
			(  @f_name,
				@l_name,				
				@dob,				
				@username,
				GETDATE(),
				@email,
				@phone
			 )

			SET @des=CONCAT(N'Added Successfully user: ', @f_name )
			EXEC dbo.USP_MakeResponse @status = 'OK',      -- varchar(50)
			                          @description = @des -- nvarchar(1000)
		END

END

GO

CREATE  PROC [dbo].[USP_User_Delete] @id INT
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	   DELETE FROM dbo.tbl_users WHERE id=@id;	
	   
	   EXEC dbo.USP_MakeResponse @Status = 'OK',      -- varchar(50)
	                             @Description = N'Deleted !' -- nvarchar(1000)

	COMMIT
	END TRY
	BEGIN CATCH
	   ROLLBACK
	   DECLARE @ErrorMessage NVARCHAR(2000)
	   SELECT @ErrorMessage = N'Error: ' + ERROR_MESSAGE()
	   EXEC dbo.USP_MakeResponse @Status = 'ERROR',      -- varchar(5)
	                                  @Description =@ErrorMessage -- nvarchar(max)	   
	   --RAISERROR(@ErrorMessage, 16, 1)
	END CATCH
END
GO 

EXEC sp_generate_class 'tbl_users'
EXEC [USP_Lazy_GenerateParamester4Proc] 'tbl_users'
GO 
SELECT * FROM dbo.tbl_users
GO 

go
create proc Common._GenerateClass4MVC(@NameSpace varchar(max), @tableSchema VARCHAR(MAX),  @tableName varchar(max))
as
begin

    DECLARE @result varchar(max) = ''

    SET @result = @result + 'using System;' + CHAR(13)
    SET @result = @result + 'using System.ComponentModel.DataAnnotations;' + CHAR(13) + CHAR(13) 

    IF (@TableSchema IS NOT NULL) 
    BEGIN
        SET @result = @result + 'namespace ' + @NameSpace  + CHAR(13) + '{' + CHAR(13) 
    END

    SET @result = @result + '
	[Table("'+@tableSchema+'.'+@TableName+'")]
	public class ' + @TableName + CHAR(13) + '    {' + CHAR(13) 

    SET @result = @result + '        #region Instance Properties' + CHAR(13)  

    SELECT @result = @result + CHAR(13)     
        + '        [Display(Name = "' + ColumnName + '")] ' + CHAR(13) 
        + CASE bRequired WHEN 'NO' 
        THEN 
        CASE WHEN Len(MaxLen) > 0 THEN '        [Required, StringLength(' + MaxLen + ')]' + CHAR(13) ELSE '        [Required] ' + CHAR(13)  END   
        ELSE
        CASE WHEN Len(MaxLen) > 0 THEN '        [StringLength(' + MaxLen + ')]' + CHAR(13) ELSE '' END  
        END
		+case DATA_TYPE when 'date'  then '        [Column(TypeName = "date")]' else '' end 
        + '
		public ' + ColumnType + ' ' + ColumnName + ' { get; set; } ' + CHAR(13) 
    FROM
    (
        SELECT  c.COLUMN_NAME   AS ColumnName 
            , c.DATA_TYPE ,
			CASE c.DATA_TYPE   
                WHEN 'bigint' THEN
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Int64?' ELSE 'Int64' END
                WHEN 'binary' THEN 'Byte[]'
                WHEN 'bit' THEN 
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Boolean?' ELSE 'Boolean' END            
                WHEN 'char' THEN 'String'
                WHEN 'date' THEN
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                        
                WHEN 'datetime' THEN
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                        
                WHEN 'datetime2' THEN  
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                        
                WHEN 'datetimeoffset' THEN 
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'DateTimeOffset?' ELSE 'DateTimeOffset' END                                    
                WHEN 'decimal' THEN  
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Decimal?' ELSE 'Decimal' END                                    
                WHEN 'float' THEN 
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Single?' ELSE 'Single' END                                    
                WHEN 'image' THEN 'Byte[]'
                WHEN 'int' THEN  
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Int32?' ELSE 'Int32' END
                WHEN 'money' THEN
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Decimal?' ELSE 'Decimal' END                                                
                WHEN 'nchar' THEN 'String'
                WHEN 'ntext' THEN 'String'
                WHEN 'numeric' THEN
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Decimal?' ELSE 'Decimal' END                                                            
                WHEN 'nvarchar' THEN 'String'
                WHEN 'real' THEN 
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Double?' ELSE 'Double' END                                                                        
                WHEN 'smalldatetime' THEN 
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                                    
                WHEN 'smallint' THEN 
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Int16?' ELSE 'Int16'END            
                WHEN 'smallmoney' THEN  
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Decimal?' ELSE 'Decimal' END                                                                        
                WHEN 'text' THEN 'String'
                WHEN 'time' THEN 
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'TimeSpan?' ELSE 'TimeSpan' END                                                                                    
                WHEN 'timestamp' THEN 
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                                    
                WHEN 'tinyint' THEN 
                    CASE C.IS_NULLABLE
                        WHEN 'YES' THEN 'Byte?' ELSE 'Byte' END                                                
                WHEN 'uniqueidentifier' THEN 'Guid'
                WHEN 'varbinary' THEN 'Byte[]'
                WHEN 'varchar' THEN 'String'
                ELSE 'Object'
            END AS ColumnType,
                c.IS_NULLABLE AS bRequired,
                CASE c.DATA_TYPE             
                WHEN 'char' THEN  CONVERT(varchar(10),c.CHARACTER_MAXIMUM_LENGTH)
                WHEN 'nchar' THEN  CONVERT(varchar(10),c.CHARACTER_MAXIMUM_LENGTH)
                WHEN 'nvarchar' THEN  CONVERT(varchar(10),c.CHARACTER_MAXIMUM_LENGTH)
                WHEN 'varchar' THEN  CONVERT(varchar(10),c.CHARACTER_MAXIMUM_LENGTH)
                ELSE ''
            END AS MaxLen,
            c.ORDINAL_POSITION 
    FROM    INFORMATION_SCHEMA.COLUMNS c
    WHERE   c.TABLE_NAME = @TableName and ISNULL(@TableSchema, c.TABLE_SCHEMA) = c.TABLE_SCHEMA  
    ) t
    ORDER BY t.ORDINAL_POSITION

    SET @result = @result + CHAR(13) + '        #endregion Instance Properties' + CHAR(13)  

    SET @result = @result  + '    }' + CHAR(13)

    IF (@TableSchema IS NOT NULL) 
    BEGIN
        SET @result = @result + CHAR(13) + '}' 
    END

    PRINT @result

end
