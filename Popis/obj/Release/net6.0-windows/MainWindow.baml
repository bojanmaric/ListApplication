   M S B A M L   `   `   `  ���� @  <Popis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PopisPopis      Popis.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     %localclr-namespace:Popis     _ [MaterialDesignThemes.Wpf, Version=4.6.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � Title$  POPIS��$	��720��$	��920�� � 
ResizeMode$ CanMinimize=� � WindowStartupLocation$ CenterScreen=�   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � WindowStyle$
 None=� � ShowInTaskbar$
 True��.���  � RowDefinitions �� $	��100 ��� $	��200 ��� $
��auto �.+�� $��0��$	��100��$	��Top=�.+��� $
��File��$
��Left=�$��16,6,0,0q�$	��Top=� J� RenderTransformOrigin$ 0.347,0.537&�$��26��$��28���� -   	 ��Name$	 txtFile��$
��Left=�$��66,3,0,0q�
 �� TextWrapping$

 Wrap=�$	��Top=�$	��380��${�False��$��27���� -    ��Name$ btnFile��$��UČITAJ JSON FAJL IZ KASE��$
��Left=�$��	66,42,0,0q� � Cursor$
 Hand�� + 'MaterialDesignRaisedSecondaryDarkButton#��[ $	��Top=�$��24��$	��379��$
{�True���� -   $ btnExit��$��Napusti 
program��$
 Hand��$
��Left=�#��[ $��
776,13,0,0q�$	��Top=�$��62��$	��128�����	#FFDA3929$��0,0,0,0q��� $��Zadnji izmenjen artikal��$
��Left=�$��	497,6,0,0q�$	��Top=�$��16(�$
��Bold"�$��Oblique$�$	��180���� -    ��Name$ lblLastEdited��$	��123��
��� � �$��20(�$
��Left=�$��	510,0,0,0q�$��Center=�$��	ExtraBold"�$��27��$��88���� -   $ lblArticleName��$
��Text��
��� � �$��12(�$
��Left=�$��
496,64,0,0q�$	��Top=�$��	ExtraBold"�$��27��$	��205���� $��Š��$��20(�$
��Left=�$��	489,0,0,0q�$��Center=�$
��Bold"�$��28��$��22���� $	��Kom��$��20(�$
��Left=�$��	599,0,0,0q�$��Center=�$
��Bold"�$��28��$��54���� -   $ lblLastQuantity��$	��123��
��� � �$��20(�$
��Left=�$��	655,0,0,0q�$��Center=�$��	ExtraBold"�$��28��$��88��� $	��200��$��1��$	��Top=�.+��� $��Šifra artikla��$
��Left=�$��	26,59,0,0q�$	��Top=�$��84��$��26���� -   $	 txtIDArticle��$��0��$
��Left=�$��	17,80,0,0q�$

 Wrap=�$	��Top=�$��93��$��31��$��16(�$
��Bold"��� $��+ Komada��$
��Left=�$��
152,55,0,0q�$	��Top=�$��26��$��95��$
��Bold"�$ 0.492,-0.175&��� -   $	 txtQuantity��$��1��$
��Left=�$��
140,80,0,0q�$

 Wrap=�$	��Top=�$	��100��$��31��$��16(�$
��Bold"��� $��Trenutno stanje��$
��Left=�$��
284,56,0,0q�$	��Top=�$��26��$	��103��$
��Bold"�$ 0.48,-0.181&��� -	   $	 txtCurrentAmount��$
��Left=�$��
270,79,0,0q�$

 Wrap=�$	��Top=�$	��120��$��31��$��16(�$
��Bold"��� $��Naziv artikla��$
��Left=�$��73,2,0,0q�$	��Top=�$��84��$��26���� -
   $	 txtArticleName��$
��Left=�$��	20,17,0,0q�$

 Wrap=�$	��Top=�$	��223��$��31��$��16(�$
��Bold"��� -   $ 
btnAddCell��$��3��$�� DODAJ
STAVKU��#��[ $
��Left=�$��
19,125,0,0q�$	��Top=�$��45��$	��158�����	#FF2C6AEE$��Normal&�$��LeftToRight=�$��Center=�$��Center=�$
 Hand�����	#FFA7A2EA$��0,0,0,0q�{��� *DataContext.ControlsEnabled+ �� RelativeSource �� *FindAncestor+ �� AncestorType
 Window�� -   $ btnDeleteCell��$��IZBRIŠI 
STAVKU��$
 Hand��$
��Left=�#��[ $��	458,5,0,0q�$	��Top=�$��40��$	��138�����	#FFDA3929$��0,0,0,0q�${�False���� -   $ btnGenerate��#��[ $��KREIRAJ
  EXCEL 
   FAJL ��$
 Hand��$��
737,63,0,0q�$��89��$	��164��$��14(����#189510$��0q�$
��Left=�$	��Top=��� -   $	 txtPrice��$
��Left=�$��
280,15,0,0q�$

 Wrap=�$	��Top=�$	��100��$��31��$��16(�$
��Bold"��� $
��Cena��$
��Left=�$��	308,0,0,0q�$	��Top=�$��26��$��63��$
��Bold"��� -   $ btnClean��$��IZBRISI POLJA��$
 Hand��$
��Left=�#��[ $��
455,78,0,0q�$	��Top=�$��41��$	��140�����#8E1BDD$��0,0,0,0q�� $��2��$	��400��$	��Top=�.+�&   System.Windows.Controls.DataGrid  -     Name$ dataGridList��$��10q��� -   $	 	txtFilter��$
��Left=�$��246,134,0,0q�$

 Wrap=�$	��Top=�$	��269��$��31��$��16(�$
��Bold"�$��1���� $��Pretraga��$
��Left=�$��337,118,0,0q�$	��Top=�$��26��$��63��$
��Bold"�$��1��