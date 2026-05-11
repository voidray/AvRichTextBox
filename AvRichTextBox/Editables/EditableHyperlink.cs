using Avalonia.Media;

namespace AvRichTextBox;

public class EditableHyperlink : EditableRun
{
   internal IBrush HyperlinkBrush
   {
       get => _hyperlinkBrush;
       set
       {
           _hyperlinkBrush = value;
           _decoration = null;
            ForceFormatting();
        }
   }
   private IBrush _hyperlinkBrush = Brushes.Blue;
   private TextDecorationCollection? _decoration;

   private TextDecorationCollection GetDecoration()
   {
       if (_decoration == null)
       {
           _decoration = [new() {
               Location = TextDecorationLocation.Underline,
               Stroke = _hyperlinkBrush,
               StrokeThicknessUnit = TextDecorationUnit.Pixel,
               StrokeThickness = 1
           }];
       }
       return _decoration;
   }

   public EditableHyperlink(string displayText, string navigateUri) 
   {  
      Id = ++FlowDocument.InlineIdCounter; 
      Text = displayText;
      NavigateUri = navigateUri;

      ForceFormatting();
      
   }

   internal void ForceFormatting()
   {
      this.Foreground = _hyperlinkBrush;
      this.TextDecorations = GetDecoration();
   }

   internal EditableHyperlink() { ForceFormatting(); }

   protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
   {
      base.OnPropertyChanged(change);

      //Prevent user change to hyperlink formatting
      if (change.Property == ForegroundProperty && !Equals(change.NewValue, _hyperlinkBrush))
      {
         SetCurrentValue(ForegroundProperty, _hyperlinkBrush);
      }
      else if (change.Property == TextDecorationsProperty && !ReferenceEquals(change.NewValue, GetDecoration()))
      {
         SetCurrentValue(TextDecorationsProperty, GetDecoration());
      }
   }

   public string NavigateUri { get; set; } = "";

   public override IEditable Clone() =>

     new EditableHyperlink(this.Text!, this.NavigateUri)
     {
        FontStyle = this.FontStyle,
        FontWeight = this.FontWeight,
        TextDecorations = this.TextDecorations,
        FontSize = this.FontSize,
        FontFamily = this.FontFamily,
        Background = this.Background,
        MyParagraphId = this.MyParagraphId,
        MyFlowDoc = this.MyFlowDoc,
        TextPositionOfInlineInParagraph = this.TextPositionOfInlineInParagraph,  //necessary because clone is produced when calculating range inline positions
        IsLastInlineOfParagraph = this.IsLastInlineOfParagraph,
        BaselineAlignment = this.BaselineAlignment,
        Foreground = this.Foreground,
        HyperlinkBrush = this.HyperlinkBrush,
     };


   public override IEditable CloneWithId()
   {
      IEditable IdClone = this.Clone();
      IdClone.Id = this.Id;
      return IdClone;
   }


#if DEBUG
   // FOR DEBUGGER PANEL
   public override string DisplayInlineText => "{>HYPERLINK<}" + $" \"{Text}\"";
#endif

}

