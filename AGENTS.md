# AGENTS Development Standards

## 1. i18n Internationalization Standards

### 1.1 Resource File Structure
- Main resource file: `Resources/Strings.resx` (default English)
- Chinese resource file: `Resources/Strings.zh-CN.resx`
- All UI text must use resource files, hard-coded strings are prohibited

### 1.2 Naming Conventions
- Use PascalCase naming
- Page title prefix: `PageXXX` (e.g., `PageQrBarcodeDecode`)
- General text: direct description (e.g., `Copy`, `Back`, `Refresh`)
- Message prompts: clear description (e.g., `CopySuccess`, `DecodeFailed`)

### 1.3 Usage Examples
```xml
<!-- Usage in XAML -->
<TextBlock Text="{x:Static resources:Strings.PageQrBarcodeDecode}" />
<Button ToolTip="{x:Static resources:Strings.Back}" />

<!-- C# usage -->
MessageBox.Show(Properties.Resources.CopySuccess, Properties.Resources.Info);
```

### 1.4 i18n Scope (Mandatory)
The following UI text must use resource files, **hard-coded strings are strictly prohibited**:
- Page title
- Button text and ToolTip
- TextBlock / Label text
- Tab headers (`TabItem.Header`) and tab content descriptions
- Home page category titles and category-related text
- MessageBox prompts (info, success, error, warning)
- Placeholder text (e.g., `SearchBox.Watermark`)

Naming convention for category-related:
- Category names: `CategoryXXX` (e.g., `CategoryEncoding`, `CategoryImage`, `CategoryData`, `CategoryQuery`)

### 1.5 Multi-Language Synchronization
When adding or modifying any text, the following files must be updated synchronously:
- `Resources/Strings.resx` (default, generally English)
- `Resources/Strings.zh-CN.resx` (Simplified Chinese)
- `Resources/Strings.en-US.resx` (English, if exists)

---

## 2. Button Icon Standards

### 2.1 Icon Library
- Use FontAwesome Solid font icons
- FontFamily: `{StaticResource FontAwesomeSolid}`

### 2.2 Back Button Style (Circle)
```xml
<Button Click="Back_Click" Width="40" Height="40" ToolTip="{x:Static resources:Strings.Back}">
    <Button.Template>
        <ControlTemplate TargetType="Button">
            <Border Background="#4A4A4A" CornerRadius="20" Width="40" Height="40" SnapsToDevicePixels="True">
                <TextBlock FontFamily="{StaticResource FontAwesomeSolid}" 
                           Text="&#xf060;" 
                           FontSize="16" 
                           FontWeight="Bold" 
                           HorizontalAlignment="Center" 
                           VerticalAlignment="Center" 
                           Foreground="#FFFFFF" />
            </Border>
        </ControlTemplate>
    </Button.Template>
</Button>
```

### 2.3 Action Button Style (Rectangle)
```xml
<Button Click="CopyResult_Click" Width="100" Height="32">
    <StackPanel Orientation="Horizontal">
        <TextBlock FontFamily="{StaticResource FontAwesomeSolid}" 
                   Text="&#xf0c5;" 
                   FontSize="14" 
                   FontWeight="Bold" 
                   Foreground="#FFFFFF" 
                   Margin="0,0,6,0" />
        <TextBlock Text="{x:Static resources:Strings.Copy}" 
                   FontSize="14" 
                   Foreground="#FFFFFF" />
    </StackPanel>
</Button>
```

### 2.4 Common Icon Codes
| Icon | Code | Usage |
|------|------|------|
| Back | `&#xf060;` | Navigate back |
| Copy | `&#xf0c5;` | Copy content |
| Clear | `&#xf2ed;` | Clear content |
| Upload Image | `&#xf093;` | Select/upload image |
| QR Code | `&#xf029;` | QR Code related |
| Barcode | `&#xf02a;` | Barcode related |
| Settings | `&#xf013;` | Settings page |
| Search | `&#xf002;` | Search function |
| Refresh | `&#xf021;` | Refresh function |

---

## 3. Home Page Tab Category Standards

### 3.1 Tab Control Structure
The home page uses `TabControl` to organize tool categories. Each category is a `TabItem`:

```xml
<TabControl Style="{StaticResource ModernTabControl}" Margin="20,30,20,20">
    <TabItem Header="{x:Static resources:Strings.CategoryEncoding}" 
             Tag="&#xf121;" 
             Style="{StaticResource ModernTabItem}">
        <ScrollViewer HorizontalScrollBarVisibility="Disabled" VerticalScrollBarVisibility="Auto">
            <WrapPanel HorizontalAlignment="Center" Margin="10">
                <!-- Tool Buttons -->
            </WrapPanel>
        </ScrollViewer>
    </TabItem>
</TabControl>
```

### 3.2 Tab Item Style
- Tab text must use `Header="{x:Static resources:Strings.CategoryXXX}"` (i18n)
- Tab icon uses the FontAwesome icon code in the `Tag` attribute
- Default state: gray text
- Selected state: white background + blue icon + dark text

### 3.3 Category Naming Convention
- Category name: `CategoryXXX` (e.g., `CategoryEncoding`, `CategoryImage`, `CategoryData`, `CategoryQuery`)
- Must add corresponding entries in `Strings.resx`, `Strings.zh-CN.resx`, and `Strings.en-US.resx`

### 3.4 Tool Button Style
```xml
<Button x:Name="BtnXXX" Click="BtnXXX_Click" Style="{StaticResource ToolButtonStyle}">
    <StackPanel Width="140" HorizontalAlignment="Center" VerticalAlignment="Center">
        <TextBlock FontFamily="{StaticResource FontAwesomeSolid}" 
                   Text="&#xfXXX;" 
                   FontSize="36" 
                   FontWeight="Bold" 
                   HorizontalAlignment="Center" 
                   TextAlignment="Center" 
                   Foreground="#FFFFFF" />
        <TextBlock Text="{x:Static resources:Strings.XXX}" 
                   FontSize="14" 
                   HorizontalAlignment="Center" 
                   TextAlignment="Center" 
                   Margin="0,10,0,0" 
                   TextWrapping="Wrap" 
                   Foreground="#FFFFFF" />
    </StackPanel>
</Button>
```

### 3.5 Adding a New Tool Category
1. Add `CategoryXXX` resource string to `Strings.resx` (English)
2. Add `CategoryXXX` resource string to `Strings.zh-CN.resx` (Chinese)
3. Add a new `TabItem` in `HomePage.xaml` with `Header` bound to the resource
4. Add tool buttons inside the `TabItem`

### 3.6 Adding a New Tool
1. Create page files: `Pages/XXXPage.xaml` and `Pages/XXXPage.xaml.cs`
2. Add resource strings to `Strings.resx`, `Strings.zh-CN.resx`, and `Strings.en-US.resx`
3. Add tool button to the corresponding `TabItem` in `HomePage.xaml`
4. Add click event handler to `HomePage.xaml.cs`

---

## 4. Page Structure Template

### 4.1 Basic Layout
```xml
<Page x:Class="DevTools.Pages.XXXPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:resources="clr-namespace:DevTools.Resources"
      xmlns:local="clr-namespace:DevTools.Pages"
      Title="XXXPage">
    <Grid Margin="20">
        <StackPanel>
            <!-- Back Button -->
            <Button Click="Back_Click" ...>
                ...
            </Button>
            
            <!-- Page Title -->
            <TextBlock Text="{x:Static resources:Strings.PageXXX}" FontSize="18" Margin="0,10" />
            
            <!-- Main Content Area -->
            <Border CornerRadius="8" BorderBrush="#4A4A4A" BorderThickness="2" Margin="0,10" Height="300">
                <Grid>
                    <!-- Content -->
                </Grid>
            </Border>
            
            <!-- Action Buttons -->
            <StackPanel Orientation="Horizontal">
                <Button Click="Action1_Click" ...>...</Button>
                <Button Click="Action2_Click" ...>...</Button>
            </StackPanel>
        </StackPanel>
    </Grid>
</Page>
```

### 4.2 Page Code Template
```csharp
using System.Windows;
using System.Windows.Controls;
using DevTools.Helpers;

namespace DevTools.Pages
{
    public partial class XXXPage : Page
    {
        public XXXPage()
        {
            InitializeComponent();
            LoadState();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            SaveState();
            NavigationService?.Navigate(new HomePage());
        }

        private void SaveState()
        {
            var state = new System.Collections.Generic.Dictionary<string, string>();
            // Save page state
            PageStateManager.SavePageState(this, state);
        }

        private void LoadState()
        {
            var state = PageStateManager.GetPageState(this);
            if (state != null)
            {
                // Restore page state
            }
        }
    }
}
```

---

## 5. Code Review Checklist

### 5.1 i18n Check
- [ ] All UI text uses resource files, no hard-coded strings (including tab headers, category titles, placeholders, MessageBox, etc.)
- [ ] `Strings.resx` (default), `Strings.zh-CN.resx`, and `Strings.en-US.resx` updated synchronously
- [ ] New categories use `CategoryXXX` naming convention

### 5.2 UI Standards Check
- [ ] Buttons use FontAwesome icons with white foreground color (#FFFFFF)
- [ ] Back button uses the circle template (40x40)
- [ ] Action button uses the rectangle template (with icon + text)
- [ ] Pages follow the unified layout template
- [ ] Tab headers use `Header="{x:Static resources:Strings.XXX}"` to bind resources

### 5.3 Architecture Check
- [ ] Home page navigation updated when adding new features (added in the correct `TabItem`)
- [ ] Use `PageStateManager` for page state management
- [ ] Button click events navigate to correct pages
- [ ] No duplicate or dead code

### 5.4 Build Check
- [ ] Project compiles with zero errors
- [ ] No new compiler warnings introduced
