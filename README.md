Source code BTL lớp 2425II_INT3505E_1_Kien-Truc-Huong-Dich-Vu

----------------------------------------------------------------------------------
BACKEND

- DotNet SDK 8.0: https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.407-windows-x64-installer

1. Tạo database trong MySQL
   - B1: Mở thư mục PastebinBackend/PastebinBackend
   - B2: Tìm file appsetting.json -> Sửa lại trường DefaultConnection theo thông tin MySQL trên máy (nếu cần)
   - B3: Mở cmd, chạy lệnh "dotnet ef database update" -> Hiện "done" là thành công
2. Cách chạy Backend
   - B1: Tại thư mục PastebinBackend/PastebinBackend, chạy file "Run BackEnd.bat", thấy thông báo chạy trên cổng 5229 là OK

- Các hàm của BackEnd:
   - Tạo và lưu mã paste mới:
      - Url: http://localhost:5229/Paste/CreatePaste
      - Method: POST
      - Body:
         - content: Nội dung văn bản cần tạo mã
         - exposure: Quyền riêng tư mã paste (0: Public, 1: Private)
         - expiresAt: Thời gian hết hạn của mã paste (nếu có). vd: "2025-12-31 20:00:00"
         - pasteName: Tên mã paste (nếu có)
      - Trả về: Mã paste được tạo (16 ký tự)
   - Lấy dữ liệu mã paste:
      - Url: http://localhost:5229/Paste/GetPasteContent?pasteKey={mã paste}
      - Method: GET
      - Trả về: Dữ liệu của mã paste
   - Lấy danh sách mã paste public được tạo gần đây:
      - Url: http://localhost:5229/Paste/GetRecentPastes
      - Method: GET
      - Trả về: Danh sách mã paste, tối đa 10 bản ghi
   - Sửa mã paste:
      - Url: http://localhost:5229/Paste/UpdatePaste
      - Method: POST
      - Body:
         - pasteKey: Mã paste
         - content: Nội dung văn bản cần tạo mã
         - exposure: Quyền riêng tư mã paste (0: Public, 1: Private)
         - expiresAt: Thời gian hết hạn của mã paste (nếu có). vd: "2025-12-31 20:00:00"
         - pasteName: Tên mã paste (nếu có)
      - Trả về: Trạng thái cập nhật
   - Xoá mã paste:
      - Url: http://localhost:5229/Paste/DeletePaste
      - Method: POST
      - Body:
         - pasteKey: Mã paste
      - Trả về: Trạng thái xoá


FRONTEND
1. Cách chạy frontend:
 - B1: dùng terminal cd đến folder pastebin-frontend rồi chạy lệnh npm i
 - B2: chạy lệnh npm run dev, thấy thông báo chạy trên port 5173 là ok
