# QuickService 리팩토링 통합 계획서

> cs파일 단위로 리팩토링을 진행하며, 각 파일마다 **현재 문제점 → 리팩토링 내용 → 테스트 케이스**를 명시한다.
>
> 모든 메서드: **20줄 이내 / 단일 책임 / 명확한 이름 / 중첩 2단계 이내 / 매직 넘버 금지**

---

## 진행 상태

| # | 파일 | 상태 | 비고 |
|---|------|------|------|
| 01 | `MessengerParameterEnums.cs` | [x] 완료 | enum PascalCase, 시계방향 순서 |
| 02 | `MouseCoordinateParameter.cs` | [x] 완료 | record 변환 |
| 03 | `MouseClickParameter.cs` | [x] 완료 | record 변환 |
| 04 | `AppInformationChangedMessage.cs` | [x] 완료 | 변경 불필요 확인 |
| 05 | `ModifierKeyStateMessage.cs` | [x] 완료 | 변경 불필요 확인 |
| 06 | `MouseClickStateMessage.cs` | [x] 완료 | 변경 불필요 확인 |
| 07 | `MouseMoveMessage.cs` | [x] 완료 | 변경 불필요 확인 |
| 08 | `PressModifierKeyMessage.cs` | [x] 완료 | 변경 불필요 확인 |
| 09 | `ConfigureModel.cs` | [x] 완료 | 변경 없음 (확인만) |
| 10 | `RegisteredApplicationModel.cs` | [x] 완료 | GetByPosition 추가 |
| 11 | `AppInformationModel.cs` | [x] 완료 | setter SRP 분리 |
| 12 | `IJsonFileService.cs` | [x] 완료 | 시그니처 정리 |
| 13 | `IConfigurationService.cs` | [x] 완료 | 변경 없음 (확인만) |
| 14 | `IDialogHostService.cs` | [x] 완료 | 시그니처 변경 |
| 15 | `IViewModel.cs` | [x] 완료 | 변경 없음 (확인만) |
| 16 | `JsonFileService.cs` | [x] 완료 | 42줄 → 3메서드 분리 |
| 17 | `ConfigurationService.cs` | [x] 완료 | 불필요 블록 제거 |
| 18 | `LaunchAppService.cs` | [x] 완료 | MessageBox 제거 |
| 19 | `GlobalKeyboardHookService.cs` | [x] 완료 | 변수명 오류 수정, 상수화 |
| 20 | `GlobalMouseHookService.cs` | [x] 완료 | 38줄 HookProc 분리 |
| 21 | `HideMainWindowService.cs` | [x] 완료 | 변경 없음 (확인만) |
| 22 | `UserSelectPathService.cs` | [x] 완료 | 변경 없음 (확인만) |
| 23 | `TrayIconService.cs` | [x] 완료 | 28줄 메서드 분리 |
| 24 | `DialogHostService.cs` | [x] 완료 | 레이어 위반 수정 |
| 25 | `BitmapExtension.cs` | [x] 완료 | 공통 로직 추출 |
| 26 | `IconExtension.cs` | [x] 완료 | 공통 로직 추출 |
| 27 | `BooleanToVisibilityConverter.cs` | [x] 완료 | IsReversed 통합 |
| 28 | `BooleanToVisibilityReverseConverter.cs` | [x] 완료 | 제거 (통합) |
| 29 | `ShellWindowViewModel.cs` | [x] 완료 | Service Locator 제거 |
| 30 | `MainViewModel.cs` | [x] 완료 | Service Locator 제거 |
| 31 | `TitleViewModel.cs` | [x] 완료 | 불필요 블록 제거 |
| 32 | `ConfigDialogViewModel.cs` | [x] 완료 | 변경 없음 (확인만) |
| 33 | `InteractionViewModel.cs` | [x] 완료 | 30줄 → 3메서드 분리 |
| 34 | `SelectedFileViewModel.cs` | [x] 완료 | 65줄 → 반복 제거 |
| 35 | `SelectLaunchAppWindowViewModel.cs` | [x] 완료 | 67줄 생성자 분리, 매직넘버 |
| 36 | `IocBuilder.cs` | [x] 완료 | 구조화 |
| 37 | `App.xaml.cs` | [x] 완료 | 불필요 블록 제거 |
| 38 | `TitleLeftButtonDownDehavior.cs` | [x] 완료 | 변경 없음 (확인만) |

---

## 파일별 리팩토링 상세

---

### #01 — `MessengerParameterEnums.cs`

**경로**: `ViewModels/Messenger/MessengerParameterEnums.cs`

**문제점**:
- enum 멤버가 ALL_CAPS (`LEFT, RIGHT, TOP, BOTTOM`) — C# 컨벤션은 PascalCase

**리팩토링**:
```csharp
// Before
public enum AppPosition { LEFT, RIGHT, TOP, BOTTOM }

// After
public enum AppPosition { Left, Top, Right, Bottom }
```
- 순서도 논리적으로 변경: Left → Top → Right → Bottom (시계방향)

**영향 범위**: 이 enum을 참조하는 모든 파일에서 `LEFT` → `Left` 등으로 변경 필요
- `InteractionViewModel.cs` (string "LEFT" 비교 → enum 직접 사용으로 개선)
- `SelectedFileViewModel.cs`
- `SelectLaunchAppWindowViewModel.cs`
- `GlobalMouseHookService.cs` (간접)

**테스트**: enum 값 4개 존재 확인, 캐스팅 정수값 확인

---

### #02 — `MouseCoordinateParameter.cs`

**경로**: `ViewModels/Messenger/Parameters/MouseCoordinateParameter.cs`

**문제점**:
- mutable class — DTO는 불변이어야 함
- X, Y가 단순 auto property

**리팩토링**:
```csharp
// Before
public class MouseCoordinateParameter
{
    public double X { get; set; }
    public double Y { get; set; }
}

// After
public record MouseCoordinateParameter(double X, double Y);
```

**테스트**: 생성 및 값 비교, 구조적 동등성 확인

---

### #03 — `MouseClickParameter.cs`

**경로**: `ViewModels/Messenger/Parameters/MouseClickParameter.cs`

**문제점**:
- mutable class, 상속 구조가 record 변환 시 함께 변경 필요

**리팩토링**:
```csharp
// Before
public class MouseClickParameter : MouseCoordinateParameter
{
    public bool IsDown { get; set; }
}

// After
public record MouseClickParameter(bool IsDown, double X = 0, double Y = 0)
    : MouseCoordinateParameter(X, Y);
```

**테스트**: IsDown true/false 생성, 좌표값 전달 확인

---

### #04~08 — Message 클래스들

**파일**: `AppInformationChangedMessage.cs`, `ModifierKeyStateMessage.cs`, `MouseClickStateMessage.cs`, `MouseMoveMessage.cs`, `PressModifierKeyMessage.cs`

**상태**: 구조적으로 깔끔함 — 변경 불필요. #02, #03 record 변환 후 호환성만 확인.

---

### #09 — `ConfigureModel.cs`

**경로**: `Models/Configure/ConfigureModel.cs`

**상태**: 단순 wrapper — 현재 단계에서는 변경 불필요.

---

### #10 — `RegisteredApplicationModel.cs`

**경로**: `Models/Configure/RegisteredApplicationModel.cs`

**문제점**:
- 4방향 프로퍼티에 접근할 때 매번 switch/if-else 필요
- 모든 ViewModel에서 동일한 분기 패턴이 반복됨

**리팩토링**:
```csharp
// 기존 프로퍼티 유지 (JSON 직렬화 호환) + 위치별 접근 헬퍼 추가
public AppInformationModel GetByPosition(AppPosition position) => position switch
{
    AppPosition.Left   => LeftAppInformation,
    AppPosition.Top    => TopAppInformation,
    AppPosition.Right  => RightAppInformation,
    AppPosition.Bottom => BottomAppInformation,
    _ => throw new ArgumentOutOfRangeException(nameof(position))
};
```

**테스트**:
- `GetByPosition_Left_ReturnsLeftAppInformation`
- `GetByPosition_Top_ReturnsTopAppInformation`
- `GetByPosition_Right_ReturnsRightAppInformation`
- `GetByPosition_Bottom_ReturnsBottomAppInformation`
- `GetByPosition_Invalid_ThrowsArgumentOutOfRange`

---

### #11 — `AppInformationModel.cs`

**경로**: `Models/Configure/AppInformationModel.cs`

**문제점**:
- `AppPath` setter에서 아이콘 추출 + 이름 추출 + 경로 저장을 동시에 수행 (SRP 위반)
- `SetInformationPropertyFromPath` — ref 파라미터 사용이 불필요하게 복잡
- private property에 getter 메서드(`GetIconImage`, `GetAppName`)를 별도로 제공 — 비일관적

**리팩토링**:
```csharp
public class AppInformationModel
{
    /// <summary>
    /// 어플리케이션 경로
    /// </summary>
    public string? AppPath { get; set; }

    /// <summary>
    /// 아이콘 이미지 (직렬화 제외)
    /// </summary>
    [JsonIgnore]
    public ImageSource? IconImage { get; private set; }

    /// <summary>
    /// 어플리케이션 표시 이름 (직렬화 제외)
    /// </summary>
    [JsonIgnore]
    public string? DisplayName { get; private set; }

    /// <summary>
    /// 경로가 유효한지 검사
    /// </summary>
    public bool HasValidPath() => !string.IsNullOrEmpty(AppPath) && File.Exists(AppPath);

    /// <summary>
    /// 경로로부터 아이콘과 이름 정보를 로드
    /// </summary>
    public void LoadInfoFromPath()
    {
        if (!HasValidPath()) return;

        IconImage   = Icon.ExtractAssociatedIcon(AppPath!)?.ToImageSource();
        DisplayName = Path.GetFileNameWithoutExtension(AppPath);
    }
}
```

핵심 변경:
- setter 부수효과 제거 → `LoadInfoFromPath()` 명시적 호출
- `GetIconImage()`, `GetAppName()` 제거 → public property 직접 접근
- `IsValidPath()` → `HasValidPath()` (더 자연스러운 네이밍)

**테스트**:
- `HasValidPath_NullPath_ReturnsFalse`
- `HasValidPath_EmptyPath_ReturnsFalse`
- `HasValidPath_NonExistentPath_ReturnsFalse`
- `DisplayName_AfterLoadInfo_ReturnsFileNameWithoutExtension`

---

### #12 — `IJsonFileService.cs`

**경로**: `Abstract/Interfaces/IJsonFileService.cs`

**리팩토링**:
```csharp
// Before: bool 반환값 — 실제로 사용하는 곳 없음
bool SaveJsonProperties<T>(T jsonObject, string jsonPath);

// After: void로 변경 (실패 시 예외)
void Save<T>(T obj, string path);
T Load<T>(string path) where T : new();
```

메서드명도 간결하게: `SaveJsonProperties` → `Save`, `GetJsonProperties` → `Load`

---

### #13 — `IConfigurationService.cs`

**상태**: 깔끔함 — 변경 불필요.

---

### #14 — `IDialogHostService.cs`

**리팩토링**:
```csharp
// Before: 구현체에서 View/ViewModel 직접 생성 (레이어 위반)
bool ShowConfigDialog(string dialogHostName, string message = "");

// After: content를 외부에서 받도록 변경
bool ShowDialog(string hostName, object content);
```

---

### #15 — `IViewModel.cs`

**상태**: 마커 인터페이스 — 변경 불필요.

---

### #16 — `JsonFileService.cs`

**경로**: `Core/Services/JsonFileService.cs`

**문제점**:
- `SaveJsonProperties`: 42줄, 검증+디렉토리생성+직렬화+쓰기+확인을 한 메서드에서 수행
- 불필요한 중괄호 블록
- `new Exception(e.Message)` — 스택 트레이스 손실
- bool 반환값 무의미 (File.Exists 확인)

**리팩토링**:
```csharp
public class JsonFileService : IJsonFileService
{
    public void Save<T>(T obj, string path)
    {
        ValidatePath(path);
        EnsureDirectoryExists(path);
        WriteJson(obj, path);
    }

    public T Load<T>(string path) where T : new()
    {
        if (!File.Exists(path))
            return CreateAndSaveDefault<T>(path);

        return DeserializeOrDefault<T>(path);
    }

    private void ValidatePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("File path cannot be null or empty.", nameof(path));
    }

    private void EnsureDirectoryExists(string filePath)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
    }

    private void WriteJson<T>(T obj, string path)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.None);
        File.WriteAllText(path, json);
    }

    private T CreateAndSaveDefault<T>(string path) where T : new()
    {
        var result = new T();
        Save(result, path);
        return result;
    }

    private T DeserializeOrDefault<T>(string path) where T : new()
    {
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json) ?? CreateAndSaveDefault<T>(path);
    }
}
```

**테스트**:
- `Save_ValidObject_CreatesFile`
- `Save_NullPath_ThrowsArgumentException`
- `Save_EmptyPath_ThrowsArgumentException`
- `Save_DirectoryNotExists_CreatesDirectory`
- `Load_ExistingFile_DeserializesCorrectly`
- `Load_NonExistentFile_CreatesDefaultAndSaves`
- `Load_CorruptedJson_CreatesDefault`
- `SaveAndLoad_RoundTrip_PreservesData`

---

### #17 — `ConfigurationService.cs`

**경로**: `Core/Services/ConfigurationService.cs`

**문제점**:
- 불필요한 중괄호 블록
- nullable 서비스 파라미터 (`IJsonFileService?`) — DI에서 null이면 안됨
- null-forgiving 연산자 (`!`) 남발

**리팩토링**:
```csharp
public class ConfigurationService : IConfigurationService
{
    private readonly string _configFilePath;
    private readonly IJsonFileService _jsonFileService;

    public ConfigurationService(IJsonFileService jsonFileService)
    {
        _jsonFileService = jsonFileService ?? throw new ArgumentNullException(nameof(jsonFileService));
        _configFilePath  = Path.Combine(Directory.GetCurrentDirectory(), "Config", "Configuration.json");
    }

    public void SaveConfiguration<T>(T configurationObj)
    {
        ArgumentNullException.ThrowIfNull(configurationObj);
        _jsonFileService.Save(configurationObj, _configFilePath);
    }

    public T GetConfiguration<T>() where T : new()
    {
        return _jsonFileService.Load<T>(_configFilePath);
    }
}
```

**테스트**:
- `Constructor_NullService_ThrowsArgumentNullException`
- `SaveConfiguration_NullObj_ThrowsArgumentNullException`
- `SaveConfiguration_ValidObj_DelegatesToJsonService` (Mock)
- `GetConfiguration_DelegatesToJsonService` (Mock)

---

### #18 — `LaunchAppService.cs`

**경로**: `Core/Services/LaunchAppService.cs`

**문제점**:
- Infrastructure에서 `MessageBox.Show()` 직접 호출 — 레이어 위반
- 에러 처리가 UI에 결합

**리팩토링**:
```csharp
public class LaunchAppService : ILaunchAppService
{
    public void LaunchApp(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Application path cannot be empty.", nameof(path));

        if (!File.Exists(path))
            throw new FileNotFoundException("Application not found.", path);

        Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
    }
}
```

호출하는 ViewModel에서 try-catch로 사용자에게 에러 표시.

**테스트**:
- `LaunchApp_NullPath_ThrowsArgumentException`
- `LaunchApp_EmptyPath_ThrowsArgumentException`
- `LaunchApp_NonExistentPath_ThrowsFileNotFoundException`

---

### #19 — `GlobalKeyboardHookService.cs`

**경로**: `Core/Services/GlobalKeyboardHookService.cs`

**문제점**:
- `_handleHookMouse` — 키보드 서비스인데 Mouse 변수명 (오류)
- Win32 상수가 생성자에서 할당 — `const`로 선언 가능
- 불필요한 중괄호 블록

**리팩토링**:
```csharp
public class GlobalKeyboardHookService : IGlobalKeyboardHookService
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYUP      = 0x0101;
    private const int WM_KEYDOWN    = 0x0104;
    private const int VK_ALT        = 164;

    private readonly LowLevelKeyboardProc _keyboardProc;
    private static IntPtr _hookHandle = IntPtr.Zero;

    public GlobalKeyboardHookService()
    {
        _keyboardProc = KeyboardHookProc;
    }

    // ... SetHook, UnHook은 동일

    public IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
    {
        if (code >= 0)
            HandleKeyEvent((int)wParam, Marshal.ReadInt32(lParam));

        return CallNextHookEx(_hookHandle, code, (int)wParam, lParam);
    }

    private void HandleKeyEvent(int eventType, int vkCode)
    {
        if (vkCode != VK_ALT) return;

        if (eventType == WM_KEYDOWN)
            WeakReferenceMessenger.Default.Send(new ModifierKeyStateMessage(true));
        else if (eventType == WM_KEYUP)
            WeakReferenceMessenger.Default.Send(new ModifierKeyStateMessage(false));
    }
}
```

핵심: `_handleHookMouse` → `_hookHandle`, 상수를 `const`로, `HandleKeyEvent` 추출

---

### #20 — `GlobalMouseHookService.cs`

**경로**: `Core/Services/GlobalMouseHookService.cs`

**문제점**:
- `MouseHookProc` 38줄, 5단계 중첩
- 메시지 생성 패턴 3번 반복
- Win32 상수가 생성자에서 할당

**리팩토링**:
```csharp
public class GlobalMouseHookService : IGlobalMouseHookService
{
    private const int WH_MOUSE_LL    = 14;
    private const int WM_MOUSEMOVE   = 0x0200;
    private const int WM_LBUTTONUP   = 0x0202;
    private const int WM_LBUTTONDOWN = 0x0201;

    private readonly LowLevelMouseProc _mouseProc;
    private static IntPtr _hookHandle = IntPtr.Zero;
    private bool _isModifierKeyDown;

    public GlobalMouseHookService()
    {
        _mouseProc = MouseHookProc;
        WeakReferenceMessenger.Default.Register<ModifierKeyStateMessage>(this, (r, m) =>
            _isModifierKeyDown = m.Value);
    }

    public IntPtr MouseHookProc(int code, IntPtr wParam, IntPtr lParam)
    {
        if (code >= 0 && _isModifierKeyDown)
            DispatchMouseEvent((int)wParam);

        return CallNextHookEx(_hookHandle, code, (int)wParam, lParam);
    }

    private void DispatchMouseEvent(int eventType)
    {
        switch (eventType)
        {
            case WM_LBUTTONDOWN: SendClickMessage(isDown: true);  break;
            case WM_LBUTTONUP:   SendClickMessage(isDown: false); break;
            case WM_MOUSEMOVE:   SendMoveMessage();               break;
        }
    }

    private void SendClickMessage(bool isDown)
    {
        var param = new MouseClickParameter(
            isDown,
            isDown ? Cursor.Position.X : 0,
            isDown ? Cursor.Position.Y : 0);
        WeakReferenceMessenger.Default.Send(new MouseClickStateMessage(param));
    }

    private void SendMoveMessage()
    {
        var param = new MouseCoordinateParameter(Cursor.Position.X, Cursor.Position.Y);
        WeakReferenceMessenger.Default.Send(new MouseMoveMessage(param));
    }
}
```

---

### #21 — `HideMainWindowService.cs`

**상태**: 6줄 메서드, 깔끔 — 변경 불필요.

---

### #22 — `UserSelectPathService.cs`

**상태**: 13줄, 단일 책임 — 변경 불필요.

---

### #23 — `TrayIconService.cs`

**경로**: `Core/Services/TrayIconService.cs`

**문제점**:
- `CreateTrayIconObject` 28줄 — 프로세스명 파싱 + 아이콘 생성 + 이벤트 등록
- `GetTrayIconMenuStrip` 24줄 — 메뉴 생성 + 이벤트 등록 반복

**리팩토링**:
```csharp
public class TrayIconService : ITrayIconService
{
    private NotifyIcon? _trayIcon;

    public TrayIconService()
    {
        _trayIcon = CreateTrayIcon();
    }

    public void VisibleTrayIconOnTaskBar(bool isVisible) => _trayIcon!.Visible = isVisible;

    private NotifyIcon CreateTrayIcon()
    {
        var icon = new NotifyIcon
        {
            Icon    = Properties.Resources.QuickService,
            Text    = GetApplicationName(),
            Visible = false,
        };
        icon.DoubleClick += OnTrayIconDoubleClick;
        icon.ContextMenuStrip = CreateContextMenu();
        return icon;
    }

    private string GetApplicationName()
    {
        var name = Process.GetCurrentProcess().ProcessName;
        var dotIndex = name.IndexOf('.');
        return dotIndex >= 0 ? name[..dotIndex] : name;
    }

    private void OnTrayIconDoubleClick(object? sender, EventArgs e)
    {
        RestoreMainWindow();
    }

    private ContextMenuStrip CreateContextMenu()
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add(CreateMenuItem("Open", (_, _) => RestoreMainWindow()));
        menu.Items.Add(CreateMenuItem("Close", (_, _) => Application.Current.Shutdown()));
        return menu;
    }

    private ToolStripMenuItem CreateMenuItem(string text, EventHandler handler)
    {
        var item = new ToolStripMenuItem(text);
        item.Click += handler;
        return item;
    }

    private void RestoreMainWindow()
    {
        _trayIcon!.Visible = false;
        var window = Application.Current.MainWindow;
        window.Show();
        window.WindowState = WindowState.Normal;
    }
}
```

핵심: `Process.Kill()` → `Application.Current.Shutdown()` (안전), 중복 로직을 `RestoreMainWindow`로 통합

---

### #24 — `DialogHostService.cs`

**경로**: `Core/Services/DialogHostService.cs`

**문제점**:
- Infrastructure에서 View(`ConfigDialogView`) + ViewModel(`ConfigDialogViewModel`) 직접 생성
- `using QuickService.ViewModels.Main` / `using QuickService.Views.UserControls` — 레이어 위반

**리팩토링**:
```csharp
// IDialogHostService 시그니처 변경에 맞춰
public class DialogHostService : IDialogHostService
{
    public bool ShowDialog(string hostName, object content)
    {
        var result = DialogHost.Show(content, hostName);
        return result is true;
    }
}
```

호출부 (TitleViewModel)에서 View/ViewModel 생성:
```csharp
private void ShowOptionDialog()
{
    var content = new ConfigDialogView { DataContext = new ConfigDialogViewModel() };
    _dialogHostService.ShowDialog("ShellWindowHost", content);
}
```

---

### #25 — `BitmapExtension.cs`

**경로**: `Extensions/BitmapExtension.cs`

**문제점**:
- IconExtension과 MemoryStream→BitmapImage 변환 로직 중복
- null 체크 후 result 변수 불필요

**리팩토링**: 공통 헬퍼 추출
```csharp
public static ImageSource ToImageSource(this Bitmap bitmap)
{
    ArgumentNullException.ThrowIfNull(bitmap);
    return ImageConversionHelper.CreateBitmapImage(
        stream => bitmap.Save(stream, ImageFormat.Png));
}
```

공통 헬퍼 (`ImageConversionHelper.cs` 신규):
```csharp
internal static class ImageConversionHelper
{
    public static BitmapImage CreateBitmapImage(Action<Stream> writeToStream)
    {
        using var stream = new MemoryStream();
        writeToStream(stream);
        stream.Position = 0;

        var image = new BitmapImage();
        image.BeginInit();
        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
        image.CacheOption   = BitmapCacheOption.OnLoad;
        image.StreamSource  = stream;
        image.EndInit();
        image.Freeze();
        return image;
    }
}
```

---

### #26 — `IconExtension.cs`

**리팩토링**: 공통 헬퍼 사용
```csharp
public static ImageSource ToImageSource(this Icon icon)
{
    ArgumentNullException.ThrowIfNull(icon);
    return ImageConversionHelper.CreateBitmapImage(stream =>
    {
        using var bitmap = icon.ToBitmap();
        bitmap.Save(stream, ImageFormat.Png);
    });
}
```

**테스트 (#25, #26 공통)**:
- `CreateBitmapImage_ValidStream_ReturnsFrozenImage`
- `ToImageSource_NullBitmap_ThrowsArgumentNullException`
- `ToImageSource_NullIcon_ThrowsArgumentNullException`

---

### #27 — `BooleanToVisibilityConverter.cs`

**문제점**: `BooleanToVisibilityReverseConverter`와 95% 동일

**리팩토링**: `IsReversed` 속성으로 통합
```csharp
public class BooleanToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// true일 경우 변환 결과를 반전
    /// </summary>
    public bool IsReversed { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var boolValue = value is bool b && b;
        if (IsReversed) boolValue = !boolValue;
        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isVisible = value is Visibility v && v == Visibility.Visible;
        return IsReversed ? !isVisible : isVisible;
    }
}
```

**테스트**:
- `Convert_True_ReturnsVisible`
- `Convert_False_ReturnsCollapsed`
- `Convert_Null_ReturnsCollapsed`
- `ConvertReversed_True_ReturnsCollapsed`
- `ConvertReversed_False_ReturnsVisible`
- `ConvertBack_Visible_ReturnsTrue`
- `ConvertBack_Collapsed_ReturnsFalse`

---

### #28 — `BooleanToVisibilityReverseConverter.cs`

**리팩토링**: 파일 삭제. XAML에서 참조를 `BooleanToVisibilityConverter`의 `IsReversed="True"`로 교체.

---

### #29 — `ShellWindowViewModel.cs`

**문제점**: `Ioc.Default.GetService<MainViewModel>()` — Service Locator 패턴

**리팩토링**:
```csharp
public partial class ShellWindowViewModel : ObservableRecipient, IViewModel
{
    public ShellWindowViewModel(MainViewModel mainViewModel)
    {
        ShellContent = mainViewModel;
    }

    [ObservableProperty]
    private IViewModel? _shellContent;
}
```

---

### #30 — `MainViewModel.cs`

**문제점**: `Ioc.Default.GetService<T>()` 3회 호출 — Service Locator

**리팩토링**:
```csharp
public partial class MainViewModel : ObservableRecipient, IViewModel
{
    public MainViewModel(
        TitleViewModel titleViewModel,
        InteractionViewModel interactionViewModel,
        SelectedFileViewModel selectedFileViewModel)
    {
        TitleContent       = titleViewModel;
        InteractionContent = interactionViewModel;
        SelectedFileContent = selectedFileViewModel;
    }

    [ObservableProperty] private IViewModel _titleContent;
    [ObservableProperty] private IViewModel _interactionContent;
    [ObservableProperty] private IViewModel _selectedFileContent;
}
```

---

### #31 — `TitleViewModel.cs`

**문제점**: 불필요한 중괄호 블록만 제거하면 깔끔

**리팩토링**:
```csharp
public TitleViewModel(
    ITrayIconService trayIconService,
    IHideMainWindowService hideMainWindowService,
    IDialogHostService dialogHostService)
{
    _trayIconService       = trayIconService;
    _hideMainWindowService = hideMainWindowService;
    _dialogHostService     = dialogHostService;
}
```

`ShowOptionDialog`에서 View/ViewModel 생성 책임 추가 (#24 변경에 따라).

---

### #32 — `ConfigDialogViewModel.cs`

**상태**: 빈 stub — 변경 불필요.

---

### #33 — `InteractionViewModel.cs`

**경로**: `ViewModels/Main/InteractionViewModel.cs`

**문제점**:
- `RegistrationQuickServiceApplication` 30줄 — 5가지 책임
- `string param` ("LEFT", "TOP" 등) — 타입 안전하지 않음
- enum 인덱스 찾는 loop 불필요하게 복잡

**리팩토링**:
```csharp
public partial class InteractionViewModel : ObservableRecipient, IViewModel
{
    private readonly IUserSelectPathService _userSelectPathService;
    private readonly IConfigurationService _configurationService;

    public InteractionViewModel(
        IUserSelectPathService userSelectPathService,
        IConfigurationService configurationService)
    {
        _userSelectPathService = userSelectPathService;
        _configurationService  = configurationService;
    }

    [RelayCommand]
    private void OpenWebSiteLink(string url)
    {
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }

    [RelayCommand]
    private void RegisterApplication(string param)
    {
        var path = _userSelectPathService.GetUserSelectedFilePath();
        if (string.IsNullOrEmpty(path)) return;

        var position = Enum.Parse<AppPosition>(param, ignoreCase: true);
        SaveAppRegistration(position, path);
        NotifyAppChanged(position);
    }

    private void SaveAppRegistration(AppPosition position, string path)
    {
        var config = _configurationService.GetConfiguration<RegisteredApplicationModel>();
        config.GetByPosition(position).AppPath = path;
        _configurationService.SaveConfiguration(config);
    }

    private void NotifyAppChanged(AppPosition position)
    {
        WeakReferenceMessenger.Default.Send(new AppInformationChangedMessage(position));
    }
}
```

핵심: `GetByPosition()` 활용으로 switch 제거, `Enum.Parse`로 loop 제거, 3개 메서드로 분리

**테스트**:
- `RegisterApplication_ValidPath_SavesConfiguration` (Mock)
- `RegisterApplication_EmptyPath_DoesNotSave` (Mock)
- `RegisterApplication_ValidPath_SendsMessage` (Mock)
- `OpenWebSiteLink_ValidUrl_StartsProcess`

---

### #34 — `SelectedFileViewModel.cs`

**경로**: `ViewModels/Main/SelectedFileViewModel.cs`

**문제점**:
- `LoadUserSelectedAppIconImages` 65줄 — 4방향 동일 패턴 반복
- `"Empty"` 매직 스트링 4회 반복

**리팩토링**:
```csharp
public partial class SelectedFileViewModel : ObservableRecipient, IViewModel
{
    private const string EmptyAppName = "Empty";
    private readonly IConfigurationService _configurationService;

    public SelectedFileViewModel(IConfigurationService configurationService)
    {
        _configurationService = configurationService;

        WeakReferenceMessenger.Default.Register<AppInformationChangedMessage>(this,
            (r, m) => UpdateAppDisplay(m.Value));

        foreach (var position in Enum.GetValues<AppPosition>())
            UpdateAppDisplay(position);
    }

    // ... ObservableProperty 필드들 유지 ...

    private void UpdateAppDisplay(AppPosition position)
    {
        var config = _configurationService.GetConfiguration<RegisteredApplicationModel>();
        if (config is null)
            throw new InvalidOperationException("사용자 설정 불러오기 실패");

        var appInfo = config.GetByPosition(position);
        var (icon, name) = ExtractDisplayInfo(appInfo);
        ApplyDisplay(position, icon, name);
    }

    private (ImageSource? icon, string name) ExtractDisplayInfo(AppInformationModel app)
    {
        if (!app.HasValidPath())
            return (Properties.Resources.EmptyIcon.ToImageSource(), EmptyAppName);

        app.LoadInfoFromPath();
        return (app.IconImage, app.DisplayName ?? EmptyAppName);
    }

    private void ApplyDisplay(AppPosition position, ImageSource? icon, string name)
    {
        switch (position)
        {
            case AppPosition.Left:   LeftAppIconImageSource   = icon; LeftAppName   = name; break;
            case AppPosition.Top:    TopAppIconImageSource    = icon; TopAppName    = name; break;
            case AppPosition.Right:  RightAppIconImageSource  = icon; RightAppName  = name; break;
            case AppPosition.Bottom: BottomAppIconImageSource = icon; BottomAppName = name; break;
        }
    }
}
```

65줄 → 3개의 작은 메서드. `GetByPosition()` 활용으로 반복 제거.

---

### #35 — `SelectLaunchAppWindowViewModel.cs`

**경로**: `ViewModels/Main/SelectLaunchAppWindowViewModel.cs`

**문제점**:
- 생성자 67줄 — 서비스 등록 + 메신저 등록 + 속성 초기화
- 매직 넘버: 300, 52, 45, 135, 225, 315
- `LoadUserSelectedAppIconImages` 37줄 — 4방향 반복
- `LaunchSelectedApp` — if-else 체인

**리팩토링**:
```csharp
public partial class SelectLaunchAppWindowViewModel : ObservableRecipient, IViewModel
{
    private const double SelectionWindowSize    = 300;
    private const double InvalidAreaDiameter    = 52;
    private const double RightTopBoundary       = 45;
    private const double TopLeftBoundary        = 135;
    private const double LeftBottomBoundary     = 225;
    private const double BottomRightBoundary    = 315;

    private readonly IConfigurationService _configurationService;
    private readonly ILaunchAppService _launchAppService;
    private Point _currentWindowCenter;

    public SelectLaunchAppWindowViewModel(
        IConfigurationService configurationService,
        ILaunchAppService launchAppService)
    {
        _configurationService = configurationService;
        _launchAppService     = launchAppService;

        WindowLength        = SelectionWindowSize;
        InvalidAreaDiameter = InvalidAreaDiameter; // const → property

        RegisterMessengers();
        LoadAllAppIcons();
    }

    private void RegisterMessengers()
    {
        WeakReferenceMessenger.Default.Register<MouseClickStateMessage>(this, (r, m) =>
            HandleMouseClick(m.Value));
        WeakReferenceMessenger.Default.Register<ModifierKeyStateMessage>(this, (r, m) =>
        { if (!m.Value) IsWindowOpen = false; });
        WeakReferenceMessenger.Default.Register<MouseMoveMessage>(this, (r, m) =>
        { if (IsWindowOpen) HandleMouseMove(new Point(m.Value.X, m.Value.Y)); });
        WeakReferenceMessenger.Default.Register<AppInformationChangedMessage>(this, (r, m) =>
            LoadAppIcon(m.Value));
    }

    private void HandleMouseClick(MouseClickParameter click)
    {
        IsWindowOpen = click.IsDown;
        IsNoneSelect = true;

        if (click.IsDown)
        {
            PositionLeft = click.X - (WindowLength / 2);
            PositionTop  = click.Y - (WindowLength / 2);
            _currentWindowCenter = new Point(click.X, click.Y);
            HandleMouseMove(_currentWindowCenter);
        }
        else
        {
            LaunchSelectedApp();
        }
    }

    private void HandleMouseMove(Point mousePosition)
    {
        if (IsWithinInvalidArea(mousePosition))
        {
            ClearSelection();
            IsNoneSelect = true;
            return;
        }

        IsNoneSelect = false;
        var angle = CalculateAngle(mousePosition);
        PointerAngle = -angle + 90;
        ApplySelection(DeterminePosition(angle));
    }

    private bool IsWithinInvalidArea(Point mouse)
    {
        var radius = InvalidAreaDiameter / 2;
        var dx = (mouse.X - _currentWindowCenter.X) / radius;
        var dy = (mouse.Y - _currentWindowCenter.Y) / radius;
        return (dx * dx + dy * dy) <= 1.0;
    }

    private double CalculateAngle(Point mouse)
    {
        var deltaX = mouse.X - _currentWindowCenter.X;
        var deltaY = _currentWindowCenter.Y - mouse.Y;
        return Math.Atan2(deltaY, deltaX) * (180 / Math.PI);
    }

    private AppPosition DeterminePosition(double angle)
    {
        if (angle < 0) angle += 360;
        return angle switch
        {
            >= BottomRightBoundary or < RightTopBoundary   => AppPosition.Right,
            >= RightTopBoundary and < TopLeftBoundary       => AppPosition.Top,
            >= TopLeftBoundary and < LeftBottomBoundary     => AppPosition.Left,
            _                                              => AppPosition.Bottom,
        };
    }

    private void ApplySelection(AppPosition position)
    {
        ClearSelection();
        switch (position)
        {
            case AppPosition.Left:   IsSelectedLeft   = true; break;
            case AppPosition.Top:    IsSelectedTop    = true; break;
            case AppPosition.Right:  IsSelectedRight  = true; break;
            case AppPosition.Bottom: IsSelectedBottom = true; break;
        }
    }

    private void ClearSelection()
    {
        IsSelectedLeft = IsSelectedTop = IsSelectedRight = IsSelectedBottom = false;
    }

    private void LoadAllAppIcons()
    {
        foreach (var pos in Enum.GetValues<AppPosition>())
            LoadAppIcon(pos);
    }

    private void LoadAppIcon(AppPosition position)
    {
        var config = _configurationService.GetConfiguration<RegisteredApplicationModel>();
        if (config is null) return;

        var app = config.GetByPosition(position);
        if (!app.HasValidPath()) return;

        app.LoadInfoFromPath();
        switch (position)
        {
            case AppPosition.Left:   LeftAppIconImageSource   = app.IconImage!; break;
            case AppPosition.Top:    TopAppIconImageSource    = app.IconImage!; break;
            case AppPosition.Right:  RightAppIconImageSource  = app.IconImage!; break;
            case AppPosition.Bottom: BottomAppIconImageSource = app.IconImage!; break;
        }
    }

    private void LaunchSelectedApp()
    {
        var config = _configurationService.GetConfiguration<RegisteredApplicationModel>();
        if (config is null) return;

        var position = GetSelectedPosition();
        if (position is null) return;

        var path = config.GetByPosition(position.Value).AppPath;
        if (!string.IsNullOrEmpty(path))
            _launchAppService.LaunchApp(path);
    }

    private AppPosition? GetSelectedPosition()
    {
        if (IsSelectedLeft)   return AppPosition.Left;
        if (IsSelectedTop)    return AppPosition.Top;
        if (IsSelectedRight)  return AppPosition.Right;
        if (IsSelectedBottom) return AppPosition.Bottom;
        return null;
    }

    // ... ObservableProperty 필드들 유지 ...
}
```

**테스트**:
- `CalculateAngle_RightDirection_Returns0`
- `CalculateAngle_TopDirection_Returns90`
- `CalculateAngle_LeftDirection_Returns180`
- `CalculateAngle_BottomDirection_ReturnsMinus90`
- `DeterminePosition_0Degrees_ReturnsRight`
- `DeterminePosition_90Degrees_ReturnsTop`
- `DeterminePosition_180Degrees_ReturnsLeft`
- `DeterminePosition_270Degrees_ReturnsBottom`
- `DeterminePosition_Boundary45_ReturnsTop`
- `DeterminePosition_Boundary135_ReturnsLeft`
- `DeterminePosition_Boundary225_ReturnsBottom`
- `DeterminePosition_Boundary315_ReturnsRight`
- `DeterminePosition_NegativeAngle_HandlesCorrectly`
- `IsWithinInvalidArea_CenterPoint_ReturnsTrue`
- `IsWithinInvalidArea_FarPoint_ReturnsFalse`
- `GetSelectedPosition_NoneSelected_ReturnsNull`
- `GetSelectedPosition_LeftSelected_ReturnsLeft`

---

### #36 — `IocBuilder.cs`

**경로**: `App/MVVM/IocBuilder.cs`

**리팩토링**:
```csharp
public static class IocBuilder
{
    public static void Build()
    {
        var services = new ServiceCollection();
        RegisterServices(services);
        RegisterViewModels(services);
        Ioc.Default.ConfigureServices(services.BuildServiceProvider());
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IJsonFileService, JsonFileService>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<ILaunchAppService, LaunchAppService>();
        services.AddSingleton<IGlobalMouseHookService, GlobalMouseHookService>();
        services.AddSingleton<IGlobalKeyboardHookService, GlobalKeyboardHookService>();
        services.AddSingleton<IHideMainWindowService, HideMainWindowService>();
        services.AddSingleton<ITrayIconService, TrayIconService>();
        services.AddSingleton<IUserSelectPathService, UserSelectPathService>();
        services.AddSingleton<IDialogHostService, DialogHostService>();
    }

    private static void RegisterViewModels(IServiceCollection services)
    {
        services.AddTransient<ShellWindowViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<TitleViewModel>();
        services.AddTransient<InteractionViewModel>();
        services.AddTransient<SelectedFileViewModel>();
        services.AddTransient<SelectLaunchAppWindowViewModel>();
    }
}
```

---

### #37 — `App.xaml.cs`

**리팩토링**: 불필요한 중괄호 블록 제거, 메서드 추출
```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    SetupGlobalHooks();
    ShowMainWindow();
    ShowSelectionWindow();
}

private void SetupGlobalHooks()
{
    Ioc.Default.GetService<IGlobalMouseHookService>()!.SetHook();
    Ioc.Default.GetService<IGlobalKeyboardHookService>()!.SetHook();
}

private void ShowMainWindow() { ... }
private void ShowSelectionWindow() { ... }
```

---

### #38 — `TitleLeftButtonDownBehavior.cs`

**상태**: 깔끔한 Behavior — 변경 불필요.

---

## 실행 순서

```
#01~#03 : Messenger 기반 타입 변경 (enum, record) — 다른 파일의 기반
    ↓
#10~#11 : Model 개선 (GetByPosition, LoadInfoFromPath) — ViewModel이 의존
    ↓
#12~#14 : Interface 시그니처 변경 — 구현체가 의존
    ↓
#16~#20 : Core Services 리팩토링 — 인터페이스 변경 반영
    ↓
#23~#24 : TrayIcon, DialogHost 리팩토링
    ↓
#25~#28 : Extensions, Converter 리팩토링
    ↓
#29~#35 : ViewModel 리팩토링 — 모든 하위 레이어 변경 반영
    ↓
#36~#37 : DI 및 App 리팩토링
    ↓
테스트 프로젝트 생성 및 전체 테스트 작성
    ↓
빌드 및 실행 검증
```
