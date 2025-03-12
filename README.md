Source code BTL lớp 2425II_INT3505E_1_Kien-Truc-Huong-Dich-Vu

----------------------------------------------------------------------------------
BACKEND

- DotNet SDK 8.0: https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.407-windows-x64-installer

1. Tạo database trong MySQL
   - B1: Mở thư mục PastebinBackend/PastebinBackend
   - B2: Tìm file appsetting.json -> Sửa lại trường DefaultConnection theo thông tin MySQL trên máy (nếu cần)
   - B3: Mở cmd, chạy lệnh "dotnet ef database update" -> Hiện "done" là thành công
2. Cách chạy Backend
   - B1: Tại thư mục PastebinBackend/PastebinBackend, mở CMD
   - B2: Chạy lệnh "dotnet run" -> Thấy hiện chạy trên cổng 5229 là OK
