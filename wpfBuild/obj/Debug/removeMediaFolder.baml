   M S B A M L   `   `   `  ����  wpfBuild     wpfBuild.removeMediaFolder   5   	   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      -   5          � Title$  removeMediaFolder��5      $	��202��6   $	��402��6,    � WindowStartupLocation$ CenterScreen=�68    � 
ResizeMode$ NoResize=�6]   $��./Fonts\#Century Gothic)�6s    � AllowsTransparency . 
������  � WindowStyle$
 None=�6�   .��5      ��  �� CornerRadius$ 10��6   ���	#CF000000._�5   
   �  � ColumnDefinitions 5      �� 5	       � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      �� 5      $
��auto �6    .+�5      �� -   5        $��label1��5   !   $��0��6   $	��Top=�6/   $��Stretch=�6G   $��30(�6e   $
��Bold"�6s   
������.��6�   
Warning!6�   �� 5       �� Orientation$ 
Horizontal=�6   $��1��62   .+�5      �� $��Stretch=�6   ���	#FFEFEFEF.��6M   This will remove6`   �� 5      -   5        $��label_item_count��5      ���	#FFEFEFEF$
��bold"�6G   .��6Y   XX6]   �� 5      ���	#FFEFEFEF.��6/   0.media items belonging to the selected folders!6`   5      �� 5      $��2��6   $ 
Horizontal=�6&   $��Center=�6?   .+�5      �� -   5        $��0,0,5,0q�5      $��75��6*   $��30��65      GlassButton#���   6A   .��6   Remove6�   �� 5      -   5        $��5,0,0,0q�5      $��75��6*   $��30��65   #���   6A   .��6�   Cancel6�   �� 5       -   5        $��folder_id_array��5    /   $y�	Collapsed=�6   6H   5!      � 5#      -   5        $��remove_progress��5#   '   $��3��6   $��30��6>   $��10q�6J     glassBorderBG#���  6V   $y�Hidden=�6�   6�   5%      5&      5'      