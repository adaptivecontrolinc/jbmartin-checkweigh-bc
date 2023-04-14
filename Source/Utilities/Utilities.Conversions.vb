
Namespace Utilities

  Partial Public Class Conversions

    ' Conversions we are using 
    '   1 Kilogram = 2.204623 pounds
    '   1 US Gallon = 3.785412 liters
    '   1 UK Gallon = 4.54609 liters

    Public Shared ReadOnly Property GramsToOunces() As Double
      Get
        Return GramsToPounds * 16
      End Get
    End Property

    Public Shared ReadOnly Property GramsToPounds() As Double
      Get
        Return KilogramsToPounds / 1000
      End Get
    End Property

    Public Shared ReadOnly Property KilogramsToOunces() As Double
      Get
        Return KilogramsToPounds * 16
      End Get
    End Property

    Public Shared ReadOnly Property KilogramsToPounds() As Double
      Get
        Return 2.204623
      End Get
    End Property

    Public Shared ReadOnly Property OuncesToGrams() As Double
      Get
        '  28.3495231
        Return PoundsToGrams / 16
      End Get
    End Property

    Public Shared ReadOnly Property OuncesToKilograms() As Double
      Get
        Return OuncesToGrams / 1000
      End Get
    End Property

    Public Shared ReadOnly Property PoundsToGrams() As Double
      Get
        Return PoundsToKilograms * 1000
      End Get
    End Property

    Public Shared ReadOnly Property PoundsToKilograms() As Double
      Get
        Return 1 / KilogramsToPounds
      End Get
    End Property

    Public Shared ReadOnly Property LitersToGallonsUK() As Double
      Get
        Return 0.219969
      End Get
    End Property

    Public Shared ReadOnly Property LitersToGallonsUS() As Double
      Get
        Return 0.264172
      End Get
    End Property

    Public Shared ReadOnly Property GallonsUSToLiters() As Double
      Get
        Return 3.785412
      End Get
    End Property

    Public Shared ReadOnly Property GallonsUKToLiters() As Double
      Get
        Return 4.54609
      End Get
    End Property

    Public Shared ReadOnly Property GallonsUKToGallonsUS() As Double
      Get
        Return 1.20094
      End Get
    End Property

    Public Shared ReadOnly Property GallonsUSToGallonsUK() As Double
      Get
        Return 0.83267
      End Get
    End Property

    Public Shared ReadOnly Property GallonsUSToPounds() As Double
      Get
        Return GallonsUSToLiters * KilogramsToPounds
      End Get
    End Property

    Public Shared ReadOnly Property PoundsPerGallonUSToGramsPerLiter() As Double
      Get
        Return PoundsToGrams / GallonsUSToLiters
      End Get
    End Property

    Public Shared ReadOnly Property MetersToYards() As Double
      Get
        Return 1.0936
      End Get
    End Property

    Public Shared ReadOnly Property TonsToKilograms() As Double
      Get
        Return 907.185
      End Get
    End Property

    Public Shared ReadOnly Property TonsToPounds() As Double
      Get
        Return 2000
      End Get
    End Property

    Public Shared ReadOnly Property YardsToMeters() As Double
      Get
        Return 0.9144
      End Get
    End Property

    Public Shared Function CentigradeToFarenheit(centigrade As Double) As Double
      ' Assumes tenths
      Return ((centigrade * 9) / 5) + 320
    End Function

    Public Shared Function CentigradeToFarenheit(centigrade As Short) As Short
      ' Assumes tenths
      Dim farenheit As Double = CentigradeToFarenheit(CDbl(centigrade))
      Return CShort(farenheit)
    End Function

    Public Shared Function FarenheitToCentigrade(farenheit As Double) As Double
      ' Assumes tenths
      Return ((farenheit - 320) / 9) * 5
    End Function

    Public Shared Function FarenheitToCentigrade(farenheit As Short) As Short
      ' Assumes tenths 
      Dim centrigrade As Double = FarenheitToCentigrade(CDbl(farenheit))
      Return CShort(centrigrade)
    End Function


    Public Shared Function ColorToOleRgb(ByVal argb As Integer) As Integer
      ' In OleRGB red is the least significant 8-bits, in .Net blue is the least significant bit (BGR versus RGB)

      Dim color = Drawing.Color.FromArgb(argb)
      Return ColorToOleRgb(color)
    End Function

    Public Shared Function ColorToOleRgb(ByVal color As System.Drawing.Color) As Integer
      ' In OleRGB red is the least significant 8-bits, in .Net blue is the least significant bit (BGR versus RGB)

      Dim red As Integer = color.R                 ' 
      Dim green As Integer = color.G * 256         ' 2^8
      Dim blue As Integer = color.B * 65536        ' 2^16

      Return red + green + blue
    End Function

    Public Shared Function OleRgbToColor(ByVal rgb As Integer) As System.Drawing.Color
      ' In OleRGB red is the least significant 8-bits, in .Net blue is the least significant bit (BGR versus RGB)
      Dim red As Integer = rgb And 255
      Dim green As Integer = (rgb \ 256) And 255
      Dim blue As Integer = (rgb \ 65536) And 255

      Return Drawing.Color.FromArgb(red, green, blue)
    End Function

    Public Shared Function OleRgbOrArgbToColor(ByVal colorInt As Integer) As System.Drawing.Color
      If colorInt < 0 OrElse colorInt > &HFFFFFF Then
        Return Drawing.Color.FromArgb(colorInt)
      Else
        Return OleRgbToColor(colorInt)
      End If
    End Function

  End Class

End Namespace
