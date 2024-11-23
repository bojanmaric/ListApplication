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
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.ObjectModel;

namespace Popis
{

    public partial class MainWindow : Window
    {

        DataTable dt;
        public List<Article> articles;

        public string fileNameSave = "";
        public Queue<string> logQueue;

        public readonly DbReader dbReader;
        public readonly DbWriter dbWriter;

        public readonly Logger logger;

        public MainWindow()
        {
            InitializeComponent();

            dbReader = new();
            dbWriter = new();
            logger = new();
            RefreshData();

            //  articles = new();
            /* dt = new();

             dt.Columns.Add("Barkod");
             dt.Columns.Add("Porez");
             dt.Columns.Add("J-M");
             dt.Columns.Add("Cena");
             dt.Columns.Add("Naziv");
             dt.Columns.Add("Kolicina");
             dt.Columns.Add("Sifra");
             dt.Columns.Add("Vrsta akrtikla");*/

            logQueue = new();

            // dataGridList.ItemsSource = dt.DefaultView;

            // When is data filtered to work double click
            //   dataGridList.BeginningEdit += (s, ss) => ss.Cancel = true;
            txtQuantity.Text = "1";
            txtCurrentAmount.IsEnabled = false;
            //  btnSave.IsEnabled = false;

            lblLastEdited.Content = "";
            lblArticleName.Content = "";
            lblLastQuantity.Content = "";
            // lblSaveTime.Content = "";
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
            try
            {
                Article selectedArtical = (Article)dataGridList.SelectedItem;
                if (selectedArtical!=null)
                {
                    txtIDArticle.Text = selectedArtical.barkod;
                    txtArticleName.Text = selectedArtical.naziv;
                    txtCurrentAmount.Text = selectedArtical.kolicina.ToString();
                    txtPrice.Text = selectedArtical.cena.ToString();
                    txtQuantity.Text = "0";
                    txtCurrentAmount.IsEnabled = true;
                    txtPrice.IsEnabled = true;
                    //btnDeleteCell.IsEnabled = true;
                }
              
            }
            catch (Exception ex)
            {
                UpdateLog($"Greska {ex.InnerException} \n {ex.Message}");

                MessageBox.Show($"{ex.InnerException}");
            }
        }
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
            try
            {
                if (!string.IsNullOrEmpty( txtIDArticle.Text))
                {

                    var (success, log) = dbWriter.IcreaseArticle(txtIDArticle.Text, double.Parse(txtQuantity.Text));
                    if (success)
                    {
                        lblArticleName.Content = txtArticleName.Text;
                        lblPrice.Content = txtPrice.Text;
                        lblLastQuantity.Content = (double.Parse(txtQuantity.Text)).ToString();
                        lblLastEdited.Content = txtIDArticle.Text;

                        logger.Log(txtIDArticle.Text, txtArticleName.Text, double.Parse(txtQuantity.Text));

                        Empty();
                        RefreshData();
                    }
                    UpdateLog(log);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.InnerException}");
                UpdateLog($"Greska {ex.InnerException} \n {ex.Message}");
            }

        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtIDArticle.Text))
                {
                    var (success, log) = dbWriter.DecreaseArticle(txtIDArticle.Text, double.Parse(txtQuantity.Text));
                    if (success)
                    {
                        logger.Log(txtIDArticle.Text, txtArticleName.Text, -double.Parse(txtQuantity.Text));
                        lblArticleName.Content=txtArticleName.Text;
                        lblPrice.Content = txtPrice.Text;
                        lblLastQuantity.Content = (-double.Parse(txtQuantity.Text)).ToString();
                        lblLastEdited.Content=txtIDArticle.Text;

                        Empty();
                        RefreshData();
                    }
                    else
                    {
                        MessageBox.Show($"Greska");
                    }
                    UpdateLog(log);
                }
            }
            catch (Exception ex)
            {
                UpdateLog($"Greska {ex.InnerException} \n {ex.Message}");

                MessageBox.Show($"{ex.InnerException}");
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

            //btnDeleteCell.IsEnabled = false;

            /* if (txtFilter.Text != "")
             {
                 txtFilter.Text = "";
                 var filtered = articles.Where<Article>(artikal => artikal.naziv.ToUpper().Contains(txtFilter.Text.ToUpper()) || artikal.sifra.ToUpper().Contains(txtFilter.Text.ToUpper()) || artikal.cena.ToString().ToUpper().Contains(txtFilter.Text.ToUpper()));
                 dataGridList.ItemsSource = filtered;
             }*/
            txtIDArticle.Focus();

        }
        public void UpdateLog(string line)
        {
            string newLine = $"{DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")} --> {line}\n";

            if (logQueue.Count == 250)
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

              //  File.WriteAllText(@$"{pathFile}.json", JsonConvert.SerializeObject(articles));

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
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            file.ShowDialog();

            if (file.FileName != "")
            {
                dbWriter.InsertArticlesFromJson(file.FileName);
                /*
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
                */
            }
            else
            {
                MessageBox.Show("Morate izabrati .json file", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void btnDeleteCell_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Brisanje", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Article selectedArtical = (Article)dataGridList.SelectedItem;
                    articles.Remove(selectedArtical);
                    Empty();
                }
            }
            catch (Exception ex)
            {
                UpdateLog($"Greska {ex.InnerException} \n {ex.Message}");

                MessageBox.Show($"{ex.InnerException}");
            }
        }
        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {
            //var filtered = articles.Where<Article>(artikal => artikal.naziv.ToUpper().StartsWith(txtFilter.Text.ToUpper())|| artikal.sifra.ToUpper().StartsWith(txtFilter.Text.ToUpper()) || artikal.cena.ToString().ToUpper().StartsWith(txtFilter.Text.ToUpper()));
            var filtered = articles.Where<Article>(artikal => artikal.naziv.ToUpper().Contains(txtFilter.Text.ToUpper()) || artikal.sifra.ToUpper().Contains(txtFilter.Text.ToUpper()) || artikal.cena.ToString().ToUpper().Contains(txtFilter.Text.ToUpper()));
            dataGridList.ItemsSource = filtered;
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                UpdateLog($"Greska {ex.InnerException} \n {ex.Message}");

                MessageBox.Show($"{ex.InnerException}");

                //btnSave_Click(sender, e);

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
                var art = articles.SingleOrDefault(x => x.barkod == txtIDArticle.Text);
                if (art != null)
                {
                    txtArticleName.Text = art.naziv;
                    txtPrice.Text = art.cena.ToString();
                    txtQuantity.SelectAll();
                    txtQuantity.Focus();
                }
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
            try
            {
                if (fileNameSave != "")
                {

                    File.WriteAllText(@$"{fileNameSave}", JsonConvert.SerializeObject(articles));
                    // lblSaveTime.Content = $"Zadnji put: {DateTime.Now.ToString("HH:mm dd/MM/yyyy")}";
                    UpdateLog($"Sacuvano trenutno stanje");

                }
                else
                {
                    MessageBox.Show("Niste učitali JSON file", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                UpdateLog($"Greska {ex.InnerException} \n {ex.Message}");

                MessageBox.Show($"{ex.InnerException}");

                // btnSave_Click(sender, e);

            }

        }

        private void btnGenerateAllArticles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathFile = getPathNameForNewFile();

                if (!string.IsNullOrEmpty(pathFile))
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
            catch (Exception ex)
            {
                UpdateLog($"Greska {ex.InnerException} \n {ex.Message}");

                MessageBox.Show($"{ex.InnerException}");
            }
        }
        private void txtCurrentAmount_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnAddCell_Click(sender, e);
            }
        }
        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                file.ShowDialog();

                if (!string.IsNullOrEmpty(file.FileName))
                {


                    List<Article> druga = JsonConvert.DeserializeObject<List<Article>>(File.ReadAllText(file.FileName))!;

                    var newList = MergeArrays(druga);
                    string pathFile = getPathNameForNewFile();
                    if (!string.IsNullOrEmpty(pathFile))
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
                        foreach (Article art in newList)
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
                        File.WriteAllText(@$"{pathFile}.json", JsonConvert.SerializeObject(newList));

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
            catch (Exception)
            {

                throw;
            }
        }
        public List<Article> MergeArrays(List<Article> array2)
        {
            // Create a dictionary to store articles using a unique identifier
            var articlesDictionary = new Dictionary<string, Article>();

            // Add articles from array1 to the dictionary
            foreach (var article in articles)
            {
                if (!articlesDictionary.ContainsKey(article.barkod))
                {
                    articlesDictionary.Add(article.barkod, article);
                }
                else
                {
                    // Merge kolicina values if the article already exists
                    articlesDictionary[article.barkod].kolicina += article.kolicina;
                }
            }

            // Add articles from array2 to the dictionary
            foreach (var article in array2)
            {
                if (!articlesDictionary.ContainsKey(article.barkod))
                {
                    articlesDictionary.Add(article.barkod, article);
                }
                else
                {
                    // Merge kolicina values if the article already exists
                    articlesDictionary[article.sifra].kolicina += article.kolicina;
                }
            }

            // Convert the dictionary values back to a list
            var mergedArray = new List<Article>(articlesDictionary.Values);

            return mergedArray;
        }
        public void RefreshData()
        {
            articles = dbReader.GetAllArticles();
            dataGridList.ItemsSource = "";
            dataGridList.ItemsSource = articles;
        }


    }

}
