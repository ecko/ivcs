   M S B A M L   `   `   `  ����C  ?wpfBuild, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:wpfBuildwpfBuild  5          wpfBuild.playlistControl   5      W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      &localclr-namespace:wpfBuild    5      -   5          >�Name$  UserControl��5   1   $��0q�5      $��./Fonts\#Century Gothic)�6    � 	Resources 5         load"   wpfBuild.BackgroundConverter  myConverter  alternatingItemStyle �� 
TargetType �� Property �� Value �� 	Converter �� RelativeSource  listItemTemplate ��Name  	TemplateA �� 
TargetType	 �� ElementName
 �� Path � ColumnDefinitions � RowDefinitions �� Padding  	TemplateB �� CornerRadius j� Stretch �� ImageSource �� Orientation��%�  &	        &	      &	 &     7  &	 �     &	 �     &	 @    �� 5      6     5	      6   �� 5      " ��6'   .�5      ��  � �� 5      �� 5      8    6   �� *Self+66   5      5      �� 5      .R�5      �� $ 
theControl��6   |�.  #���  63   5      �� 5      " ��6&   .R�5      � $��25��6
   $	��0,0q�6Q   ��6   �� $	 UserControl��$
 ActualWidth� 5       �� 5$       5&      �� 5'      $
��Auto �6   �� 5(      .+�51      �� $��13(�6   ���#F2F2F2$��0��63   $��0��6C   ��6P   �� $
 	SongTitle��� 53      $��0��6   $��1��6!   $��0,-2,0,0q�6.   .+�54      �� $��10(�6   ���#BFBFBF��64   �� $
 
SongArtist��� 55      $��10(�6   ���#8C8C8C$��italic$�64   $ 10,0,0,0q�6g   ��6G   �� $
 	SongAlbum�6g   56      57      58      �� 5:      " ��6&   .R�5;      � $��50��6
   $��	-7,0,-7,0q�6Q   ��6   �� $	 UserControl��$
 ActualWidth� 5<      �� 5=      $
��Auto �6   �� 5>      �� 5?      $
��Auto �6    5A      �� 5B      $
��Auto �6   �� 5C      .+�5F      �� $��0��6   $��0��6   $��2��6*   
���   �$��1q�6O   $ 0,7,7,0��6c   $��Center=�6z   $��Center=�6�   $��45��6�   $��45��6�   $��3,2,3,2q�6�   ��5G      �� 5H      $ UniformToFill=�6    6+   �� $
 SongCoverImage�6Y   5J      �� 5M      $��16(�6   ���#F2F2F2$��1��63   $��0��6C   $��	0,10,0,-3q�6p   ��6P   �� $
 	SongTitle�6p   �� 5Y      $��1��6   $��1��6!   $ 
Horizontal=�6.   $��Stretch=�6G   .+�5Z      �� $��14(�6   ���#BFBFBF��64   �� $
 
SongArtist��� 5[      $��14(�6   ���#8C8C8C$��italic$�64   $ 10,0,0,0q�6g   ��6G   �� $
 	SongAlbum�6g   5\      �� 5^      $��13(�6   ���#BFBFBF$��2��6T   $��1��6d   $��Right=�6q   $ 0,0,7,0q�6�   ��63   �� $
 
SongLength�6�   5_      5`      5a       � Triggers 5c      :� 5d       :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5e      �� #��[  6   5f      .��5i      �� -   5         ��Name$ myScrollViewer��5i      $��0q�6(   $��Hidden=�6<   $��Hidden=�6c   .��5j      �� -   5         ��Name$ 	listItems��5j      $��0q�6    #��[ 64     ListViewStyle1#���  6o   
������  $� VirtualizationMode$ 	Recycling=�6�    $� IsVirtualizing .  5k       �� Color �� Opacity��%�   (��       5l      *SystemColors.ControlBrushKey+)(�� .     5m      * SystemColors.HighlightBrushKey+)�� 5l      $ #0E273B��6F   $ 1Q�6V   �� 5m      $ #0E273B��6H   $ 1Q�6X   5n       �� View 5o      �� 5p      -   5        gvItems��! �� ColumnHeaderContainerStyle 5q      �� 5r      " ��6   .�5s   	   ��  � y�$ 	Collapsed=�6&   �� 5t   	    � ��$ 0��6"   5u   	   .P�5w      ��  �� CellTemplate# [ 6   5x      5z      5{      5|      