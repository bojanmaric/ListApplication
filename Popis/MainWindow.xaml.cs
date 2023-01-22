using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpreadsheetLight;

namespace Popis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Article
    {
        public string barkod { get; set; }
        public double porez { get; set; }

        public string jedinica_mere { get; set; }
        public double cena { get; set; }

        public string naziv { get; set; }

        public double kolicina { get; set; }
        public string sifra { get; set; }

        public int vrsta_artikla { get; set; }


    }
    public partial class MainWindow : Window
    {

        DataTable dt;
        public List<Article> articles;

        public string fileNameSave = "";
        public Queue<string> logQueue;
        public MainWindow()
        {
            InitializeComponent();
            articles = new();
            dt = new();

            dt.Columns.Add("Barkod");
            dt.Columns.Add("Porez");
            dt.Columns.Add("J-M");
            dt.Columns.Add("Cena");
            dt.Columns.Add("Naziv");
            dt.Columns.Add("Kolicina");
            dt.Columns.Add("Sifra");
            dt.Columns.Add("Vrsta akrtikla");

            logQueue = new();

            dataGridList.ItemsSource = dt.DefaultView;

            // When is data filtered to work double click
            dataGridList.BeginningEdit += (s, ss) => ss.Cancel = true;
            txtQuantity.Text = "1";
            txtCurrentAmount.IsEnabled = false;
            btnSave.IsEnabled = false;

            lblLastEdited.Content = "";
            lblArticleName.Content = "";
            lblLastQuantity.Content = "";
            lblSaveTime.Content = "";
            lblPrice.Content = "";
            txtIDArticle.Focus();
        }


        // Control text box is it input only decimal numbers
        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void dataGridList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridList.SelectedIndex > -1)
            {

                if (txtFilter.Text != "")
                {

                    Article selectedArtical = (Article)dataGridList.SelectedItem;

                    txtIDArticle.Text = selectedArtical.barkod;

                    txtArticleName.Text = selectedArtical.naziv;
                    txtCurrentAmount.Text = selectedArtical.kolicina.ToString();
                    txtPrice.Text = selectedArtical.cena.ToString();
                    txtQuantity.Text = "0";
                    txtCurrentAmount.IsEnabled = true;
                    // txtArticleName.IsEnabled = true;
                    txtPrice.IsEnabled = true;
                    btnDeleteCell.IsEnabled = true;

                }
                else
                {
                    txtIDArticle.Text = articles[dataGridList.SelectedIndex].barkod;

                    txtArticleName.Text = articles[dataGridList.SelectedIndex].naziv;
                    txtCurrentAmount.Text = articles[dataGridList.SelectedIndex].kolicina.ToString();
                    txtPrice.Text = articles[dataGridList.SelectedIndex].cena.ToString();
                    txtQuantity.Text = "0";
                    txtCurrentAmount.IsEnabled = true;
                    // txtArticleName.IsEnabled = true;
                    txtPrice.IsEnabled = true;
                    btnDeleteCell.IsEnabled = true;

                }
            }

        }


        // When should save fale to exel file to choose name
        public string getPathNameForNewFile()
        {
            SaveFileDialog path = new SaveFileDialog();

            if (path.ShowDialog() == true)
            {
                return path.FileName;
            }
            return "";
        }
        private void btnAddCell_Click(object sender, RoutedEventArgs e)
        {
            bool finded = false;
            if (txtIDArticle.Text != "" && txtArticleName.Text == "")
            // Case when user change only quantity for specify barcode
            {
                for (int i = 0; i < articles.Count; i++)
                {
                    if (articles[i].barkod == txtIDArticle.Text)
                    {
                        finded = true;
                        articles[i].kolicina = articles[i].kolicina + Int32.Parse(txtQuantity.Text);
                        // update quantity of filtered field in both case
                        dt.Rows[i][5] = articles[i].kolicina;
                        lblLastEdited.Content = txtIDArticle.Text;
                        lblArticleName.Content = articles[i].naziv;
                        lblLastQuantity.Content = txtQuantity.Text;
                        lblPrice.Content = articles[i].cena;



                        UpdateLog($"[Info]- Dodat artikal na stanje Sirfa: {articles[i].barkod} | Naziv: {articles[i].naziv} | " +
                                  $"Novo stanje {articles[i].kolicina} | Staro stanje {articles[i].kolicina - Int32.Parse(txtQuantity.Text)}");
                        Empty();
                        return;

                    }

                }



            }
            else if (txtIDArticle.Text != "" && txtArticleName.Text != "" && txtPrice.Text != "")
            {
                // Case when user select item and should change price and total quantity
                for (int i = 0; i < articles.Count; i++)
                {
                    if (articles[i].barkod == txtIDArticle.Text)
                    {
                        finded = true;
                        articles[i].cena = double.Parse(txtPrice.Text);

                        dt.Rows[i][3] = articles[i].cena;
                        if (articles[i].naziv != txtArticleName.Text)
                        {
                            articles[i].naziv = txtArticleName.Text;
                            dt.Rows[i][4] = articles[i].naziv;
                        }
                        if (articles[i].barkod != txtIDArticle.Text)
                        {
                            articles[i].barkod = txtIDArticle.Text;
                        }
                        if (txtCurrentAmount.Text != articles[i].kolicina.ToString())
                        {

                            UpdateLog($"[Info]- Dodat artikal na stanje Sirfa: {articles[i].barkod} | Naziv: {txtArticleName.Text} | " +
                                 $"Novo stanje  {Int32.Parse(txtCurrentAmount.Text)}| Staro stanje {articles[i].kolicina}");

                            articles[i].kolicina = double.Parse(txtCurrentAmount.Text);
                            // update quantity of filtered field in both case
                            dt.Rows[i][5] = articles[i].kolicina;

                            lblLastEdited.Content = txtIDArticle.Text;
                            lblArticleName.Content = articles[i].naziv;
                            lblLastQuantity.Content = txtQuantity.Text;
                            lblPrice.Content = articles[i].cena;


                            Empty();

                        }
                        else
                        {
                            articles[i].kolicina = articles[i].kolicina + Int32.Parse(txtQuantity.Text);
                            // update quantity of filtered field in both case
                            dt.Rows[i][5] = articles[i].kolicina;
                            dt.Rows[i][4] = articles[i].naziv;
                            if (articles[i].naziv != txtArticleName.Text)
                            {
                                articles[i].naziv = txtArticleName.Text;
                            }
                            if (articles[i].barkod != txtIDArticle.Text)
                            {
                                articles[i].barkod = txtIDArticle.Text;
                            }
                            lblLastEdited.Content = txtIDArticle.Text;
                            lblArticleName.Content = articles[i].naziv;
                            lblLastQuantity.Content = txtQuantity.Text;
                            lblPrice.Content = articles[i].cena;

                            UpdateLog($"[Info]- Dodat artikal na stanje Sirfa: {articles[i].barkod} | Naziv: {articles[i].naziv} | " +
                                  $"Novo stanje {articles[i].kolicina} | Staro stanje {articles[i].kolicina - Int32.Parse(txtQuantity.Text)}");
                            Empty();

                        }
                        return;

                    }

                }

            }

            if (!finded)
            {
                if (txtArticleName.Text != "" && txtIDArticle.Text != "" && txtPrice.Text != "" && txtQuantity.Text != "")
                {
                    Article article = new Article()
                    {
                        barkod = txtIDArticle.Text,
                        sifra = txtIDArticle.Text,
                        vrsta_artikla = 4,
                        naziv = txtArticleName.Text,
                        cena = double.Parse(txtPrice.Text),
                        kolicina = double.Parse(txtQuantity.Text),
                        porez = 20,
                        jedinica_mere = "KOM"
                    };
                    articles.Add(article);
                    DataRow dr = dt.NewRow();

                    dr[0] = article.sifra;
                    dr[1] = article.porez;
                    dr[2] = article.jedinica_mere;
                    dr[3] = article.cena;
                    dr[4] = article.naziv;
                    dr[5] = article.kolicina;
                    dr[6] = article.barkod;
                    dr[7] = article.vrsta_artikla;


                    dt.Rows.Add(dr);
                    lblArticleName.Content = article.naziv;
                    lblLastQuantity.Content = txtQuantity.Text;
                    lblLastEdited.Content = txtIDArticle.Text;
                    lblPrice.Content = article.cena;

                    UpdateLog($"[Info]- Dodat novi artikal {article.barkod} | {article.naziv} | " +
                          $" kolicina {article.kolicina}");
                    Empty();
                }
                else
                {
                    MessageBox.Show("Polja Naziv, Kolicina, Cena, Sifra MORAJU BITI POPUNJENA", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }

        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

            if (txtIDArticle.Text != "" && txtArticleName.Text == "")
            {
                for (int i = 0; i < articles.Count; i++)
                {
                    if (articles[i].barkod == txtIDArticle.Text)
                    {
                        if (articles[i].kolicina - Int32.Parse(txtQuantity.Text) >= 0)
                        {

                            articles[i].kolicina = articles[i].kolicina - Int32.Parse(txtQuantity.Text);
                        }
                        else
                        {
                            MessageBox.Show($"Nije moguce da kolicina bude manja od nule! \n Trenutno stanje {articles[i].kolicina}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

                            UpdateLog($"[Greska]- Nije moguce oduzeti artikal sa stanja  Sirfa: {articles[i].barkod} | Naziv: {articles[i].naziv} | " +
                                      $" zeljno Novo stanje {articles[i].kolicina - Int32.Parse(txtQuantity.Text)} | trenutno stanje {articles[i].kolicina}");

                            txtIDArticle.Focus();
                            return;
                        }
                        // update quantity of filtered field in both case
                        dt.Rows[i][5] = articles[i].kolicina;
                        lblLastEdited.Content = txtIDArticle.Text;
                        lblArticleName.Content = articles[i].naziv;
                        lblLastQuantity.Content = txtQuantity.Text;
                        lblPrice.Content = articles[i].cena;



                        UpdateLog($"[Info]- Oduzet artikal sa stanja  Sirfa: {articles[i].barkod} | Naziv: {articles[i].naziv} | " +
                                  $"Novo stanje {articles[i].kolicina} | Staro stanje {articles[i].kolicina + Int32.Parse(txtQuantity.Text)}");
                        Empty();
                        return;

                    }

                }



            }
            else
            {

                MessageBox.Show("Nema artikal sa tom sifrom");
                Empty();
            }


        }
        public void Empty()
        {
            txtQuantity.Text = "1";
            txtIDArticle.Clear();
            txtArticleName.Clear();
            txtCurrentAmount.Clear();
            txtPrice.Clear();
            txtCurrentAmount.IsEnabled = false;

            btnDeleteCell.IsEnabled = false;

            if (txtFilter.Text != "")
            {
                txtFilter.Text = "";
                var filtered = articles.Where<Article>(artikal => artikal.naziv.ToUpper().Contains(txtFilter.Text.ToUpper()) || artikal.sifra.ToUpper().Contains(txtFilter.Text.ToUpper()) || artikal.cena.ToString().ToUpper().Contains(txtFilter.Text.ToUpper()));
                dataGridList.ItemsSource = filtered;
            }
            txtIDArticle.Focus();

        }
        //function where we need to add new article in list and also add new row in data grid
        public void displayData()
        {
            // double rabat = double.Parse(txtDiscount.Text);

        }
        public void UpdateLog(string line)
        {
            string newLine = $"{DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")} --> {line}\n";


            if (logQueue.Count == 10)
            {
                logQueue.Dequeue();
            }

            logQueue.Enqueue(newLine);

            txtLog.Text = "";

            foreach (string item in logQueue)
            {
                txtLog.Text = txtLog.Text + item;
            }

        }
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = getPathNameForNewFile();

            if (pathFile != null)
            {
                SLDocument excel = new SLDocument();

                excel.SetCellValue(1, 1, "Barkod");
                excel.SetCellValue(1, 2, "Porez");
                excel.SetCellValue(1, 3, "J-M");
                excel.SetCellValue(1, 4, "Cena");
                excel.SetCellValue(1, 5, "Naziv");
                excel.SetCellValue(1, 6, "Kolicina");
                excel.SetCellValue(1, 7, "Sifra");
                excel.SetCellValue(1, 8, "Vrsta artikla");
                excel.SetCellValue(1, 9, "Suma");

                int row = 2;
                foreach (Article art in articles)
                {
                    if (art.kolicina == 0) continue;//ukoliko je kolicina 0 ne treba nam takav proizvod

                    excel.SetCellValue(row, 1, art.barkod);
                    excel.SetCellValue(row, 2, art.porez);
                    excel.SetCellValue(row, 3, art.jedinica_mere);
                    excel.SetCellValue(row, 4, art.cena);
                    excel.SetCellValue(row, 5, art.naziv);
                    excel.SetCellValue(row, 6, art.kolicina);
                    excel.SetCellValue(row, 7, art.sifra);
                    excel.SetCellValue(row, 8, art.vrsta_artikla);
                    excel.SetCellValue(row, 9, art.cena * art.kolicina);

                    row++;
                }

                excel.SaveAs($"{pathFile}.xlsx");

                File.WriteAllText(@$"{pathFile}.json", JsonConvert.SerializeObject(articles));

                MessageBox.Show("Uspešno kreiran fajl", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                Process process = new Process();
                process.StartInfo.FileName = $"{pathFile}.xlsx";
                process.StartInfo.Arguments = "ProcessStart.cs";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
            else
            {
                MessageBox.Show("Morate uneti kako će se zvati dokument", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }
        /// <summary>
        /// nesto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            file.ShowDialog();



            if (file.FileName != "")
            {
                fileNameSave = file.FileName.ToString();

                txtFile.Text = file.FileName.ToString();
                btnSave.IsEnabled = true;

                try
                {

                    dynamic fajl = JsonConvert.DeserializeObject(File.ReadAllText(file.FileName));

                    foreach (var art in fajl)
                    {
                        //"barkod":"1","porez":20.000000,"jedinica_mere":"KOM","cena":1400.000000,
                        //"naziv":"PVC PLAFONJRA 18W","sifra":"11950","vrsta_artikla":4},

                        //dodavanje necega ...

                        if (art["kolicina"] > -1)
                        {
                            articles.Add(new Article()
                            {
                                barkod = art["barkod"],
                                porez = art["porez"],
                                jedinica_mere = art["jedinica_mere"],
                                cena = art["cena"],
                                naziv = art["naziv"],
                                sifra = art["barkod"],
                                vrsta_artikla = art["vrsta_artikla"],

                                kolicina = art["kolicina"]
                            });
                            DataRow dr = dt.NewRow();

                            dr[0] = art["barkod"];
                            dr[1] = art["porez"];
                            dr[2] = art["jedinica_mere"];
                            dr[3] = art["cena"];
                            dr[4] = art["naziv"];
                            dr[5] = art["kolicina"];
                            dr[6] = art["barkod"];
                            dr[7] = art["vrsta_artikla"];


                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            articles.Add(new Article()
                            {
                                barkod = art["barkod"],
                                porez = art["porez"],
                                jedinica_mere = art["jedinica_mere"],
                                cena = art["cena"],
                                naziv = art["naziv"],
                                sifra = art["barkod"],
                                vrsta_artikla = art["vrsta_artikla"],

                                kolicina = 0
                            });
                            DataRow dr = dt.NewRow();

                            dr[0] = art["barkod"];
                            dr[1] = art["porez"];
                            dr[2] = art["jedinica_mere"];
                            dr[3] = art["cena"];
                            dr[4] = art["naziv"];
                            dr[5] = 0;
                            dr[6] = art["barkod"];//poreska osnovica
                            dr[7] = art["vrsta_artikla"];


                            dt.Rows.Add(dr);

                        }
                    }

                    if (articles.Count > 0)
                    {
                        btnFile.IsEnabled = false;

                        MessageBox.Show("Za dodavanje artikla moze da se koristi \"+\" na tastaturi \n" +
                            "Za oduzimanje moze da se koristi \"-\" na tastaturi", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtIDArticle.Focus();

                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Došlo je do greške", "Greška", MessageBoxButton.OK, MessageBoxImage.Information);
                    throw;
                }

            }
            else
            {
                MessageBox.Show("Morate izabrati .json file", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteCell_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Brisanje", MessageBoxButton.YesNo, MessageBoxImage.Question);


            if (result == MessageBoxResult.Yes)
            {

                if (txtFilter.Text != "")
                {

                    Article selectedArtical = (Article)dataGridList.SelectedItem;
                    articles.Remove(selectedArtical);
                    dt.Rows.RemoveAt(dataGridList.SelectedIndex);

                }
                else
                {
                    articles.RemoveAt(dataGridList.SelectedIndex);

                    dt.Rows.RemoveAt(dataGridList.SelectedIndex);
                }

                Empty();
            }

        }

        /// <summary>
        /// filtriranje podataka ...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {
            //var filtered = articles.Where<Article>(artikal => artikal.naziv.ToUpper().StartsWith(txtFilter.Text.ToUpper())|| artikal.sifra.ToUpper().StartsWith(txtFilter.Text.ToUpper()) || artikal.cena.ToString().ToUpper().StartsWith(txtFilter.Text.ToUpper()));
            var filtered = articles.Where<Article>(artikal => artikal.naziv.ToUpper().Contains(txtFilter.Text.ToUpper()) || artikal.sifra.ToUpper().Contains(txtFilter.Text.ToUpper()) || artikal.cena.ToString().ToUpper().Contains(txtFilter.Text.ToUpper()));
            dataGridList.ItemsSource = filtered;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {


            if (fileNameSave != "")
            {
                MessageBoxResult result = MessageBox.Show("Da li zelite da sačuvate pre izlaska?", "Sačuvati", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {


                    File.WriteAllText(@$"{fileNameSave}", JsonConvert.SerializeObject(articles));


                    Environment.Exit(0);

                }
                else
                {
                    Environment.Exit(0);

                }

            }
            else
            {
                Environment.Exit(0);

            }
        }

        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            Empty();
        }

        private void txtIDArticle_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtQuantity.SelectAll();
                txtQuantity.Focus();

            }
        }

        private void txtQuantity_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnAddCell_Click(sender, e);
            }
            if (e.Key == System.Windows.Input.Key.Add)
            {
                btnAddCell_Click(sender, e);
            }
            if (e.Key == System.Windows.Input.Key.Subtract)
            {
                btnRemove_Click(sender, e);
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (fileNameSave != "")
            {

                File.WriteAllText(@$"{fileNameSave}", JsonConvert.SerializeObject(articles));
                lblSaveTime.Content = $"Zadnji put: {DateTime.Now.ToString("HH:mm dd/MM/yyyy")}";

            }
            else
            {
                MessageBox.Show("Niste učitali JSON file", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void btnGenerateAllArticles_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = getPathNameForNewFile();

            if (pathFile != null)
            {

                SLDocument excel = new SLDocument();

                excel.SetCellValue(1, 1, "Barkod");
                excel.SetCellValue(1, 2, "Porez");
                excel.SetCellValue(1, 3, "J-M");
                excel.SetCellValue(1, 4, "Cena");
                excel.SetCellValue(1, 5, "Naziv");
                excel.SetCellValue(1, 6, "Kolicina");
                excel.SetCellValue(1, 7, "Sifra");
                excel.SetCellValue(1, 8, "Vrsta artikla");
                excel.SetCellValue(1, 9, "Suma");

                int row = 2;
                foreach (Article art in articles)
                {
                    excel.SetCellValue(row, 1, art.barkod);
                    excel.SetCellValue(row, 2, art.porez);
                    excel.SetCellValue(row, 3, art.jedinica_mere);
                    excel.SetCellValue(row, 4, art.cena);
                    excel.SetCellValue(row, 5, art.naziv);
                    excel.SetCellValue(row, 6, art.kolicina);
                    excel.SetCellValue(row, 7, art.sifra);
                    excel.SetCellValue(row, 8, art.vrsta_artikla);
                    excel.SetCellValue(row, 9, art.cena * art.kolicina);

                    row++;
                }

                excel.SaveAs($"{pathFile}.xlsx");

                MessageBox.Show("Uspešno kreiran fajl", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                Process process = new Process();
                process.StartInfo.FileName = $"{pathFile}.xlsx";
                process.StartInfo.Arguments = "ProcessStart.cs";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
            else
            {
                MessageBox.Show("Morate uneti kako će se zvati dokument", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
