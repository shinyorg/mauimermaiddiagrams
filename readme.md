# Shiny MAUI Diagrams

Render Mermaid JS flowchart diagrams natively in .NET MAUI using pure MAUI Graphics (IDrawable/ICanvas). No WebView, no SkiaSharp, no reflection — fully AOT-compliant.

---

## Features

### Mermaid Flowchart Rendering

| Capability | Description |
|:-----------|:------------|
| Flowchart directions | `graph TD`, `graph LR`, `graph BT`, `graph RL` |
| Node shapes | Rectangle `[text]`, Rounded `(text)`, Stadium `([text])`, Circle `((text))`, Diamond `{text}`, Hexagon `{{text}}` |
| Edge styles | Solid `-->`, Open `---`, Dotted `-.->`, Dotted open `-.-`, Thick `==>`, Thick open `===` |
| Edge labels | `-->\|label\|` pipe-delimited labels |
| Subgraphs | `subgraph title ... end` with nesting |
| Comments | `%% comment` line comments |
| Edge chaining | `A --> B --> C` multi-target chains |

### Layout Engine

| Feature | Description |
|:--------|:------------|
| Algorithm | Sugiyama layered graph (same family as Graphviz, dagre/Mermaid.js) |
| Phases | Cycle removal, layer assignment, crossing minimization, positioning, edge routing |
| Directions | Top-to-bottom, bottom-to-top, left-to-right, right-to-left |
| Subgraph bounds | Automatic bounding box calculation with padding |

### Theming

| Theme | Description |
|:------|:------------|
| `DefaultTheme` | Blue/purple — matches Mermaid.js default |
| `DarkTheme` | Dark navy background with red accents |
| `ForestTheme` | Natural green tones |
| `NeutralTheme` | Light gray, minimal color |

### Interaction

| Feature | Description |
|:--------|:------------|
| Pan | Drag to pan the diagram |
| Pinch-to-zoom | Pinch gesture for zoom in/out |
| ZoomLevel | Bindable property for programmatic zoom control |

---

## Getting Started

### 1. Install

Add a project reference or NuGet package:

```xml
<ProjectReference Include="path/to/Shiny.Maui.MermaidDiagrams.csproj" />
```

### 2. Configure MauiProgram.cs

```csharp
using Shiny.Maui.MermaidDiagrams.Extensions;

builder.UseShinyDiagrams();
```

### 3. Add XAML Namespace

```xml
xmlns:diagram="http://shiny.net/maui/diagrams"
```

### 4. Use the Control

```xml
<diagram:MermaidDiagramControl
    DiagramText="graph TD; A[Start] --> B{Decision}; B -->|Yes| C[Do It]; B -->|No| D[Skip];" />
```

---

## XAML Usage

### Basic Flowchart

```xml
<diagram:MermaidDiagramControl
    DiagramText="graph TD; A-->B; A-->C; B-->D; C-->D;" />
```

### With Theme

```xml
<diagram:MermaidDiagramControl
    DiagramText="{Binding MermaidText}"
    Theme="{StaticResource DarkTheme}" />
```

### With Zoom

```xml
<diagram:MermaidDiagramControl
    DiagramText="{Binding MermaidText}"
    ZoomLevel="1.5" />
```

### Error Handling

```xml
<diagram:MermaidDiagramControl
    x:Name="diagram"
    DiagramText="{Binding MermaidText}" />

<Label Text="{Binding Source={x:Reference diagram}, Path=ParseError}"
       TextColor="Red"
       IsVisible="{Binding Source={x:Reference diagram}, Path=ParseError, Converter={StaticResource IsNotNull}}" />
```

---

## Mermaid Syntax Reference

### Directions

```
graph TD    %% Top to Bottom (default)
graph TB    %% Top to Bottom (alias)
graph BT    %% Bottom to Top
graph LR    %% Left to Right
graph RL    %% Right to Left
```

### Node Shapes

```
A[Rectangle]
B(Rounded Rectangle)
C([Stadium])
D((Circle))
E{Diamond}
F{{Hexagon}}
```

### Edge Types

```
A --> B       %% Solid arrow
A --- B       %% Solid line (no arrow)
A -.-> B      %% Dotted arrow
A -.- B       %% Dotted line
A ==> B       %% Thick arrow
A === B       %% Thick line
```

### Edge Labels

```
A -->|Yes| B
A -->|No| C
```

### Subgraphs

```
graph TD
    subgraph Frontend
        A[Web App]
        B[Mobile App]
    end
    subgraph Backend
        C[API Server]
        D[Database]
    end
    A --> C
    B --> C
    C --> D
```

### Comments

```
graph TD
    %% This is a comment
    A --> B
```

---

## Bindable Properties

| Property | Type | Default | Description |
|:---------|:-----|:--------|:------------|
| `DiagramText` | `string` | `""` | Mermaid flowchart text to render |
| `Theme` | `DiagramTheme` | `DefaultTheme` | Visual theme for the diagram |
| `ZoomLevel` | `float` | `1.0` | Zoom scale factor |
| `ParseError` | `string?` | `null` | Error message if parsing fails (read-only) |
| `LayoutOptions` | `DiagramLayoutOptions` | defaults | Layout spacing and sizing options |

## Layout Options

| Property | Type | Default | Description |
|:---------|:-----|:--------|:------------|
| `NodeWidth` | `float` | `150` | Default node width |
| `NodeHeight` | `float` | `50` | Default node height |
| `HorizontalSpacing` | `float` | `60` | Horizontal gap between nodes |
| `VerticalSpacing` | `float` | `80` | Vertical gap between layers |
| `SubgraphPadding` | `float` | `30` | Padding inside subgraph boxes |
| `FontSize` | `float` | `14` | Font size for text |
| `Margin` | `float` | `40` | Outer margin around the diagram |

---

## Architecture

```
Mermaid Text → Lexer → Parser → DiagramModel → Layout Engine → LayoutResult → Renderer → ICanvas
```

| Layer | Description |
|:------|:------------|
| **Parsing** | Hand-written lexer + recursive-descent parser — zero dependencies, AOT safe |
| **Models** | POCOs for nodes, edges, subgraphs, directions, shapes |
| **Layout** | Sugiyama layered graph algorithm (5 phases) |
| **Rendering** | `IDrawable` implementation drawing via `ICanvas` |
| **Control** | `MermaidDiagramControl : ContentView` wrapping `GraphicsView` with gestures |

---

## Custom Themes

Create a custom theme by subclassing `DiagramTheme`:

```csharp
public class MyCustomTheme : DiagramTheme
{
    public override Color BackgroundColor => Colors.White;
    public override Color TextColor => Colors.Black;
    public override float FontSize => 14;

    public override NodeRenderStyle DefaultNodeStyle => new()
    {
        FillColor = Color.FromArgb("#E3F2FD"),
        StrokeColor = Color.FromArgb("#1565C0"),
        TextColor = Colors.Black
    };

    public override EdgeRenderStyle DefaultEdgeStyle => new()
    {
        StrokeColor = Color.FromArgb("#757575"),
        LabelBackgroundColor = Colors.White,
        LabelTextColor = Colors.Black
    };

    public override SubgraphRenderStyle DefaultSubgraphStyle => new()
    {
        FillColor = Color.FromArgb("#F5F5F5"),
        StrokeColor = Color.FromArgb("#BDBDBD"),
        TitleColor = Colors.Black
    };
}
```

---

## Programmatic Usage

You can use the parser and layout engine independently of the control:

```csharp
using Shiny.Maui.MermaidDiagrams.Parsing;
using Shiny.Maui.MermaidDiagrams.Layout;

// Parse mermaid text to a model
var model = MermaidParser.Parse("graph TD; A-->B; B-->C;");

// Run layout
var engine = new SugiyamaLayoutEngine();
var options = new DiagramLayoutOptions { NodeWidth = 120, VerticalSpacing = 60 };
var result = engine.Layout(model, options);

// result.NodeLayouts, result.EdgeRoutes, result.SubgraphLayouts, result.TotalBounds
```

---

## Requirements

- .NET 10.0+
- .NET MAUI (iOS, Android, Mac Catalyst)

## License

MIT
