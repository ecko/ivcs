   M S B A M L   `   `   `  ���� C  ?wpfBuild, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:wpfBuildwpfBuild      wpfBuild.playlistControl   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    &localclr-namespace:wpfBuild    -     >�Name$  UserControl��$��0q�$��./Fonts\#Century Gothic)� � 	Resources    load"   wpfBuild.BackgroundConverter  myConverter  alternatingItemStyle �� 
TargetType �� Property �� Value �� 	Converter �� RelativeSource  listItemTemplate ��Name  	TemplateA �� 
TargetType	 �� ElementName
 �� Path � ColumnDefinitions � RowDefinitions �� Padding  	TemplateB �� CornerRadius j� Stretch �� ImageSource �� Orientation��%�  &	        &	      &	 
     7  &	 D     &	 p     &	 �    ��   �� " ��.���  � �� �� 8    �� *Self+�� .R��� $ 
theControl��|�.  #���  �� " ��.R�� $��25��$	��0,0q����� $	 UserControl��$
 ActualWidth� ��  �� $
��Auto ��� .+��� $��13(����#F2F2F2$��0��$��0������ $
 	SongTitle��� $��0��$��1��$��0,-2,0,0q�.+��� $��10(����#BFBFBF���� $
 
SongArtist��� $��10(����#8C8C8C$��italic$�$ 10,0,0,0q����� $
 	SongAlbum��� " ��.R�� $��50��$��	-7,0,-7,0q����� $	 UserControl��$
 ActualWidth� �� $
��Auto ��� �� $
��Auto � �� $
��Auto ��� .+��� $��0��$��0��$��2��
���   �$��1q�$ 0,7,7,0��$��Center=�$��Center=�$��45��$��45��$��3,2,3,2q����� $ UniformToFill=� �� $
 SongCoverImage��� $��16(����#F2F2F2$��1��$��0��$��	0,10,0,-3q����� $
 	SongTitle��� $��1��$��1��$ 
Horizontal=�$��Stretch=�.+��� $��14(����#BFBFBF���� $
 
SongArtist��� $��14(����#8C8C8C$��italic$�$ 10,0,0,0q����� $
 	SongAlbum��� $��13(����#BFBFBF$��2��$��1��$��Right=�$ 0,0,7,0q����� $
 
SongLength� � Triggers :�  :� RoutedEvent$ FrameworkElement.Loaded��.[��� #��[  .���� -    ��Name$ myScrollViewer��$��0q�$��Hidden=�$��Hidden=�.���� -    ��Name$ 	listItems��$��0q�#��[   ListViewStyle1#���  
������  $� VirtualizationMode$ 	Recycling=� $� IsVirtualizing .   �� Color �� Opacity��%�   (��       *SystemColors.ControlBrushKey+)(��      * SystemColors.HighlightBrushKey+)�� $ #0E273B��$ 1Q��� $ #0E273B��$ 1Q� �� View �� -   gvItems��! �� ColumnHeaderContainerStyle �� " ��.���  � y�$ 	Collapsed=���  � ��$ 0��.P���  �� CellTemplate# [ 