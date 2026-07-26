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

## 2. README i18n Synchronization Standards

### 2.1 Scope
When modifying or updating README files, **both English and Chinese versions must be updated synchronously**:
- `README.md` (English)
- `README_ZH.md` (Simplified Chinese)

### 2.2 Synchronization Requirements
The following content must be kept consistent across both versions:
- Project title and description
- Feature list (translated appropriately)
- RESTful API documentation
- Installation instructions
- Usage steps
- Development documentation
- Project structure
- Localization information
- Changelog/version information
- License
- Footer slogan

### 2.3 Synchronization Process
1. Update `README.md` first with English content
2. Translate the changes and update `README_ZH.md` immediately
3. Verify both files have the same structure and sections
4. Ensure cross-language links work correctly (`README.md` ↔ `README_ZH.md`)

### 2.4 Exceptions
- Language-specific content (e.g., language names in localization section) may differ
- Microsoft Store badge links may include language parameters (`?hl=zh-cn&gl=CN`)

---

## 3. Design System Standards

### 3.1 Color Consistency

All colors must be defined as resources in `Themes/MaterialStylesDark.xaml`. **Hard-coded color values in page XAML files are prohibited.** All pages must use the same color scheme.

### 3.2 Typography Consistency

All pages must use consistent typography:
- Font sizes must be uniform across all pages
- Font weights must be consistent for the same element types
- Text colors must be consistent for the same element types

### 3.3 Component Style Consistency

- **Buttons**: All buttons must use the same style from `MaterialStylesDark.xaml`
- **TextBox**: All text input fields must use the same style
- **Content Border**: All content areas must use consistent border styles and corner radii
- **Card Layout**: All card elements must use consistent layout and spacing

### 3.4 Layout Consistency

All pages must follow the unified layout structure:
```xml
<Grid Margin="20">
    <StackPanel>
        <!-- Back Button -->
        <Button Click="Back_Click" ... />
        
        <!-- Page Title -->
        <TextBlock Text="{x:Static resources:Strings.PageXXX}" FontSize="18" Margin="0,10" />
        
        <!-- Main Content Area -->
        <Border CornerRadius="8" BorderThickness="2" Margin="0,10">
            <Grid>
                <!-- Content -->
            </Grid>
        </Border>
        
        <!-- Action Buttons -->
        <StackPanel Orientation="Horizontal">
            <Button Click="Action1_Click" ... />
            <Button Click="Action2_Click" ... />
        </StackPanel>
    </StackPanel>
</Grid>
```

### 3.5 Spacing Consistency

- Page margin: 20px
- Section spacing: 10px
- Button margin: 4px
- Content padding: 16px

### 3.6 Style Resource Usage

All UI elements must reference style resources instead of inline styles:

```xml
<!-- Correct: Use resource -->
<Border Background="{StaticResource SurfaceBrush}" />
<TextBlock Foreground="{StaticResource OnSurfaceBrush}" />

<!-- Wrong: Hard-coded value -->
<Border Background="#1A1A1A" />
<TextBlock Foreground="#FFFFFF" />
```

### 3.7 Icon Standards
- Use FontAwesome Solid font (`{StaticResource FontAwesomeSolid}`)
- Icon colors must be consistent across all pages
- Common icon sizes: 16px (button), 36px (tool card)

---

## 4. Button Icon Standards

### 4.1 Icon Library
- Use FontAwesome Solid font icons
- FontFamily: `{StaticResource FontAwesomeSolid}`

### 4.2 Back Button Style (Circle)
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

### 4.3 Action Button Style (Rectangle)
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

### 4.4 Common Icon Codes
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

## 5. Home Page Tab Category Standards

### 5.1 Tab Control Structure
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

### 5.2 Tab Item Style
- Tab text must use `Header="{x:Static resources:Strings.CategoryXXX}"` (i18n)
- Tab icon uses the FontAwesome icon code in the `Tag` attribute
- Default state: gray text
- Selected state: white background + blue icon + dark text

### 5.3 Category Naming Convention
- Category name: `CategoryXXX` (e.g., `CategoryEncoding`, `CategoryImage`, `CategoryData`, `CategoryQuery`)
- Must add corresponding entries in `Strings.resx`, `Strings.zh-CN.resx`, and `Strings.en-US.resx`

### 5.4 Tool Button Style
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

### 5.5 Adding a New Tool Category
1. Add `CategoryXXX` resource string to `Strings.resx` (English)
2. Add `CategoryXXX` resource string to `Strings.zh-CN.resx` (Chinese)
3. Add a new `TabItem` in `HomePage.xaml` with `Header` bound to the resource
4. Add tool buttons inside the `TabItem`

### 5.6 Adding a New Tool
1. Create page files: `Pages/XXXPage.xaml` and `Pages/XXXPage.xaml.cs`
2. Add resource strings to `Strings.resx`, `Strings.zh-CN.resx`, and `Strings.en-US.resx`
3. Add tool button to the corresponding `TabItem` in `HomePage.xaml`
4. Add click event handler to `HomePage.xaml.cs`

---

## 6. Page Structure Template

### 6.1 Basic Layout
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

### 6.2 Page Code Template
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

## 7. Design Directory Standards

### 7.1 Directory Structure
The `DevTools.design/` directory contains the HTML design prototype for the DevTools application. It mirrors the WPF page structure and serves as the visual reference for UI development.

```
DevTools.design/
├── DevTools.design          # Canvas metadata (JSON), registers all pages as nodes
├── colors_and_type.css      # Design tokens (CSS custom properties)
├── validation-report.json   # Validation output
└── pages/                   # HTML pages (one per tool/category)
    ├── home.html             # Home page with tab navigation
    ├── encoding-tools.html   # Encoding tools category page
    ├── image-tools.html      # Image tools category page
    ├── data-tools.html       # Data processing category page
    ├── query-tools.html      # Query tools category page
    ├── md5.html              # MD5 tool page
    ├── url-encode.html       # URL encode/decode tool page
    ├── string-escape.html    # String escape tool page
    ├── random-string.html    # Random string generator page
    ├── timestamp.html        # Timestamp tool page
    ├── qrcode.html           # QR code generator page
    ├── barcode.html          # Barcode generator page
    ├── base64-to-image.html  # Base64 to image converter page
    ├── image-to-base64.html  # Image to Base64 converter page
    ├── signature.html        # Handwritten signature page
    ├── qr-barcode-decode.html # QR/barcode decoder page
    ├── json-format.html      # JSON formatter page
    └── district-code.html    # District code query page
```

### 7.2 Design Tokens
All design tokens are defined in `colors_and_type.css` using CSS custom properties with the `--dt-` prefix:

```css
:root {
  --dt-background: #F8F9FC;
  --dt-foreground: #1D1D1F;
  --dt-primary: #007AFF;
  --dt-border: #E5E5EA;
  --dt-radius-md: 12px;
  --dt-shadow-sm: 0 1px 3px rgba(0,0,0,0.06);
  --dt-font-sans: 'Inter', 'SF Pro Display', -apple-system, ...;
}
```

- **Token prefix**: `--dt-` (e.g., `--dt-primary`, `--dt-radius-md`)
- **Hard-coded color values in pages are prohibited** — always reference tokens via `var(--dt-xxx)`
- When adding new tokens, define them in `colors_and_type.css` and inline them into each page's `<style id="theme-vars">` block

### 7.3 Page Structure
Each HTML page must follow the unified structure:

1. **`<style id="theme-vars">`** — Inlined design tokens (copy from `colors_and_type.css`)
2. **Tailwind utility static CSS** — Static replacements for Tailwind classes (no runtime CDN dependency)
3. **`<style id="semantic-token-fallback">`** — Semantic token mappings (`.bg-primary`, `.text-muted-foreground`, etc.)
4. **FontAwesome CDN** — `<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@6.5.1/css/all.min.css">`
5. **Component styles** — Page-specific component CSS (`.tool-card`, `.tab-item`, `.search-input`, etc.)
6. **`<body>`** — Page content using semantic classes and inline styles referencing `var(--dt-xxx)`

### 7.4 Canvas File (`DevTools.design`)
The `.design` file is a JSON registry of all canvas pages. Each page node must have:

```json
{
  "id": "page-xxx",
  "title": "页面标题",
  "type": "page",
  "version": 1,
  "canvasData": { "x": 0, "y": 0, "group": 0 },
  "devMetadata": {
    "htmlSrc": "pages/xxx.html",
    "interactions": []
  }
}
```

- Every HTML page in `pages/` must have a corresponding node in `.design`
- `htmlSrc` must be a relative path starting with `pages/`
- Node IDs must be unique and use the `page-xxx` naming convention

### 7.5 Page Naming Conventions
- **Category pages**: `xxx-tools.html` (e.g., `encoding-tools.html`, `image-tools.html`)
- **Tool pages**: `xxx.html` using kebab-case (e.g., `md5.html`, `url-encode.html`, `qr-barcode-decode.html`)
- Page `<title>` must match the tool name in Chinese

### 7.6 Icon Standards (HTML Pages)
- Use FontAwesome 6.5.1 via CDN link in `<head>`
- Icon classes: `fa-solid fa-xxx` (e.g., `fa-solid fa-lock`, `fa-solid fa-qrcode`)
- Icon colors reference tokens: `style="color: var(--dt-primary);"`
- Icon sizes set via inline `font-size`

### 7.7 Component Styles (Category Pages)
Category pages (`home`, `encoding-tools`, `image-tools`, `data-tools`, `query-tools`) must define these component classes:

| Class | Purpose |
|-------|---------|
| `.tool-card` | Tool card container (140x120, white bg, border, shadow, hover effect) |
| `.icon-circle` | Circular icon container (48x48, centered) |
| `.label` | Tool card label text (14px, centered) |
| `.tab-item` | Tab navigation item (flex, gap, border-bottom indicator) |
| `.active-encoding/image/data/query` | Active tab state (blue text + bottom border) |
| `.search-input` | Pill-shaped search input (rounded-full, muted bg) |

### 7.8 Adding a New Design Page
1. Create `DevTools.design/pages/xxx.html` following the page structure in 7.3
2. Add a new node to `DevTools.design/DevTools.design` with unique `id` and correct `htmlSrc`
3. Use design tokens from `colors_and_type.css` — no hard-coded colors
4. Add FontAwesome icon via CDN link if the page uses icons
5. Define any page-specific component styles in a `<style>` block before `</head>`

### 7.9 Validation
- Run validation script to verify `.design` integrity: `node validate-design-workspace.mjs`
- Validation checks: valid JSON, non-empty `data`, existing `htmlSrc` paths, unique node IDs
- `validation-report.json` must show `success: true` before delivery

---

## 8. CHANGELOG Standards

### 8.1 Format
All notable changes must be documented in `CHANGELOG.md`. Follow the [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) format:

```markdown
## [version] - YYYY-MM-DD

### Added
- **Feature Name** - Description of the new feature
  - Sub-item details

### Changed
- **Change Name** - Description of the change
  - Sub-item details

### Fixed
- **Bug Name** - Description of the bug fix
  - Sub-item details

---
```

### 8.2 Version Numbering
- Use semantic versioning: `MAJOR.MINOR.PATCH`
- `MAJOR`: Breaking changes
- `MINOR`: New features
- `PATCH`: Bug fixes

### 8.3 Section Types
- **Added**: New features, new pages, new APIs
- **Changed**: Modified existing functionality, UI changes, documentation updates
- **Fixed**: Bug fixes, error corrections, security fixes

### 8.4 Entry Guidelines
- Start each entry with a bold feature/change/bug name
- Use past tense (e.g., "Added", "Changed", "Fixed")
- Include sub-items for detailed changes
- Use backticks for code references (e.g., `net8.0-windows`)
- Keep descriptions concise but informative
- Use `---` as separator between versions

### 8.5 Synchronization
- When updating CHANGELOG.md, also update the version number in both README files:
  - `README.md`: **Current Version:** X.Y.Z
  - `README_ZH.md`: **当前版本：** X.Y.Z

---

## 9. Code Review Checklist

### 9.1 i18n Check
- [ ] All UI text uses resource files, no hard-coded strings (including tab headers, category titles, placeholders, MessageBox, etc.)
- [ ] `Strings.resx` (default), `Strings.zh-CN.resx`, and `Strings.en-US.resx` updated synchronously
- [ ] New categories use `CategoryXXX` naming convention

### 8.2 UI Standards Check
- [ ] Buttons use FontAwesome icons with white foreground color (#FFFFFF)
- [ ] Back button uses the circle template (40x40)
- [ ] Action button uses the rectangle template (with icon + text)
- [ ] Pages follow the unified layout template
- [ ] Tab headers use `Header="{x:Static resources:Strings.XXX}"` to bind resources

### 9.3 Architecture Check
- [ ] Home page navigation updated when adding new features (added in the correct `TabItem`)
- [ ] Use `PageStateManager` for page state management
- [ ] Button click events navigate to correct pages
- [ ] No duplicate or dead code

### 9.4 Build Check
- [ ] Project compiles with zero errors
- [ ] No new compiler warnings introduced

### 9.5 Design Directory Check
- [ ] Every HTML page in `DevTools.design/pages/` has a corresponding node in `DevTools.design/DevTools.design`
- [ ] No hard-coded color values in design pages — all use `var(--dt-xxx)` tokens
- [ ] FontAwesome CDN link present in pages that use icons
- [ ] Category pages define component classes (`.tool-card`, `.tab-item`, etc.)
- [ ] `validation-report.json` shows `success: true`
