   M S B A M L   `   `   `  ����G  CBEB_csharp05, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:BEB_csharp05BEB_csharp05  5   	       BEB_csharp05.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:BEB_csharp05     5   	     � Title$%  Bioenergie Bassen GmbH & Co. KG��5
      $	��600��61   $	��800��6>    � 	Resources 5         myListboxStyle �� 	Resources �� Color��%�   &	        �� 5   
    5      ���� 5      $	 Red��6T   (��       6"   * SystemColors.HighlightBrushKey+)6T   �� 5      $ Green��6R   (��       6"   *SystemColors.ControlBrushKey+)6R   5      5      5      .��5      � .+�5   
   �� -   5         ��Name$ 
tabControl��5      .@�5      �� $	��INI��6   .��5      � ���	#FFE5E5E5 � RowDefinitions 5      �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5       $
��auto �6(   �� 5!      $
��auto �6(   �� 5"      $
��auto �6(   �� 5#      $
��auto �6(   �� 5$      $
��auto �6(   �� 5%      $
��auto �6(   �� 5&      $
��auto �6(   �� 5'      $
��auto �6(   �� 5(      $
��auto �6(   �� 5)      $
��auto �6(   �� 5*      $
��auto �6(   �� 5+      $
��auto �6(   �� 5,      $
��auto �6(   �� 5-      $
��auto �6(    � ColumnDefinitions 5/      �� 50      $
��auto �6+   �� 51      $
��auto �6+   �� 52      $��* �6+   �� 53      $
��auto �6+   .+�56      �� $��0��6    $��0��6)   $��0��66   $��5q�6F   �� 57      $��1��6    $��1��6)   $��0��66   $��5q�6F   �� 58      $��2��6    $��2��6)   $��0��66   $��5q�6F   �� 59      $��3��6    $��3��6)   $��0��66   $��5q�6F   �� 5:      $��4��6    $��4��6)   $��0��66   $��5q�6F   �� 5;      $��5��6    $��5��6)   $��0��66   $��5q�6F   �� 5<      $��6��6    $��6��6)   $��0��66   $��5q�6F   �� 5=      $��7��6    $��7��6)   $��0��66   $��5q�6F   �� 5>      $��8��6    $��8��6)   $��0��66   $��5q�6F   �� 5?      $��9��6    $��9��6)   $��0��66   $��5q�6F   �� 5@      $��10��6    $��10��6*   $��0��68   $��5q�6H   �� 5A      $��11��6    $��11��6*   $��0��68   $��5q�6H   �� 5B      $��12��6    $��12��6*   $��0��68   $��5q�6H   �� 5C      $��13��6    $��13��6*   $��0��68   $��5q�6H   �� 5D      $��14��6    $��14��6*   $��0��68   $��5q�6H   �� 5E      $��15��6    $��15��6*   $��0��68   $��5q�6H   �� 5F      $��16��6    $��16��6*   $��0��68   $��5q�6H   �� 5G      $��17��6    $��17��6*   $��0��68   $��5q�6H   �� 5I      -   5         ��Name$ Button_ReadIni��5I      $��0��65   $��2��6B   $��INI einlesen��6R   6i   �� 5J      -   5        $ Button_CheckFtpDir��5J      $��0��69   $��3��6F   $��Check��6V   6f   �� 5L      $��Programmordner:��6    $��1��67   $��1��6D   �� 5M      -   5         ��Name$ TextBlock_AppDir��5M   =   $��1��6    $��2��6-   ��6W   �� 	 �� Path$	 AppDir�
 �� Mode$
 oneWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5O      $��	INI-Pfad:��6    $��2��61   $��1��6>   �� 5P      -   5        $ TextBlock_IniPath��5P   =   $��2��6    $��2��6-   ��6X   �� $	 IniPath�$
 oneWay=�$ PropertyChanged=�A� 5Q      -   5         A�Name$ status_IniPath��5Q      $��2��66   $��3��6C   $��10��6T   $��10��6_   
��� ���6k   �� 5S      $��DB-Pfad:��6    $��3��60   $��1��6=   �� 5T      -   5        $ TextBlock_DbPath��5T   =   $��3��6    $��2��6-   ��6W   �� $	 DbPath�$
 OneWay=�$ PropertyChanged=�A� 5U      -   5        $ status_DbPath��5U      $��3��65   $��3��6B   $��10��6S   $��10��6^   
��� ���6j   �� 5W      $��Lokaler Input Ordner��6    $��5��6<   $��1��6I   �� 5X      -	   5        $ TextBlock_LocDirInput��5X   =   $��5��6    $��2��6-   ��6\   �� $	 LocDirInput�$
 OneWay=�$ PropertyChanged=�A� 5Y      -
   5        $ status_LocDirInput��5Y      $��5��6:   $��3��6G   $��10��6X   $��10��6c   
��� ���6o   �� 5[      $��Lokaler Operation Ordner��6    $��6��6@   $��1��6M   �� 5\      -   5        $ TextBlock_LocDirOperation��5\   =   $��6��6    $��2��6-   ��6`   �� $	 LocDirOperation�$
 OneWay=�$ PropertyChanged=�A� 5]      -   5        $ status_LocDirOperation��5]      $��6��6>   $��3��6K   $��10��6\   $��10��6g   
��� ���6s   �� 5_      $��Lokaler Scales Ordner��6    $��7��6=   $��1��6J   �� 5`      -   5        $ TextBlock_LocDirScales��5`   =   $��7��6    $��2��6-   ��6]   �� $	 LocDirScales�$
 OneWay=�$ PropertyChanged=�A� 5a      -   5        $ status_LocDirScales��5a      $��7��6;   $��3��6H   $��10��6Y   $��10��6d   
��� ���6p   �� 5c      $��Lokaler Excel Ordner��6    $��8��6<   $��1��6I   �� 5d      -   5        $ TextBlock_LocDirExcel��5d   =   $��8��6    $��2��6-   ��6\   �� $	 LocDirExcel�$
 OneWay=�$ PropertyChanged=�A� 5e      -   5        $ status_LocDirExcel��5e      $��8��6:   $��3��6G   $��10��6X   $��10��6c   
��� ���6o   �� 5g      $��FTP Input Ordner��6    $��10��68   $��1��6F   �� 5h      -   5        $ TextBlock_FtpDirInput��5h   >   $��10��6    $��2��6.   ��6]   �� $	 FtpDirInput�$
 OneWay=�$ PropertyChanged=�A� 5i      -   5        $ status_FtpDirInput��5i      $��10��6:   $��3��6H   $��10��6Y   $��10��6d   
��� ���6p   �� 5k      $��FTP Operation Ordner��6    $��11��6<   $��1��6J   �� 5l      -   5        $ TextBlock_FtpDirOperation��5l   >   $��11��6    $��2��6.   ��6a   �� $	 FtpDirOperation�$
 OneWay=�$ PropertyChanged=�A� 5m      -   5        $ status_FtpDirOperation��5m      $��11��6>   $��3��6L   $��10��6]   $��10��6h   
��� ���6t   �� 5o      $��FTP Scales Ordner��6    $��12��69   $��1��6G   �� 5p      -   5        $ TextBlock_FtpDirScales��5p   >   $��12��6    $��2��6.   ��6^   �� $	 FtpDirScales�$
 OneWay=�$ PropertyChanged=�A� 5q      -   5        $ status_FtpDirScales��5q      $��12��6;   $��3��6I   $��10��6Z   $��10��6e   
��� ���6q   5�      5�      �� 5�      $	��FTP��6   .��5�      � ���	#FFE5E5E5 5�      �� 5�      $
��auto �6(   �� 5�      $
��auto �6(   �� 5�      $
��auto �6(   �� 5�      $
��auto �6(    5�      �� 5�      $��* �6+   �� 5�      $��* �6+   .+�5�      � -   5         �Name$ Progressbar1��5�   "   $��0��68   $��4��6E   $��20��6Y    � Maximum$	 100Q�6e    � Value 6s   �� $	 TaskProgress1�$
 OneWay=�$ PropertyChanged=�� 5�      -   5        $ Progressbar2��5�   "   $��1��68   $��4��6E   $��20��6Y   $	 100Q�6e    6s   �� $	 TaskProgress2�$
 OneWay=�$ PropertyChanged=��� 5�      $��2��6!   .+�5�      �� -   5        $��	Abbrechen��5�   !   $��3��6P   {�.  6d   �� 5�       �� Orientation$ 
Horizontal=�6%   $��Right=�6>   .+�5�      �� $��Aktualisierung alle ��6(   $��Center=�6D   �� 5�      ��6(   �� $#	 HourTimePeriodForNewFtpUpdate�$
 TwoWay=�$ PropertyChanged=��� 5�      $��	 Stunden.��6(   $��Center=�69   5�      �� 5�      $ 
Horizontal=�6%   $��Right=�6>   .+�5�      �� $��Nächste Aktualisierung: ��6(   $��Center=�6H   �� 5�      $��Center=�6�   ��6(   �� $	 StartTimeFtpAutoRead�$
 OneWay=� �� StringFormat$ {0:dd.MM.yyyy HH:mm:ss}��$ PropertyChanged=�6�   5�      �� 5�      $ 
Horizontal=�6%   $��Right=�6>   .+�5�      �� -   5         ��Name$ ComboBox_ListToShow��5�   '   $	��200��6E   .@�5�   "   ��  �� 
IsSelected . .��6A   Input lokal6N   �� 5�   "   .��6/   	Input FTP6:   �� 5�   "   .��6/   Operation lokal6@   �� 5�   "   .��6/   Operation FTP6>   5�      �� 5�      -   5        $��	auflisten��5�   %   69   5�      �� 5�      $ 
Horizontal=�6&   $��Right=�6?   .+�5�      �� .��6$   " Max. Anzahl zu ladender Dateien:6F   �� 5�      $��50��6|   ��6&   �� $	 MaxFilesCount�$
 TwoWay=�$ PropertyChanged=�6|   5�      �� 5�      -   5        $��	FTP Input��5�   !   $��3��6W   �� 5�      -   5        $��	DIR Input��5�   !   $��3��6W   �� 5�      -   5        $��FTP Operation��5�   !   69   �� 5�      -   5        $��DIR Operation��5�   !   69   �� 5�      $"��FTP Scales (nicht vorhanden)��6$    �� TextAlignment$ Center=�6H   �� 5�      -    5        $��
DIR Scales��5�   !   66   5�      �� 5�      -!   5         ��Name$ 	ListView1��5�      #��[  5�      $��2��5�      $��3��5�      $��6��5�      ��5�      �� $	 ListView1Content�$
 OneWay=�$ PropertyChanged=�5�      5�      �� 5�      -"   5        $��Tabelle��5�      .��5�      � ���	#FFE5E5E5 5�      �� 5�       5�      �� 5�      $
��auto �6+   �� 5�      $��* �6+   .+�5�      &   System.Windows.Controls.DataGrid  -#   5          Name$ 	datagrid1��5�      $��1��5�         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$	 100 5�      $ Right=�5�         
IsReadOnly .    CanUserSortColumns .    CanUserResizeColumns .    CanUserReorderColumns . ��5�      �� $	 MyDataTable�$
 OneWay=�$ PropertyChanged=�5�      �� 5�      $	��Top=�6"   .+�5�      �� $��Resources/beb_logo.png��6    $��Uniform=�6@   $	��150��6R   $��0,0,0,10q�6^    � ToolTip$&  Bioenergie Bassen GmbH & Co. KG ��6p   �� 5�      $��Tabelle:��6$   $��Bottom=�64   �� 5�      -$   5        $ comboBox_Table��5�   #   $��0,0,0,10q�6n   .@�5�      ��  . .��6>   Futter6F   �� 5�      .��6+   	Betrieb64   �� 5�      .��6+   Waage62   5�      �� 5�      $��Gruppierung:��6$   $��Bottom=�68   �� 5�      -%   5        $ comboBox_Group��5�   #   $��0,0,0,10q�6n   .@�5�      �� .��6+   Alles62   �� 5�      .��6+   	Stunden64   �� 5�       . .��6=   Tage6C   �� 5�      .��6+   Monate63   �� 5�      .��6+   Jahre62   5�      �� 5�      y�6%   �� $	 ControlVisibility�$
 OneWay=�$ PropertyChanged=�.+�5�      �� $��Sorte:��6$   $��Bottom=�62   �� 5�      -&   5        $ comboBox_Sorte��5�   '   $��0,0,0,10q�6r   {�.  .@�5�   "   �� .��6/   Alles66   �� 5�   "   .��6/   
Gärrest68   �� 5�   "    . .��6A   Gülle6H   �� 5�   "   .��6/   Rindergülle6<   �� 5�   "   .��6/   Schweinegülle6>   �� 5�   "   .��6/   Mais65   5      �� 5     $��Kunde:��6(   $��Bottom=�66   �� 5     -'   5        $ comboBox_Kunde��5  '   $��0,0,0,10q�6r   {�.  .@�5  "   ��  . .��6A   Alle6G   5     5     �� 5	     $
��von:��6$   $��Bottom=�60   (  "System.Windows.Controls.DatePicker  5
     -(   5          Name$ datePickerVon��5
  %      IsTodayHighlighted . $��0,0,0,10q�5        SelectedDate 5     �� $	 FirstDayofCurrentMonth�$
 OneWay=�$ PropertyChanged=�5     �� 5     -)   5        $ comboBox_EndDatePattern��5  #   .@�5     �� .��6,   bis:62   �� 5     .��6,   1 Tag63   �� 5      . .��6=   	1 Monat6F   �� 5     .��6,   1 Jahr64   5       5     -*   5        $ datePickerBis��5  %    . {�.  $��0,0,0,10q�5     9� 5     -+   5          9�Name$  SqlExpander1��5  $   $	��SQL��6;   $��Bottom=�6H   $��9(�6c   .��5     �� -,   5        ! ��Name$! textBox_SQL��5  &   " �� TextWrapping$
" Wrap=�6;   $��10(�6�   $	��150��6�   ��6O   �� $	 
SqlCommand�$
 TwoWay=�$ PropertyChanged=�6�   5     �� 5     --   5        $ button_TableToExcel��5  !   $��in Excel speichern��6>   6\   5      5!     5"     �� 5#     -.   5        $	��LOG��5#     .��5$     �� ��6   �� $	 CurrentLogFileContent�$
 OneWay=�$ PropertyChanged=�5%     5&     5'     5(     