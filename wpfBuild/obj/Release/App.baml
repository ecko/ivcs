   M S B A M L   `   `   `  ���� d  `PresentationFramework.Classic, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35jMclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.ClassicMicrosoft.Windows.Themes  C ?wpfBuild, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:wpfBuild.converterswpfBuild.converters    wpfBuild.App   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    >d2http://schemas.microsoft.com/expression/blend/2008    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    pMicrosoft_Windows_ThemesMclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Classic    6
converters!clr-namespace:wpfBuild.converters      �� 	Resources     GlassButton �� 
TargetType �� CornerRadius ��Name � RowDefinitions J� Opacity �� RelativeTransform �� CenterX �� CenterY	 �� ScaleX
 �� ScaleY �� AngleX �� AngleY �� CenterX �� CenterY �� Angle �� CenterX �� CenterY T� X T� Y � Color � Offset ��Name �� EndPoint �� 
StartPoint � 	Resources  Key g� 	BeginTime �� 
TargetName �� TargetProperty O� KeyTime O� Value �� Triggers  P� Property! P� Value" �� 
TargetName# �� Property$ �� Value% N� EnterActions  Storyboard1& N� ExitActions' ��Name  Storyboard2  GlassRepeatButton  GlassToggleButton( F�Name  
shapePause) �� Stretch* J� SnapsToDevicePixels 
 MyPath+ �� StrokeLineJoin 	 Shine 	 glassBorderBG, �� Color 
 	glassGlow  	iconShine  glassToggle- A�Name. J� BitmapEffect/ O� 	GlowColor0 O� GlowSize1 O� Opacity2 �Name3 � ColumnDefinitions4 R� KeyTime  System.Windows.Visibility5   Visible6 R� Value7 :� RoutedEvent  	showPause  showPlay  SliderButtonStyle8 �� 
TargetType  SliderThumbStyle  NormalBrush  NormalBorderBrush  	DarkBrush  DisabledBackgroundBrush  HorizontalSlider9 m� 	Placement  
GlyphBrush: m� Fill  
LightBrush; ]� DecreaseRepeatButton< ]� Thumb= ]� IncreaseRepeatButton  VerticalSlider  HorizontalLightBrush  HorizontalNormalBorderBrush> �� Triggers  HorizontalNormalBrush  PressedBrush  DisabledForegroundBrush  WindowBackgroundBrush  SelectedBackgroundBrush   DefaultedBorderBrush ! PressedBorderBrush " DisabledBorderBrush # SolidBorderBrush $ LightBorderBrush % LightColorBrush & ThumbStyleGlass 
' whitey ( glassTabControl-  'System.Windows.Input.KeyboardNavigation?   TabNavigation@ ��NameA ��NameB ��NameC K� IsItemsHostD K� ZIndexE   TabIndexF   DirectionalNavigationG ��Name ) glassTabControl_noTabStripH �� BasedOn * glassTabItemI ]� 
ConditionsJ �� PropertyK �� Value + 
glowRemove , addGlow - 	audioIconL ��Name . 	videoIcon / 	imageIcon 0 TabControlNoTabStrip5   /Microsoft.Windows.Themes.ClassicBorderDecoratorM   ClassicBorderBrushN   
BackgroundO   BorderBrushP   BorderStyleQ   BorderThickness 1 clockTemplateR � 	Resources%  wpfBuild.converters.HourToAngle'  !wpfBuild.converters.MinuteToAngle
S �� TopT �� LeftU ��NameV �� Mode 2 hourToAngleW �� 	Converter 3 minuteToAngle��%�L  &	        7  7  &	 k    7  7  &	 �
    7  7  &	 �   &	 �   &	 8    &		 �    &	
 �    &	 �    &	 �    7  7  &	 ?    &	 �    &	 H    &	 4!    '��  $    &	 ]%    &	 �%    &	 	&    &	 _&    &	 �&    &	 '    &	 �'    &	 �'    &	 �'    &	 �'    &	 �'    &	 +(    &	  �(    &	! �(    &	" -)    &	# =)    &	$ M)    &	 ])    &	% m)    &	& })    '�� "-    &	' a-    (�� v-    *" SystemColors.ControlTextBrushKey+)(�� �-    * SystemColors.HighlightBrushKey+)(�� �-    *!SystemColors.WindowTextBrushKey+)(�� �-    *!SystemColors.HighlightTextBrush+)(�� �-    *SystemColors.ControlBrushKey+)&	( �-    &	) �5    7 ( &	* �5    7 + 7 , '�� �>    'd�  ?    &	- T?    &	. �?    &	/ @    &	0 c@    &	1 EC    7 2 7 3 �� " ��.R��� ���	#FFFFFFFF$��1,1,1,1q�$ 4,4,4,4��._��� $ border�����	#7F000000���	#FF000000$��1,1,1,1q�$ 4,4,4,4��._��  �� $��0.512* ��� $��0.488* �.+��� $
 glow��$��Stretch=�$
��Auto��$��2��$ 4,4,4,4��$ 0Q���
�  V� .~��� $	 0.5Q�$	 0.5Q�$	 1.741Q�$
 2.036Q��� $ 0Q�$ 0Q�$	 0.5Q�$	 0.5Q��� $ 0Q�$	 0.5Q�$	 0.5Q�T� $ -0.013Q�$ 0.433Q�.��� $ 	#B28DBDFF��$ 0Q�� $ 	#008DBDFF��$ 1Q��� $ content��#��z��$��Center=�$��Center=�$
��Auto��$��2�����	#FFEFEFEF$��./Fonts\#Century Gothic)��� $ shine��$��Stretch=�$��0,0,0,0q�$ 4,4,0,0��$	 0.8Q����� $ 0.5,1&�$ 0.5,0&�.��� $ 	#99FFFFFF��$ 0Q�� $ 	#33FFFFFF��$ 1Q� ���� Storyboard1 .��U� $ 00:00:00X�$
 glow��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 1Q��� Storyboard2 .��U� $ 00:00:00X�$
 glow��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 0Q� P�   � ��! . . ��� $
" glow��# � y�$$ Hidden=��� $" border��# � ��$ �	#CC000000�� $" shine��# �@J�Opacity$	$ 0.4Q�P�   �@J�IsMouseOver! . % �� 8��  & �� $!' Storyboard2_BeginStoryboard��8�� P�   � {�! .  . ��� $" shine��# �@J�Opacity$	$ 0.0Q��� $" content��# � ��$ �	#88EFEFEF�� " ��.R��� ���	#FFFFFFFF$��1,1,1,1q�$ 4,4,4,4��._��� $ border�����	#7F000000���	#FF000000$��1,1,1,1q�$ 4,4,4,4��._��  �� $��0.512* ��� $��0.488* �.+��� $
 glow��$��Stretch=�$
��Auto��$��2��$ 4,4,4,4��$ 0Q���
�  V� .~��� $	 0.5Q�$	 0.5Q�$	 1.741Q�$
 2.036Q��� $ 0Q�$ 0Q�$	 0.5Q�$	 0.5Q��� $ 0Q�$	 0.5Q�$	 0.5Q�T� $ -0.013Q�$ 0.433Q�.��� $ 	#B28DBDFF��$ 0Q�� $ 	#008DBDFF��$ 1Q��� $ content��#��z��$��Center=�$��Center=�$
��Auto��$��2�����	#FFEFEFEF$��./Fonts\#Century Gothic)��� $ shine��$��Stretch=�$��0,0,0,0q�$ 4,4,0,0��$	 0.8Q����� $ 0.5,1&�$ 0.5,0&�.��� $ 	#99FFFFFF��$ 0Q�� $ 	#33FFFFFF��$ 1Q� ���� Storyboard1 .��U� $ 00:00:00X�$
 glow��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 1Q��� Storyboard2 .��U� $ 00:00:00X�$
 glow��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 0Q� P�   � ��! . . ��� $
" glow��# � y�$$ Hidden=��� $" border��# � ��$ �	#CC000000�� $" shine��# �@J�Opacity$	$ 0.4Q�P�   �@J�IsMouseOver! . % �� 8��  & �� $!' Storyboard2_BeginStoryboard��8�� P�   � {�! .  . ��� $" shine��# �@J�Opacity$	$ 0.0Q��� $" content��# � ��$ �	#88EFEFEF�� " d�.R��� ���	#FFFFFFFF$��1,1,1,1q�$ 4,4,4,4��._��� $ border�����	#7F000000���	#FF000000$��1,1,1,1q�$ 4,4,4,4��._��  �� $��0.512* ��� $��0.488* �.+��� $
 glow��$��Stretch=�$
��Auto��$��2��$ 4,4,4,4��$ 0Q���
�  V� .~��� $	 0.5Q�$	 0.5Q�$	 1.741Q�$
 2.036Q��� $ 0Q�$ 0Q�$	 0.5Q�$	 0.5Q��� $ 0Q�$	 0.5Q�$	 0.5Q�T� $ -0.013Q�$ 0.433Q�.��� $ 	#B28DBDFF��$ 0Q�� $ 	#008DBDFF��$ 1Q��� $ ContentControl��#��z��$��Center=�$��Center=�$
��Auto��$��2�����	#FFEFEFEF$��./Fonts\#Century Gothic)��� $ shine��$��Stretch=�$��0,0,0,0q�$ 4,4,0,0������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#99FFFFFF��$ 0Q�� $ 	#33FFFFFF��$ 1Q� ���� Storyboard1 .��U� $ 00:00:00X�$
 glow��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 1Q��� Storyboard2 .��U� $ 00:00:00X�$
 glow��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 0Q� P�   � ��! . . ��� $
" glow��# � y�$$ Hidden=��� $" border��# � ��$ �	#CC000000�� $" shine��# �@J�Opacity$	$ 0.4Q�P�   �@J�IsMouseOver! . % �� 8��  & �� $!' Storyboard2_BeginStoryboard��8�� P�   �@d�	IsChecked$
! trueY�. ��� $" ContentControl��# � ��
$ ������ $" ContentControl��# � ��$
$ Bold"�P�   � {�! .  . ��� $" shine��# �@J�Opacity$	$ 0.0Q��� $" ContentControl��# � ��$ �	#88EFEFEFF� $( triangle_play_Copy1�����	#FFFFFFFF$
) Fill=����	#FF000000$
��Left=�$��92.913,160.612,0,0q�$	��Top=�$��24.837��$��38.268��g����V�h�	�{	0 �	h��������ӈ�2�=��	w��
�w�s
��w*Rt
1+±�	V�h�		}�.  * .  F� $
) Fill=�$+ Round=����	#FF000000���	#FF000000+����@B @B ���- �� �@B ��- �@B @B �� $ 0.5,1&�$ 0.5,0&�.��� $ 	#99FFFFFF��$ 0Q�� $ 	#33FFFFFF��$ 1Q��� $, 	#7F000000��
�  V� .~��� $	 0.5Q�$	 0.5Q�$	 1.741Q�$
 2.036Q��� $ 0Q�$ 0Q�$	 0.5Q�$	 0.5Q��� $ 0Q�$	 0.5Q�$	 0.5Q�T� $ -0.013Q�$ 0.433Q�.��� $ 	#B28DBDFF��$ 0Q�� $ 	#008DBDFF��$ 1Q��� $ 0.5,1&�$ 0.5,0&�.��� $ 	#ddFFFFFF��$ 0Q�� $ 	#50FFFFFF��$ 1Q��� " d�.R�� .+�(� .���� $	��100��$	��100��.+�A� $- border_ellipse��#��� 	 ���	#FFFFFFFF$��0,0,0,0q�$	��100��$	��100��$ 1Q�$��3��A� $- black_bg��#��� 	 $��1,1,0,0q�$��98��$��98��$��1�����	#FF000000. O� $/ 	#FF1668D8��$0 9Q�$1 1Q�A� $- 	innerGlow��#��� 
 $ 0.595Q�$��95��$��95��$��2,2,0,0q�* .  }�.  �� $ shine��$ 	50,50,0,0��#���  $��	2.5,2,0,0q�$
��94.5��$��48��* . � $2 Shapes_Grid��$	��100��$	��100��$��Stretch=�$��Stretch=�3 �� $
��0.5* ��� $
��0.5* �.+��� $ border_left��$��18��#��� 	 #���  $��1,1,1,1q�$ 7,7,7,7��$��Right=�$��0,0,3,0q�$��70��}�.  * .  $y�Visible=�$ 0Q��� $ border_right��$��2��$��1��$ 7,7,7,7��$
��Left=�$��Center=�$��18��$��70��$��1,1,1,1q�#���  #��� 	 $��3,0,0,0q�$y�Visible=�$ 0Q�F� $( 	play_path��#��� 	 $
) Fill=�#���  $��Center=�$��10,14.587,0,14.96q�g�������5t�fNl"y�^px�	���0�� �"���C� Ud-;[���.�:����/�Z��Ho#���5t	$��46.355��$��Center=�$��2��$ 1Q� ���� showPlay .��U� $ 00:00:00X�$ border_left��$ (UIElement.Opacity)�.\��� $ 00:00:00.1000000��$ 0Q�U� $ 00:00:00X�$ border_right��$ (UIElement.Opacity)�.\��� $ 00:00:00.1000000��$ 0Q�U� $ 00:00:00X�$ 	play_path��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 1Q��� 	showPause .��U� $ 00:00:00X�$ 	play_path��$ (UIElement.Opacity)�.\��� $ 00:00:00.1000000��$ 0Q�T� $ 00:00:00X�$ border_left��$ (UIElement.Visibility)�..�i� $4 00:00:00.3000000��#6 Z5 U� $ 00:00:00X�$ border_left��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 1Q�U� $ 00:00:00X�$ border_right��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 1Q�T� $ 00:00:00X�$ border_right��$ (UIElement.Visibility)�..�i� $4 00:00:00.3000000��#6 Z5  :� $7 ToggleButton.Checked��.[��� $ ' showPause_BeginStoryboard1��8��  :� $7 ToggleButton.Unchecked��.[��� $' showPlay_BeginStoryboard1��8�� �� "8 ��.��� # �@J�SnapsToDevicePixels$ . �� # �@�OverridesDefaultStyle$ . �� # � ��$ .  �� # � |�$ .  �� # � ��$ �� " ��.R��� 
������ �� "8 n�.��� # �@J�SnapsToDevicePixels$ . �� # �@�OverridesDefaultStyle$ . �� # � ��$$ 20���� # � ��$$ 20���� # � ��$ �� " n�.R�A� $��Ellipse��#���  #���  $��1�� P�   �@J�IsMouseOver! . . ��� $" Ellipse��#$ �  # � ��P�   � {�! .  . ��� $" Ellipse��#$ �  # � ���� " ��.R��  �� $
��Auto ��� $
��Auto �#��z���� $
��Auto �.+�m� $��TopTick��* . $	9 Top=�#: �  $��4��$y�	Collapsed=��� $��TrackBackground��$��0q�$ 2��$��4��$��1��#���  #���  $��1q�]� $��
PART_Track��$��1��; �� #���  $��Slider.DecreaseLarge��< n� #���  = �� #���  $��Slider.IncreaseLarge��m� $��
BottomTick��* . $��2��#: z��$9 Bottom=�$��4��$y�	Collapsed=� P�   �@��TickPlacement$! TopLeft=�. ��� $" TopTick��# � y�$$ Visible=�P�   �@��TickPlacement$! BottomRight=�. ��� $" 
BottomTick��# � y�$$ Visible=�P�   �@��TickPlacement$
! Both=�. ��� $" TopTick��# � y�$$ Visible=��� $" 
BottomTick��# � y�$$ Visible=��� " ��.R�� 3 �� $
��Auto ��� $
��Auto �#��z���� $
��Auto �.+�m� $��TopTick��* . $
9 Left=�#: �  $��4��$y�	Collapsed=��� $��TrackBackground��$��0q�$ 2��$��4��$��1��#���  #���  $��1q�]� $��
PART_Track��$��1��; �� #���  $��Slider.DecreaseLarge��< n� #���  = �� #���  $��Slider.IncreaseLarge��m� $��
BottomTick��* . $��2��#: z��$9 Right=�$��4��$y�	Collapsed=� P�   �@��TickPlacement$! TopLeft=�. ��� $" TopTick��# � y�$$ Visible=�P�   �@��TickPlacement$! BottomRight=�. ��� $" 
BottomTick��# � y�$$ Visible=�P�   �@��TickPlacement$
! Both=�. ��� $" TopTick��# � y�$$ Visible=��� $" 
BottomTick��# � y�$$ Visible=��� "8 ��.��� # �@J�SnapsToDevicePixels$ . �� # �@�OverridesDefaultStyle$ . > P�   �@��Orientation$! 
Horizontal=�. ��� # � ��$	$ 104���� # � ��$$ 21���� # � ��#$ �  P�   �@��Orientation$! Vertical=�. ��� # � ��$$ 21���� # � ��$	$ 104���� # � ��#$ �  �� $	 0,0&�$	 0,1&���� � $
 #FFF��$	 0.0Q�� $
 #CCC��$	 1.0Q��� $	 0,0&�$	 1,0&���� � $
 #FFF��$	 0.0Q�� $
 #CCC��$	 1.0Q��� $	 0,0&�$	 0,1&���� � $
 #FFF��$	 0.0Q�� $
 #EEE��$	 1.0Q��� $	 0,0&�$	 1,0&���� � $
 #FFF��$	 0.0Q�� $
 #EEE��$	 1.0Q��� $	 0,0&�$	 0,1&���� � $
 #FFF��$	 0.0Q�� $
 #AAA��$	 1.0Q��� $	 0,0&�$	 0,1&���� � $
 #BBB��$	 0.0Q�� $
 #EEE��$	 0.1Q�� $
 #EEE��$	 0.9Q�� $
 #FFF��$	 1.0Q��� $
, #888���� $
, #EEE���� $
, #FFF���� $
, #DDD���� $	 0,0&�$	 0,1&���� � $
 #CCC��$	 0.0Q�� $
 #444��$	 1.0Q��� $	 0,0&�$	 1,0&���� � $
 #CCC��$	 0.0Q�� $
 #444��$	 1.0Q��� $	 0,0&�$	 0,1&���� � $
 #777��$	 0.0Q�� $
 #000��$	 1.0Q��� $	 0,0&�$	 0,1&���� � $
 #444��$	 0.0Q�� $
 #888��$	 1.0Q��� $
, #AAA���� $
, #888���� $
, #AAA���� $
, #444���� $
, #DDD���� "8 n�.��� # �@�OverridesDefaultStyle$ . �� # � ��$ Z� �� # � ��#$ � ���� # � ��$$ 21���� # � ��$$ 11���� # � ��$ �� " n�.R��� $ 3��$��1,1,1,1q����	#FFFFFFFF
���   �._��� ���	#FF000000$��1,1,1,1q�$ 3��$
��Auto��$
��Auto�����	#7F000000._�� $
��Auto��$
��Auto�� �� $��0.512* ��� $��0.488* �.+��� $
 glow��$��Stretch=�$
��Auto��$��2��$ 3��$ 0Q���
�  V� .~��� $	 0.5Q�$	 0.5Q�$	 1.741Q�$
 2.036Q��� $ 0Q�$ 0Q�$	 0.5Q�$	 0.5Q��� $ 0Q�$	 0.5Q�$	 0.5Q�T� $ -0.013Q�$ 0.433Q�.��� $ 	#B28DBDFF��$ 0Q�� $ 	#008DBDFF��$ 1Q��� $ shine��$��Stretch=�$��0q�$ 3,3,0,0������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#99FFFFFF��$ 0Q�� $ 	#33FFFFFF��$ 1Q� P�   �@n�
IsDragging! . . ��� #$ � ��# � ��P�   � {�! .  . ��� #$ � ��# � ���� "8 ��.��� # � ��$
$ Left=��� # � ��$$ Center=��� $, 	#FFEFEFEF���� $, 
DodgerBlue���� $, #0E273B���� $, DimGray���� $, Orange���� $, 	#FF0A0A0A���� "8 ��.��� # � ��$ �	#FFEFEFEF�� # � ��
$ ���� �� # � ��$ �	#00FFFFFF�� # � ��$$ 3q��� # � ��$$ 0q��� # � ��$$ 1q��� # � ��$$ 10���� # � ��$$ 10���� # � ��$$ Center=��� # � ��$$ Center=��� # � ��$ �� " ��.R�� }�. * . $? Local=� �� $@ RowDefinition0��$
��Auto ��� $@ RowDefinition1��$��* �3 �� $A ColumnDefinition0���� $A ColumnDefinition1��$��0 �.+��� $B HeaderPanel��$��Center=�$��Stretch=�$��0��$��0��C . $D 1��$E 1��� $2 ContentPanel��$��0��$��1��$F 	Contained=�$E 2��$? Local=�.+��� $ classicBorderDecorator��#��z��._��� $G PART_SelectedContentHost��#* z* $��SelectedContent���� $ HeaderBG��$��Stretch=�$
��Auto��#��z��$��1�� P�   �@��TabStripPlacement$! Bottom=�. ��� $" HeaderPanel��# � ��$$ 1���� $" ContentPanel��# � ��$$ 0���� $" RowDefinition0��# � ��$$ * ��� $" RowDefinition1��# � ��$
$ Auto ��� $" HeaderPanel��# � ��$$ 2,0,2,2q��� $" HeaderBG��# � ��$$ 1���� $" classicBorderDecorator��$ Z� # � ��P�   �@��TabStripPlacement$
! Left=�. ��� $" HeaderPanel��# � ��$$ 0���� $" ContentPanel��# � ��$$ 0���� $" HeaderPanel��# � ��$$ 0���� $" ContentPanel��# � ��$$ 1���� $" ColumnDefinition0��# � ��$
$ Auto ��� $" ColumnDefinition1��# � ��$$ * ��� $" RowDefinition0��# � ��$$ * ��� $" RowDefinition1��# � ��$$ 0 ��� $" HeaderPanel��# � ��$$ 2,2,0,2q�P�   �@��TabStripPlacement$! Right=�. ��� $" HeaderPanel��# � ��$$ 0���� $" ContentPanel��# � ��$$ 0���� $" HeaderPanel��# � ��$$ 1���� $" ContentPanel��# � ��$$ 0���� $" ColumnDefinition0��# � ��$$ * ��� $" ColumnDefinition1��# � ��$
$ Auto ��� $" RowDefinition0��# � ��$$ * ��� $" RowDefinition1��# � ��$$ 0 ��� $" HeaderPanel��# � ��$$ 0,2,2,2q�P�   � {�! .  . ��� #$ � ��# � ���� "8 ��8H   �� "8 ��.��� # � ��$$ 	12,2,12,2q��� # � ��#$ � ���� # � ��$ �	#FFEFEFEF�� # � ��$$ ./Fonts\#Century Gothic)��� # � ��$$ 13(��� # � ��$$ Stretch=��� # � ��$$ Stretch=��� # �@�FocusVisualStyle$ Z� �� # � ��$ �� " �� ���� addGlow .��U� $ 00:00:00X�$
 glow��$ (UIElement.Opacity)�.\��� $ 00:00:00.2000000��$
 0.35Q��� 
glowRemove .��U� $ 00:00:00X�$
 glow��$ (UIElement.Opacity)�.\��� $ 00:00:00.3000000��$ 0Q�.R�� .+��� $ border�����	#8AFFFFFF$��0,0,0,0q�$ 3,3,3,3��$	��Top=�$
��Auto��$��1��$��	-1,0,-1,0q�._��� $ border1�����	#7F000000���	#FF000000$��0.5,0,0.5,1q�$ 3,3,3,3��._��  �� $��0.512* ��� $��0.488* �.+��� $
 glow��$��Stretch=�$
��Auto��$��2��$ 3,3,3,3��$ 0Q���
�  V� .~��� $	 0.5Q�$	 0.5Q�$	 1.741Q�$
 2.036Q��� $ 0Q�$ 0Q�$	 0.5Q�$	 0.5Q��� $ 0Q�$	 0.5Q�$	 0.5Q�T� $ -0.013Q�$ 0.433Q�.��� $ 	#B28DBDFF��$ 0Q�� $ 	#008DBDFF��$ 1Q��� $ shine��$��Stretch=�$��0,0,0,0q�$ 3,3,0,0��$
 0.35Q����� $ 0.5,1&�$ 0.5,0&�.��� $ 	#99FFFFFF��$ 0Q�� $ 	#33FFFFFF��$ 1Q��� #* z* $��Header����. $��5,3,5,3q�$��2��$��Center=�$��Center=� ]� I �� J �@J�IsMouseOverK . �� J �@��
IsSelectedK .  & �� $ ' glowRemove_BeginStoryboard��8��  % �� $' addGlow_BeginStoryboard��8�� P�   �@��
IsSelected! . . ��� # �@K�ZIndex$$ 1���� # � ��$ �	#FF1E90FF�� $" shine��# �@J�Opacity$$ 0.095Q��� $" border��# � ��$$ 1,0,1,0q�P�   �@��TabStripPlacement$! Bottom=�P�   �@��TabStripPlacement$
! Left=�P�   �@��TabStripPlacement$! Right=�]� I �� J �@��
IsSelectedK . �� J �@��TabStripPlacement$	K Top=�./��� # � ��$$ -2q�]� I �� J �@��
IsSelectedK . �� J �@��TabStripPlacement$K Bottom=�]� I �� J �@��
IsSelectedK . �� J �@��TabStripPlacement$
K Left=�./��� # � ��$$ 	11,2,14,2q��� # � ��$$ -2q�]� I �� J �@��
IsSelectedK . �� J �@��TabStripPlacement$K Right=�./��� # � ��$$ 	14,2,11,2q��� # � ��$$ -2q�P�   � {�! .  . ��� #$ � ��# � ���� "8 ��.��� # �@�FocusVisualStyle$ Z� �� "8 d�.��� # �@�FocusVisualStyle$ Z� �� .R��� $L audioIconImage��$%��Resources/images\silk\music.png��$��17��$��16���� .R��� $L videoIconImage��$$��Resources/images\silk\film.png��$��17��$��16���� .R��� $L imageIconImage��$'��!Resources/images\silk\picture.png��$��17��$��16���� "8 ��.��� # � ��#$ � ���� # � ��#$ � ���� # � ��#$ ZM �� # � ��$$ 3q��� # � ��$$ 0q��� # � ��$$ 1q��� # � ��$$ 10���� # � ��$$ 10���� # � ��$$ Center=��� # � ��$$ Center=��� # � ��$ �� " ��.R�� }�. * . $? Local=� �� $@ RowDefinition0��$
��Auto ��� $@ RowDefinition1��$��* �3 �� $A ColumnDefinition0���� $A ColumnDefinition1��$��0 �.+��� $B HeaderPanel��$��2,2,2,0q�
������ C . $D 1��$��0��$��0��$E 1��$y�	Collapsed=�� $2 ContentPanel��$��0��$��1��$F 	Contained=�$E 2��$? Local=�.+�  #N z��#O z��$P Raised=�#Q z��._��� $G PART_SelectedContentHost��$��0q�#* z* $��SelectedContent���� .R�� R ��  hourToAngle   minuteToAngle .+�A� $	��108��$	��108��$��3������ .��� $ 	LightBlue��$ 0Q�� $ DarkBlue��$ 1Q�A� $��Center=�$��Center=�$	��104��$	��104��
����ح�$��3������ .��� $ DarkBlue��$ 0Q�� $ 	LightBlue��$ 1Q��� $	��102��$	��102��.+�A� $��8��$��8��
���   �$S 46��$T 46���� $S 5��$T 48��
���   �$��4��$��8��z��� $ 2Q�$ 46Q�$ 0Q��� $S 5��$T 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$ 30Q��� $S 5��$T 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$ 60Q��� $S 5��$T 48��
���   �$��4��$��8��z��� $ 2Q�$ 46Q�$ 90Q��� $S 5��$T 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 120Q��� $S 5��$T 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 150Q��� $S 5��$T 48��
���   �$��4��$��8��z��� $ 2Q�$ 46Q�$	 180Q��� $S 5��$T 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 210Q��� $S 5��$T 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 240Q��� $S 5��$T 48��
���   �$��4��$��8��z��� $ 2Q�$ 46Q�$	 270Q��� $S 5��$T 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 300Q��� $S 5��$T 49��
���   �$��2��$��6��z��� $ 2Q�$ 46Q�$	 330Q��� $U HourHand��$S 21��$T 48��
���   �$��4��$��30��z��� 	HourHand2��$ 2Q�$ 30Q� �� $V OneWay=�8W   �� $U 
MinuteHand��$S 6��$T 49��
���   �$��2��$��45��z��� $ 1Q�$ 45Q� �� $V OneWay=�8W  