   M S B A M L   `   `   `  ���� C  ?wpfBuild, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:wpfBuildwpfBuild  5clr-namespace:wpfBuild.controlswpfBuild.controls  O Kmscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem d `PresentationFramework.Classic, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35jMclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.ClassicMicrosoft.Windows.Themes E'clr-namespace:KineticScrollingPrototypeKineticScrollingPrototype  [ WSystem.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms     wpfBuild.Window1   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    &localclr-namespace:wpfBuild    8customControlsclr-namespace:wpfBuild.controls    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    7System&clr-namespace:System;assembly=mscorlib    >d2http://schemas.microsoft.com/expression/blend/2008    pMicrosoft_Windows_ThemesMclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Classic    4uc'clr-namespace:KineticScrollingPrototype    Mwf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms    -   $��window_main��  � Title$
  IVCS��$	��800��$	��600�� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�$��./Fonts\#Century Gothic)� � 	Resources  
  Brush1 �� EndPoint �� 
StartPoint � Color � Offset  	audioIcon  	videoIcon  	imageIcon  MediaInfoLabel	 �� 
TargetType
 �� Property �� Value  MediaInfoLabelRightCol �� BasedOn  MediaInfoValue 
 whitey  MediaInfoValueRightCol 	 StarToggleButton �� 
TargetType �� Triggers P� Property P� Value �� 
TargetName  
 HorizontalSliderUpThumbStyle J� SnapsToDevicePixels J� Opacity " HorizontalSliderDownThumbStyle  SliderRepeatButtonStyle  HorizontalSliderThumbStyle5  /Microsoft.Windows.Themes.ClassicBorderDecorator   
Background   ClassicBorderBrush   BorderBrush   BorderStyle   BorderThickness ��Name   VerticalSliderLeftThumbStyle ! VerticalSliderRightThumbStyle  VerticalSliderThumbStyle  media_position_slider � ColumnDefinitions � RowDefinitions m�Name m� Fill m� 	Placement  Name  ]�Name! ]� DecreaseRepeatButton" �� DecreaseLarge# ]� IncreaseRepeatButton$ �� IncreaseLarge% ]� Thumb& n�Name  ThumbStyleGlass��%�  &	        &	 Z     &	 �     &	 �     &	 *    &	 �    7  &	 �    &	 ,    7  &		 V    &	
 �    &	 k    &	 9	    &	 �	    &	 I    &	     &	      &	 Z    7  7  7  7 
 7  �� $ 1,0.5&�$ 0,0.5&�.��� $ 	#FF000000��$ 0Q�� $ 	#FFFFFFFF��$ 1Q��� .R��� $%��Resources/images\silk\music.png��$��17��$��16���� .R��� $$��Resources/images\silk\film.png��$��17��$��16���� .R��� $'��!Resources/images\silk\picture.png��$��17��$��16���� "	 ��.��� 
 � ��$ Right=��� 
 � ��$ 13(��� 
 � ��$
 Bold"��� 
 � ��
 ������ "	 ��8   .��� 
 � ��$
 Left=��� "	 ��.��� 
 � ��$ Right=��� 
 � ��$ 11(��� 
 � ��# �  �� 
 � ��$ 0,-1,0,1q��� "	 ��8   .��� 
 � ��$
 Left=��� "	 d�.��� 
 � �� �� " d�.R��� $��12��$��12��.+�F� $
��star��
�������s����@KL     � 	=  	= �     	= ���- ��j р� �ا �@KL @T� р�[ @T� �@T� �ا � z ��j ��ا  	= ���j  	= р�[     	 P�  �@d�	IsChecked$
 TrueY�. ��� $
 star��
 � ��
 � ����� 
 �@�FocusVisualStyle Z� �� "	 n�.��� 
 �@�OverridesDefaultStyle . �� 
 � �� Z� �� 
 � ��# � ���� 
 � ��$ 21���� 
 � ��$ 11���� 
 � �� �� " n�.R���  . .+�F� #��z��=�����ا @o@�    @o@�    ��[ �@KL @B р�[ @B ��ا ��[ 	F� #��� ��$	��1.0��+���� �  -1� � `�S �@KL  � �`�S  � 	F� #��� ��$	��1.0��4����`�S  � р�[  � Ѡ7� `�S Ѡ7�  �8�     �8	F� #��� ��$	��1.0��+����`� ��!�`� `�S �@KL `� �`�S `� 	F� #��� ��$	��1.0��4����`�S `� р�[ `� �`�� `�S �`�� ��)�@B ��)	F� #��z��$	 0.5Q�4����@T� ��!р� ��!р� ��[ �`�S �� �@T� ��[ 	 P�  �@n�
IsDragging . . ��� # � ��
 � ��P�  � {� .  . ��� # � ��
 � ���� "	 n�.��� 
 �@�OverridesDefaultStyle . �� 
 � �� Z� �� 
 � ��# � ���� 
 � ��$ 21���� 
 � ��$ 11���� 
 � �� �� " n�.R���  . .+�F� #��z��=�����ا     �        �    ��� �@KL @o@р�[ @o@��ا ��� 	F� #��� ��$	��1.0��4���Ѐ��  � � �  � � � ��� �@KL  �8�`�S  �8	F� #��� ��$	��1.0��+����`�S  �8р�[  �8Ѡ7� ��� Ѡ7�     	F� #��� ��$	��1.0��4����@T� `� �`� `� �`� ��� �@KL ��)�`�S ��)	F� #��� ��$	��1.0��+����`�S ��)р�[ ��)�`�� ��� �`�� @B 	F� #��z��$	 0.5Q�4����@T� �� р� �� р� ��� �`�S ��!�@T� ��� 	 P�  �@n�
IsDragging . . ��� # � ��
 � ��P�  � {� .  . ��� # � ��
 � ���� "	 ��.��� 
 �@�OverridesDefaultStyle . �� 
 � �� .  �� 
 � |� .  �� 
 � ��
 ���� �� 
 � �� �� " ��.R��� #��z���� "	 n�.��� 
 �@�OverridesDefaultStyle . �� 
 � �� Z� �� 
 � ��# � ���� 
 � ��$ 21���� 
 � ��$ 11���� 
 � �� �� " n�.R�  # z��# Z $ Raised=�$ 3q�._��� $ ForegroundRect��#��z��$��-1q�$	 0.5Q� P�  �@n�
IsDragging . . ��� # � ��
 � ��P�  � {� .  . ��� # � ��
 � ���� "	 n�.��� 
 �@�OverridesDefaultStyle . �� 
 � �� Z� �� 
 � ��# � ���� 
 � ��$ 21���� 
 � ��$ 11���� 
 � �� �� " n�.R���  . .+�F� #��z��=����@o@�ا �@o@    р�[     �    @KL �    ��[ р�[ �ا 	F� #��� ��$	��1.0��+���� -1 � �`�S  � � � @KL � � `�S 	F� #��� ��$	��1.0��4���� � `�S � � ��[ �`�S �7� � �8�7� � �8    	F� #��� ��$	��1.0��+������!`� �`�S `� �`� @KL �`� `�S 	F� #��� ��$	��1.0��4����`� `�S �`� ��[ �`�S `�� ���)`�� ���)@B 	F� #��z��$	 0.5Q�4������!@T� ���!�� р�[ �� р� `�S р�[ @T� 	 P�  �@n�
IsDragging . . ��� # � ��
 � ��P�  � {� .  . ��� # � ��
 � ���� "	 n�.��� 
 �@�OverridesDefaultStyle . �� 
 � ��$ 0,0,0,0q��� 
 � �� Z� �� 
 � ��# � ���� 
 � ��$ 21���� 
 � ��$ 11���� 
 � �� �� " n�.R���  . .+�F� #��z��=����    �ا �        ����     �@o@@KL �@o@��[ ���� �ا 	F� #��� ��$	��1.0��4���� � ��� � �  � ����  � � �8@KL � �8`�S 	F� #��� ��$	��1.0��+���� �8`�S � �8��[ ���� �7� �    �7� 	F� #��� ��$	��1.0��4����`� @T� �`� `� ���� `� ���)@KL ���)`�S 	F� #��� ��$	��1.0��+������)`�S ���)��[ ���� `�� �@B `�� 	F� #��z��$	 0.5Q�4���Ѐ� @T� р� �� ���� �� ���!`�S ���� @T� 	 P�  �@n�
IsDragging . . ��� # � ��
 � ��P�  � {� .  . ��� # � ��
 � ���� "	 n�.��� 
 �@�OverridesDefaultStyle . �� 
 � �� Z� �� 
 � ��# � ���� 
 � ��$ 21���� 
 � ��$ 11���� 
 � �� �� " n�.R�  # z��# Z $ Raised=�$ 3q�._��� #��z��$��-1q�$	 0.5Q� P�  �@n�
IsDragging . . ��� # � ��
 � ��P�  � {� .  . ��� # � ��
 � ���� "	 ��.��� 
 �@��IsPressAndHoldEnabled .  �� 
 � ��
 ���� �� 
 � ��# � ���� 
 � �� �� " ��.R��� #��z��#��z��#��z��#��z��._��  �� $��* � �� $
��Auto ��� $
��Auto �#��z���� $
��Auto �.+�m� $ TopTick��$��4��$y�	Collapsed=�# z��$	 Top=�  $ TrackBackground��$��0,0,0,1q�$��4��$��1��._��� .+��� $ PART_SelectionRange��
���  ��$	��4.0��]� $  
PART_Track��$��1��! �� 8��  #��Z" # �� 8�� #��Z$ % n� $& Thumb��8�� $��17��$��25��$��Center=�m� $ 
BottomTick��$��4��$y�	Collapsed=�$��2��# z��$ Bottom=� P�  �@��TickPlacement$ TopLeft=�. ��� $ TopTick��
 � y�$ Visible=��� $ Thumb��8  
 � ���� $ TrackBackground��
 � ��$ 0,3,0,0q�P�  �@��TickPlacement$ BottomRight=�. ��� $ 
BottomTick��
 � y�$ Visible=��� $ Thumb��8  
 � ���� $ TrackBackground��
 � ��$ 0,0,0,3q�P�  �@��TickPlacement$
 Both=�. ��� $ TopTick��
 � y�$ Visible=��� $ 
BottomTick��
 � y�$ Visible=�P�  �@��IsSelectionRangeEnabled . . ��� $ PART_SelectionRange��
 � y�$ Visible=�.��� ���� ' �� ImageSource$-' 'Resources/images/background_800x600.jpg��.+��� -   $��bg_image_stamp��$3��-Resources/images/background_800x600_stamp.png��$	��800��$	��600��|�.  ( J� IsHitTestVisible( .  � $��Stretch=�$��Stretch=�) � ShowGridLines) .   ��  �� -   * �Name$* grid_status��$��50 ��� -   $* 	grid_main���� -   $* grid_control��$��85 �.+�� -   + �Name$+ media_status��$��0��$��0,10,0,0q� �� $
��auto ��� �� $
��auto � �� $
��auto ��� $
��auto �.+��� $��0��$��0��$��Stretch=�$	��Top=�, �� Orientation$, 
Horizontal=�.+��� -   $��media_status_statusicon��$,��&Resources/images/silk/control_play.png��$��4,0,2,0q�$��16��$��16��  	iconShine- J� OpacityMask#- �  �� -   . ��Name$. media_status_filetype��
�������$��14(�$��Stretch=�/ �� Padding$/ 2,0,2,0q��� $%��Resources/images/silk/music.png��$��2,0,2,0q�$��16��$��16��#- �  �� -	   $. media_status_songtime��
�������$��14(�$��Stretch=�$/ 2,0,2,0q��� $��1��$��0��$��Center=�$	��Top=�$, 
Horizontal=�.+��� $%��Resources/images/silk/phone.png��$��2,0,2,0q�$��16��$��16��#- �  �� $-��'Resources/images/silk/transmit_blue.png��$��2,0,2,0q�$��16��$��16��#- �  �� $.��(Resources/images/silk/weather_cloudy.png��$��2,0,2,0q�$��16��$��16��#- �  �� $1��+Resources/images/silk/weather_lightning.png��$��2,0,2,0q�$��16��$��16��#- �  �� -
   $��current_page_title��
�������$��14(�$
��Home��$��Stretch=�$/ 2,0,2,0q��� $,��&Resources/images/silk/weather_snow.png��$��2,0,2,0q�$��16��$��16��#- �  �� $+��%Resources/images/silk/weather_sun.png��$��2,0,2,0q�$��16��$��16��#- �  �� $"��Resources/images/silk/cd.png��$��2,0,2,0q�$��16��$��16��#- �  �� $#��Resources/images/silk/dvd.png��$��2,0,2,0q�$��16��$��16��#- �  �� $��2��$��0��$��Right=�$	��Top=�$, 
Horizontal=�.+��� -   $%��Resources/images/silk/clock.png��$��2,0,2,0q�$��16��$��16��#- �  �� -   $. status_time��$��14(�
�������$
��TIME��$/ 2,0,2,0q��� -   $1��+Resources/images/silk/calendar_view_day.png��$��2,0,2,0q�$��16��$��16��#- �  �� -   $. status_date��$��14(�
�������$
��DATE��$/ 2,0,7,0q��� $��0��$��1��$��2��$
��Left=�$��Center=�$, 
Horizontal=�.+��� -   $. media_status_artists��
�������$��17(�$/ 5,0,7,0q��� -   $. media_status_title��
�������$��17(�$
��Bold"�$/ 5,0,7,0q��� -   $. media_status_album��
�������$��17(�$��Italic$�$/ 5,0,7,0q��� -   0 ��Name$0 media_status_stars��$��2��$��1��$��Center=�$��Center=�$, 
Horizontal=�$��90��$��14��$��0,2,0,0q�z��� 1 �� ScaleX$	1 1.5Q�2 �� ScaleY$	2 1.5Q�.+�d� -   #��� 	 d� -   #��� 	 d� -   #��� 	 d� -   #��� 	 d� -   #��� 	 � -   $+ media_control��$��2��$��0,0,0,10q�.+�� ) .  $��Stretch=�$��Stretch=� �� �� ��  �� $��17 ��� �� .+��� $��0��$��3��$��0��$��10�����	#FFFFFFFF$	��0.7q�$��	10,3,10,0q�3 �� CornerRadius$3 2��$��Stretch=�$	��Top=����	#C9000000._��� -   $��media_position_rect��$��3�����	#FF1D6DA9$��0��$
��Left=�$3 2��$��Center=��� -   4 ��Name$4 media_position��$��0��$��3��$��0��$��	10,0,10,0q�$��17��$	��Top=�#���  5 � FocusVisualStyle5 Z� � $��0��$��1��$��2�� �� $
��Auto ���  �� �� .+��� -   $4 media_volume��$��0��$��0��$	��150��$��10,7,3,0q�6 � Value$	6 100Q�7 �� TickFrequency$7 10Q�8 � SmallChange$8 1Q�9 � Maximum$	9 100Q��� $��0��$��1��$, 
Horizontal=�$��Center=�$��10,0,0,0q�.+��� -   $��media_volume_image��$%��Resources/images/silk/sound.png��$��0,0,13,0q�$��16��$��16��#- �  d� -   : d�Name$: 
media_mute��$��75��$��2q�$
��Mute��  GlassToggleButton#���  �� -   ; ��Name$; media_volume_label��
�������$��Center=�$��Center=�$��0,5,0,0q�$��45��$
��100%���� $��1��$��0��$��2��.+��� -   < ��Name$< media_volume_up��$��75��$��25��$��2q�$��+��= �� Delay$	= 100��> �� Interval$	> 225��{�.    GlassRepeatButton#���  �� -    $< media_volume_down��$��75��$��25��$��2q�$��-��$	= 100��$	> 225��#���  �� $, 
Horizontal=�$��1��$��1��$��2��$��Center=�$��Center=�.+��� -!   ? ��Name$? media_previous��$��5q�  GlassButton#���  $�� |<< ��$��60��$��35��d� -"   $: 
media_play��$	��5,2q�  glassToggle#���  �� -#   $? 
media_next��$��5q�#���  $�� >>| ��$��60��$��35���� $��2��$��1��$��2��$, 
Horizontal=�$��Center=�$��Center=�.+�d� -$   $: button_media_repeat��$��70��$��35��$��5q�$��Repeat��#���  d� -%   $: button_media_shuffle��$��70��$��35��$��5q�$��Shuffle��#���  �� -&   $? homeExit��$��70��$��5q�$��35��$
��Exit��$
��bold"�#���  �� -'   @ ��Name$@ tabControl1��$��1��A �� TabStripPlacement$	A Top=�
������ $��Stretch=�$��Stretch=�$��0q�  TabControlNoTabStrip#��[ B �� SelectedIndex$B 0��5 Z� .@��� $
��HOME��  glassTabItem#���  .���  �� �� ��  �� �� �� .+��� -(   $? hp_audio��$��audio��$��0��$��0��#���  $��60��$	��150��$��18(��� -)   $? hp_manage_media��$��manage media��$��1��$��0��#���  $��60��$	��150��$��18(��� -*   $? hp_video��$��video��$��2��$��0��#���  $��60��$	��150��$��18(��� -+   $? hp_obd��$��diagnostics��$��0��$��2��#���  $��60��$	��150��$��18(��� -,   $? 
hp_control��$��control��$��1��$��2��#���  $��60��$	��150��$��18(��� --   $? hp_settings��$��settings��$��2��$��2��#���  $��60��$	��150��$��18(��� $��AUDIO��#���  .��� $��Stretch=�$��Stretch=� �� ��  �� �� .+��� $��0��$��0��$��2��$��0,75,0,0q�.+��� -.   C ��Name$C image1_border��
���   �$��1q�$3 	0,15,15,0��$��Center=�$��Center=�$	��196��$	��200��$y�Hidden=����� D j� Stretch$D UniformToFill=�-/   
image1��E J� BitmapEffectE �� .��F� F F� Color$F Black��G F� 	Direction$G 20Q�H F� ShadowDepth$H 25Q�I F� Softness$I 1Q�J F� Opacity$	J 0.5Q�z��� K �� CenterX$K 0Q�L �� CenterY$L 0Q�M �� AngleX$M 0Q�N �� AngleY$N 5Q��� ���	#aa000000$��Center=�$��Center=����� O �� ElementName$O image1_border��P �� Path$P Height����� $O image1_border��$P Width����� $O image1_border��$P BorderThickness�3 �� $O image1_border��$P CornerRadius�y��� $O image1_border��$P 
Visibility����� $D UniformToFill=�-0   
image2��Q �� 	TransformQ �� $1 1Q�$2 -1Q�R �� CenterX$	R 200Q�S �� CenterY$S 98Q�- �� $	 0,0&�$	 0,1&�.��� $ 0Q�$ Black��� $	 0.6Q�$ Transparent��z��� $K 30Q�$L 2Q�$	M -30Q�$N 10Q��� -1   $@ now_playingTabs��$A Bottom=�$��Stretch=�$��Stretch=�$��1��$��0��$��2��  glassTabControl#���  .@��� -2   $��visualization_tab��$��Visualization��#���  .���� -3   $��media_visualization_grids��( .  $	��400��$	��275��._��� -4   $C media_visualization��( .  ���� $O media_visualization_grids��$P Height����� $O media_visualization_grids��$P Width��� $��Visualization 2��#���  .���� -5   $ C media_visualization_border��
���   �$	��0.7q�$	��356��$	��256��T �� Padding$T 2q�z��� $K 0Q�$L 0Q�$M 0Q�$N 5Q�._�� -6   $+ media_visualization_2��$��0,0,4,0q�$	��350��$	��250��$��Right=�$��Center=��� $��Media Details��#���  .���� -7   $C mediainfo_container��
���   �$��1q�$	��350��$��Center=�$3 7�����	#550A0A0A._�� .+��� -8   $ mediainfo_media_progress�����	#990A0A0A$��0��$
��Left=�� $��3q� �� ��  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� .+��� -9   $. mediainfo_title��$��0��$��0��$��2��$��18(�$
��Wrap=�#���  �� -:   $. mediainfo_filepath��$��0��$��1��$��2��$
��Wrap=�#���  �� $��0��$��2��$��2��$	��325��$��1�����#D2D219$	 0.7Q��� -;   $0 mediainfo_stars��$��0��$��3��$��2��$��Center=�$��Center=�$, 
Horizontal=�$��-55,-7,3,13q�$��20��z��� $1 2Q�$2 2Q�.+�d� #��� 	 ( .  d� #��� 	 ( .  d� #��� 	 ( .  d� #��� 	 ( .  d� #��� 	 ( .  �� $��0��$��4��$, Vertical=�$��0,0,5,0q�.+��� #���  Artist�� -<   $. mediainfo_artist��#���  �� #���  Album�� -=   $. mediainfo_album��#���  �� #���  $
��Wrap=�Title�� ->   $. mediainfo_trackname��#���  �� #���  Track number�� -?   $. mediainfo_tracknumber��#���  �� #���  Genres�� -@   $. mediainfo_genre��#���  �� #���  Year�� -A   $. mediainfo_year��#���  �� $��1��$��4��$, Vertical=�$��5,0,0,0q�.+��� #���  	Bitrate�� -B   $. mediainfo_bitrate��#���  �� #���  
Channels�� -C   $. mediainfo_channels��#���  �� #���  
Filesize�� -D   $. mediainfo_filesize��#���  �� #���  Playlist number�� -E   $. mediainfo_playlistnumber��#���  �� #���  	Playcount�� -F   $. mediainfo_playcount��#���  �� #���  Last played�� -G   $. mediainfo_lastplayed��#���  �� $��Now Playing��#���  .��+   %KineticScrollingPrototype.CustJournal  -H   U  Name$U custJournal��$
��Auto��$��Stretch=�$
��Auto���� $��
Management��#���  .���� ._��� -I   #���  $A Bottom=�.@��� $��Media Folders��#���  .��� $	��3,0q� �� $
��Auto ���  �� $	��250 ��� .+��� $��0��$��0��$	��180��$	��200��$3 0,9,9,9��#��[ $��1q�$��Center=�$��5q����� -J   mediafolder_preview��'   !wpfBuild.controls.mediaFolderList  -K   V  Name$V mediafolderlist��$��1��$��0��$	��250��$��Stretch=�$��Stretch=��� $��0��$��1��$��5,0,5,0q�.+��� $��19(�#��[ $��Options���  �� ��  �� .+��� -L   $? mediafolder_add��$��0��#���  $��0,2,4,6q�{�.  .���� 
��� d �$��30(�$
��Bold"�$��+���� -M   $? mediafolder_remove��$��1��#���  $��4,2,0,6q�{�.  .���� 
���  ��$��30(�$
��Bold"�$��-���� -N   $!? mediafolder_rescan_selected��#���  $��30��$	��0,2q�$��Rescan selected folder��{�.  �� -O   $? mediafolder_rescan_all��#���  $��30��$	��0,4q�$��Rescan all folders���� #���  $��30��$��0,8,0,0q�$��Explore folder��{�.  �� $��1��$��1��.+��� $��19(�#��[ Media Folder Path�  �� $
��Auto ���  �� .+��� -P   $? mediafolder_browse��$��0��$��	Browse...��$	��3,0q�$��75��$��35��#���  �� -Q   W ��Name$W mediafolder_folderpath��$��1��$	��3,0q�$��Center=��� -R   X ��Name$#X mediafolder_search_subfolders��
���   �$	��0,4q�.���� $��18(�#��[ $��5,4,0,0q�$'��!Include subfolders in the search.��	� -S   Y 	�Name$Y mediafolder_type_audio��$��13(�
���   �$	��0,1q�.���� $��18(�#��[ $��Audio��$��5,4,0,0q�	� -T   $Y mediafolder_type_video��$��13(�
���   �$	��0,1q�.���� $��18(�#��[ $��Video��$��5,4,0,0q�	� -U   $Y mediafolder_type_image��$��13(�
���   �$	��0,1q�$y�	Collapsed=�.���� $��16(�#��[ $��Image��$��5,0,0,0q�� -V   Z �Name$Z mediafolder_progress��$��15��$��5q��� $��Explore��#���  .��� $	��3,0q� �� ��  �� .+��� $��0��$��19(�#��[ $��Media Folder���� $��1��$��19(�#��[ $��Media Details���� $��
Management��#���  .��� $��3,0,3,0q� ��  �� $	��120 ��� .+�� $��0�� �� ��  �� .+��� $��0��$��0��.+��� -W   $��management_label��$��16(�$	��155��
�������$��Options��R� -X   $��management_treeview��$	��150��$��0q�$��0q�$��13(�.@�Q� $��Now Playing��Q� $��Media��.@�Q� $��Audio��Q� $��Videos��Q� $��Images��Q� $��	Playlists��.@�Q� -Y   $��New Playlist��Q� $��Removable Storage���� $��1��.+��� $, 
Horizontal=�$��Center=�.+��� $��90��$��35��$��5q�#���  .��
Play Now�� -Z   $? management_enqueue��$��90��$��35��$��5q�#���  .��	Enqueue�� -[   $? management_enqueue_all��$��90��$��35��$��5q�$��Enqueue All��#���  �� $, 
Horizontal=�$��Center=�.+��� -\   $? management_delete��$��90��$��35��$��5q�#���  .��Delete Media�� $��90��$��35��$��5q�#���  .��More Details�� -]   $��management_listview��$��1��[ $� IsVirtualizing[ . $��0q�$��14(�\ �� View\ �� .P��� $��Title��] �� DisplayMemberBinding] �� $	P [0]��� $��Rating��] �� $	P [1]��� $��Artist��] �� $	P [2]��� $��Album��] �� $	P [3]��� $��Length��] �� $	P [4]��� $��ManagementV2��#���  .��� $	��5,0q� �� �� $
��Auto ���  �� $
��Auto ��� $
��Auto ��� .+��� -^   ^ ��Name$^ management_medialist��$��0��$��0��.@��� .��Audio�� .��Video�� .��Images�� -_   $^ management_playlists��$��2��$��0��.@��� .��Now Playing�� .��	Playlists9� -`   _ 9�Name$_ expander_media_options��$��Media Options��$��0��$��1��.���� .+��� .��Rename�� .��Delete�� .��
Play Now�� .��	Enqueue9� -a   $_ expander_playlist_options��$��Playlist Options��$��2��$��1��.���� .+��� .��Rename�� .��Delete�� .��Enque Playlist�� .��Make playlist active   wpfBuild.playlistControl  -b   `  Name$!` management_media_customview��$��0��$��2��$
��Auto��$
��Auto��$��Stretch=�$��Stretch=��� $��1��$��2��$��Center=�$��0q�.+��� #���  $��5q�$��30��$��75��.��ADD�� #���  $��5q�$��30��$��75��.��REMOVE�� $��60��$��3��
������
�������$	��0.5���� #���  $��5q�$��30��$��75��.��	ADD all�� #���  $��5q�$��30��$��75��.��
REMOVE all  -c   $$` management_playlist_customview��$��2��$��2��$
��Auto��$
��Auto��$��Stretch=�$��Stretch=��� $��Browse Videos��#���  .���  �� $
��auto ���  �� $
��auto ��� .+��� $��0��$��0��$��19(�#��[ $#��Page X of Y (XX videos total)���� $��1��$, 
Horizontal=�.+��� $��5q�$��50��$��25��#���  $��<<���� -d   $? videos_thumbs_refresh��$��5q�$��50��$��25��#���  $��>>��� $��1��$��2�� �� �� ��  �� �� �� .+��� $��0��$��1��$��Center=�.+��� 
���   �$	��220��$	��100���� $��Center=�TEST�� -e   $��tabitem_video��$��VIDEO��#���  .��� -f    �� ��  �� .+��� #��[ $	��375��$	��250��
���   �._�7  1System.Windows.Forms.Integration.WindowsFormsHost  $��0��$��0��$	��350��$	��200��$��0q�a   Padding$a 0q�b   Child.b    System.Windows.Forms.Panel  -g   vlcPanel��c   BorderStyle$
c None=�"  System.Windows.Forms.Controld   Width$	d 350��e   Height$	e 200��  -h   $U medialist_video��$��1��$
��Auto��$��Stretch=�$
��Auto���� $	��OBD��#���  .���  �� ��  �� $
��auto ��� .+��� $��0��$��0��$, 
Horizontal=�.+��� $��19(�#��[ $	��5,0q�$��	COM Port:���� -i   $^ obd_com_list���� -j   $? obd_com_rescan��$��Refresh��$��5q�$��25��$��75��#���  �� $��1��$��0��$, 
Horizontal=�.+��� $��19(�#��[ $	��5,0q�$��Status:���� $��19(�
��� ���$	��5,0q�$��Connecting...���� -k   $? obd_com_connect��$��Connect��$��5q�$��25��$��80��{�.  #���  �� $��0��$��1��.+��� -l   $W obd_message��$��20��$
��010C��$��4q�#��[ ���	#7F000000#��[ $��1q�d� -m   $: 
obd_toggle��$	��320��$��50��$��3q�$*��$Use the text above for communication��f d� 	IsChecked$f FalseY�#���  �� -n   $? obd_log_clear��$	��120��$��50��$��3q�$��	Clear Log��#���  �� -o   $. obd_rpm��$��19(�#��[ $��50��$��10,150,10,10q�$��Center=�$	��RPM���� -p   $W obd_log��$��1��$��1��#��[ ���	#7F000000#��[ $��1q�$��4q�g �� TextWrapping$
g Wrap=�$h �� HorizontalScrollBarVisibility$
h Auto=��� $��CONTROL��#���  .���  �� ��  �� $
��auto ��� .+��� $��0��$��0��$, 
Horizontal=�.+��� $��19(�#��[ $	��5,0q�$��	COM Port:���� -q   $^ control_com_list���� -r   $? control_com_rescan��$��Refresh��$��5q�$��25��$��75��#���  �� $��1��$��0��$, 
Horizontal=�.+��� $��19(�#��[ $	��5,0q�$��Status:���� -s   $. control_status_message��$��19(�
��� ���$	��5,0q�$��Disconnected���� -t   $? control_com_connect��$��Connect��$��5q�$��25��$��80��{�.  #���  �� $��0��$��1��$��Center=�.+�d� -u   $: control_toggle��$	��120��$��50��$��Output Toggle��$f FalseY�#���  �� -v   $W control_log��$��1��$��1��#��[ ���	#7F000000#��[ $��1q�$��4q�$
g Wrap=�$
h Auto=��� $	��GPS��#���  �� $��SETTINGS��#���  .���  ��  �� $
��auto ��� .+��� .+��� $��14(�
�������$
��bold"�$��2,0,7,0q�.��	General�� $��11(�
�������$��2,0,7,0q�.��ECThis page allows you to manage various settings related to the IVCS�� #���  $A Bottom=�$��0��$��1��.@��� $��General��#���  .���� .+��� 2  ,System.Windows.Markup.XmlAttributePropertiesi  XmlSpacei preserve$
��Wrap=���
									date & time format
									song display format
									"modules"/plugins that are loaded (a whole other tab for management would be cool)

									�� -w   $? show_taskbar��$��Show Windows taskbar��$	��160��$��35��#���  �� -x   $? button_compact_db��$��Compact Database��$	��160��$��35��#���  �� $��Media��#���  �� $��Audio��#���  �� $��Date & Time��#���  .���  �� ��  �� �� $
��auto �   dateTimej S� 
ObjectType��%   &	       S� "j ��.+��� $��0��$��0��$	��120��$	��108��  clockTemplate#��[ k � DataContextk �� $	P Now�l �� Source#l [ �� $��0��$��1��.��TIME� $��1��$��1�� �� ��  �� .+��� $��0��.��DATE�� $��1��$��Right=�.��
Settings�� $��Button Test��#���  .���  �� ��  �� �� .+��� $��1��$��0��$��2��$��Center=�.+��� $, 
Horizontal=�$��Center=�$��Center=�.+��� #���  $��AUDIO��$��90��$��45��$��10q��� #���  $��AUDIO��$��90��$��45��$��10q��� #���  $��AUDIO��$��90��$��45��$��10q��� $, 
Horizontal=�$��Center=�$��Center=�.+��� #���  $��AUDIO��$��90��$��45��$��10q��� #���  $��AUDIO��$��90��$��45��$��10q��� #���  $��AUDIO��$��90��$��45��$��10q��� $, 
Horizontal=�$��Center=�$��Center=�.+��� #���  $��AUDIO��$��90��$��45��$��10q��� #���  $��AUDIO��$��90��$��45��$��10q��� #���  $��AUDIO��$��90��$��45��$��10q��� $��0��$��1��.+��� -y   $C image1_border123��$��0q�$��Center=�$��Center=�$��55��$	��200��._��� -z   $. testing_123_block��$��55(�asdf�� $��Center=�$��Center=����� $O image1_border123��$P Height����� $O image1_border123��$P Width�._��� ���� $O testing_123_block��$
P Text����� $O testing_123_block��$P FontSize�z��� $1 1Q�$2 -1Q�$R 0Q�$S 25Q�- �� $	 0,0&�$	 0,1&�.��� $ 0Q�$ Black��� $	 0.6Q�$ Transparent��z��� $K 30Q�$L 1Q�$	M -30Q�$N 3Q�