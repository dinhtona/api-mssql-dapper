# api-mssql-dapper

WebAPI using DotNet5.0 with MSSQL Server and Dapper

Cách sử dụng:

1.  Tải xuống project và chạy câu lệnh sau trong VS Code Terminal:

    dotnet restore

2.  Mở file "Create DB" trong thư mục SQLFile bằng Microsoft SQl Server Management Studio và nhấn F5 hoặc Excute để khởi tạo database.

3.  Trong file appsettings.json, chỉnh sửa các thông số:

        "SQLSettings":
        {
            "Host":"localhost", //Your server instance
            "DBName":"DEMO_DB", //your Db name
            "User":"sa" //your User name login to server
            //Password: *** //password will be saved in secret
        }

4.  Khởi tạo giá trị cho password một cách bí mật. Visual code Terminal:

    dotnet user-secrets init
    dotnet user-secrets set SQLSettings:Password your_password_login_to_server

5.  Run with Visual code Terminal:

    dotnet run

Youtube:
https://www.youtube.com/watch?v=Eq4lZ5OD1As

Website:
https://laptrinhvb.net/bai-viet/thiet-ke-web/--dotNet5-0---Web-API---MSSQL-Server---Dapper/8bd617715fd38559.html
