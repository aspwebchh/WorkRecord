   M S B A M L   `   `   `  ���� E  ADH_ZhouBao, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:DH_ZhouBao
DH_ZhouBao      DH_ZhouBao.AllJob   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    >d2http://schemas.microsoft.com/expression/blend/2008    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    (localclr-namespace:DH_ZhouBao      � Title$  大家的工作��$	��500��$	��500��.��� $��10q� � RowDefinitions �� $��50 ���  � ColumnDefinitions �� $	��100 ��� .+��� -    ��Name$ listView��$��0��$��1��$��2������  �� View �� P��� .���� $��选择��$��5q�$
��Left=�$��50�� �� CellTemplate �� .R��� -   $��5q�����  �� Path$ id���  �� Width$	 100�� �� DisplayMemberBinding �� $
 name�.���� $��姓名��$��5q�$
��Left=��� $	 150�� �� $ add_time�	 �� StringFormat$	 
yyyy-MM-dd��.���� $��提交时间��$��5q�$
��Left=��� $	 150�� �� $ wk�.���� $��	第几周��$��5q�$
��Left=��� -   $��0��$��0��$
��Left=�$��Center=�   ButtonNormal#��[  .���� $��导出��� $��0��$��1��$��Right=�$��Center=�.+��� -   $
��Name��$	��100��$��0��$��1q�$��0,4,0,0q�.��
 � 	Resources
   	HelpBrush j� TileMode �� Opacity j� Stretch j� 
AlignmentX !� Visual��%\   &	       !� $
 None=�$	 0.5Q�$
 None=�$
 Left=� �� $��根据姓名查找��.������  �� 
TargetType" �� �� Triggers P�  P� Property � �� P� Value Z� . ���  �� Property � �� �� Value# [ P�  � ��$  ��. ���  � ��# [ .���� -   #��[  $��Center=�$��5,0,0,0q�.���� $��筛选��