# CLAUDE.md

> Bu dosya bu projede Claude Code'un nasıl davranması gerektiğini anlatır. **Her zaman Türkçe konuş, kısa ve öz ol.** Kod ingilizce, sohbet türkçe.

---

## 1. Genel Davranış Kuralları

- **Asla kafadan iş yapma.** Eksik bilgi varsa SOR. Tahmin etme, varsayma.
- **Kısa ve öz cevap ver.** Açıklama paragrafı yazma, gerektiği kadar konuş.
- Kod yazmadan önce kullanıcıya planı doğrula. Onay almadan birden fazla dosya oluşturma.
- Kullanıcı bir entity verirse, ona ait DTO/Interface/Service/Controller'ı **otomatik üretme** — adım adım iste.
- Mevcut dosyalardaki naming, pattern, code style'a **birebir uy**. Kendinden bir şey ekleme.
- Yorum satırı ekleme (kullanıcı istemedikçe).

---

## 2. Proje Yapısı (Clean Architecture + MVC)

```
E-StoreMVCTemplate.Domain          → Entities, Enums (saf POCO)
E-StoreMVCTemplate.Application     → DTOs, Interfaces (sözleşmeler)
E-StoreMVCTemplate.Infrastructure  → Data (AppDbContext), Services (impl)
E-StoreMVCTemplate.Web             → Controllers, Views, ViewComponents, wwwroot
```

**Bağımlılık yönü:** Web → Infrastructure → Application → Domain
Domain hiçbir şeye bağlı değil. Application sadece Domain'i bilir.

---

## 3. Geliştirme Sırası (KESİNLİKLE BU SIRAYLA)

Yeni bir feature/entity eklerken **her zaman** şu sırayla ilerlenir. Bir adımı atlama, sırayı bozma:

1. **Entity** → `Domain/Entities/`
2. **DbSet + Fluent Config** → `Infrastructure/Data/AppDbContext.cs`
3. **DTO'lar** → `Application/DTOs/{EntityName}/`
4. **Interface (method imzaları)** → `Application/Interfaces/I{Entity}Service.cs`
5. **Service (implementasyon)** → `Infrastructure/Services/{Entity}Service.cs`
6. **DI Kaydı** → `Web/Program.cs`
7. **Controller** → `Web/Controllers/{Entity}Controller.cs`
8. **View** → `Web/Views/{Entity}/`

Kullanıcı "Cart için DTO yaz" derse → sadece DTO yaz, Service'e geçme.
Kullanıcı "Service yaz" derse → Interface'in hazır olduğunu doğrula, sonra yaz.

---

## 4. Naming Conventions

### DTO'lar
Pattern: **`{Action}{Entity}Dto`** veya **`{Entity}{Context}Dto`**

| Amaç | Örnek |
|---|---|
| Ekleme | `AddToCartDto`, `CreateProductDto` |
| Listeleme | `CartListDto`, `ProductListDto` |
| Detay/Tek kayıt | `CartByIdDto`, `ProductDetailDto` |
| Güncelleme | `UpdateProductDto` |

### Interface
`I{Entity}Service` → `ICartService`, `IProductService`

### Service
`{Entity}Service : I{Entity}Service`

### Controller
`{Entity}Controller : Controller`

---

## 5. Service Yazım Kuralları

- Constructor injection ile `AppDbContext` alınır.
- **Exception fırlat** — null/return ile sessiz geçme. Ürün/cart bulunamazsa `throw new Exception("...")` veya custom exception.
- `async/await` her zaman, `Task` dönüş tipi.
- EF Core: `Include` → `ThenInclude` zinciriyle ilişkili veriler çekilir.
- Liste döndüren methodlar `Select` ile DTO'ya proje edilir, `Entity` dışarı sızdırılmaz.

**Şablon:**
```csharp
public class XService : IXService
{
    private readonly AppDbContext _context;

    public XService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<XListDto>> GetAllAsync()
    {
        return await _context.Xs
            .Select(x => new XListDto { ... })
            .ToListAsync();
    }
}
```

---

## 6. Controller Yazım Kuralları

- `Controller` base class'tan türetilir (API değil, MVC).
- Service constructor injection ile alınır.
- Action'lar `IActionResult` veya `Task<IActionResult>` döner.
- `View(model)` ile view'a model gönderilir.

**Şablon:**
```csharp
public class XController : Controller
{
    private readonly IXService _xService;

    public XController(IXService xService)
    {
        _xService = xService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Detail(int id)
    {
        var data = await _xService.GetByIdAsync(id);
        return View(data);
    }
}
```

---

## 7. AppDbContext Kuralları

- `IdentityDbContext<AppUser>` extend edilir.
- Yeni entity → `DbSet<X>` eklenir.
- İlişki kurulurken `OnModelCreating` içinde Fluent API kullanılır.
- Cascade döngüsü riskine dikkat: cycle ihtimali olan ilişkilerde `DeleteBehavior.NoAction` veya `SetNull`.
- Decimal alanlar otomatik `decimal(18,2)` (mevcut config korunur).

---

## 8. DI (Program.cs)

Yeni service eklendiğinde unutmadan kaydet:
```csharp
builder.Services.AddScoped<IXService, XService>();
```

---

## 9. Cevap Formatı

- Kod bloğu ver, üstüne 1-2 cümle açıklama yaz, altına gereksiz özet **yazma**.
- Birden fazla dosya gerekiyorsa → önce kullanıcıya sor: "Önce hangisini yazayım?"
- Hata/eksik gördüğünde → düzeltmeden önce belirt, kullanıcı onaylasın.
- Belirsizlik varsa **mutlaka soru sor** (entity ilişkisi, alan tipi, business rule vs).

---

## 10. Yasaklar

- ❌ Kullanıcıya sormadan birden fazla dosya yaratma
- ❌ Pattern dışına çıkıp "daha iyi" diye yeni yapı önerme (önce sor)
- ❌ Repository pattern, UnitOfWork, MediatR gibi şeyleri kendiliğinden ekleme
- ❌ AutoMapper kullanma (manuel mapping yapılıyor)
- ❌ Entity'yi controller/view'a direkt sızdırma — her zaman DTO
- ❌ Uzun açıklama, gereksiz disclaimer, "umarım yardımcı olmuştur"