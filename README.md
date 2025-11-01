# openCV - Canlı İşleme

Kısa açıklama
- Bu proje, OpenCvSharp kullanarak Windows Forms (WinForms) üzerinde canlı kamera akışı yakalayıp ekranda gösterir, basit görüntü işleme (grayscale, Canny, threshold, adaptive threshold, blur) uygular ve işlenmiş kareleri kaydetme imkânı sağlar.

Özellikler
- Canlı kamera akışı başlatma / durdurma.
- Gerçek zamanlı işlem modları: `Grayscale`, `Canny`, `Threshold`, `AdaptiveThreshold`, `Blur`.
- Modlara göre dinamik parametre kontrolü (ör. `Düşük Eşik`, `Yüksek Eşik`).
- Orijinal ve işlenmiş görüntülerin yan yana gösterimi.
- Kare kaydetme (PNG / JPEG).
- Basit FPS gösterimi.

Gereksinimler
- Windows
- .NET 8 (hedef framework: `net8.0-windows`)
- Visual Studio 2022 (veya `dotnet` CLI)
- NuGet paketleri:
  - `OpenCvSharp4`
  - `OpenCvSharp4.Extensions`
  - `OpenCvSharp4.runtime.win`

Kurulum ve çalıştırma
1. Depoyu açın (Visual Studio ile veya terminalde).
2. Paketleri yükleyin / geri yükleyin:
   - Visual Studio: Solution üzerinde sağ tıkl → `Restore NuGet Packages`
   - CLI:
     ```
     dotnet restore
     dotnet build
     dotnet run --project openCV
     ```
3. Visual Studio kullanıyorsanız `openCV` projesini açın ve `Build > Rebuild Solution` ardından `Start` (F5) ile çalıştırın.
4. (Öneri) OpenCvSharp native kütüphaneleri düzgün çalışsın diye proje platform hedefinizi (x64) kontrol edin: __Build__ → __Configuration Manager__ → Platform: `x64` (gerektiğinde değiştirin).

Arayüz ve kullanım (kısa)
- `Kamera Başlat` — seçili kamera indeksini (`Kamera #`) kullanarak kamerayı açar.
- `Kamera Durdur` — akışı durdurur ve kaynakları serbest bırakır.
- `Kare Kaydet` — son işlenmiş kareyi kaydeder (SaveFileDialog ile hedef seçilir; varsayılan ad: `snapshot.png`).
- `İşlem Seç` (açılır menü) — işleme modunu seçin:
  - `Canny` → `Düşük Eşik` ve `Yüksek Eşik`
  - `Threshold` → `Eşik`
  - `AdaptiveThreshold` → `Blok Boyutu (tek)` ve `C değeri`
  - `Blur` → `Filtre Boyutu (tek)`
  - `Grayscale` / `None` → parametre gerekmez
- `FPS` — uygulamanın gösterim hızını gösterir (saniyedeki kare sayısı).

Neden `Düşük Eşik` / `Yüksek Eşik` var?
- `Canny` bir kenar algılama algoritmasıdır. İki eşik değeri girilir:
  - `Düşük Eşik` (lower threshold) — zayıf kenarları belirler,
  - `Yüksek Eşik` (upper threshold) — güçlü kenarları belirler.
- Diğer modların anlamları: `Eşik` (binary threshold), `Blok Boyutu` ve `C` (adaptive threshold), `Filtre Boyutu` (Gaussian blur çekirdeği).

Önemli notlar / hata giderme
- "Parameter is not valid" hatası: eski versiyonlarda GDI+ görüntü kopyalama/Dispose kaynaklı hatalardan çıkıyordu. Bu proje güvenli kopyalama ve `lastProcessedBitmap` saklama yöntemi ile bu durumu azaltır. Hâlen hata alırsanız:
  - Projeyi temizleyip (`Build > Clean`) yeniden derleyin.
  - `OpenCvSharp4.runtime.win` paketinin yüklü ve hedef platformunuzun uygun (x64 veya x86) olduğundan emin olun.
  - Başka bir uygulamanın kamerayı kullanmadığından emin olun.
- `TextBox` / `PictureBox` hatası: Designer içinde `pbOriginal` ve `pbProcessed` `PictureBox` olarak tanımlı olmalıdır. Eğer `TextBox` ise `Image` özelliği yoktur — `Form1.Designer.cs` kontrol edin.
- `Dispose` çakışmaları: `Dispose(bool)` yönteminin duplicate tanımı olursa derleme hatası olur. Otomatik designer `Dispose`'ı içerir; ek `Dispose` override'ı `Form1.cs` içinde bulunuyorsa kaldırın ve kaynak temizliğini `FormClosing` içinde yapın.

Geliştirme ipuçları
- Performans arttırmak için işleme (OpenCV) ve yakalamayı UI thread dışında bir `Task`/Thread içinde çalıştırıp sadece UI güncellemesini `Invoke` ile yapabilirsiniz.
- Kamera listesini numeric değer yerine insan okuyabileceği device isimleriyle göstermek için `DirectShowLib` veya MediaFoundation ile cihazları enumerate edebilirsiniz.
- Yeni bir işleme modu eklemek için:
  1. `cbMode` içine yeni mod adını ekleyin.
  2. `ProcessFrame(Mat src)` metoduna yeni case ekleyin ve uygun OpenCvSharp işlemini uygulayın.
  3. `UpdateControlsVisibility()` içinde o moda uygun parametre etiketleri/limitlerini ayarlayın.

Önemli dosyalar (ilk bakış için)
- `openCV/Form1.cs` — uygulama mantığı, görüntü yakalama, işleme, kaydetme.
- `openCV/Form1.Designer.cs` — form kontrollerinin tanımı.
- `openCV/openCV.csproj` — proje ayarları ve NuGet bağımlılıkları.
- `openCV/Program.cs` — uygulama giriş noktası.

Geliştiriciler için kurulum (kısa komutlar)
