# Gói Con Tim Làm Quà - Dành Tặng Huyền ❤️ (C# & Web Edition)

Dự án tái hiện trọn vẹn và nâng cấp hiệu ứng tỏ tình lập trình viên từ video TikTok [Kỹ Sư 4.0 (@ks40m)](https://www.tiktok.com/@ks40m/video/7682039986950180103) dành riêng tặng cho Huyền.

---

## 🌟 Tính Năng Nổi Bật

1. **Hiệu ứng Pixel Art Retro đồng bộ 100% với giai điệu bài hát:**
   - Bầu trời đêm với hàng chục ngôi sao lấp lánh và mặt trăng khuyết huyền ảo.
   - Nhân vật bạn nam (Hiếu - màu đỏ cam đội mũ xanh) và bạn nữ (Huyền - màu hồng cài nơ vàng).
   - Lời thoại mở đầu: *"Hiếu thích Huyền"*, *"Huyền cười đẹp lắm"*, *"Huyền đồng ý nhé?"*.
   - Bạn nữ giơ nhành hoa hướng dương (`🌻`) ở câu *"Em trao nhành hoa..."*.
   - Bạn nam nâng niu trái tim rực đỏ đập thình thịch (`💖`) và nói: *"Hiếu gói tim tặng Huyền"*.
   - Nhành hoa úa tàn theo thời gian ở *"Sợ hoa sẽ tàn úa..."*.
   - Bạn nam bước lại gần và nhắn gửi: *"về đây với Hiếu"*.
   - Hai nhân vật bước lại gần và ôm chặt nhau trong vòng tay ấm áp (*"Hiếu giữ Huyền bên trong vòng tay"*).
   - Bạn nam hỏi: *"ở bên Hiếu mãi nhé?"*.
   - Bạn nữ đồng ý: *"ừ, Huyền đồng ý"* cùng màn pháo hoa tim rực rỡ!

2. **Khung Debugger Code IDE (Chuẩn VS Code / JetBrains):**
   - Đầy đủ 17 dòng mã async/await tỏ tình cùng tên Huyền:
     ```javascript
     async function goiConTim(Huyền) {
       const chanThanh = gather(20);
       const moi = await doi(chanThanh);
       if (!Huyền.dongY) return cho();

       const hoa = await Huyền.trao("hoa");
       const qua = goi(conTim, hoa);

       while (hoa.tanUa) {
         qua.nhipDap += tick();
       }

       await Huyền.veDay();
       const tay = giu(Huyền, chatHon);
       if (cachRoi) return khong(tay);
       return nangNiu(Huyền, suotDoi);
     }
     ```
   - Con trỏ debugger `▶` màu xanh lướt từng dòng code khớp chuẩn xác với từng câu hát.
   - Tô màu cú pháp (Syntax Highlighting) chuyên nghiệp: từ khóa, tên hàm, chuỗi, biến số.

3. **Thanh Marquee Ticker & Sticker Bình luận TikTok:**
   - Dòng chữ chạy ngang mượt mà: `Gói Con Tim Làm Quà • Hiếu ❤️ Huyền • Phạm Minh Hiếu • karaoke mode...`
   - Sticker bình luận TikTok góc trên: `Reply to @huyen's comment: if (Huyền đồng ý)`.

4. **Tính Năng Tùy Chỉnh Riêng Cho Crush:**
   - Bấm phím **C** (hoặc nút **Tùy chỉnh**): Có thể chỉnh sửa nhanh tên người nhận, lời ngỏ ý và câu đồng ý bất kỳ lúc nào.

---

## 🚀 Cách Sử Dụng

### 1. Bản C# Desktop App (Windows Native):
- **Chạy ngay:** Nhấp đúp vào [run.bat](file:///c:/Users/phamm/Downloads/GOI%20CON%20TIM%20LAM%20QUA/run.bat) hoặc [GoiConTimLamQua.exe](file:///c:/Users/phamm/Downloads/GOI%20CON%20TIM%20LAM%20QUA/GoiConTimLamQua.exe).
- **Biên dịch lại từ mã nguồn:** Nhấp đúp vào [build.bat](file:///c:/Users/phamm/Downloads/GOI%20CON%20TIM%20LAM%20QUA/build.bat) (Sử dụng trình biên dịch `csc.exe` có sẵn trong Windows .NET Framework, không cần cài đặt thêm bất kỳ SDK nào).

### 2. Bản Web App (Chạy trên mọi trình duyệt / Điện thoại):
- **Mở ngay:** Nhấp đúp vào [start_web.bat](file:///c:/Users/phamm/Downloads/GOI%20CON%20TIM%20LAM%20QUA/start_web.bat) hoặc mở trực tiếp [index.html](file:///c:/Users/phamm/Downloads/GOI%20CON%20TIM%20LAM%20QUA/index.html).
- Hỗ trợ đổi giao diện Khung Phone / Mở rộng, thanh tua nhạc tương tác, nút phát/dừng lớn, và chế độ toàn màn hình.

---

## ⌨️ Phím Tắt Tiện Ích

| Phím tắt | Chức năng |
| :--- | :--- |
| **Space** | Tạm dừng / Tiếp tục phát nhạc & hiệu ứng |
| **R** | Phát lại từ đầu (00:00) |
| **T** hoặc **Ctrl + T** | Bật / Tắt phụ đề lời bài hát (Karaoke mode) |
| **C** | Mở hộp thoại tùy chỉnh tên Crush & Lời thoại |
| **F** hoặc **F11** | Chế độ toàn màn hình (Full Screen) |
| **Mũi tên Trái / Phải** | Tua lùi / tiến 3 giây |
| **Click vào thanh tiến trình** | Nhảy đến bất kỳ thời điểm nào trong bài hát |
