   M S B A M L   `   `   `  ���� @  <Popis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PopisPopis      Popis.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     %localclr-namespace:Popis     _ [MaterialDesignThemes.Wpf, Version=4.6.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � Title$  POPIS��$	��720��$	��920�� � 
ResizeMode$ CanMinimize=� � WindowStartupLocation$ CenterScreen=�   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � WindowStyle$
 None=� � ShowInTaskbar$
 True��.���  � RowDefinitions �� $	��100 ��� $	��200 ��� $
��auto �.+�� -   $��0��$	��100��$	��Top=�.+��� $
��File��$
��Left=�$��16,6,0,0q�$	��Top=� J� RenderTransformOrigin$ 0.347,0.537&�$��26��$��28���� -   	 ��Name$	 txtFile��$
��Left=�$��66,3,0,0q�
 �� TextWrapping$

 Wrap=�$	��Top=�$	��302��${�False��$��27���� -    ��Name$ btnFile��$��UČITAJ JSON FAJL IZ KASE��$
��Left=�$��	66,42,0,0q� � Cursor$
 Hand�� + 'MaterialDesignRaisedSecondaryDarkButton#��[ $	��Top=�$��24��$	��302��$
{�True���� -   $ btnExit��$��X��$
 Hand��$
��Left=� * &MaterialDesignFloatingActionDarkButton#��[ $��	872,8,0,0q�$��35��$��35��$��20(�$��	ExtraBold"�$	��Top=����	#FFDA3929���	#FFDA3929$��0,0,0,0q��� $��Zadnji izmenjen��$
��Left=�$��	443,4,0,0q�$	��Top=�$��16(�$
��Bold"�$��Oblique$�$	��130���� -    ��Name$ lblLastEdited��$	��123��
��� � �$��20(�$
��Left=�$��	614,1,0,0q�$	��Top=�$��	ExtraBold"�$��28��$��88���� -   $ lblArticleName��$
��Text��
��� � �$��12(�$
��Left=�$��
448,65,0,0q�$	��Top=�$��	ExtraBold"�$��27��$	��310���� $��Š��$��20(�$
��Left=�$��	596,2,0,0q�$	��Top=�$
��Bold"�$��28��$��22���� $	��Kom��$��20(�$
��Left=�$��
618,34,0,0q�$	��Top=�$
��Bold"�$��28��$��54���� -   $ lblLastQuantity��$	��123��
��� � �$��20(�$
��Left=�$��	673,0,0,0q�$��Center=�$��	ExtraBold"�$��28��$��88���� $
��Cena��$��20(�$
��Left=�$��
454,34,0,0q�$	��Top=�$
��Bold"�$��28��$��54���� -   $ lblPrice��$	��123��
��� � �$��20(�$
��Left=�$��	508,0,0,0q�$��Center=�$��	ExtraBold"�$��28��$��91��� $	��200��$��1��$	��Top=�.+��� $��Šifra artikla��$
��Left=�$��	26,59,0,0q�$	��Top=�$��84��$��26���� -	   $	 txtIDArticle��$��0��$
��Left=�$��	17,80,0,0q�$

 Wrap=�$	��Top=�$��93��$��31��$��16(�$
��Bold"��� $��
+/- Komada��$
��Left=�$��
152,55,0,0q�$	��Top=�$��26��$��95��$
��Bold"�$ 0.492,-0.175&��� -
   $	 txtQuantity��$��1��$
��Left=�$��
140,80,0,0q�$

 Wrap=�$	��Top=�$	��100��$��31��$��16(�$
��Bold"��� $��Trenutno stanje��$
��Left=�$��
284,56,0,0q�$	��Top=�$��26��$	��103��$
��Bold"�$ 0.48,-0.181&��� -   $	 txtCurrentAmount��$
��Left=�$��
270,79,0,0q�$

 Wrap=�$	��Top=�$	��120��$��31��$��16(�$
��Bold"��� $��Naziv artikla��$
��Left=�$��73,2,0,0q�$	��Top=�$��84��$��26���� -   $	 txtArticleName��$
��Left=�$��	20,17,0,0q�$

 Wrap=�$	��Top=�$	��223��$��31��$��16(�$
��Bold"��� -   $ 
btnAddCell��$��3��$�� DODAJ
STAVKU��#��[ $
��Left=�$��
19,125,0,0q�$	��Top=�$��45��$	��120�����	#FF2C6AEE$��Normal&�$��LeftToRight=�$��Center=�$��Center=�$
 Hand�����	#FFA7A2EA$��0,0,0,0q�{��� *DataContext.ControlsEnabled+ �� RelativeSource �� *FindAncestor+ �� AncestorType
 Window�� -   $ btnDeleteCell��$��IZBRIŠI STAVKU��$
 Hand��$
��Left=�#��[ $��	458,5,0,0q�$	��Top=�$��40��$	��138�����	#FFDA3929$��0,0,0,0q�${�False���� -   $ btnGenerate��#��[ $2��,   KREIRAJ EXCEL FAJL 
ARTIKLE SA KOLIČINOM��$
 Hand��$��
722,87,0,0q�$��46��$	��177��$��12(����#189510$��0q�$
��Left=�$	��Top=��� -   $	 txtPrice��$
��Left=�$��
280,15,0,0q�$

 Wrap=�$	��Top=�$	��100��$��31��$��16(�$
��Bold"��� $
��Cena��$
��Left=�$��	308,0,0,0q�$	��Top=�$��26��$��63��$
��Bold"��� -   $ btnClean��$��OČISTI POLJA��$
 Hand��$
��Left=�#��[ $��
455,64,0,0q�$	��Top=�$��41��$	��140�����#8E1BDD$��0,0,0,0q��� -   $ btnSave��$
��Left=�$��	735,6,0,0q�$	��Top=�$��45��$	��158�����	#FF2C6AEE$
 Hand�����	#FFA7A2EA$��0,0,0,0q�.���  �� $	��24* ��� $��7* �.+�'  !MaterialDesignThemes.Wpf.PackIcon  $��30��$��30��   Kind$ ContentSave=�$��	-33,0,0,0q�$��2���� $��SAČUVAJ�����#FFF$��11,3,-11,-3q�$��2���� -   $ lblSaveTime��$��Right=�$��0,56,8,0q�$	��Top=�$��12(�$��	ExtraBold"����	#FF2C6AEE$��Zadnji put: ��$	��181���� -   $ 	btnRemove��$��3��$��ODUZMI 
STAVKU��#��[ $
��Left=�$��176,125,0,0q�$	��Top=�$��45��$	��120�����	#FFDA3929$��0,0,0,0q�$��Normal&�$��LeftToRight=�$��Center=�$��Center=�$
 Hand�����	#FFA7A2EA{��� *DataContext.ControlsEnabled+ �� *FindAncestor+" ��� -   $ btnGenerateAllArticles��#��[ $$��  KREIRAJ EXEL 
SVIH ARTIKALA��$
 Hand��$��733,143,0,0q�$��46��$	��158��$��12(����#176510$��0q�$
��Left=�$	��Top=�� $��2��$	��400��$	��Top=�.+�&   System.Windows.Controls.DataGrid  -     Name$ dataGridList��$��10q�9� -    9�Name$ explanderLog�� 9� ExpandDirection$
 Down=�$��20(�
������� 9� 
IsExpanded$ False��$��Bottom=�$��	0,0,0,-20q����� ._��� $��Center=�
�������$��
ExtraBlack"�
ISTORIJA���� ���#d3d3d3$	��200��$	��Top=�.���� -    ��Name$ txtLog��$��13(�$��10,0,0,0q��� -   $	 	txtFilter��$
��Left=�$��356,139,0,0q�$

 Wrap=�$	��Top=�$	��269��$��37��$��16(�$
��Bold"�$��1���� $��Pretraga��$
��Left=�$��455,128,0,0q�$	��Top=�$��26��$��63��$
��Bold"�$��1��